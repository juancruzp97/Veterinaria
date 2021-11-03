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

namespace VeterinariaFrontend
{
    public partial class FrmConsultaMascota : Form
    {
        private IGestorVeterinaria servicio;
        Mascota oMascota;
        public FrmConsultaMascota()
        {
            InitializeComponent();
            oMascota = new Mascota();
            servicio = new FactoryVeterinaria().CrearGestor();
        }

        private async void FrmConsultaMascota_Load(object sender, EventArgs e)
        {
            Habilitar(false);
            cboMascota.SelectedIndex = -1;
            cboMascota.DropDownStyle = ComboBoxStyle.DropDownList;
            txtNombre.Focus();
            await CargarComboCliente();
            
        }



        //BOTONES
        private void btnEditar_Click(object sender, EventArgs e)
        {
            HabilitarOtro(false);
            Habilitar(true);
        }

        private async void btnConsultar_Click(object sender, EventArgs e)
        {
            //int cod = cboCliente.SelectedIndex;
            dgvAtencion.Rows.Clear();
            int numero = await ConsultarIdMascota();
            List<Atencion> aten = await ConsultarAtencion(numero);
            List<int> detalles = await ConsultarDetAtencion(numero);
            int j = 0;
            for (int i = 0; i < aten.Count; i++)
            {
                
                dgvAtencion.Rows.Add(new object[] {detalles[j],aten[j].Fecha, aten[j].Descripcion, aten[j].Importe });
                j++;
            }
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
        private async void btnAceptar_ClickAsync(object sender, EventArgs e)
        {
            oMascota.Nombre = txtNombre.Text;
            oMascota.Edad = Convert.ToInt32(txtEdad.Text);
            oMascota.TipoMascota = cboTipo.SelectedIndex + 1;

            bool up = await UpdateMascota(oMascota);
            if (up == true)
            {
                MessageBox.Show("Mascota Actualizada!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Habilitar(false);
                HabilitarOtro(true);
                return;

            }
            else
            {
                MessageBox.Show("Problemas al Actualizar Mascota!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Habilitar(false);
                HabilitarOtro(true);
                return;
            }


        }


        //EVENTOS

        private async void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!cb.Focused)
            {
                return;
            }
            else
            {
                dgvAtencion.Rows.Clear();
                int cbo = cboCliente.SelectedIndex + 1;
                await CargarComboMascota(cbo);
            }
        }

        private async void cboMascota_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.Focused == false)
            {
                return;
            }
            else
            {
                dgvAtencion.Rows.Clear();
                int id = cboCliente.SelectedIndex + 1;
                oMascota = (Mascota)cboMascota.SelectedItem;
                int masc = oMascota.CodigoMascota;

                txtNombre.Text = oMascota.Nombre;
                txtEdad.Text = oMascota.Edad.ToString();
                cboTipo.SelectedIndex = oMascota.TipoMascota - 1;

                List<Atencion> lista = await ConsultarAtencion(masc);
                await CargarDgvAsync(lista, masc);
            }
        }

        private async void dgvAtencion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAtencion.CurrentCell.ColumnIndex == 4)
            {
                int cliente = cboCliente.SelectedIndex + 1;
                string mascota = cboMascota.Text;
                int det = Convert.ToInt32(dgvAtencion.CurrentRow.Cells[0].Value);
                int idMascota = await ConsultarIdMascota();
                bool compare = await DeleteDetalle(idMascota, det);
                if (compare == true)
                {
                    dgvAtencion.Rows.Remove(dgvAtencion.CurrentRow);
                    MessageBox.Show("Detalle Borrado", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //dgvAtencion.Rows.Clear();
                }

            }
        }

        //API
        private async Task<bool> DeleteDetalle(int idMascota, int det)
        {
            string url = "https://localhost:44310/api/Veterinaria/DeleteDetalle" + "/" + idMascota.ToString() + "/" + det.ToString();
            HttpClient cliente = new HttpClient();
            var result = await cliente.DeleteAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<bool>(content);
            //result.IsSuccess = content.IsSuccessStatusCode;
            //result.ResultJson = res;
            return res;
        }
        private async Task<List<Atencion>> ConsultarAtencion(int numero)
        {
            string url = "https://localhost:44310/api/Atencion/GetAtencion" + "/" + numero.ToString();
            HttpClient cliente = new HttpClient();
            var result = await cliente.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            List<Atencion> lista = JsonConvert.DeserializeObject<List<Atencion>>(content);
            return lista;
        }
        private async Task<List<int>> ConsultarDetAtencion(int numero)
        {
            string url = "https://localhost:44310/api/Atencion/GetDetalleAtencion" + "/" + numero.ToString();
            HttpClient cliente = new HttpClient();
            var resultado = await cliente.GetAsync(url);
            var content = await resultado.Content.ReadAsStringAsync();
            List<int> retorno = JsonConvert.DeserializeObject<List<int>>(content);

            return retorno;

        }
        private async Task<int> ConsultarIdMascota()
        {
            int cod = cboCliente.SelectedIndex + 1;
            //string idC =Convert.ToString( cboCliente.SelectedIndex + 1);
            string masc = cboMascota.Text;
            string url = "https://localhost:44310/api/Veterinaria/GetIdMascota" + "/" + cod.ToString() + "/" + masc;
            HttpClient cliente = new HttpClient();
            var resultado = await cliente.GetAsync(url);
            var content = await resultado.Content.ReadAsStringAsync();
            int nro = JsonConvert.DeserializeObject<int>(content);
            return nro;
        }
        private async Task CargarDgvAsync(List<Atencion> lista, int masc)
        {
            List<int> det = await ConsultarDetAtencion(masc);
            int j = 0;
            for (int i = 0; i < lista.Count; i++)
            {
                dgvAtencion.Rows.Add(new object[] { det[j], lista[j].Fecha, lista[j].Descripcion, lista[j].Importe });
                j++;
            }
        }
        private async Task<bool> UpdateMascota(Mascota mascota)
        {
            string url = "https://localhost:44310/api/Mascota/UpdateMascota";
            HttpClient cliente = new HttpClient();
            string data = JsonConvert.SerializeObject(mascota);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var resultado = await cliente.PutAsync(url, content);
            bool succes = resultado.IsSuccessStatusCode;

            return succes;
        }

        //METODOS

        private async Task CargarComboMascota(int cbo)
        {
            string url = "https://localhost:44310/api/Mascota/ConsultarMascota" + "/" + cbo.ToString();
            HttpClient cliente = new HttpClient();
            var result = await cliente.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            List<Mascota> lst = JsonConvert.DeserializeObject<List<Mascota>>(content);

            cboMascota.DataSource = lst;
            cboMascota.ValueMember = "CodigoMascota";
            cboMascota.DisplayMember = "Nombre";
            cboMascota.SelectedIndex = -1;
            cboMascota.DropDownStyle = ComboBoxStyle.DropDownList;
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

        public void Habilitar(bool x)
        {
            txtNombre.Enabled = x;
            txtEdad.Enabled = x;
            cboTipo.Enabled = x;

        }
        public void HabilitarOtro(bool x)
        {
            cboCliente.Enabled = x;
            cboMascota.Enabled = x;
            dgvAtencion.Enabled = x;
        }

        

       
    }
}
