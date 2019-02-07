using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{
    public partial class NewPayment : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public NewPayment()
        {
            InitializeComponent();
            AddDataToComboBox(comboBox1, "Select [№_договора] From BankCredits WHERE [№_договора] NOT IN(Select [№_договора] FROM Payments)", "BankCredits", "№_договора", "№_договора");
        }

        private void NewPayment_Load(object sender, EventArgs e)
        {

        }
        void AddDataToComboBox(ComboBox comboBox, string query, string tableName, string displayMember, string valueMember)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    conn.Open();
                    DataSet ds = new DataSet();
                    da.Fill(ds, tableName);
                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                    comboBox.DataSource = ds.Tables[tableName];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0 || Convert.ToDouble(textBox2.Text) < 0)
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            string CreditAmount = GetDataFromQuery("Select [Разер кредита] FROM BankCredits Where [№_договора]='"+comboBox1.Text+"'");
            string CreditTake = GetDataFromQuery("Select [Дата выдачи кредита] FROM BankCredits Where [№_договора]='"+comboBox1.Text+"'");
            string CreditReturn = GetDataFromQuery("Select [Расчётная дата] FROM BankCredits Where [№_договора]='"+comboBox1.Text+"'");
            int days = (int)((Convert.ToDateTime(CreditReturn) - Convert.ToDateTime(CreditTake)).TotalDays);
            int month = (int)(days / 12);
            double monthAmount = Convert.ToDouble(CreditAmount) / month;

            DateTime nextDay = DateTime.Today.AddDays(Convert.ToDouble(textBox2.Text) / (monthAmount / 30));
            
            SqlConnection connect = new SqlConnection(ConnectionString);
            string sql = "Insert [Payments]([№_договора],[Дата внесения платежа], [Размер ежемесячного платежа], [Размер внесенного платежа], [Дата следующего платежа], [Выплаченная сумма]) VALUES(@Document, @Date, @Amount1, @Amount2, @NextDate, @PaidSum)";
            SqlCommand cmd_SQL = new SqlCommand(sql, connect);
            cmd_SQL.Parameters.AddWithValue("Document", comboBox1.Text);
            cmd_SQL.Parameters.AddWithValue("Date", DateTime.Today);
            cmd_SQL.Parameters.AddWithValue("Amount1", monthAmount);
            cmd_SQL.Parameters.AddWithValue("Amount2",Convert.ToDouble(textBox2.Text));
            cmd_SQL.Parameters.AddWithValue("NextDate", nextDay);
            cmd_SQL.Parameters.AddWithValue("PaidSum", Convert.ToDouble(textBox2.Text));
            try
            {
                connect.Open();
                int n = cmd_SQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(@"Error: " + ex.Message);
            }
            finally
            {
                connect.Close();
            }
            Close();
        }
        string GetDataFromQuery(string sqlquery)
        {
            string result;
            SqlConnection connect = new SqlConnection(ConnectionString);
            connect.Open();
            SqlCommand command = new SqlCommand(sqlquery, connect);
            result = command.ExecuteScalar().ToString();
            connect.Close();
            return result;
        }
    }
}
