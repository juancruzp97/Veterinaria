
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
    public partial class FrmAltaAtencion : Form
    {
        private IGestorVeterinaria _gestor;
        Mascota oMascota;
        Clientes oCliente;
        private bool nuevo = false;
        public FrmAltaAtencion()
        {
            InitializeComponent();
            _gestor = new FactoryVeterinaria().CrearGestor();
            oMascota = new Mascota();
            oCliente = new Clientes();
        }

        private async void AltaAtencion_Load(object sender, EventArgs e)
        {
            await CargarComboCliente();
            cboCliente.Enabled = true;
            cboCliente.SelectedIndex = -1;
            txtMascota.Enabled = false;
            txtEdad.Enabled = false;
            cboTipo.Enabled = false;
            dtPicker.Enabled = false;
            txtImporte.Enabled = false;
            txtDescripcion.Enabled = false;
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
        public async Task CargarListBox(int indice)
        {                              
                string url = "https://localhost:44310/api/Mascota/ConsultarMascota" + "/" + indice.ToString();
                HttpClient cliente = new HttpClient();
                var result = await cliente.GetAsync(url);
                var content = await result.Content.ReadAsStringAsync();
                List<Mascota> lst = JsonConvert.DeserializeObject<List<Mascota>>(content);
                lstBoxMascota.DataSource = lst;
                lstBoxMascota.ValueMember = "CodigoMascota";
                lstBoxMascota.DisplayMember = "Nombre";            
        }

        
        private async Task<List<Mascota>> GetMascotas(int id)
        {
            string url = "https://localhost:44310/api/Mascota/ConsultarMascota" + "/" + id.ToString();
            HttpClient cliente = new HttpClient();
            var result = await cliente.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            List<Mascota> lst = JsonConvert.DeserializeObject<List<Mascota>>(content);

            return lst;
        }

        
        public bool Validaciones()
        {
            if (string.IsNullOrEmpty(cboCliente.Text))
            {
                MessageBox.Show("Debe seleccionar un cliente...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboCliente.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtMascota.Text))
            {
                MessageBox.Show("Debe ingresar una mascota...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMascota.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtEdad.Text))
            {
                MessageBox.Show("Debe igresar una edad...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtEdad.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(cboTipo.Text))
            {
                MessageBox.Show("Debe seleccionar un tipo de mascota...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboTipo.Focus();
                return false;
            }



            return true;
        }
        private bool ValidacionesDetalle()
        {
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                MessageBox.Show("Debe agregar una Descripcion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtImporte.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtImporte.Text))
            {
                MessageBox.Show("Debe agregar un importe...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtImporte.Focus();
                return false;
            }

            return true;
        }
        private void limpiar()
        {
            txtDescripcion.Text = string.Empty;
            //txtEdad.Text = string.Empty;
            txtImporte.Text = string.Empty;
            //txtMascota.Text = string.Empty;
            //cboCliente.SelectedIndex = -1;
            cboTipo.SelectedIndex = -1;

        }
        private void Habilitar(bool x)
        {
            //cboCliente.Enabled = x;
            txtDescripcion.Enabled = x;
            dtPicker.Enabled = x;
            txtImporte.Enabled = x;
            txtMascota.Enabled = x;
            txtEdad.Enabled = x;
            cboTipo.Enabled = x;

        }




        //BOTONES
        private void btnEditar_Click(object sender, EventArgs e)
        {

            if (cboCliente.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar cliente", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //editar = true;
                //dgvAtencion.ReadOnly = false;
                dtPicker.Enabled = true;
                txtDescripcion.Enabled = true;
                txtImporte.Enabled = true;
                dtPicker.Enabled = false;
                txtDescripcion.Enabled = false;
                txtImporte.Enabled = false;
                btnAgregarDetalle.Enabled = false;
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

        private async void btnAgregar_Click_1(object sender, EventArgs e)
        {

            if (oMascota.ListaAtencion.Count == 0)
            {
                MessageBox.Show("No existen Atenciones para Agregar", "Precaucion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                bool test = await InsertarMascotaAtencion(oMascota);
                if (test)
                {
                    MessageBox.Show("Atencion Agregada Con Exito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    MessageBox.Show("Problemas al agregar Atencion", "Precaucion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
        }
        private async Task<bool> InsertarMascotaAtencion(Mascota mascota)
        {
            string url = "https://localhost:44310/api/Atencion/InsertarAtencion";
            HttpClient cliente = new HttpClient();
            string data = JsonConvert.SerializeObject(mascota);
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


        //int filas = _gestor.MascotaNombre(txtMascota.Text).Rows.Count;

        //if (filas == 0)
        //{
        //    Validaciones();
        //    for (int i = 0; i < dgvAtencion.Rows.Count; i++)
        //    {
        //        Atencion atencion = new Atencion();
        //        atencion.Fecha = Convert.ToDateTime(dgvAtencion.Rows[i].Cells["Fecha"].Value);
        //        atencion.Descripcion = dgvAtencion.Rows[i].Cells["Descripcion"].Value.ToString();
        //        atencion.Importe = Convert.ToDouble(dgvAtencion.Rows[i].Cells["Importe"].Value);
        //        oMascota.AgregarAtencion(atencion);
        //    }
        //    //falta if
        //    _gestor.AgregarMascotaAtencion(oMascota, cboCliente.SelectedIndex + 1);
        //   // _gestor.AgregarMascotaAtencion(oMascota);
        //    lstBoxMascota.Items.Clear();
        //    await CargarListBox(cboCliente.SelectedIndex + 1);
        //    //CargarDGV();
        //    dtPicker.Value = DateTime.Today;
        //    txtDescripcion.Text = string.Empty;
        //    cboTipo.SelectedIndex = -1;
        //    txtImporte.Text = string.Empty;
        //    txtMascota.Enabled = false;
        //    txtEdad.Enabled = false;
        //    cboTipo.Enabled = false;
        //    dtPicker.Focus();



        //    return;
        //}
        ////        {
        ////            int codigo = Convert.ToInt32(dgvAtencion.CurrentRow.Cells[0].Value);
        ////            DateTime fecha = Convert.ToDateTime(dgvAtencion.CurrentRow.Cells[1].Value);
        ////            string descrp = dgvAtencion.CurrentRow.Cells[2].Value.ToString();
        ////            double importe = Convert.ToDouble(dgvAtencion.CurrentRow.Cells[3].Value);
        ////            if (_gestor.UpdateAtencion(id, codigo, fecha, importe, descrp) == true)
        ////            {
        ////                MessageBox.Show("Se Actualizo Detalle Atencion", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        ////                editar = false;
        ////                //return;
        ////            }
        ////            else
        ////            {
        ////                MessageBox.Show("Problemas al Actualizar", "Precaucion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        ////                return;
        ////            }
        ////        }
        ////    }

        //else if (filas > 0)
        //{

        //    //int det = prox - 1;
        //    //int j = dgvAtencion.Rows.Count - det;

        //    int prox = _gestor.ProximoDetalle(id);
        //    for (int i = 0; i < dgvAtencion.Rows.Count; i++)
        //    {

        //        int k = Convert.ToInt32(dgvAtencion.Rows[i].Cells["id"].Value);
        //        if (k == prox)
        //        {
        //            Atencion oAtencion = new Atencion();
        //            oAtencion.Fecha = Convert.ToDateTime(dgvAtencion.Rows[i].Cells["Fecha"].Value);
        //            oAtencion.Descripcion = dgvAtencion.Rows[i].Cells["Descripcion"].Value.ToString();
        //            oAtencion.Importe = Convert.ToDouble(dgvAtencion.Rows[i].Cells["Importe"].Value);
        //            oMascota.AgregarAtencion(oAtencion);
        //            _gestor.InsertarAtencion(prox, id, oAtencion.Fecha, oAtencion.Descripcion, oAtencion.Importe);
        //            prox++;
        //        }
        //    }
        //    MessageBox.Show("Se Agrego con Exito!", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //}
        //}



        private void btnActualizar_Click(object sender, EventArgs e)
        {
            int mascota = _gestor.GetIdMascota(cboCliente.SelectedIndex + 1, txtMascota.Text);

            int codigo = Convert.ToInt32(dgvAtencion.CurrentRow.Cells["id"].Value);
            DateTime fecha = dtPicker.Value;
            string descrp = txtDescripcion.Text;
            double importe = Convert.ToDouble(txtImporte.Text);
            //_gestor.UpdateAtencion(mascota, codigo, fecha, importe, descrp);
            dgvAtencion.Rows.Clear();
            CargarDGV();
            btnAgregar.Enabled = true;
        }
      
        private void btnAgregarDetalle_Click_1(object sender, EventArgs e)
        {
            if (ValidacionesDetalle() == true)
            {
                int id = dgvAtencion.Rows.Count + 1;
                DateTime fecha = dtPicker.Value;
                double importe = Convert.ToDouble(txtImporte.Text);
                string desc = txtDescripcion.Text.ToString();
                if (nuevo == true)
                {
                    dgvAtencion.Rows.Add(new object[] { id, fecha, desc, importe });
                    txtImporte.Text = string.Empty;
                    txtDescripcion.Text = string.Empty;
                    dtPicker.Value = DateTime.Today;
                }
                else if (nuevo == false)
                {
                    int cant = dgvAtencion.Rows.Count - 1;
                    int det = _gestor.ProximoDetalle(_gestor.GetIdMascota(cboCliente.SelectedIndex + 1, txtMascota.Text));

                    dgvAtencion.Rows.Add(new object[] { det, fecha, desc, importe });
                    txtImporte.Text = string.Empty;
                    txtDescripcion.Text = string.Empty;
                    dtPicker.Value = DateTime.Today;
                }
                //Atencion atencion = new Atencion();
                //atencion.Descripcion = txtDescripcion.Text;
                //atencion.Importe = Convert.ToInt32(txtImporte.Text);
                //atencion.Fecha = dtPicker.Value;
                //oMascota.AgregarAtencion(atencion);
                //int det = _gestor.ProximoDetalle(_gestor.GetIdMascota(cboCliente.SelectedIndex + 1, txtMascota.Text));
                //int cant = dgvAtencion.Rows.Count;
                //int det = Convert.ToInt32(dgvAtencion.Rows[cant].Cells["id"].Value) + 1;


                // dgvAtencion.Rows.Add(new object[] { det, atencion.Fecha, atencion.Descripcion, atencion.Importe });

            }
            else
            {
                return;
            }
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {

            nuevo = true;//habilitar todos los campos
            cboCliente.Enabled = true;
            btnActualizar.Enabled = false;
            txtDescripcion.Enabled = true;
            dtPicker.Enabled = true;
            txtImporte.Enabled = true;
            txtMascota.Enabled = true;
            txtEdad.Enabled = true;
            cboTipo.Enabled = true;
            dgvAtencion.Rows.Clear();
            limpiar();

        }
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            int idMascota = _gestor.GetIdMascota(cboCliente.SelectedIndex + 1, txtMascota.Text);

            if (_gestor.DeleteAtencion(idMascota) == true)
            {
                if (_gestor.DeleteMascota(idMascota) == true)
                {
                    MessageBox.Show("Mascota Eliminada", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dgvAtencion.Rows.Clear();
                    limpiar();
                    lstBoxMascota.Items.Clear();
                    return;
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar Mascota", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Problemas al eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("¿Esta seguro que desea cancelar?", "Precaución", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            limpiar();
            cboCliente.SelectedIndex = -1;
            cboTipo.SelectedIndex = -1;
            dgvAtencion.Rows.Clear();
            lstBoxMascota.Items.Clear();
            Habilitar(false);
            btnAgregar.Enabled = true;
        }


        //EVENTOS
        private async void lstBoxMascota_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lt = (ListBox)sender;
            if(lt.Focused == false)
            {
                return;
            }
            else
            {
                int id = cboCliente.SelectedIndex + 1;
                if (id == 0)
                {
                    return;
                }
                else
                {
                    int indice = lstBoxMascota.SelectedIndex;
                    btnAgregar.Enabled = true;
                    await CargarCamposAsync(id, indice);
                    CargarDGV();
                    btnAgregarDetalle.Enabled = true;
                    txtDescripcion.Enabled = true;
                    txtImporte.Enabled = true;
                    dtPicker.Enabled = true;

                    oMascota.Nombre = txtMascota.Text;
                    oMascota.Edad = Convert.ToInt32(txtEdad.Text);
                    oMascota.TipoMascota = cboTipo.SelectedIndex - 1;

                    //for (int i = 0; i < dgvAtencion.Rows.Count; i++)
                    //{
                    //    Atencion atencion = new Atencion();
                    //    atencion.CodAtencion = Convert.ToInt32(dgvAtencion.Rows[i].Cells["ID"].Value);
                    //    atencion.Descripcion = dgvAtencion.Rows[i].Cells["Descripcion"].Value.ToString();
                    //    atencion.Fecha = Convert.ToDateTime(dgvAtencion.Rows[i].Cells["Fecha"].Value);
                    //    atencion.Importe = Convert.ToDouble(dgvAtencion.Rows[i].Cells["Importe"]);
                    //    oMascota.AgregarAtencion(atencion);
                    //}
                }
            }
     
      

        }
        private async Task CargarCamposAsync(int id, int indice)
        {
            limpiar();
            dgvAtencion.Rows.Clear();

            List<Mascota> lst = await GetMascotas(id);

            txtMascota.Text = lst[indice].Nombre;
            txtEdad.Text = lst[indice].Edad.ToString();
            cboTipo.SelectedIndex = lst[indice].TipoMascota - 1;






            //oMascota.
            //oMascota = (Mascota)lstBoxMascota.SelectedItem;

            //txtMascota.Text = oMascota.Nombre;
            //txtEdad.Text = oMascota.Edad.ToString();
            //cboTipo.SelectedIndex = oMascota.TipoMascota - 1;

            //DataTable tabla = new DataTable();
            //tabla = _gestor.MascotaNombre(nom);

            //Carga Campos
            //foreach (DataRow item in tabla.Rows)
            //{
            //    int cod = Convert.ToInt32(item[0].ToString());
            //    txtMascota.Text = item[1].ToString();
            //    txtEdad.Text = item[2].ToString();
            //    cboTipo.SelectedIndex = Convert.ToInt32(item[3]) - 1;
            //}
            //int id = _gestor.GetIdMascota(cboCliente.SelectedIndex +1, lstBoxMascota.Text);

            ////Cargar DGV
            //List<Atencion> atencion = _gestor.ObtenerAtencion(id);

            //int j = 0;
            //for (int i = 0; i < atencion.Count; i++)
            //{
            //    dgvAtencion.Rows.Add(new object[] { "", atencion[j].Fecha, atencion[j].Descripcion, atencion[j].Importe });
            //    j++;
            //}
        }


        private async void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ComboBox cb = (ComboBox)sender;
            if (cb.Focused ==  false)
            {
                return;
            }
            else
            {
                int test = cb.SelectedIndex;
                int indice = cboCliente.SelectedIndex + 1;
                if (indice == 0)
                {
                    return;
                }
                else
                {
                    limpiar();
                    dgvAtencion.Rows.Clear();
                    
                    lstBoxMascota.DataSource = null;
                    lstBoxMascota.Items.Clear();
                    await CargarListBox(indice);
                    return;
                }
               
            }
        }
        private void dgvAtencion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnAgregar.Enabled = false;
            if (dgvAtencion.CurrentCell.ColumnIndex == 4)
            {
                int det = Convert.ToInt32(dgvAtencion.CurrentRow.Cells[0].Value);
                int id = _gestor.GetIdMascota(cboCliente.SelectedIndex + 1, txtMascota.Text);
                if (_gestor.DeleteDetalleAtencion(id, det) == true)
                {
                    dgvAtencion.Rows.Remove(dgvAtencion.CurrentRow);
                    MessageBox.Show("Detalle Borrado", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dgvAtencion.Rows.Clear();
                    CargarDGV();
                    return;
                }
                else
                {
                    MessageBox.Show("Problemas al eliminar Detalles", "Precaución", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else
            {
                btnAgregarDetalle.Enabled = false;
                int fila = dgvAtencion.CurrentRow.Index;
                txtDescripcion.Text = dgvAtencion.Rows[fila].Cells["Descripcion"].Value.ToString();
                txtImporte.Text = dgvAtencion.Rows[fila].Cells["Importe"].Value.ToString();
                dtPicker.Value = Convert.ToDateTime(dgvAtencion.Rows[fila].Cells["Fecha"].Value);

            }
        }

        private void btnAgregarDetalle_Click(object sender, EventArgs e)
        {
            oMascota = (Mascota)lstBoxMascota.SelectedItem;

            int i = dgvAtencion.Rows.Count;
            int det =Convert.ToInt32(dgvAtencion.Rows[i-1].Cells["ID"].Value);
            Atencion oAtencion = new Atencion();
            oAtencion.CodAtencion = det + 1;
            oAtencion.Descripcion = txtDescripcion.Text;
            oAtencion.Fecha = dtPicker.Value;
            oAtencion.Importe = Convert.ToDouble(txtImporte.Text);
            oMascota.AgregarAtencion(oAtencion);

            dgvAtencion.Rows.Add(new object[] { oAtencion.CodAtencion, oAtencion.Fecha, oAtencion.Descripcion, oAtencion.Importe });
            limpiar();
        }


        private void CargarDGV()
        {
            //Cargar DGV
            int id = _gestor.GetIdMascota(cboCliente.SelectedIndex + 1, lstBoxMascota.Text);
            List<int> det = new List<int>();
            det = _gestor.GetIdAtencion(id);
            List<Atencion> atencion = _gestor.ObtenerAtencion(id);

            int j = 0;
            for (int i = 0; i < atencion.Count; i++)
            {
                dgvAtencion.Rows.Add(new object[] { det[j], atencion[j].Fecha, atencion[j].Descripcion, atencion[j].Importe });
                //det++;
                j++;
            }
        }


    }
}
