using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{
    public partial class Deposits : Form
    {
        public static int IdOfDeposit;
        public static bool edit;
        public static string DepositName;
        public static string DepositPercent;
        public static string DepositId;
        public static string AccountNumber;
        public static string AccountAmount;
        public static string PassportNumber;
        public static string AccountType;
        public Deposits()
        {
            InitializeComponent();
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From DepositTypes");
            IdOfDeposit = dataGridView1.Rows.Count;
        }

        private void Deposits_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet1.DepositTypes". При необходимости она может быть перемещена или удалена.
            this.depositTypesTableAdapter1.Fill(this.bankDataSet1.DepositTypes);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet1.BankDeposits". При необходимости она может быть перемещена или удалена.
            this.bankDepositsTableAdapter1.Fill(this.bankDataSet1.BankDeposits);

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

        private void доступныеВидыКартToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Доступные виды вкладов";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From DepositTypes");
            IdOfDeposit = dataGridView1.Rows.Count;
        }

        private void выданныеКартыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Вклады клиентов банка";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Вклады клиентов банка")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits WHERE [№_счета]='" + textBox1.Text + "';");
            }
            else if (label1.Text == "Доступные виды вкладов")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From DepositTypes WHERE [Id]='" + textBox1.Text + "';");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Вклады клиентов банка")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits");
            }
            else if (label1.Text == "Доступные виды вкладов")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From DepositTypes;");
            }
            textBox1.Text = "";
        }

        private void фильтрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Доступные виды вкладов")
            {
                MessageBox.Show("Фильтрация недоступна");
            }
            else if (label1.Text == "Вклады клиентов банка")
            {
                DepositsFiltration filtration = new DepositsFiltration();
                filtration.Show();
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Доступные виды вкладов")
            {
                Delete delete = new Delete();
                delete.ShowDialog();
                if (delete.DialogResult == DialogResult.OK)
                {
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "DELETE FROM DepositTypes WHERE [Id]='" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value + "';");
                }
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From DepositTypes;");
            }
            else if (label1.Text == "Вклады клиентов банка")
            {
                Delete delete = new Delete();
                delete.ShowDialog();
                if (delete.DialogResult == DialogResult.OK)
                {
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "DELETE FROM BankDeposits WHERE [№_счета]='" + dataGridView1[0, dataGridView1.CurrentRow.Index].Value + "';");
                }
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits;");
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = false;
            if (label1.Text == "Доступные виды вкладов")
            {
                IdOfDeposit = dataGridView1.Rows.Count;
                EditDeposits editDeposits = new EditDeposits();
                editDeposits.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From DepositTypes;");
            }
            else if (label1.Text == "Вклады клиентов банка")
            {
                GetDeposit getDeposit = new GetDeposit();
                getDeposit.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits;");
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edit = true;
            if (label1.Text == "Доступные виды вкладов")
            {
                DepositId = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                DepositName = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
                DepositPercent = dataGridView1[2, dataGridView1.CurrentRow.Index].Value.ToString();
                EditDeposits editDeposits = new EditDeposits();
                editDeposits.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From DepositTypes;");
            }
            else if (label1.Text == "Вклады клиентов банка")
            {
                AccountNumber = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                AccountAmount = Convert.ToDouble(dataGridView1[3, dataGridView1.CurrentRow.Index].Value).ToString();
                PassportNumber = dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
                AccountType = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
                GetDeposit getDeposit = new GetDeposit();
                getDeposit.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits;");
            }
        }
    }
}
