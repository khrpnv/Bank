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
    public partial class Clients : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public static bool edit = false;
        public static string FIO;
        public static string PhoneNumber;
        public static string ClientsPassport;
        public static string BirthDate;
        public static string Code;
        public static string Mail;
        public static string Phone;
        public Clients()
        {
            InitializeComponent();
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [ФИО],[№_паспорта],[Дата рождения],[Идентификационный код],[Электронная почта], [№_телефона] FROM Clients INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона]");
        }

        private void Clients_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet.PhonesBook". При необходимости она может быть перемещена или удалена.
            this.phonesBookTableAdapter.Fill(this.bankDataSet.PhonesBook);
        }

        private void доступныеВидыКартToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Информация о клиентах";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [ФИО],[№_паспорта],[Дата рождения],[Идентификационный код],[Электронная почта], [№_телефона] FROM Clients INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона]");
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 menu = new Form1();
            menu.Show();
            this.Close();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void видToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void телефоннаяКнигаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Телефонная книга";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From PhonesBook");
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = false;
            if (label1.Text == "Телефонная книга")
            {
                EditPhonesBook edt = new EditPhonesBook();
                edt.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From PhonesBook");
            }
            else if(label1.Text == "Информация о клиентах")
            {
                EditClient edit = new EditClient();
                edit.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [ФИО],[№_паспорта],[Дата рождения],[Идентификационный код],[Электронная почта], [№_телефона] FROM Clients INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона]");
            }
        }

        private void Clients_FormClosing(object sender, FormClosingEventArgs e)
        {
            phonesBookTableAdapter.Update(bankDataSet);
        }

        private void правкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Телефонная книга")
            {
                int row = dataGridView1.CurrentRow.Index;
                Delete deleteForm = new Delete();
                deleteForm.ShowDialog();
                if (deleteForm.DialogResult == DialogResult.OK)
                {
                    Delete(dataGridView1[0, row].Value.ToString());
                }
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From PhonesBook");
            }
            else if (label1.Text == "Информация о клиентах")
            {
                int row = dataGridView1.CurrentRow.Index;
                Delete deleteForm = new Delete();
                deleteForm.ShowDialog();
                if (deleteForm.DialogResult == DialogResult.OK)
                {
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "DELETE Clients WHERE [Идентификационный код]='" + dataGridView1[3, row].Value.ToString() + "';");
                }
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [ФИО],[№_паспорта],[Дата рождения],[Идентификационный код],[Электронная почта] FROM Clients INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона]");
            }
        }

        private static void Delete(string number)
        {
            SqlConnection connect = new SqlConnection(ConnectionString);
            string sql = "DELETE [PhonesBook] Where Номер_телефона=@Номер_телефона;";
            SqlCommand cmd_SQL = new SqlCommand(sql, connect);
            cmd_SQL.Parameters.AddWithValue("Номер_телефона", number);
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

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = true;
            if (label1.Text == "Телефонная книга")
            {
                PhoneNumber = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                FIO = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
                EditPhonesBook edt = new EditPhonesBook();
                edt.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From PhonesBook");
            }
            else if(label1.Text == "Информация о клиентах")
            {
                ClientsPassport = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
                BirthDate = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
                Code = dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
                Mail = dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
                Phone = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
                EditClient edit = new EditClient();
                edit.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [ФИО],[№_паспорта],[Дата рождения],[Идентификационный код],[Электронная почта], [№_телефона] FROM Clients INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона]");
            }
        }   
    }
}
