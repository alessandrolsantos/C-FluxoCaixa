using LancTrans.Forms;
using System;
using System.Configuration;
using System.Web.Configuration;
using System.Windows.Forms;

namespace LancTrans
{
    public partial class frmMenu : Form
    {
        public frmMenu(string usuario)
        {
            InitializeComponent();
            lblUsuario.Text += usuario;
            lblData.Text = DateTime.Now.ToLongDateString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmContas contas = new frmContas();
            contas.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmSaldoContas saldoContas = new frmSaldoContas();
            saldoContas.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.Date > Convert.ToDateTime("2017-11-01"))
            {
                MessageBox.Show("Licença expirada, contate o desenvolvedor!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            frmLancamentos lancamentos = new frmLancamentos();
            lancamentos.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmUsuarios usuarios = new frmUsuarios();
            usuarios.ShowDialog();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
