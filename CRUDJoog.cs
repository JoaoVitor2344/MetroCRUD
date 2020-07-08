using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetroCRUD
{
    public partial class CRUDJoog : MetroFramework.Forms.MetroForm
    {
        SqlConnection con = new SqlConnection("Data Source=aula2020.database.windows.net;Initial Catalog=Joao;User ID=tds02;Password=@nuvem2020;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public void CarregaDgvJogo()
        {
            String query = "SELECT * FROM Jogos";
            SqlCommand cmd = new SqlCommand(query, con);
            if (con.State == ConnectionState.Open) { con.Close(); }
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable jogo = new DataTable();
            da.Fill(jogo);
            dgvJogos.DataSource = jogo;
            con.Close();
        }

        public void CarregaCbxEstilo()
        {
            String car = "SELECT Id, estilo FROM Estilos";
            SqlCommand cmd = new SqlCommand(car, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(car, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "estilo");
            cbxEstilo.ValueMember = "Id";
            cbxEstilo.DisplayMember = "estilo";
            cbxEstilo.DataSource = ds.Tables["estilo"];
            con.Close();
        }

        public CRUDJoog()
        {
            InitializeComponent();
            CarregaDgvJogo();
            CarregaCbxEstilo();
        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Localizar";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", this.txtId.Text);
                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    txtId.Text = rd["Id"].ToString();
                    txtNome.Text = rd["Nome"].ToString();
                    cbxEstilo.Text = rd["Estilo"].ToString();
                    txtCriador.Text = rd["Criador"].ToString();
                    txtPreço.Text = rd["Preço"].ToString();
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Nenhum registro encontrado!", "Sem registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
            catch(Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "Inserir";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@estilo", cbxEstilo.Text);
                cmd.Parameters.AddWithValue("@criador", txtCriador.Text);
                cmd.Parameters.AddWithValue("@preco", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPreço.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Cadastro Realizado com sucesso!", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregaDgvJogo();
            }
            catch (Exception er)
            {

                MessageBox.Show(er.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "Atualizar";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = Convert.ToInt32(txtId.Text);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@estilo", cbxEstilo.Text);
            cmd.Parameters.AddWithValue("@criador", txtCriador.Text);
            cmd.Parameters.AddWithValue("@preço", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPreço.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            CarregaDgvJogo();
            MessageBox.Show("Registro atualizado com sucesso!", "Atualizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();
            txtId.Text = "";
            txtNome.Text = "";
            cbxEstilo.Text = "";
            txtCriador.Text = "";
            txtPreço.Text = "";
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "Deletar";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", this.txtId.Text);
            con.Open();
            cmd.ExecuteNonQuery();
            CarregaDgvJogo();
            MessageBox.Show("Registro apagado com sucesso!", "Excluir", MessageBoxButtons.OK, MessageBoxIcon.Information);
            con.Close();
            txtId.Text = "";
            txtNome.Text = "";
            cbxEstilo.Text = "";
            txtCriador.Text = "";
            txtPreço.Text = "";
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvJogos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvJogos.Rows[e.RowIndex];
                txtId.Text = row.Cells[0].Value.ToString();
                txtNome.Text = row.Cells[1].Value.ToString();
                cbxEstilo.Text = row.Cells[2].Value.ToString();
                txtCriador.Text = row.Cells[3].Value.ToString();
                txtPreço.Text = row.Cells[4].Value.ToString();
            }
        }
    }
}
