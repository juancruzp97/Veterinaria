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


namespace VeterinariaFrontend
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea salir?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                return;
            }
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        SqlConnection conexion = new SqlConnection("server=localhost ; database=db_Veterinaria ; INTEGRATED SECURITY = true");


        private void btnLogin_Click_1(object sender, EventArgs e)
        {

            conexion.Open();
            SqlCommand comando = new SqlCommand("SELECT USUARIO, CONTRASEÑA FROM LOGIN WHERE USUARIO = @vusuario AND CONTRASEÑA = @vcontraseña", conexion);
            comando.Parameters.AddWithValue("vusuario", txtUsuario.Text);
            comando.Parameters.AddWithValue("vcontraseña", txtContraseña.Text);

            SqlDataReader lector = comando.ExecuteReader();

            if (lector.Read())
            {
                conexion.Close();
                this.Hide();
                Form1 pantalla = new Form1();
                pantalla.Show();
            }
            else
            {
                conexion.Close();
                MessageBox.Show("Usuario y/o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUsuario.Clear();
                txtContraseña.Clear();
                txtUsuario.Focus();
            }

        }
    }
}
