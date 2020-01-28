using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LancTrans.Forms
{
    public partial class frmUsuarios : Form
    {
        int ID = 0;
        string comando = @"SELECT * FROM USUARIOS WHERE LOGIN <> 'master'";

        public frmUsuarios()
        {
            InitializeComponent();
            DB.Comandos.Atualizar(comando, grdUsuarios);
            grdUsuarios.Columns["CODIGO"].Visible = false;
            grdUsuarios.Columns["SENHA"].Visible = false;
            grdUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (edtNome.Text != ""&& edtLogin.Text != ""&& edtSenha.Text != "")
            {

                DB.Comandos.ExecutarComando("insert into usuarios(nome, login, senha) values(" + string.Format("'{0}'", edtNome.Text) + "," + string.Format("'{0}'", edtLogin.Text) + "," + string.Format("'{0}'", edtSenha.Text) + ")");
                MessageBox.Show("Registro inserido com sucesso.");
                DB.Comandos.Atualizar(comando, grdUsuarios);
                ClearData();
            }
            else
            {
                MessageBox.Show("Favor preecher o nome, login e senha!");
            }
        }
        private void ClearData()
        {
            edtNome.Text = "";
            edtLogin.Text = "";
            edtSenha.Text = "";
            ID = 0;
            btnInserir.Enabled = true;
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (edtNome.Text != "")
            {
                DB.Comandos.ExecutarComando("update usuarios set nome = " + string.Format("'{0}'", edtNome.Text) + ", login = " + string.Format("'{0}'", edtLogin.Text) + ", senha = " + string.Format("'{0}'", edtSenha.Text) + " where codigo=" + ID);
                MessageBox.Show("Registro alterado com sucesso.");
                DB.Comandos.Atualizar(comando, grdUsuarios);
                ClearData();
            }
            else
            {
                MessageBox.Show("Favor clicar duas vezes sobre o usuário a ser editado!");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (edtNome.Text != "" && ID != 0)
            {
                if (MessageBox.Show("Tem certeza que deseja excluir o usuário " + edtNome.Text + "?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DB.Comandos.ExecutarComando("delete from usuarios where codigo=" + ID);
                    MessageBox.Show("Registro excluído com sucesso.");
                    DB.Comandos.Atualizar(comando, grdUsuarios);
                    ClearData();
                }
            }
            else
            {
                MessageBox.Show("Favor clicar duas vezes sobre o usuário a ser excluido!");
            }
        }

        private void grdContas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ID = Convert.ToInt32(grdUsuarios.Rows[e.RowIndex].Cells[0].Value.ToString());
                edtNome.Text = grdUsuarios.Rows[e.RowIndex].Cells[1].Value.ToString();
                edtLogin.Text = grdUsuarios.Rows[e.RowIndex].Cells[2].Value.ToString();
                edtSenha.Text = grdUsuarios.Rows[e.RowIndex].Cells[3].Value.ToString();
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

        private void frmUsuarios_Load(object sender, EventArgs e)
        {

        }
    }
}
