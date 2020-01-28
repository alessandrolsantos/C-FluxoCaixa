using iTextSharp.text.pdf;
using System;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;

namespace LancTrans
{
    public partial class frmSaldoContas : Form
    {
        int ID = 0;
        private string comando = @"
SELECT
  SC.CODIGO,
  C.DESCRICAO AS CONTA,
  COALESCE(SC.SALDO, 0) AS SALDO
FROM SALDO_CONTAS SC
LEFT JOIN CONTAS C ON C.CODIGO = SC.CODIGO_CONTA";

        public frmSaldoContas()
        {
            InitializeComponent();
            DB.Comandos.Atualizar(comando, grdSaldoContas);
            grdSaldoContas.Columns["SALDO"].DefaultCellStyle.Format = "C";
            grdSaldoContas.Columns["CODIGO"].Visible = false;
            grdSaldoContas.Columns["CONTA"].Width = 330;
            grdSaldoContas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        float valor;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                valor = float.Parse(edtValor.Text);
            }
            catch (Exception)
            {

            }
        }

        private void edtValor_Enter(object sender, EventArgs e)
        {
            edtValor.Text = valor.ToString();
        }

        private void edtValor_Leave(object sender, EventArgs e)
        {
            edtValor.Text = valor.ToString("C");
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (edtDescricao.Text != "")
            {
                edtValor.Text = valor.ToString("0.00", new System.Globalization.CultureInfo("pt-BR")).Replace(",", ".");
                DB.Comandos.ExecutarComando("update saldo_contas set saldo=" + edtValor.Text + " where codigo=" + ID);
                MessageBox.Show("Registro alterado com sucesso.");
                DB.Comandos.Atualizar(comando, grdSaldoContas);
                ClearData();
            }
            else
            {
                MessageBox.Show("Favor clicar duas vezes sobre o saldo a ser editado!");
            }
        }

        private void ClearData()
        {
            edtDescricao.Text = "";
            edtValor.Text = "";
            ID = 0;
        }

        private void grdSaldoContas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // printDocument1.Print();

            SaveDataGridViewToPDF(grdSaldoContas, "C:\\PDFs\\");
        }

        public static void SaveDataGridViewToPDF(DataGridView Dv, string FilePath)
        {
            FontFactory.RegisterDirectories();
            iTextSharp.text.Font myfont = FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            pdfDoc.Open();
            PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(FilePath + "DataGridViewExport.pdf", FileMode.Create));
            pdfDoc.Open();
            PdfPTable _mytable = new PdfPTable(Dv.ColumnCount -1);

            for (int j = 0; j < Dv.Columns.Count; ++j)
            {
                if (Dv.Columns[j].HeaderText != "CODIGO")
                {

                    Phrase p = new Phrase(Dv.Columns[j].HeaderText, myfont);
                    PdfPCell cell = new PdfPCell(p);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    _mytable.AddCell(cell);
                }
            }
            //-------------------------
            for (int i = 0; i < Dv.Rows.Count - 1; ++i)
            {
                for (int j = 0; j < Dv.Columns.Count; ++j)
                {
                    if (Dv.Columns[j].HeaderText != "CODIGO")
                    {

                        Phrase p = new Phrase(Dv.Rows[i].Cells[j].Value.ToString(), myfont);
                        PdfPCell cell = new PdfPCell(p);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                        _mytable.AddCell(cell);
                    }
                }
            }
            //------------------------           
            pdfDoc.Add(_mytable);
            pdfDoc.Close();
            System.Diagnostics.Process.Start(FilePath);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Bitmap bm = new Bitmap(this.grdSaldoContas.Width, this.grdSaldoContas.Height);
            //grdSaldoContas.DrawToBitmap(bm, new Rectangle(0, 0, this.grdSaldoContas.Width, this.grdSaldoContas.Height));
            //e.Graphics.DrawImage(bm, 0, 0);
        }

        private void grdSaldoContas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdSaldoContas.Rows[e.RowIndex].Cells[0].Value.ToString());
                edtDescricao.Text = grdSaldoContas.Rows[e.RowIndex].Cells[1].Value.ToString();
                edtValor.Text = grdSaldoContas.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
            catch (Exception)
            {
                ClearData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Creating iTextSharp Table from the DataTable data
            PdfPTable pdfTable = new PdfPTable(grdSaldoContas.ColumnCount);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 30;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row
            foreach (DataGridViewColumn column in grdSaldoContas.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdfTable.AddCell(cell);
            }

            //Adding DataRow
            foreach (DataGridViewRow row in grdSaldoContas.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {
                        pdfTable.AddCell(cell.Value.ToString());
                    }

                }
            }

            //Exporting to PDF
            string folderPath = "C:\\PDFs\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (FileStream stream = new FileStream(folderPath + "DataGridViewExport.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                pdfDoc.Add(pdfTable);
                pdfDoc.Close();
                stream.Close();
            }
        }
    }
}
