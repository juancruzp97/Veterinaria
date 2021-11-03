
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
using VeterinariaBackend.Negocio;
using VeterinariaFrontend.Cliente;

namespace VeterinariaFrontend
{
    public partial class FrmAgregarMascota : Form
    {
        Mascota oMascota;
        //Clientes oCliente;
        private IGestorVeterinaria _gestor;
        public FrmAgregarMascota()
        {
            InitializeComponent();
            oMascota = new Mascota();
            _gestor = new FactoryVeterinaria().CrearGestor();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            await CargarComboCliente();
        }

        private async Task CargarComboCliente()
        {
            string url = "https://localhost:44310/api/Veterinaria/ConsultarCliente";
            HttpClient cliente = new HttpClient();
            var result = await cliente.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            List<Clientes> lst = JsonConvert.DeserializeObject<List<Clientes>>(content);

            cboCliente.DataSource = lst;
            cboCliente.ValueMember = "Codigo";
            cboCliente.DisplayMember = "Nombre";
            cboCliente.SelectedIndex = -1;
            cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
        }



        //private async void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    await CargarMascota();

        //}

        //private async Task CargarMascota()
        //{
        //    int cbo = cboCliente.SelectedIndex + 1;
        //    string url = "https://localhost:44310/api/Veterinaria/ConsultarMascota" + "/" + cbo.ToString();
        //    HttpClient cliente = new HttpClient();
        //    var result = await cliente.GetAsync(url);
        //    var content = await result.Content.ReadAsStringAsync();
        //    List<Mascota> lst = JsonConvert.DeserializeObject<List<Mascota>>(content);

        //    cboMascota.DataSource = lst;
        //    cboMascota.ValueMember = "CodigoMascota";
        //    cboMascota.DisplayMember = "Nombre";
        //    cboMascota.SelectedIndex = -1;
        //    cboMascota.DropDownStyle = ComboBoxStyle.DropDownList;
        //}

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int id = dgvAtencion.Rows.Count + 1;
            Atencion oAtencion = new Atencion();
            //oMascota = (Mascota)cboMascota.SelectedItem;
            oAtencion.Descripcion = txtDescripcion.Text.ToString();
            oAtencion.Fecha = dtPicker.Value;
            oAtencion.Importe = Convert.ToDouble(txtImporte.Text.ToString());

            oMascota.AgregarAtencion(oAtencion);

            dgvAtencion.Rows.Add(new object []{ id, oAtencion.Fecha, oAtencion.Descripcion, oAtencion.Importe });

        }

        private async void btnAceptar_Click(object sender, EventArgs e)
        {

            oMascota.Nombre = txtNombre.Text;
            oMascota.Edad = Convert.ToInt32(txtEdad.Text);
            oMascota.TipoMascota = cboTipo.SelectedIndex + 1;
            int id = cboCliente.SelectedIndex + 1;

            string data = JsonConvert.SerializeObject(oMascota);

            bool succes = await GrabarMascotaAsync(data, id);
            if (succes)
            {
                MessageBox.Show("Mascota registrada con éxito!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un inconveniente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            cboCliente.SelectedIndex = -1;
            cboTipo.SelectedIndex = -1;
            dtPicker.Value = DateTime.Today;
            txtImporte.Text = string.Empty;
            dgvAtencion.Rows.Clear();
        }

        private async Task<bool> GrabarMascotaAsync(string data, int id)
        {
            string url = "https://localhost:44310/api/Veterinaria/AgregarMascota"+"/"+id.ToString();
            using(HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(data, Encoding.UTF8,"application/json");
                var result = await client.PostAsync(url, content);
                string response = await result.Content.ReadAsStringAsync();
                return response.Equals("Ok");
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("¿Esta seguro que desea cancelar?", "Precaución", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            cboCliente.SelectedIndex = -1;
            cboTipo.SelectedIndex = -1;
            txtNombre.Text = string.Empty;
            txtEdad.Text = string.Empty;
            dtPicker.Value = DateTime.Today;
            txtDescripcion.Text = string.Empty;
            txtImporte.Text = string.Empty;
            dgvAtencion.Rows.Clear();
        }
    }
}
