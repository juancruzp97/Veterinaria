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
        public FrmConsultaMascota()
        {
            InitializeComponent();
            servicio = new FactoryVeterinaria().CrearGestor();
        }

        private async void FrmConsultaMascota_Load(object sender, EventArgs e)
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

        private async void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!cb.Focused)
            {
                return;
            }
            else
            {
                await CargarComboMascota();
            }
        }


        private async Task CargarComboMascota()
        {
            int cbo = cboCliente.SelectedIndex + 1;
            string url = "https://localhost:44310/api/Veterinaria/ConsultarMascota" + "/" + cbo.ToString();
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

        private async Task<List<int>> ConsultarDetAtencion(int numero)
        {
            string url = "https://localhost:44310/api/Veterinaria/GetDetalleAtencion" + "/" + numero.ToString();
            HttpClient cliente = new HttpClient();
            var resultado = await cliente.GetAsync(url);
            var content = await resultado.Content.ReadAsStringAsync();
            List<int> retorno = JsonConvert.DeserializeObject<List<int>>(content);

            return retorno;

        }

        private async Task<int> ConsultarIdMascota()
        {
            int cod =  cboCliente.SelectedIndex + 1;
            //string idC =Convert.ToString( cboCliente.SelectedIndex + 1);
            string masc = cboMascota.Text;
            string url = "https://localhost:44310/api/Veterinaria/GetIdMascota"+"/" + cod.ToString() + "/" + masc;
            HttpClient cliente = new HttpClient();
            var resultado = await cliente.GetAsync(url);
            var content = await resultado.Content.ReadAsStringAsync();
            int nro = JsonConvert.DeserializeObject<int>(content);
            return nro;
        }

        private async Task<List<Atencion>> ConsultarAtencion(int numero)
        {
            string url = "https://localhost:44310/api/Veterinaria/GetAtencion" + "/" + numero.ToString();
            HttpClient cliente = new HttpClient();
            var result = await cliente.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            List<Atencion> lista = JsonConvert.DeserializeObject<List<Atencion>>(content);
            return lista;
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

        private async Task<bool> DeleteDetalle(int idMascota,int det)
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
    }
}
