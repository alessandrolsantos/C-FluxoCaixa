using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace LancTrans
{
    public partial class frmContas : Form
    {

        int ID = 0;
        string comando = @"SELECT * FROM CONTAS";

        public frmContas()
        {
            InitializeComponent();
            DB.Comandos.Atualizar(comando, grdContas);
            grdContas.Columns["CODIGO"].Visible = false;
            grdContas.Columns["DESCRICAO"].Width = 410;
            this.ActiveControl = edtDescricao;
            grdContas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (edtDescricao.Text != "")
            {
                string descricao = string.Format("'{0}'", edtDescricao.Text);

                DB.Comandos.ExecutarComando("insert into contas(descricao) values(" + descricao + ")");
                MessageBox.Show("Registro inserido com sucesso.");
                DB.Comandos.Atualizar(comando, grdContas);
                ClearData();
            }
            else
            {
                MessageBox.Show("Favor preecher a descrição!");
            }
        }

        private void ClearData()
        {
            edtDescricao.Text = "";
            ID = 0;
            btnInserir.Enabled = true;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (edtDescricao.Text != "")
            {
                string descricao = string.Format("'{0}'", edtDescricao.Text);

                DB.Comandos.ExecutarComando("update contas set descricao=" + descricao + " where codigo=" + ID);
                MessageBox.Show("Registro alterado com sucesso.");
                DB.Comandos.Atualizar(comando, grdContas);
                ClearData();
            }
            else
            {
                MessageBox.Show("Favor clicar duas vezes sobre a conta a ser editada!");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (edtDescricao.Text != "" && ID != 0)
            {
                if (MessageBox.Show("Tem certeza que deseja excluir a conta " + edtDescricao.Text + "?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DB.Comandos.ExecutarComando("delete from contas where codigo=" + ID);
                    MessageBox.Show("Registro excluído com sucesso.");
                    DB.Comandos.Atualizar(comando, grdContas);
                    ClearData();
                }
            }
            else
            {
                MessageBox.Show("Favor clicar duas vezes sobre a conta a ser excluida!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }



        private void printDocument1_PrintPage_1(object sender, PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.grdContas.Width, this.grdContas.Height);

            grdContas.DrawToBitmap(bm, new Rectangle(0, 0, this.grdContas.Width, this.grdContas.Height));
            e.Graphics.DrawImage(bm, 0, 0);
        }
        
        private void grdContas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdContas.Rows[e.RowIndex].Cells[0].Value.ToString());
                edtDescricao.Text = grdContas.Rows[e.RowIndex].Cells[1].Value.ToString();
                btnInserir.Enabled = false;
            }
            catch (Exception)
            {
                ClearData();
            }
        }

        private void grdContas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClearData();
        }

    }
}
