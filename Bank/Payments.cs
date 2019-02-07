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
    public partial class Payments : Form
    {
        public static string NumberOfCredit;
        public static string AlreadyPaidSum;
        public Payments()
        {
            InitializeComponent();
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select Payments.[№_договора],[Дата внесения платежа],[Размер ежемесячного платежа],[Размер внесенного платежа],[Дата следующего платежа],[Выплаченная сумма],[Разер кредита]*(Cast((Select [Процент переплат] FROM CreditTypes WHere [Id]=BankCredits.[Id_кредита]) as float)/100+1)-[Выплаченная сумма] as 'Осталось выплатить' From BankCredits JOIN Payments ON BankCredits.[№_договора]=Payments.[№_договора];");
        }

        private void Payments_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select Payments.[№_договора],[Дата внесения платежа],[Размер ежемесячного платежа],[Размер внесенного платежа],[Дата следующего платежа],[Выплаченная сумма],[Разер кредита]*(Cast((Select [Процент переплат] FROM CreditTypes WHere [Id]=BankCredits.[Id_кредита]) as float)/100+1)-[Выплаченная сумма] as 'Осталось выплатить' From BankCredits JOIN Payments ON BankCredits.[№_договора]=Payments.[№_договора];");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 main = new Form1();
            main.Show();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(dataGridView1[6, dataGridView1.CurrentRow.Index].Value).ToString() == "0")
            {
                MessageBox.Show("Запрещено! Кредит уже погашен.");
                return;
            }
            Pay pay = new Pay();
            NumberOfCredit = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            pay.ShowDialog();
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select Payments.[№_договора],[Дата внесения платежа],[Размер ежемесячного платежа],[Размер внесенного платежа],[Дата следующего платежа],[Выплаченная сумма],[Разер кредита]*(Cast((Select [Процент переплат] FROM CreditTypes WHere [Id]=BankCredits.[Id_кредита]) as float)/100+1)-[Выплаченная сумма] as 'Осталось выплатить' From BankCredits JOIN Payments ON BankCredits.[№_договора]=Payments.[№_договора];");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NewPayment payment = new NewPayment();
            payment.ShowDialog();
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "Select Payments.[№_договора],[Дата внесения платежа],[Размер ежемесячного платежа],[Размер внесенного платежа],[Дата следующего платежа],[Выплаченная сумма],[Разер кредита]*(Cast((Select [Процент переплат] FROM CreditTypes WHere [Id]=BankCredits.[Id_кредита]) as float)/100+1)-[Выплаченная сумма] as 'Осталось выплатить' From BankCredits JOIN Payments ON BankCredits.[№_договора]=Payments.[№_договора];");
        }
    }
}
