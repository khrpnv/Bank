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
    public partial class DepositsFiltration : Form
    {
        public DepositsFiltration()
        {
            InitializeComponent();
        }

        private void DepositsFiltration_Load(object sender, EventArgs e)
        {
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string firstParam = textBox1.Text;
            string secondParam = textBox2.Text;
            if (textBox1.Text.Length == 0) firstParam = "0";
            if (textBox2.Text.Length == 0) secondParam = "100000000";
            int Num;
            if(!int.TryParse(firstParam, out Num) || !int.TryParse(secondParam, out Num))
            {
                MessageBox.Show("Неправильно введены данные!");
            }
            if (Convert.ToDouble(firstParam) > Convert.ToDouble(secondParam))
            {
                MessageBox.Show("Перваое значение фильтра больше, чем второе!");
            }
            else if (Convert.ToDouble(firstParam) < 0 || Convert.ToDouble(secondParam) < 0)
            {
                MessageBox.Show("Введите положительные значения фильтров!");
            }
            else
            {
                BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits Where [Размер вклада] BETWEEN '" + firstParam + "' AND '" + secondParam + "'");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select * From BankDeposits");
        }
    }
}
