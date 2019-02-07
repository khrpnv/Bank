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
    public partial class GetCredit : Form
    {
        DateTime today = DateTime.Today;
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public GetCredit()
        {
            InitializeComponent();
            AddDataToComboBox(comboBox1, "Select [Название кредитной программы], [Id] From CreditTypes", "CreditTypes", "Название кредитной программы", "Id");
            AddDataToComboBox(comboBox2, "Select [№_паспорта] From Clients", "Clients", "№_паспорта", "№_паспорта");
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void GetCredit_Load(object sender, EventArgs e)
        {
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Num;
            double Number;
            if(textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
            {
                MessageBox.Show("Введите данные!");
                return;
            }
            if (!int.TryParse(textBox1.Text, out Num) || textBox1.Text.Length != 6 || !double.TryParse(textBox2.Text, out Number))
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            if (Convert.ToDouble(textBox2.Text) > Convert.ToDouble(GetDataFromQuery("Select [Максимальная сумма] From CreditTypes Where Id="+comboBox1.SelectedValue)))
            {
                MessageBox.Show("Слишком большая сумма!");
                return;
            }
            string period = GetDataFromQuery("Select [Срок] From CreditTypes Where Id=" + comboBox1.SelectedValue);
            string[] words = period.Split(' ');
            int years = Convert.ToInt32(words[0]);
            SqlConnection connect = new SqlConnection(ConnectionString);
            string sql = "Insert [BankCredits]([№_договора],[Дата выдачи кредита], [Разер кредита], [Расчётная дата], [Статус кредита], [№_паспорта], [Id_кредита]) VALUES(@Document, @Date, @Amount, @FinalDate, @Status, @Passport, @Id)";
            SqlCommand cmd_SQL = new SqlCommand(sql, connect);
            cmd_SQL.Parameters.AddWithValue("Id", comboBox1.SelectedValue);
            cmd_SQL.Parameters.AddWithValue("Document", textBox1.Text);
            cmd_SQL.Parameters.AddWithValue("Date", today.ToString("dd.MM.yyyy"));
            cmd_SQL.Parameters.AddWithValue("Amount", Convert.ToDouble(textBox2.Text));
            cmd_SQL.Parameters.AddWithValue("FinalDate", today.AddYears(years));
            cmd_SQL.Parameters.AddWithValue("Status", "Выдан");
            cmd_SQL.Parameters.AddWithValue("Passport", comboBox2.SelectedValue);
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
            this.Close();
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
