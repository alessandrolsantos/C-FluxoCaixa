namespace LancTrans
{
    partial class frmSaldoContas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSaldoContas));
            this.btnAlterar = new System.Windows.Forms.Button();
            this.edtDescricao = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grdSaldoContas = new System.Windows.Forms.DataGridView();
            this.edtValor = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdSaldoContas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAlterar
            // 
            this.btnAlterar.Location = new System.Drawing.Point(416, 57);
            this.btnAlterar.Name = "btnAlterar";
            this.btnAlterar.Size = new System.Drawing.Size(75, 23);
            this.btnAlterar.TabIndex = 2;
            this.btnAlterar.Text = "&Alterar";
            this.btnAlterar.UseVisualStyleBackColor = true;
            this.btnAlterar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // edtDescricao
            // 
            this.edtDescricao.Location = new System.Drawing.Point(70, 25);
            this.edtDescricao.Multiline = true;
            this.edtDescricao.Name = "edtDescricao";
            this.edtDescricao.ReadOnly = true;
            this.edtDescricao.Size = new System.Drawing.Size(421, 20);
            this.edtDescricao.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Descrição";
            // 
            // grdSaldoContas
            // 
            this.grdSaldoContas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSaldoContas.Location = new System.Drawing.Point(12, 86);
            this.grdSaldoContas.Name = "grdSaldoContas";
            this.grdSaldoContas.RowTemplate.ReadOnly = true;
            this.grdSaldoContas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSaldoContas.Size = new System.Drawing.Size(479, 312);
            this.grdSaldoContas.TabIndex = 3;
            this.grdSaldoContas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSaldoContas_CellClick);
            this.grdSaldoContas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSaldoContas_CellDoubleClick);
            // 
            // edtValor
            // 
            this.edtValor.Location = new System.Drawing.Point(70, 57);
            this.edtValor.Name = "edtValor";
            this.edtValor.Size = new System.Drawing.Size(141, 20);
            this.edtValor.TabIndex = 1;
            this.edtValor.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.edtValor.Enter += new System.EventHandler(this.edtValor_Enter);
            this.edtValor.Leave += new System.EventHandler(this.edtValor_Leave);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(274, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(300, 106);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmSaldoContas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 410);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.edtValor);
            this.Controls.Add(this.btnAlterar);
            this.Controls.Add(this.edtDescricao);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grdSaldoContas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSaldoContas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Saldo das Contas";
            ((System.ComponentModel.ISupportInitialize)(this.grdSaldoContas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAlterar;
        private System.Windows.Forms.TextBox edtDescricao;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdSaldoContas;
        private System.Windows.Forms.TextBox edtValor;
        private System.Windows.Forms.Button button1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.Button button2;
    }
}