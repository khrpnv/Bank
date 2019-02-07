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
    public partial class EditCards : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public EditCards()
        {
            InitializeComponent();
            if (Cards.edit)
                AddDataToForm(textBox1, textBox2, textBox3, textBox4, textBox5);
        }

        private void EditCards_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double Num;
            int number;
            if (textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || textBox1.Text.Length == 0 || textBox4.Text.Length == 0 || textBox5.Text.Length == 0)
            {
                MessageBox.Show("Введите данные!");
                return;
            }
            if (!int.TryParse(textBox2.Text, out number) || !double.TryParse(textBox3.Text, out Num) || !double.TryParse(textBox4.Text, out Num) || !double.TryParse(textBox5.Text, out Num))
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            if (!Cards.edit)
            {
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "Insert [CardTypes]([Id],[Название карты], [Срок действия(в годах)], [Стоимость оформления], [Стоимость обслуживания], [Максимальная сумма для хранения]) VALUES(@Id, @Name, @Period, @Price1, @Price2, @Amount)";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Id", Cards.AmountOfRows);
                cmd_SQL.Parameters.AddWithValue("Name", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("Period", textBox2.Text);
                cmd_SQL.Parameters.AddWithValue("Price1", Convert.ToDouble(textBox3.Text));
                cmd_SQL.Parameters.AddWithValue("Price2", Convert.ToDouble(textBox4.Text));
                cmd_SQL.Parameters.AddWithValue("Amount", Convert.ToDouble(textBox5.Text));
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
                string sql = "UPDATE [CardTypes] SET [Название карты]=@Name, [Срок действия(в годах)]=@Period, [Стоимость оформления]=@Price1, [Стоимость обслуживания]=@Price2, [Максимальная сумма для хранения]=@Amount WHERE [Id]=@Id;";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Id", Cards.Id);
                cmd_SQL.Parameters.AddWithValue("Name", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("Period", textBox2.Text);
                cmd_SQL.Parameters.AddWithValue("Price1", Convert.ToDouble(textBox3.Text));
                cmd_SQL.Parameters.AddWithValue("Price2", Convert.ToDouble(textBox4.Text));
                cmd_SQL.Parameters.AddWithValue("Amount", Convert.ToDouble(textBox5.Text));
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
            Close();
        }
        private static void AddDataToForm(TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5)
        {
            textBox1.Text = Cards.CardType;
            textBox2.Text = Cards.Period.ToString();
            textBox3.Text = Cards.Price1.ToString();
            textBox4.Text = Cards.Price2.ToString();
            textBox5.Text = Cards.Amount.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
