using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{
    public partial class EditClient : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public EditClient()
        {
            InitializeComponent();
            AddDataToComboBox(comboBox1, "Select [Номер_телефона] From PhonesBook", "PhonesBook", "Номер_телефона", "Номер_телефона");
            if (Clients.edit)
            {
                textBox1.Text = Clients.ClientsPassport;
                dateTimePicker1.Text = Clients.BirthDate;
                textBox2.Text = Clients.Code;
                textBox3.Text = Clients.Mail;
                comboBox1.SelectedValue = Clients.Phone;
                textBox2.Enabled = false;
                textBox1.Enabled = false;
                comboBox1.Enabled = false;
            }
            if (!Clients.edit)
            {
                textBox2.Enabled = true;
                textBox1.Enabled = true;
                comboBox1.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void EditClient_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            int Num;
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0 || comboBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите данные!");
                return;
            }
            if(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")) <= DateTime.Parse(dateTimePicker1.Text) || !int.TryParse(textBox2.Text, out Num))
            {
                MessageBox.Show("Ошибка ввода данных!");
                return;
            }
            if (!regex.Match(textBox3.Text).Success)
            {
                MessageBox.Show("Неправильно введен адресс электронной почты!");
                return;
            }
            if (!Clients.edit)
            {
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "Insert [Clients]([№_паспорта],[Дата рождения], [№_телефона], [Идентификационный код], [Электронная почта]) VALUES(@Passport, @Date, @Phone, @Code, @Mail)";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Passport", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("Date", dateTimePicker1.Text);
                cmd_SQL.Parameters.AddWithValue("Phone", comboBox1.SelectedValue);
                cmd_SQL.Parameters.AddWithValue("Code", textBox2.Text);
                cmd_SQL.Parameters.AddWithValue("Mail", textBox3.Text);
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
                string sql = "UPDATE [Clients] SET [Дата рождения]=@Date, [Электронная почта]=@Mail, [№_телефона]=@Phone WHERE [Идентификационный код]=@Code;";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Date", dateTimePicker1.Value);
                cmd_SQL.Parameters.AddWithValue("Phone", comboBox1.SelectedValue);
                cmd_SQL.Parameters.AddWithValue("Code", textBox2.Text);
                cmd_SQL.Parameters.AddWithValue("Mail", textBox3.Text);
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
    }
}
