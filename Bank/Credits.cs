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
    public partial class Credits : Form
    {
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public static int CreditId;
        public static string CreditName;
        public static double MaxSum;
        public static int MaxAge;
        public static int MinAge;
        public static string Period;
        public static double Percent;
        public static bool IsEditing = false;
        public Credits()
        {
            InitializeComponent();
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From CreditTypes");
        }

        private void Credits_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet1.CreditTypes". При необходимости она может быть перемещена или удалена.
            this.creditTypesTableAdapter1.Fill(this.bankDataSet1.CreditTypes);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bankDataSet1.BankCredits". При необходимости она может быть перемещена или удалена.
            this.bankCreditsTableAdapter1.Fill(this.bankDataSet1.BankCredits);

        }

        private void доступныеВидыКартToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Доступные кредитные программы";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From CreditTypes");
        }

        private void выданныеКартыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "Выданные кредиты";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankCredits");
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(label1.Text == "Выданные кредиты")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM BankCredits Where [№_договора]='" + textBox1.Text + "';");
            }
            else if(label1.Text == "Доступные кредитные программы")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM CreditTypes Where [Id]='" + textBox1.Text + "';");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Выданные кредиты")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM BankCredits;");
            }
            else if (label1.Text == "Доступные кредитные программы")
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM CreditTypes;");
            }
            textBox1.Text = "";
        }

        private void фильтрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Выданные кредиты")
            {
                dataGridView1.Height = 190;
                dataGridView1.Top += 80;
                panel1.Visible = true;
                label1.Visible = false;
            }
        }

        private void видToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void правкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM BankCredits;");
            dataGridView1.Height = 330;
            dataGridView1.Top -= 80;
            panel1.Visible = false;
            label1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked && !checkBox2.Checked)
            {
                MessageBox.Show("Выберите фильтр!");
                return;
            }
            if (checkBox1.Checked && textBox2.Text.Length==0 && textBox3.Text.Length == 0)
            {
                MessageBox.Show("Введите значение фильтров!");
                return;
            }
            if (checkBox2.Checked && DateTime.Parse(dateTimePicker1.Text) > DateTime.Parse(dateTimePicker2.Text))
            {
                MessageBox.Show("Первая дата больше чем вторая!");
                return;
            }
            if (textBox2.Text.Length == 0) textBox2.Text = "0";
            if (checkBox1.Checked && checkBox2.Checked)
            {
                if (textBox3.Text.Length == 0)
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [№_паспорта],Payments.[№_договора],[Дата выдачи кредита],[Разер кредита],[Статус кредита],[Размер ежемесячного платежа],[Дата внесения платежа] FROM BankCredits JOIN Payments ON Payments.[№_договора]=BankCredits.[№_договора] WHERE [Размер ежемесячного платежа]>'" + textBox2.Text + "' AND [Дата внесения платежа] BETWEEN CONVERT(DATE,'" + dateTimePicker1.Text + "',104) AND CONVERT(DATE,'" + dateTimePicker2.Text + "', 104)");
                else
                {
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [№_паспорта],Payments.[№_договора],[Дата выдачи кредита],[Разер кредита],[Статус кредита],[Размер ежемесячного платежа],[Дата внесения платежа] FROM BankCredits JOIN Payments ON Payments.[№_договора]=BankCredits.[№_договора] WHERE [Размер ежемесячного платежа] BETWEEN '" + textBox2.Text + "' AND '" + textBox3.Text + "' AND [Дата внесения платежа] BETWEEN CONVERT(DATE,'" + dateTimePicker1.Text + "',104) AND CONVERT(DATE,'" + dateTimePicker2.Text + "', 104)");
                }
            }
            else if (checkBox1.Checked)
            {
                if (textBox3.Text.Length == 0)
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [№_паспорта],Payments.[№_договора],[Дата выдачи кредита],[Разер кредита],[Статус кредита],[Размер ежемесячного платежа],[Дата внесения платежа] FROM BankCredits JOIN Payments ON Payments.[№_договора]=BankCredits.[№_договора] WHERE [Размер ежемесячного платежа]>'"+textBox2.Text+"'");
                else
                {
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [№_паспорта],Payments.[№_договора],[Дата выдачи кредита],[Разер кредита],[Статус кредита],[Размер ежемесячного платежа],[Дата внесения платежа] FROM BankCredits JOIN Payments ON Payments.[№_договора]=BankCredits.[№_договора] WHERE [Размер ежемесячного платежа] BETWEEN '" + textBox2.Text + "' AND '"+textBox3.Text+"'");
                }
            }
            else if (checkBox2.Checked)
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [№_паспорта],Payments.[№_договора],[Дата выдачи кредита],[Разер кредита],[Статус кредита],[Размер ежемесячного платежа],[Дата внесения платежа] FROM BankCredits JOIN Payments ON Payments.[№_договора]=BankCredits.[№_договора] WHERE [Дата внесения платежа] BETWEEN CONVERT(DATE,'" + dateTimePicker1.Text + "',104) AND CONVERT(DATE,'" + dateTimePicker2.Text + "', 104)");
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Выданные кредиты")
            {
                GetCredit getForm = new GetCredit();
                getForm.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM BankCredits;");
            }
            else if (label1.Text == "Доступные кредитные программы")
            {
                CreditId = dataGridView1.Rows.Count;
                EditCredits editCredits = new EditCredits();
                editCredits.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM CreditTypes;");
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Выданные кредиты")
            {
                MessageBox.Show("Запрещено!");
            }
            else if(label1.Text == "Доступные кредитные программы")
            {
                Delete deleteForm = new Delete();
                deleteForm.ShowDialog();
                if(deleteForm.DialogResult == DialogResult.OK)
                {
                    BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "DELETE FROM CreditTypes WHERE [Id]='"+dataGridView1[0, dataGridView1.CurrentRow.Index].Value+"';");
                }
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM CreditTypes;");
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Выданные кредиты")
            {
                MessageBox.Show("Запрещено!");
            }
            else if(label1.Text == "Доступные кредитные программы")
            {
                GetData(dataGridView1);
                EditCredits editCredits = new EditCredits();
                editCredits.ShowDialog();
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT * FROM CreditTypes;");
            }
        }

        void GetData(DataGridView dataGridView)
        {
            CreditName = dataGridView[1, dataGridView.CurrentRow.Index].Value.ToString();
            MaxSum = Convert.ToDouble(dataGridView[2, dataGridView.CurrentRow.Index].Value);
            MaxAge = (int)dataGridView[3, dataGridView.CurrentRow.Index].Value;
            MinAge = (int)dataGridView[4, dataGridView.CurrentRow.Index].Value;
            Period = dataGridView[5, dataGridView.CurrentRow.Index].Value.ToString();
            Percent = Convert.ToDouble(dataGridView[6, dataGridView.CurrentRow.Index].Value);
            CreditId = (int)dataGridView[0, dataGridView.CurrentRow.Index].Value;
            IsEditing = true;
        }
    }
}
