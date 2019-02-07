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
    public partial class Authorization : Form
    {
        string login = "admin";
        string password = "1234567890";
        public Authorization()
        {
            InitializeComponent();
        }

        private void Authorization_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == login && textBox2.Text == password)
            {
                Form1 main = new Form1();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!");
            }
        }
    }
}
