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
    public partial class EditPhonesBook : Form
    {
        string FIO;
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public EditPhonesBook()
        {
            InitializeComponent();
            if (Clients.edit)
            {
                AddDataToFrom(textBox1, textBox2, Clients.PhoneNumber, Clients.FIO);
            }
        }

        private void EditPhonesBook_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int Num;
            if (textBox1.Text.Any(c => char.IsLetter(c)) || textBox1.Text.Length != 10 || !int.TryParse(textBox1.Text, out Num))
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            if (!Clients.edit)
            {
                if (textBox2.Text.Length == 0 || IsNumberContains(textBox2.Text))
                {
                    MessageBox.Show("Неправильно введены данные!");
                    return;
                }
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "Insert [PhonesBook](Номер_телефона, ФИО) VALUES(@Номер_телефона, @ФИО)";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Номер_телефона", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("ФИО", textBox2.Text);
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
                string sql = "UPDATE [PhonesBook] SET [Номер_телефона]=@Номер_телефона WHERE [ФИО]=@ФИО;";
                string sql2 = "UPDATE [PhonesBook] SET [ФИО]=@ФИО WHERE [Номер_телефона]=@Номер_телефона;";

                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                SqlCommand cmd_SQL_2 = new SqlCommand(sql2, connect);

                cmd_SQL.Parameters.AddWithValue("Номер_телефона", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("ФИО", Clients.FIO);
                cmd_SQL_2.Parameters.AddWithValue("Номер_телефона", textBox1.Text);
                cmd_SQL_2.Parameters.AddWithValue("ФИО", textBox2.Text);

                try
                {
                    connect.Open();
                    if (textBox1.Text != Clients.PhoneNumber)
                    {
                        int n = cmd_SQL.ExecuteNonQuery();
                    }
                    if (textBox2.Text != Clients.FIO)
                    {
                        int m = cmd_SQL_2.ExecuteNonQuery();
                    }
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
        public static void AddDataToFrom(TextBox phoneNumber, TextBox FIO, string phoneNumberString, string FIOString)
        {
            phoneNumber.Text = phoneNumberString;
            FIO.Text = FIOString;
        }
        static bool IsNumberContains(string input)
        {
            foreach (char c in input)
                if (Char.IsNumber(c))
                    return true;
            return false;
        }
    }
}
