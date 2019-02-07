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
    public partial class Cards : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public static int AmountOfRows;
        public static bool edit=false;
        public static String CardType;
        public static int Period;
        public static double Price1;
        public static double Price2;
        public static double Amount;
        public static int Id;
        public Cards()
        {
            InitializeComponent();
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From CardTypes");
            if (label1.Text == "Доступные виды карт")
                AmountOfRows = dataGridView1.RowCount;
        }

        private void Cards_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet.BankCards". При необходимости она может быть перемещена или удалена.
            this.bankCardsTableAdapter.Fill(this.bankDataSet.BankCards);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet.CardTypes". При необходимости она может быть перемещена или удалена.
            this.cardTypesTableAdapter.Fill(this.bankDataSet.CardTypes);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet.BankCards". При необходимости она может быть перемещена или удалена.
            this.bankCardsTableAdapter.Fill(this.bankDataSet.BankCards);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet.CardTypes". При необходимости она может быть перемещена или удалена.
            this.cardTypesTableAdapter.Fill(this.bankDataSet.CardTypes);

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void доступныеВидыКартToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Доступные виды карт";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From CardTypes");
            AmountOfRows = dataGridView1.RowCount;
        }

        private void выданныеКартыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Выданные карты";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankCards");
            AmountOfRows = dataGridView1.RowCount;
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 menu = new Form1();
            menu.Show();
            this.Close();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = false;
            if(label1.Text == "Доступные виды карт")
            {
                EditCards edt = new EditCards();
                edt.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From CardTypes");
            }
            if(label1.Text == "Выданные карты")
            {
                GetCard getCard = new GetCard();
                getCard.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankCards");
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            if (label1.Text == "Доступные виды карт")
            {
                Delete deleteForm = new Delete();
                deleteForm.ShowDialog();
                if (deleteForm.DialogResult == DialogResult.OK)
                {
                    Delete(dataGridView1[0, row].Value.ToString(), "CardTypes");
                    AmountOfRows--;
                }
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From CardTypes");
            }
            else if(label1.Text == "Выданные карты")
            {
                Delete deleteForm = new Delete();
                deleteForm.ShowDialog();
                if (deleteForm.DialogResult == DialogResult.OK)
                {
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "DELETE BankCards WHERE [№_карты]='"+ dataGridView1[0, row].Value.ToString() + "';");
                }
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankCards");
            }
        }
        private static void Delete(string Id, string TableName)
        {
            SqlConnection connect = new SqlConnection(ConnectionString);
            string sql = "DELETE ["+TableName+"] Where [Id]=@Id;";
            SqlCommand cmd_SQL = new SqlCommand(sql, connect);
            cmd_SQL.Parameters.AddWithValue("Id", Id);
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

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = true;
            if(label1.Text == "Доступные виды карт")
            {
                Id = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
                CardType = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
                Period = Convert.ToInt32(dataGridView1[2, dataGridView1.CurrentRow.Index].Value);
                Price1 = Convert.ToDouble(dataGridView1[3, dataGridView1.CurrentRow.Index].Value);
                Price2 = Convert.ToDouble(dataGridView1[4, dataGridView1.CurrentRow.Index].Value);
                Amount = Convert.ToDouble(dataGridView1[5, dataGridView1.CurrentRow.Index].Value);
                EditCards edt = new EditCards();
                edt.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From CardTypes");
            }
            else if(label1.Text == "Выданные карты")
            {
                MessageBox.Show("Запрещено!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(label1.Text == "Выданные карты")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM BankCards WHERE [№_карты]='"+textBox1.Text+"';");
            }
            else if (label1.Text == "Доступные виды карт")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM CardTypes WHERE [Id]='" + textBox1.Text + "';");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Выданные карты")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM BankCards");
            }
            else if (label1.Text == "Доступные виды карт")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM CardTypes;");
            }
            textBox1.Text = "";
        }
    }
}
