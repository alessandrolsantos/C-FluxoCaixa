using System;
using System.Windows.Forms;

namespace LancTrans.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        bool loginValido = false;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string comando = @"select nome from usuarios where login = " + string.Format("'{0}'", edtLogin.Text) + " and senha  = " + string.Format("'{0}'", edtSenha.Text);
            var usuario = DB.Comandos.ConsultaValor(comando);

            if (DateTime.Now.Date > Convert.ToDateTime("2017-11-01"))
            {
                MessageBox.Show("Licença expirada, contate o desenvolvedor!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }


            if (usuario.Rows.Count > 0)
            {
                loginValido = true;

                this.Hide();
                frmMenu fMenu = new frmMenu(usuario.Rows[0]["NOME"].ToString());
                fMenu.ShowDialog();
                this.Close();


            }
            else
            {
                MessageBox.Show("Senha ou usuário inválidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!loginValido)
            {
                Application.Exit();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
