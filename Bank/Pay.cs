using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Bank
{
    public partial class Pay : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public Pay()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double Num;
            if(!double.TryParse(textBox2.Text, out Num) || Convert.ToDouble(textBox2.Text) < 0)
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            DateTime today = DateTime.Today;
            SqlConnection connect = new SqlConnection(ConnectionString);
            string sql = "UPDATE [Payments] SET [Дата внесения платежа]=@Date, [Размер внесенного платежа]=@Amount, [Дата следующего платежа]=DATEADD(day, CAST(FLOOR((@Amount*30)/[Размер ежемесячного платежа]) AS INT), [Дата следующего платежа]), [Выплаченная сумма]=[Выплаченная сумма]+@Amount WHERE [№_договора]=@Number;";
            SqlCommand cmd_SQL = new SqlCommand(sql, connect);
            cmd_SQL.Parameters.AddWithValue("Date", today);
            cmd_SQL.Parameters.AddWithValue("Amount", Convert.ToDouble(textBox2.Text));
            cmd_SQL.Parameters.AddWithValue("Number", textBox1.Text);
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
            CloseCredit(textBox1.Text);
            Close();
        }

        private void Pay_Load(object sender, EventArgs e)
        {
            textBox1.Text = Payments.NumberOfCredit;
        }

        private void CloseCredit(string number)
        {
            SqlConnection connect = new SqlConnection(ConnectionString);
            string sql = "UPDATE [BankCredits] SET [Статус кредита]=@Status WHERE [Разер кредита]*(Cast((Select [Процент переплат] FROM CreditTypes WHere [Id]=BankCredits.[Id_кредита]) as float)/100+1)-(Select [Выплаченная сумма] From Payments Where [№_договора]='" + number+ "')=0 AND [№_договора]='" + number + "';";
            SqlCommand cmd_SQL = new SqlCommand(sql, connect);
            cmd_SQL.Parameters.AddWithValue("Status", "Погашен");
            try
            {
                connect.Open();
                int n = cmd_SQL.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error", ex);
            }
            finally
            {
                connect.Close();
            }
        }
    }
}
