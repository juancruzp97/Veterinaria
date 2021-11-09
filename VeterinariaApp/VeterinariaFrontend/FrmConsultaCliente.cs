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
    public partial class FrmConsultaCliente : Form
    {
        public enum TipoM
        {
            PERRO,
            GATO,
            ARAÑA,
            IGUANA
        }
        private Clientes oCliente;
        private int idcliente = 0;
        public FrmConsultaCliente()
        {
            InitializeComponent();
            oCliente = new Clientes();
        }
        private async void FrmConsultaCliente_Load(object sender, EventArgs e)
        {
            await CargarComboCliente();
            Habilitar(false);
            dgvMascota.Enabled = false;
        }

        


        //METODOS
        public async Task CargarComboCliente()
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
        private void Limpiar()
        {

            txtNombre.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtDocumento.Text = string.Empty;
            txtEdad.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            cboSexo.SelectedIndex = -1;
        }
        private void Habilitar(bool v)
        {

            txtNombre.Enabled = v;
            txtDireccion.Enabled = v;
            txtDocumento.Enabled = v;
            txtEdad.Enabled = v;
            txtTelefono.Enabled = v;
            cboSexo.Enabled = v;
        }
        private void CargarCampos()
        {
            oCliente = (Clientes)cboCliente.SelectedItem;
            if (Object.Equals(oCliente, null))
            {
                return;
            }
            else
            {

                idcliente = oCliente.Codigo;
                txtNombre.Text = oCliente.Nombre;
                txtDireccion.Text = oCliente.Direccion.ToString();
                txtDocumento.Text = oCliente.Documento.ToString();
                txtEdad.Text = oCliente.Edad.ToString();
                txtTelefono.Text = oCliente.Telefono.ToString();
                if (oCliente.Sexo == true)
                {
                    cboSexo.SelectedIndex = 0;
                }
                else
                {
                    cboSexo.SelectedIndex = 1;
                }
                CargarDGV(idcliente);
                return;

            }
        }
        private async void CargarDGV(int id)
        {
            if (id == 0)
            {
                return;
            }
            else
            {
                List<Mascota> lstM = await ObtenerMascota(id);

                if (lstM == null)
                {
                    return;

                }
                else
                {
                    for (int i = 0; i < lstM.Count; i++)
                    {
                        Mascota oMascota = new Mascota();
                        oMascota.CodigoMascota = lstM[i].CodigoMascota;
                        oMascota.Edad = lstM[i].Edad;
                        oMascota.Nombre = lstM[i].Nombre;
                        oMascota.TipoMascota = lstM[i].TipoMascota;
                        string tipoM = "";
                        switch (oMascota.TipoMascota)
                        {
                            case 1:
                                tipoM = "Perro";
                                break;
                            case 2:
                                tipoM = "Gato";
                                break;
                            case 3:
                                tipoM = "Araña";
                                break;
                            case 4:
                                tipoM = "Iguana";
                                break;
                            default:
                                break;
                        }
                        oCliente.AgregarMascota(oMascota);
                        dgvMascota.Rows.Add(new object[] { oMascota.CodigoMascota, oMascota.Nombre, tipoM, oMascota.Edad });                        
                    }
                }
            }

        }
    
       


        //EVENTOS
        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!cb.Focused)
            {
                return;
            }
            else
            {
                int indice = cboCliente.SelectedIndex + 1;
                if (indice == 0)
                {
                    return;
                }
                else
                {
                    Limpiar();
                    dgvMascota.Rows.Clear();
                }
                
                
               
            }
        }
        private async void dgvMascota_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvMascota.CurrentRow.ReadOnly = false;
            dgvMascota.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvMascota.EditMode = DataGridViewEditMode.EditOnKeystroke;
            int cliente = oCliente.Codigo;
            int indice = dgvMascota.CurrentRow.Index;

            if (dgvMascota.CurrentCell.ColumnIndex == 4)
            {
                int idM = Convert.ToInt32(dgvMascota.Rows[indice].Cells["ID"].Value);
                bool test = await EliminarMascota(idM);


                if (test)
                {
                    MessageBox.Show("Mascota Eliminada", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvMascota.Rows.Clear();
                    CargarDGV(cliente);
                    return;

                }
                else
                {
                    MessageBox.Show("Error al eliminar Mascota", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (dgvMascota.CurrentCell.ColumnIndex == 5)
            {
                int index = dgvMascota.CurrentRow.Index;
                Mascota oMascota = new Mascota();
                oMascota.Nombre = dgvMascota.Rows[index].Cells["Nombre"].Value.ToString();
                oMascota.CodigoMascota = Convert.ToInt32(dgvMascota.Rows[index].Cells["ID"].Value);
                oMascota.Edad = Convert.ToInt32(dgvMascota.Rows[index].Cells["Edad"].Value);
                string tip = dgvMascota.Rows[index].Cells["Especie"].Value.ToString();
                switch (tip)
                {
                    case "Perro":
                        oMascota.TipoMascota = 1;
                        break;
                    case "Gato":
                        oMascota.TipoMascota = 2;
                        break;
                    case "Araña":
                        oMascota.TipoMascota = 3;
                        break;
                    case "Iguana":
                        oMascota.TipoMascota = 4;
                        break;
                    default:
                        break;
                }
                bool up = await UpdateMascota(oMascota);

                if (up)
                {
                    MessageBox.Show("Mascota Actualizada", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvMascota.Rows.Clear();
                    CargarDGV(cliente);
                    return;
                }
                else
                {
                    MessageBox.Show("Error al Actualizar Mascota", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

        }



        //BOTONES
        private void btnEditar_Click(object sender, EventArgs e)
        {
            Habilitar(true);
        }
        private async void btnAceptar_ClickAsync(object sender, EventArgs e)
        {
            oCliente = null;
            oCliente = new Clientes();
            oCliente.Codigo = cboCliente.SelectedIndex +1;
            oCliente.Direccion = txtDireccion.Text;
            oCliente.Documento = Convert.ToInt32(txtDocumento.Text);
            oCliente.Edad = Convert.ToInt32(txtEdad.Text);
            oCliente.Telefono = Convert.ToInt32(txtTelefono.Text);
            oCliente.Nombre = txtNombre.Text;
            if(cboSexo.SelectedIndex == 0)
            {
                oCliente.Sexo = true;
            }
            else
            {
                oCliente.Sexo = false;
            }

            bool test = await UpdateCliente(oCliente);

            if (test)
            {
                MessageBox.Show("Cliente Actualizado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Habilitar(false);
                return;
            }
            else
            {
                MessageBox.Show("Error al Actualizar Cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            CargarCampos();
            dgvMascota.Enabled = true;
        }


        //API
    
        private async Task<bool> UpdateMascota(Mascota oMascota)
        {
            string url = "https://localhost:44310/api/Mascota/UpdateMascota";
            HttpClient cliente = new HttpClient();
            string data = JsonConvert.SerializeObject(oMascota);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var resultado = await cliente.PutAsync(url, content);
            bool succes = resultado.IsSuccessStatusCode;
            if (succes)
            {
                return succes;
            }
            else
            {
                succes = false;
                return succes;
            }

            
        }
        private async Task<bool> EliminarMascota(int idM)
        {
            bool test = await EliminarAtencion(idM);
            if (test)
            {
                string url = "https://localhost:44310/api/Mascota/DeleteMascota/" + idM.ToString();
                HttpClient cliente = new HttpClient();
                var result = await cliente.DeleteAsync(url);
                bool succes = true;
                if (result.IsSuccessStatusCode)
                {
                    return succes;
                }
                else
                {
                    succes = false;
                    return succes;
                }
            }
            else
            {
                return false;
            }
            
        }
        private async Task<bool> EliminarAtencion(int idM)
        {
            string url = "https://localhost:44310/api/Atencion/DeleteAtencion/" + idM.ToString();
            HttpClient cliente = new HttpClient();
            var result = await cliente.DeleteAsync(url);
            bool success = true;
            if (result.IsSuccessStatusCode)
            {
                return success;
            }
            else
            {
                success = false;
                return success;
            }
        }
        private async Task<bool> UpdateCliente(Clientes clientes)
        {
            string url = "https://localhost:44310/api/Cliente/UpdateCliente";
            HttpClient cliente = new HttpClient();
            string data = JsonConvert.SerializeObject(clientes);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var resultado = await cliente.PutAsync(url, content);
            bool succes = true;

            if (resultado.IsSuccessStatusCode)
            {
                return succes;
            }
            else
            {
                succes = false;
                return succes;
            }


        }
        private async Task<List<Mascota>> ObtenerMascota(int id)
        {
            string url = "https://localhost:44310/api/Mascota/ObtenerMascota" + "/" + id.ToString();
            HttpClient cliente = new HttpClient();
            var result = await cliente.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            List<Mascota> lst = JsonConvert.DeserializeObject<List<Mascota>>(content);

            return lst;
        }
    }
}
