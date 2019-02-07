using System;
using System.IO;
using System.Windows.Forms;
using ExcelObj = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Bank
{
    public partial class Reports : Form
    {
        DateTime dateTime = DateTime.Today;
        public Reports()
        {
            InitializeComponent();
        }

        private void Reports_Load(object sender, EventArgs e)
        {
        }
        void SaveTableForReport1(DataGridView Table, String Year)
        {
            string fileName = String.Empty;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xls files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
            }
            else
                return;

            ExcelObj.Application excelapp = new ExcelObj.Application();
            ExcelObj.Workbook workbook = excelapp.Workbooks.Add();
            ExcelObj.Worksheet worksheet = workbook.ActiveSheet;

            ExcelObj.Range heading = worksheet.Range[worksheet.Cells[1,1], worksheet.Cells[1,5]];
            heading.Merge(true);
            heading.Value = "Количество денег, выданых в качестве кредитов";

            worksheet.Rows[2].Columns[1] = "Год:";
            worksheet.Rows[2].Columns[2] = Year;

            worksheet.Rows[3].Columns[1] = "Сумма:";
            if (Table.Rows[0].Cells[0].Value.ToString().Length == 0)
                worksheet.Rows[3].Columns[2].Value = "0 грн";
            else worksheet.Rows[3].Columns[2].Value = ""+Math.Floor(Convert.ToDecimal(Table.Rows[0].Cells[0].Value))+" грн";

            worksheet.Rows[4].Columns[1] = "Дата:";
            worksheet.Rows[4].Columns[2] = dateTime.ToString("dd.MM.yyyy");

            //style
            heading.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            heading.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            worksheet.Cells.Style.HorizontalAlignment = ExcelObj.XlHAlign.xlHAlignCenter;
            worksheet.Columns.ColumnWidth = 12;
            worksheet.Cells.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            worksheet.Cells[2, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
            worksheet.Cells[2, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
            worksheet.Cells[3, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
            worksheet.Rows[3].Columns[2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
            worksheet.Cells[4, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
            worksheet.Cells[4, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
            ExcelObj.Range styleRange = worksheet.Range[worksheet.Cells[2, 3], worksheet.Cells[2, 5]];
            styleRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
            ExcelObj.Range styleRangeDate = worksheet.Range[worksheet.Cells[4, 3], worksheet.Cells[4, 5]];
            styleRangeDate.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
            ExcelObj.Range styleRangeMoney = worksheet.Range[worksheet.Cells[3, 3], worksheet.Cells[3, 5]];
            styleRangeMoney.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);

            excelapp.AlertBeforeOverwriting = false;
            workbook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            saveFileDialog1.Dispose();
            excelapp.Quit();
        }
        void SaveTableForReport2(DataGridView Table, String Date1, String Date2)
        {
            string fileName = String.Empty;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xls files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog1.FileName;
            }
            else
                return;

            ExcelObj.Application excelapp = new ExcelObj.Application();
            ExcelObj.Workbook workbook = excelapp.Workbooks.Add();
            ExcelObj.Worksheet worksheet = workbook.ActiveSheet;

            ExcelObj.Range heading = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 4]];
            heading.Merge(true);
            heading.Value = "Количество новых владельцев карт";

            worksheet.Rows[2].Columns[1] = "Период:";
            worksheet.Rows[2].Columns[2] = "с "+Date1;
            worksheet.Rows[2].Columns[3] = "по " + Date2;

            worksheet.Rows[3].Columns[1] = "Количество:";
            if (Table.Rows[0].Cells[0].Value.ToString().Length == 0)
                worksheet.Rows[3].Columns[2] = "0";
            else worksheet.Rows[3].Columns[2] = "" + Table.Rows[0].Cells[0].Value.ToString() + " человек";

            worksheet.Rows[4].Columns[1] = "Дата:";
            worksheet.Rows[4].Columns[2] = dateTime.ToString("dd.MM.yyyy");

            //style
            heading.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            heading.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
            worksheet.Cells.Style.HorizontalAlignment = ExcelObj.XlHAlign.xlHAlignCenter;
            worksheet.Cells.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            worksheet.Columns.ColumnWidth = 12;
            worksheet.Cells[2, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            worksheet.Cells[2, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSeaGreen);
            worksheet.Cells[2, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSeaGreen);
            worksheet.Cells[3, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            worksheet.Rows[3].Columns[2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSeaGreen);
            worksheet.Cells[4, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            worksheet.Cells[4, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSeaGreen);
            worksheet.Cells[2, 4].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSeaGreen);
            ExcelObj.Range styleRangeDate = worksheet.Range[worksheet.Cells[4, 3], worksheet.Cells[4, 4]];
            styleRangeDate.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSeaGreen);
            ExcelObj.Range styleRangeMoney = worksheet.Range[worksheet.Cells[3, 3], worksheet.Cells[3, 4]];
            styleRangeMoney.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSeaGreen);

            excelapp.AlertBeforeOverwriting = false;
            workbook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            saveFileDialog1.Dispose();
            excelapp.Quit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string year = textBox1.Text;
            int Num;
            if(year.Length == 0 || !int.TryParse(year, out Num))
            {
                MessageBox.Show("Введите год!");
                return;
            }
            string query = "SELECT SUM([Разер кредита]) FROM BankCredits WHERE YEAR([Дата выдачи кредита])="+textBox1.Text;
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, query);
            SaveTableForReport1(dataGridView1, year);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string date1 = dateTimePicker1.Text;
            string date2 = dateTimePicker2.Text;
            if(DateTime.Parse(date1) > DateTime.Parse(date2))
            {
                MessageBox.Show("Первая дата больше чем вторая!");
                return;
            }
            string query = "SELECT COUNT(*) FROM BankCards WHERE [Дата оформления] BETWEEN CONVERT(date, '"+date1+"', 104) AND CONVERT(date, '"+date2+"', 104) ";
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, query);
            SaveTableForReport2(dataGridView1, date1, date2);
        }

        private void назадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 main = new Form1();
            main.Show();
            this.Close();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int Num;
            if (textBox2.Text.Length!=6 || !int.TryParse(textBox2.Text, out Num))
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [№_договора], [Расчётная дата], [ФИО] FROM BankCredits Inner join Clients on clients.[№_паспорта]=bankcredits.[№_паспорта] inner join PhonesBook on PhonesBook.[Номер_телефона]=Clients.[№_телефона] Where LEN([Статус кредита])=7 AND BankCredits.[№_договора]='" + textBox2.Text + "'");
            if(dataGridView1.Rows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Данные отсутствуют!");
            }
            else SaveFirstReport(dataGridView1);
        }

        void SaveFirstReport(DataGridView Table)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Word document|*.docx";
            saveFileDialog1.Title = "Save the Word Document";

            Word.Application winword = new Word.Application();
            winword.Visible = false;
            Word.Document document = winword.Documents.Add();

           
            foreach (Word.Section section in document.Sections)
            {
                Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.Size = 20;
                headerRange.Font.Bold = 2;
                headerRange.Text = "Справка о закрытии кредитного договора";
            }

            DateTime dateTime = DateTime.UtcNow.Date;
            document.Content.SetRange(10, 0);
            document.Content.Font.Name = "Times New Roman";
            document.Content.Font.Size = 14;
            document.Content.Bold = 1;
            document.Content.Text = $"\nНастоящим подтверждаем, что {dataGridView1.Rows[0].Cells[2].Value.ToString()} оформил(a) в банке НБ \"БАНК\" кредитный договор с номером {dataGridView1.Rows[0].Cells[0].Value}.\nПо состоянию на {dataGridView1.Rows[0].Cells[1].Value.ToString()} задолженность по кредиту отсутствует, кредитный договор закрыт.\nДата {dateTime.ToString("dd.MM.yyyy")}";

            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                string docName = saveFileDialog1.FileName;
                if (docName.Length > 0)
                {
                    object oDocName = (object)docName;
                    document.SaveAs(ref oDocName);
                    document.Close();
                    document = null;
                    winword.Quit();
                    winword = null;
                }
            }
        }

        void SaveSecondReport(DataGridView Table)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Word document|*.docx";
            saveFileDialog1.Title = "Save the Word Document";

            Word.Application winword = new Word.Application();
            winword.Visible = false;
            Word.Document document = winword.Documents.Add();


            foreach (Word.Section section in document.Sections)
            {
                Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.Size = 20;
                headerRange.Font.Bold = 2;
                headerRange.Text = "Справка о наличии счета в банке";
            }

            DateTime dateTime = DateTime.UtcNow.Date;
            document.Content.SetRange(10, 0);
            document.Content.Font.Name = "Times New Roman";
            document.Content.Font.Size = 14;
            document.Content.Bold = 1;
            document.Content.Text = $"\nНастоящим подтверждаем, что {dataGridView1.Rows[0].Cells[2].Value.ToString()} открыл(а) в банке НБ \"БАНК\" счет с номером {dataGridView1.Rows[0].Cells[0].Value} и размером {dataGridView1.Rows[0].Cells[3].Value.ToString()} гривен.\nПо состоянию на {dataGridView1.Rows[0].Cells[1].Value.ToString()} счет активен.\nДата {dateTime.ToString("dd.MM.yyyy")}";

            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                string docName = saveFileDialog1.FileName;
                if (docName.Length > 0)
                {
                    object oDocName = (object)docName;
                    document.SaveAs(ref oDocName);
                    document.Close();
                    document = null;
                    winword.Quit();
                    winword = null;
                }
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int Num;
            if (textBox3.Text.Length != 12)
            {
                MessageBox.Show("Неправильно введены данные!");
                return;
            }
            BuildInQueries.SQLQuery(bindingNavigator1, dataGridView1, "SELECT [№_счета], [Дата открытия], [ФИО], [Размер вклада] FROM BankDeposits Inner join Clients on clients.[№_паспорта]=bankdeposits.[№ паспорта] inner join PhonesBook on PhonesBook.[Номер_телефона]=Clients.[№_телефона] WHERE [№_счета]='"+textBox3.Text+"'");
            if (dataGridView1.Rows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Данные отсутствуют!");
            }
            else SaveSecondReport(dataGridView1);
        }
    }
}
