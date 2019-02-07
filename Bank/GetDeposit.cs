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
    public partial class GetDeposit : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public GetDeposit()
        {
            InitializeComponent();
            if (Deposits.edit)
            {
                textBox2.Text = Deposits.AccountNumber;
                textBox1.Text = Deposits.AccountAmount;
                comboBox1.Text = Deposits.PassportNumber;
                comboBox2.Text = Deposits.AccountType;
                textBox2.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
            AddDataToComboBox(comboBox1, "Select [№_паспорта] From Clients", "Clients", "№_паспорта", "№_паспорта");
            AddDataToComboBox(comboBox2, "Select [Id],[Наименование вклада] From DepositTypes", "DepositTypes", "Наименование вклада", "Id");
        }

        private void GetDeposit_Load(object sender, EventArgs e)
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


        private void button1_Click(object sender, EventArgs e)
        {
            Int64 Num;
            double num;
            if(textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox2.Text.Length != 12)
            {
                MessageBox.Show("Ошибка ввода данных!");
                return;
            }
            if(!double.TryParse(textBox1.Text, out num) || Convert.ToDouble(textBox1.Text) < 0)
            {
                MessageBox.Show("Ошибка ввода данных!");
                return;
            }
            if(!long.TryParse(textBox2.Text, out Num))
            {
                MessageBox.Show("Ошибка ввода данных!");
                return;
            }
            if (!Deposits.edit)
            {
                string percent = GetDataFromQuery("Select [Процентная ставка] From DepositTypes WHERE [Id]="+comboBox2.SelectedValue+";");
                double income = Convert.ToDouble(textBox1.Text) * (Convert.ToDouble(percent) / 100);
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "Insert [BankDeposits]([№_счета],[Дата открытия], [Приход], [Размер вклада], [№ паспорта], [Id вклада]) VALUES(@Number, @Date, @Income, @Amount, @Passport, @Id)";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Number", textBox2.Text);
                cmd_SQL.Parameters.AddWithValue("Date", DateTime.Today);
                cmd_SQL.Parameters.AddWithValue("Amount", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("Income", Convert.ToString(income));
                cmd_SQL.Parameters.AddWithValue("Passport", comboBox1.SelectedValue);
                cmd_SQL.Parameters.AddWithValue("Id", comboBox2.SelectedValue);
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
            }
            else
            {
                string percent = GetDataFromQuery("Select [Процентная ставка] From DepositTypes WHERE [Id]=" + comboBox2.SelectedValue + ";");
                double income = Convert.ToDouble(textBox1.Text) * (Convert.ToDouble(percent) / 100);
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "UPDATE [BankDeposits] SET [Размер вклада]=@Amount, [Приход]=@Income WHERE [№_счета]=@Number;";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Amount", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("Income", Convert.ToString(income));
                cmd_SQL.Parameters.AddWithValue("Number", textBox2.Text);
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
            }
            Close();
        }
    }
}
