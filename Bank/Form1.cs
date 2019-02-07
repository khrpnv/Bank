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
using System.Net.Mail;
using System.Net;

namespace Bank
{
    public partial class Form1 : Form
    {
        bool done = false;
        const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Bank.mdf;Integrated Security = True";
        public Form1()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cards cards = new Cards();
            this.Hide();
            cards.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            QueryEdit queryEdit = new QueryEdit();
            this.Hide();
            queryEdit.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Credits credits = new Credits();
            this.Hide();
            credits.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Deposits deposits = new Deposits();
            this.Hide();
            deposits.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clients clients = new Clients();
            clients.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BuildInQueries buildInQueres = new BuildInQueries();
            buildInQueres.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Payments payments = new Payments();
            this.Hide();
            payments.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Reports reports = new Reports();
            this.Hide();
            reports.Show();
        }

        List<string> GetDataFromQuery(string query)
        {
            List<string> emails = new List<string>();
            using (SqlConnection connection = new SqlConnection(
               ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                   emails.Add(reader.GetValue(0).ToString());
                }
            }
            return emails;
        }
        void SendLetter(string recipient, string name)
        {
            MailAddress from = new MailAddress("bankmailfortask@gmail.com", "Банк");
            MailAddress to = new MailAddress(recipient);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Задолженность";
            m.Body = $"Уважаемый (ая) {name}!\n У вас есть задолженность по кредиту на {DateTime.Now.ToString("dd/MM/yyyy")}. Погасите ее в ближайшее время. В противном случае Вам будет начислен штраф, размер которого указан в вашем кредитном договоре.\nСпасибо!";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("bankmailfortask@gmail.com", "Ilyakhrip14");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

        void SendLettersToPeople(List<string> emails, List<string> names)
        {
            if (emails.Count == 0) return;
            for(int i = 0; i < emails.Count; i++)
            {
                SendLetter(emails[i], names[i]);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            string todayDate = dateTime.ToString("dd");
            string query1 = "SELECT [Электронная почта] From Payments INNER JOIN BankCredits ON Payments.[№_договора] = BankCredits.[№_Договора] Inner Join Clients on BankCredits.[№_паспорта] = Clients.[№_паспорта] Where [Дата следующего платежа]<convert(date, '" + DateTime.Now.ToString("dd/MM/yyyy") + "',104)";
            string query2 = "SELECT [ФИО] From Payments INNER JOIN BankCredits ON Payments.[№_договора] = BankCredits.[№_Договора] Inner Join Clients on BankCredits.[№_паспорта] = Clients.[№_паспорта] INNER JOIN PhonesBook ON Clients.[№_телефона] = PhonesBook.[Номер_телефона] Where [Дата следующего платежа]<convert(date, '" + DateTime.Now.ToString("dd/MM/yyyy") + "',104)";
            if (todayDate == "27")
            {
                try
                {
                    SendLettersToPeople(GetDataFromQuery(query1), GetDataFromQuery(query2));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при отправке!");
                }
                MessageBox.Show("Письма отправлены успешно!");
            }
            else MessageBox.Show("Запрещено!");
        }
    }
}
