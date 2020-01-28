using iTextSharp.text.pdf;
using LancTrans.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LancTrans.Forms
{
    public partial class frmLancamentos : Form
    {

        private string comando = "SELECT CODIGO, DESCRICAO FROM CONTAS";
        int ID = 0;
        bool telaCarregada = false;

        string comandoLancamentos;
        List<Lancamentos> listaLancamentos = new List<Lancamentos>();

        public frmLancamentos()
        {
            InitializeComponent();

            ClearData();

            edtValor.Text = valor.ToString("C");
            CarregarCombo();
            CarregarGrid();

            formatarGrid();
            telaCarregada = true;

        }

        private void formatarGrid()
        {
            grdLancamentos.Columns["DATA_HORA"].HeaderText = "Data de lançamento";
            grdLancamentos.Columns["DESCRICAO"].HeaderText = "Descrição";
            grdLancamentos.Columns["CREDITO"].HeaderText = "Entrada/Crédito";
            grdLancamentos.Columns["DEBITO"].HeaderText = "Saída/ Débito";
            grdLancamentos.Columns["SALDO"].HeaderText = "Saldo";

            grdLancamentos.Columns["CREDITO"].DefaultCellStyle.Format = "C";
            grdLancamentos.Columns["DEBITO"].DefaultCellStyle.Format = "C";
            grdLancamentos.Columns["SALDO"].DefaultCellStyle.Format = "C";

            grdLancamentos.Columns["CREDITO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdLancamentos.Columns["DEBITO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grdLancamentos.Columns["SALDO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grdLancamentos.Columns["CODIGO"].Visible = false;
            grdLancamentos.Columns["TIPO"].Visible = false;
            grdLancamentos.Columns["CODIGO_CONTA"].Visible = false;
            grdLancamentos.Columns["VALOR"].Visible = false;

            grdLancamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void PopularSaldo()
        {
            decimal saldoAtual = 0;

            if (listaLancamentos.Count > 0)
                saldoAtual = (decimal)listaLancamentos[0].saldo;

            comando = @"
SELECT
  SC.SALDO,
  COALESCE(SUM(CASE WHEN L.TIPO = 'D' THEN L.VALOR  END), 0) AS DEBITO,
  COALESCE(SUM(CASE WHEN L.TIPO = 'C' THEN L.VALOR  END), 0) AS CREDITO
FROM
  LANCAMENTOS L
INNER JOIN
  SALDO_CONTAS SC ON SC.CODIGO_CONTA = L.CODIGO_CONTA
WHERE L.DATA_HORA >" + string.Format("'{0}'", dtDataIni.Value.ToString("yyyy-MM-dd")) + " AND L.CODIGO_CONTA = " + cmbContas.SelectedValue.ToString();

            var retorno = DB.Comandos.ConsultaValor(comando);

            decimal totalCredito = Convert.ToDecimal(retorno.Rows[0]["CREDITO"]);
            decimal totalDebito = Convert.ToDecimal(retorno.Rows[0]["DEBITO"]);
            decimal saldoAnterior = saldoAtual - totalCredito + totalDebito;

            List<Lancamentos> listaPopulada = new List<Lancamentos>();

            Lancamentos lanc = new Lancamentos();

            listaLancamentos.OrderBy(x => x.data_hora);

            for (int i = 0; i < listaLancamentos.Count; i++)
            {
                if (i == 0)
                {
                    lanc = new Lancamentos();
                    lanc.data_hora = dtDataIni.Value.Date;
                    lanc.descricao = "SALDO ANTERIOR";
                    lanc.saldo = saldoAnterior;
                    listaPopulada.Add(lanc);
                    saldoAnterior = lanc.saldo;
                }

                lanc = new Lancamentos();
                lanc.data_hora = listaLancamentos[i].data_hora;
                lanc.descricao = listaLancamentos[i].descricao;
                lanc.credito = listaLancamentos[i].credito;
                lanc.debito = listaLancamentos[i].debito;
                lanc.saldo = listaLancamentos[i].tipo.Equals("C") ? (decimal)(saldoAnterior + lanc.credito) : (decimal)(saldoAnterior - lanc.debito);

                lanc.credito = listaLancamentos[i].credito == 0 ? null : listaLancamentos[i].credito;
                lanc.debito = listaLancamentos[i].debito == 0 ? null : listaLancamentos[i].debito;

                lanc.codigo = listaLancamentos[i].codigo;
                lanc.codigo_conta = listaLancamentos[i].codigo_conta;
                lanc.tipo = listaLancamentos[i].tipo;
                lanc.valor = listaLancamentos[i].valor;
                saldoAnterior = lanc.saldo;
                listaPopulada.Add(lanc);

                if (i == listaLancamentos.Count() - 1)
                {
                    lanc = new Lancamentos();
                    lanc.data_hora = DateTime.Now.Date;
                    lanc.descricao = "SALDO ATUAL CONTA";
                    lanc.saldo = saldoAtual;
                    listaPopulada.Add(lanc);
                }

            }

            grdLancamentos.DataSource = null;
            grdLancamentos.DataSource = listaPopulada;
            grdLancamentos.Refresh();
            formatarGrid();
        }

        private void CarregarGrid()
        {
            comandoLancamentos = @"
SELECT
  L.CODIGO,
  L.DATA_HORA,
  L.DESCRICAO,
  L.VALOR,
  L.TIPO,
  L.CODIGO_CONTA,
  (SELECT L1.VALOR FROM LANCAMENTOS L1 WHERE L1.CODIGO = L.CODIGO AND L.TIPO = 'C') CREDITO,
  (SELECT L1.VALOR FROM LANCAMENTOS L1 WHERE L1.CODIGO = L.CODIGO AND L.TIPO = 'D') DEBITO,
  SC.SALDO
FROM
  LANCAMENTOS L
INNER JOIN
  SALDO_CONTAS SC ON SC.CODIGO_CONTA = L.CODIGO_CONTA
WHERE L.DATA_HORA BETWEEN" + string.Format("'{0}'", dtDataIni.Value.ToString("yyyy-MM-dd")) + " AND " + string.Format("'{0}'", dtDataFinal.Value.ToString("yyyy-MM-dd 23:59:59")) + " AND L.CODIGO_CONTA = " + cmbContas.SelectedValue.ToString();

            listaLancamentos = DB.Comandos.GetLancamentos(comandoLancamentos);
            PopularSaldo();
        }

        private void CarregarCombo()
        {
            DB.Comandos.Atualizar(comando, cmbContas);

            cmbContas.ValueMember = "CODIGO";
            cmbContas.DisplayMember = "DESCRICAO";
        }

        decimal valor;

        private void edtValor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (edtValor.Text != "")
                {
                    if (edtValor.Text.Equals("0"))
                    {
                        edtValor.Text = "";
                        valor = 0;
                    }
                    else
                        valor = decimal.Parse(edtValor.Text);
                }
                else
                    valor = 0;
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

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (edtDescricao.Text != "" && cmbContas.Text != "")
            {
                string tipo = rbCredito.Checked ? "'C'" : "'D'";

                edtValor.Text = valor.ToString("0.00", new System.Globalization.CultureInfo("pt-BR")).Replace(",", ".");
                comando = @"insert into lancamentos(data_hora, descricao, valor, tipo, codigo_conta) values("
                + string.Format("'{0}'", dtDataLancamento.Value.ToString("yyyy-MM-dd HH:mm:ss")) + ","
                + string.Format("'{0}'", edtDescricao.Text)
                + "," + edtValor.Text + ","
                + tipo + ","
                + cmbContas.SelectedValue.ToString() + ");";

                DB.Comandos.ExecutarComando(comando);

                comando = "update saldo_contas set saldo = saldo " + (rbCredito.Checked ? "+" : "-") + edtValor.Text + " where codigo_conta = " + cmbContas.SelectedValue.ToString();

                DB.Comandos.ExecutarComando(comando);

                valor = 0;
                CarregarGrid();
                ClearData();
            }
            else
            {
                MessageBox.Show("Favor preencher a descrição.");
            }

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PopularSaldo();
        }

        private void grdLancamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearData();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (edtDescricao.Text != "" && cmbContas.Text != "")
            {
                if (MessageBox.Show("Tem certeza que deseja alterar a conta " + edtDescricao.Text + "?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string descricao = string.Format("'{0}'", edtDescricao.Text);

                    string tipo = rbCredito.Checked ? "'C'" : "'D'";

                    string newValue = edtValor.Text;
                    string oldValue = grdLancamentos.CurrentRow.Cells["VALOR"].Value.ToString();

                    edtValor.Text = valor.ToString("0.00", new System.Globalization.CultureInfo("pt-BR")).Replace(",", ".");
                    comando = string.Format("update lancamentos set data_hora = {0}, descricao = {1}, valor = {2}, tipo = {3}, codigo_conta = {4} where codigo = " + ID,
                    string.Format("'{0}'", dtDataLancamento.Value.ToString("yyyy-MM-dd HH:mm:ss")),
                    string.Format("'{0}'", edtDescricao.Text),
                    edtValor.Text,
                    tipo,
                    cmbContas.SelectedValue.ToString());

                    DB.Comandos.ExecutarComando(comando);

                    bool tipoAlterado = grdLancamentos.CurrentRow.Cells["TIPO"].Value.ToString() != tipo.Replace("'", "");
                    bool valorAlterado = grdLancamentos.CurrentRow.Cells["VALOR"].Value.ToString() != newValue;

                    if (tipoAlterado || valorAlterado)
                    {
                        if (tipo == "'C'")
                        {
                            if (tipoAlterado)
                            {
                                comando = "update saldo_contas set saldo = saldo + " + oldValue + " + " + edtValor.Text.ToString().Replace(",", ".") + " where codigo_conta = " + cmbContas.SelectedValue.ToString();
                            }
                            else
                            {
                                comando = "update saldo_contas set saldo = saldo - " + oldValue + " + " + edtValor.Text.ToString().Replace(",", ".") + " where codigo_conta = " + cmbContas.SelectedValue.ToString();
                            }

                            DB.Comandos.ExecutarComando(comando);
                        }
                        else
                        {
                            if (tipoAlterado)
                            {
                                comando = "update saldo_contas set saldo = saldo - " + oldValue + " - " + edtValor.Text.ToString().Replace(",", ".") + " where codigo_conta = " + cmbContas.SelectedValue.ToString();
                            }
                            else
                            {
                                comando = "update saldo_contas set saldo = saldo + " + oldValue + " - " + edtValor.Text.ToString().Replace(",", ".") + " where codigo_conta = " + cmbContas.SelectedValue.ToString();
                            }

                            DB.Comandos.ExecutarComando(comando);
                        }
                    }

                    MessageBox.Show("Registro alterado com sucesso.");
                    CarregarGrid();
                    ClearData();
                }
                btnInserir.Enabled = true;
            }
            else
            {
                MessageBox.Show("Favor clicar duas vezes sobre a conta a ser alterada!");
            }
        }

        private void ClearData()
        {
            ID = 0;
            dtDataLancamento.Value = DateTime.Now;
            edtDescricao.Text = "";
            rbCredito.Checked = true;
            edtValor.Text = "0";
            btnInserir.Enabled = true;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (edtDescricao.Text != "" && ID != 0)
            {
                if (MessageBox.Show("Tem certeza que deseja excluir o lançamento " + edtDescricao.Text + "?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DB.Comandos.ExecutarComando("delete from lancamentos where codigo=" + ID);

                    DB.Comandos.ExecutarComando("update saldo_contas set saldo = saldo " + (rbDebito.Checked ? "+" : "-") + edtValor.Text.ToString().Replace(",", ".") + " where codigo_conta = " + cmbContas.SelectedValue.ToString());

                    MessageBox.Show("Registro excluído com sucesso.");
                    CarregarGrid();
                    ClearData();
                }
            }
            else
            {
                MessageBox.Show("Favor clicar duas vezes sobre a conta a ser excluida!");
            }
        }

        private void grdLancamentos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdLancamentos.CurrentRow.Cells["codigo"].Value.ToString());
                dtDataLancamento.Text = grdLancamentos.CurrentRow.Cells["data_hora"].Value.ToString();
                cmbContas.SelectedValue = grdLancamentos.CurrentRow.Cells["codigo_conta"].Value.ToString();
                edtDescricao.Text = grdLancamentos.CurrentRow.Cells["descricao"].Value.ToString();
                rbCredito.Checked = grdLancamentos.CurrentRow.Cells["tipo"].Value.ToString().Equals("C");
                rbDebito.Checked = grdLancamentos.CurrentRow.Cells["tipo"].Value.ToString().Equals("D");
                edtValor.Text = grdLancamentos.CurrentRow.Cells["valor"].Value.ToString();
                btnInserir.Enabled = false;
                btnAlterar.Enabled = true;
                btnExcluir.Enabled = true;
            }
            catch (Exception)
            {
                ClearData();
            }
        }

        private void grdLancamentos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value.Equals("SALDO ANTERIOR"))
            {
                DataGridViewRow row = grdLancamentos.Rows[e.RowIndex];
                row.DefaultCellStyle.BackColor = Color.Silver;
                row.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }

            if (e.ColumnIndex == 2 && e.Value.Equals("SALDO ATUAL CONTA"))
            {
                DataGridViewRow row = grdLancamentos.Rows[e.RowIndex];
                row.DefaultCellStyle.BackColor = Color.Silver;
                row.DefaultCellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //printPreviewDialog1.Document = printDocument1;
            //printPreviewDialog1.ShowDialog();
            CarregarGrid();
            SaveDataGridViewToPDF(grdLancamentos, Environment.CurrentDirectory + "\\Relatório\\");
        }

        public void SaveDataGridViewToPDF(DataGridView Dv, string FilePath)
        {
            string folderPath = Environment.CurrentDirectory + "\\Relatório\\"; //"C:\\PDFs\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            iTextSharp.text.FontFactory.RegisterDirectories();
            iTextSharp.text.Font myfont = iTextSharp.text.FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 15, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 0f);

            pdfDoc.Open();
            PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(FilePath + "Extrato da conta.pdf", FileMode.Create));
            pdfDoc.Open();
            PdfPTable _mytable = new PdfPTable(Dv.ColumnCount - 4);

            float[] widths = new float[] { 2.1f, 5f, 1.5f, 1.6f, 1.5f };
            _mytable.SetWidths(widths);

            _mytable.WidthPercentage = 90;

            //_mytable.DefaultCell.Border = Rectangle.NO_BORDER;

            _mytable.DefaultCell.BorderColor = new iTextSharp.text.BaseColor(System.Drawing.Color.White);




            iTextSharp.text.Paragraph ph = new iTextSharp.text.Paragraph("Extrato da conta ", myfont);
            ph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            pdfDoc.Add(ph);

            myfont = iTextSharp.text.FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            ph = new iTextSharp.text.Paragraph("Conta: " + cmbContas.Text, myfont);
            ph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            pdfDoc.Add(ph);

            // adiciona a linha em branco(enter) ao paragrafo
            ph.Add(new iTextSharp.text.Chunk("\n"));

            ph = new iTextSharp.text.Paragraph("Período: " + dtDataIni.Text + " até " + dtDataFinal.Text, myfont);
            pdfDoc.Add(ph);

            ph = new iTextSharp.text.Paragraph("Dada de emissão: " + DateTime.Now, myfont);
            pdfDoc.Add(ph);




            ph = new iTextSharp.text.Paragraph();

            // cria um objeto sepatador (um traço)
            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();

            // adiciona o separador ao paragravo
            ph.Add(seperator);

            // adiciona a linha em branco(enter) ao paragrafo
            ph.Add(new iTextSharp.text.Chunk("\n"));

            // imprime o pagagrafo no documento
            // pdfDoc.Add(ph);

            ph.Add(new iTextSharp.text.Chunk("\n"));

            pdfDoc.Add(ph);

            myfont = iTextSharp.text.FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 8, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            for (int j = 0; j < Dv.Columns.Count; ++j)
            {
                if (Dv.Columns[j].HeaderText != "codigo" && Dv.Columns[j].HeaderText != "tipo" && Dv.Columns[j].HeaderText != "codigo_conta" && Dv.Columns[j].HeaderText != "valor")
                {
                    iTextSharp.text.Phrase p = new iTextSharp.text.Phrase(Dv.Columns[j].HeaderText, myfont);
                    PdfPCell cell = new PdfPCell(p);
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                    cell.BorderWidth = 0;
                    _mytable.AddCell(cell);
                }
            }

            myfont = iTextSharp.text.FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            //-------------------------
            for (int i = 0; i < Dv.Rows.Count; ++i)
            {
                for (int j = 0; j < Dv.Columns.Count; ++j)
                {
                    if (Dv.Columns[j].HeaderText != "codigo" && Dv.Columns[j].HeaderText != "tipo" && Dv.Columns[j].HeaderText != "codigo_conta" && Dv.Columns[j].HeaderText != "valor")
                    {

                        iTextSharp.text.Phrase p = new iTextSharp.text.Phrase();// Dv.Rows[i].Cells[j].Value == null ? null : Dv.Rows[i].Cells[j].Value.ToString(), myfont);
                        PdfPCell cell = new PdfPCell();

                        if (Dv.Columns[j].HeaderText == "Saldo" || Dv.Columns[j].HeaderText == "Saída/ Débito" || Dv.Columns[j].HeaderText == "Entrada/Crédito")
                        {
                            myfont = iTextSharp.text.FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                            if (Dv.Columns[j].HeaderText == "Saída/ Débito")
                                myfont = iTextSharp.text.FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
                            else
                            if (Dv.Columns[j].HeaderText == "Entrada/Crédito")
                                myfont = iTextSharp.text.FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLUE);
                            else
                            if (Dv.Columns[j].HeaderText == "Saldo")
                            {
                                var saldo = Dv.Rows[i].Cells[j].Value == null ? 0 : Convert.ToDouble(Dv.Rows[i].Cells[j].Value);
                                if (saldo < 0 && i == Dv.Rows.Count - 1)
                                    myfont = iTextSharp.text.FontFactory.GetFont("Tahoma", BaseFont.IDENTITY_H, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.RED);
                            }

                            p = new iTextSharp.text.Phrase(Dv.Rows[i].Cells[j].Value == null ? null : Convert.ToDecimal(Dv.Rows[i].Cells[j].Value.ToString()).ToString("N2"), myfont);
                            cell = new PdfPCell(p);
                            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;

                            if (i == 0 || i == Dv.Rows.Count - 1)
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.Silver);

                        }
                        else
                        if (Dv.Columns[j].HeaderText == "Data de lançamento" && Dv.Rows[i].Cells[j + 1].Value.ToString() == "SALDO ANTERIOR" || Dv.Rows[i].Cells[j + 1].Value.ToString() == "SALDO ATUAL CONTA")
                        {
                            p = new iTextSharp.text.Phrase(Dv.Rows[i].Cells[j].Value == null ? null : Convert.ToDateTime(Dv.Rows[i].Cells[j].Value.ToString()).ToString("dd/MM/yyyy"), myfont);
                            cell = new PdfPCell(p);
                            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                            if (i == 0 || i == Dv.Rows.Count - 1)
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.Silver);
                        }
                        else
                        {
                            p = new iTextSharp.text.Phrase(Dv.Rows[i].Cells[j].Value == null ? null : Dv.Rows[i].Cells[j].Value.ToString(), myfont);
                            cell = new PdfPCell(p);
                            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;

                            if (i == 0 || i == Dv.Rows.Count - 1)
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.Silver);
                        }

                        cell.BorderWidth = 0;

                        // _mytable.DefaultCell.BorderWidth = 0;

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
            Bitmap bm = new Bitmap(this.grdLancamentos.Width, this.grdLancamentos.Height);

            //Font fonte = new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel);
            //e.Graphics.DrawString("teste", fonte, Brushes.Black, 20, 150);

            grdLancamentos.DrawToBitmap(bm, new Rectangle(0, 0, this.grdLancamentos.Width, this.grdLancamentos.Height));
            e.Graphics.DrawImage(bm, 0, 0);
        }

        private void cmbContas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (telaCarregada)
                CarregarGrid();
        }
    }
}
