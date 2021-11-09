using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeterinariaBackend.Dominio;

namespace VeterinariaFrontend
{
    public partial class FrmAltaCliente : Form
    {
        Clientes oCliente;
        public FrmAltaCliente()
        {
            InitializeComponent();
            oCliente = new Clientes();
        }

        private void FrmAltaCliente_Load(object sender, EventArgs e)
        {
            cboSexo.DropDownStyle = ComboBoxStyle.DropDownList;
            habilitar(false);
        }

        private void Nuevo_Click(object sender, EventArgs e)
        {
            habilitar(true);
            Limpiar();
            btnAgregarMascota.Enabled = false;
        }


        //METODOS
        private void habilitar(bool v)
        {
            txtCliente.Enabled = v;
            txtDireccion.Enabled = v;
            txtDocumento.Enabled = v;
            txtEdad.Enabled = v;
            txtTelefono.Enabled = v;
            cboSexo.Enabled = v;

        }
        private void Limpiar()
        {
            txtCliente.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtDocumento.Text = string.Empty;
            txtEdad.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            cboSexo.SelectedIndex = -1;

        }

        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            btnAgregarMascota.Enabled = true;
            oCliente.Nombre = txtCliente.Text;
            if(cboSexo.SelectedIndex == 0)
            {
                oCliente.Sexo = true;
            }
            else
            {
                oCliente.Sexo = false;
            }
            oCliente.Telefono = Convert.ToInt32(txtTelefono.Text);
            oCliente.Documento = Convert.ToInt32(txtDocumento.Text);
            oCliente.Direccion = txtDireccion.Text;

            if(oCliente != null)
            {
                bool test = await InsertarCliente(oCliente);

                if (test)
                {
                    MessageBox.Show("Cliente Agregado con Exito!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    habilitar(false);
                    return;
                }
                else
                {
                    MessageBox.Show("Error al agregar Cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                return;
            }
           

        }

        private async Task<bool> InsertarCliente(Clientes oCliente)
        {
            string url = "https://localhost:44310/api/Cliente/InsertarCliente";
            HttpClient cliente = new HttpClient();
            string data = JsonConvert.SerializeObject(oCliente);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var result = await cliente.PostAsync(url, content);
            bool check = true;
            if (result.IsSuccessStatusCode)
            {
                return check;
            }
            else
            {
                check = false;
                return check;
            }

        }

        private void btnAgregarMascota_Click(object sender, EventArgs e)
        {
            FrmAltaAtencion frmAltaAtencion = new FrmAltaAtencion();
            frmAltaAtencion.ShowDialog();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
            habilitar(false);
            Nuevo.Focus();
        }
    }
}
