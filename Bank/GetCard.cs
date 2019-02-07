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
    public partial class GetCard : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public GetCard()
        {
            InitializeComponent();
            AddDataToComboBox(comboBox1, "Select [№_паспорта] From Clients", "Clients", "№_паспорта", "№_паспорта");
            AddDataToComboBox(comboBox2, "Select [Id], [Название карты] FROM CardTypes", "CardTypes", "Название карты", "Id");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
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
            Int64 Num;
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
            {
                MessageBox.Show("Введите данные!");
                return;
            }
            if (!Int64.TryParse(textBox1.Text, out Num) || textBox1.Text.Length != 16 || !Int64.TryParse(textBox2.Text, out Num) || Convert.ToInt64(textBox1.Text) < 0 || Convert.ToInt16(textBox2.Text) < 0)
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            if (!Cards.edit)
            {
                SqlConnection connect = new SqlConnection(ConnectionString);
                string sql = "Insert [BankCards]([№_карты],[Статус карты], [Дата оформления], [Код для оплаты в интернете], [№_паспорта], [Id_карты]) VALUES(@Number, @Status, @Date, @InternetCode, @Passport, @Id)";
                SqlCommand cmd_SQL = new SqlCommand(sql, connect);
                cmd_SQL.Parameters.AddWithValue("Number", textBox1.Text);
                cmd_SQL.Parameters.AddWithValue("Status", "Активна");
                cmd_SQL.Parameters.AddWithValue("Date", DateTime.Today);
                cmd_SQL.Parameters.AddWithValue("InternetCode", textBox2.Text);
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
            Close();
        }

        private void GetCard_Load(object sender, EventArgs e)
        {

        }
    }
}
