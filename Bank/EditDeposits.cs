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
    public partial class EditDeposits : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        string DepositPercentage = Deposits.DepositPercent;
        public EditDeposits()
        {
            InitializeComponent();
            if (Deposits.edit)
            {
                textBox1.Text = Deposits.DepositName;
                textBox2.Text = Deposits.DepositPercent;
            }
        }

        private void EditDeposits_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double Num;
            if(textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || !double.TryParse(textBox2.Text, out Num) || Convert.ToDouble(textBox2.Text) < 0 )
            {
                MessageBox.Show("Ошибка ввода данных!");
                return;
            }
            if (!Deposits.edit)
            {
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "Insert [DepositTypes]([Id],[Наименование вклада], [Процентная ставка], [Статус счета]) VALUES(@Id, @Name, @Percent, @Status)";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Id", Deposits.IdOfDeposit);
                cmd_SQL.Parameters.AddWithValue("Name", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("Percent", textBox2.Text);
                cmd_SQL.Parameters.AddWithValue("Status", "Действителен");
                try
                {
                    connect.Open();
                    int n = cmd_SQL.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    // throw new ApplicationException("error insert new_tovar", ex);
                }
                finally
                {
                    connect.Close();
                }
            }
            else
            {
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "UPDATE [DepositTypes] SET [Наименование вклада]=@Title, [Процентная ставка]=@Percent WHERE [Id]=@Id;";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Title", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("Percent", Convert.ToDouble(textBox2.Text));
                cmd_SQL.Parameters.AddWithValue("Id", Deposits.DepositId);
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
                if (DepositPercentage != textBox2.Text)
                {
                    ChangeDataInClientsDeposits();
                }
            }
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void ChangeDataInClientsDeposits()
        {
            SqlConnection connect = new SqlConnection(ConnectionString);
            string sql = "UPDATE [BankDeposits] SET [Приход]=[Размер вклада]*(@Percent/100) WHERE [Id вклада]=@Id;";
            SqlCommand cmd_SQL = new SqlCommand(sql, connect);
            cmd_SQL.Parameters.AddWithValue("Percent", Convert.ToDouble(textBox2.Text));
            cmd_SQL.Parameters.AddWithValue("Id", Deposits.DepositId);
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
    }
}
