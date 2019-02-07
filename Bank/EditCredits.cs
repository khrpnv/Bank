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
    public partial class EditCredits : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public EditCredits()
        {
            InitializeComponent();
            if (Credits.IsEditing)
            {
                textBox1.Text = Credits.CreditName;
                textBox2.Text = ""+Credits.MaxSum;
                textBox3.Text = ""+Credits.MaxAge;
                textBox4.Text = ""+Credits.MinAge;
                textBox5.Text = Credits.Period;
                textBox6.Text = ""+Credits.Percent;
            }
        }

        private void EditCredits_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Num;
            double Number;
            if(textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox1.Text.Length == 0 || textBox4.Text.Length == 0 || textBox5.Text.Length == 0 || textBox6.Text.Length == 0)
            {
                MessageBox.Show("Введите данные!");
                return;
            }
            if (!double.TryParse(textBox2.Text, out Number) || !int.TryParse(textBox3.Text, out Num) || !int.TryParse(textBox4.Text, out Num) || !double.TryParse(textBox6.Text, out Number))
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            if (Convert.ToDouble(textBox3.Text) < Convert.ToDouble(textBox4.Text))
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            if (!Credits.IsEditing)
            {
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "Insert [CreditTypes]([Id],[Название кредитной программы], [Максимальная сумма], [Максимальный возраст], [Минимальный возраст], [Срок], [Процент переплат]) VALUES(@Id, @Name, @MaxSum, @MaxAge, @MinAge, @Period, @Overpay)";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Id", Credits.CreditId);
                cmd_SQL.Parameters.AddWithValue("Name", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("MaxSum", Convert.ToDouble(textBox2.Text));
                cmd_SQL.Parameters.AddWithValue("MaxAge", textBox3.Text);
                cmd_SQL.Parameters.AddWithValue("MinAge", textBox4.Text);
                cmd_SQL.Parameters.AddWithValue("Period", textBox5.Text);
                cmd_SQL.Parameters.AddWithValue("Overpay", Convert.ToDouble(textBox6.Text));
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
                string sql = "UPDATE [CreditTypes] SET [Название кредитной программы]=@Name, [Максимальная сумма]=@MaxSum, [Максимальный возраст]=@MaxAge, [Минимальный возраст]=@MinAge, [Срок]=@Period, [Процент переплат]=@Overpay WHERE [Id]=@Id;";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Id", Credits.CreditId);
                cmd_SQL.Parameters.AddWithValue("Name", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("MaxSum", Convert.ToDouble(textBox2.Text));
                cmd_SQL.Parameters.AddWithValue("MaxAge", textBox3.Text);
                cmd_SQL.Parameters.AddWithValue("MinAge", textBox4.Text);
                cmd_SQL.Parameters.AddWithValue("Period", textBox5.Text);
                cmd_SQL.Parameters.AddWithValue("Overpay", Convert.ToDouble(textBox6.Text));
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
            this.Close();
        }
    }
}
