using LancTrans.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Z.EntityFramework.Extensions;

namespace LancTrans.DB
{
    public static class Comandos
    {
        public static SQLiteConnection con = new SQLiteConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString);
        public static SQLiteCommand cmd;
        public static SQLiteDataAdapter adapt;
        public static SQLiteDatabase sqLiteDatbase;
        public static int ID = 0;

        public static void ExecutarComando(string comanado)
        {
            try
            {
                cmd = new SQLiteCommand(comanado, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Não foi concluir a ação.");            
                e.ToString();
                throw;
            }
        }

        public static void Atualizar(string comando, DataGridView grid)
        {
            try
            {
                con.Open();
                DataTable dt = new DataTable();
                adapt = new SQLiteDataAdapter(comando, con);
                adapt.Fill(dt);
                grid.DataSource = dt;
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Não foi possível concluir a ação.");
                e.ToString();
                throw;
            }
        }

        public static DataTable ConsultaValor(string comando)
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SQLiteDataAdapter(comando, con);
            adapt.Fill(dt);
            con.Close();
            return dt;            
        }

        public static void Atualizar(string comando, ComboBox comboBox)
        {
            try
            {
                con.Open();
                DataTable dt = new DataTable();
                adapt = new SQLiteDataAdapter(comando, con);
                adapt.Fill(dt);
                comboBox.DataSource = dt;
                con.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Não foi possível concluir a ação.");
                e.ToString();
                throw;
            }
        }

        public static List<Lancamentos> GetLancamentos(string comando)
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SQLiteDataAdapter(comando, con);
            adapt.Fill(dt);

            List<Lancamentos> listaLancamentos = new List<Lancamentos>();


            foreach (DataRow item in dt.Rows)
            {
                Lancamentos lancamento = new Lancamentos();
                lancamento.tipo = item["TIPO"].ToString();
                lancamento.data_hora = Convert.ToDateTime(item["DATA_HORA"]);
                lancamento.descricao = item["DESCRICAO"].ToString();
                lancamento.debito = item["DEBITO"].ToString() == "" ? 0 : Convert.ToDecimal(item["DEBITO"]);
                lancamento.credito = item["CREDITO"].ToString() == "" ? 0 :item["CREDITO"] == null ? 0 : Convert.ToDecimal(item["CREDITO"]);
                lancamento.valor = Convert.ToDecimal(item["VALOR"]);
                lancamento.saldo = Convert.ToDecimal(item["SALDO"]);
                lancamento.codigo = Convert.ToInt16(item["CODIGO"]);
                lancamento.codigo_conta = Convert.ToInt16(item["CODIGO_CONTA"]);
                listaLancamentos.Add(lancamento);
            }
            con.Close();

            return listaLancamentos;
        }

    }
}
