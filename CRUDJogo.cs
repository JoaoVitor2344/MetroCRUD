using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MetroCRUD
{
    public partial class CRUDJogo : MetroFramework.Forms.MetroForm
    {
        SqlConnection con = new SqlConnection("");

        public CRUDJogo()
        {
            InitializeComponent();
        }

        public void CarregaDgvCliente()
        {
            String query = "SELECT * FROM Cliente";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable cliente = new DataTable();
            da.Fill(cliente);
            MetroDgv.DataSource = cliente;
            con.Close();
        }

        private void MbtnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MbtnCadastrar_Click(object sender, EventArgs e)
        {

        }
    }
}
