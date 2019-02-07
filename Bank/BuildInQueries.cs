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
    public partial class BuildInQueries : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public BuildInQueries()
        {
            InitializeComponent();
        }

        private void BuildInQueries_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите дату для проверки!");
            }
            else
            {
                string date = textBox1.Text;
                SQLQuery(bindingNavigator1, dataGridView1, "SELECT [Дата выдачи кредита], [ФИО], [Номер_телефона], [Статус кредита], Clients.[№_паспорта], [Идентификационный код] From BankCredits INNER JOIN Clients ON BankCredits.[№_паспорта] = Clients.[№_паспорта] INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона] Where [Дата выдачи кредита]<convert(date, '" + date + "',104)");
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 menu = new Form1();
            menu.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SQLQuery(bindingNavigator1, dataGridView1, "SELECT [ФИО], [Номер_телефона], Clients.[№_паспорта], [Идентификационный код], [Название карты] From BankCards INNER JOIN Clients ON BankCards.[№_паспорта] = Clients.[№_паспорта] INNER JOIN CardTypes ON BankCards.[Id_карты] = CardTypes.[Id] INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона] Where [Id_карты]=6");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SQLQuery(bindingNavigator1, dataGridView1, "SELECT [ФИО], [Номер_телефона], [Статус кредита], Clients.[№_паспорта], [Электронная почта], [Дата следующего платежа] From Payments INNER JOIN BankCredits ON Payments.[№_договора] = BankCredits.[№_Договора] Inner Join Clients on BankCredits.[№_паспорта] = Clients.[№_паспорта] INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона] Where [Дата следующего платежа]<convert(date, '" + DateTime.Now.ToString("dd/MM/yyyy") + "',104)");
        }

        public static void SQLQuery(BindingNavigator bindingNavigator1, DataGridView dataGridView1, String QueryText)
        {
            try
            {
                BindingSource bs1 = new BindingSource();
                SqlConnection sqlconn = new SqlConnection(ConnectionString);
                sqlconn.Open();
                SqlDataAdapter oda = new SqlDataAdapter(QueryText, sqlconn);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                bs1.DataSource = dt;
                bindingNavigator1.BindingSource = bs1;
                dataGridView1.DataSource = bs1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error: " + ex.Message);
            }
        }
    }
}
