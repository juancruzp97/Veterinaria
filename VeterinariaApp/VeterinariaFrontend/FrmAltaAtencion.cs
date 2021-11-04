
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private void AltaAtencion_Load(object sender, EventArgs e)
        {
            CargarComboCliente();
            cboCliente.Enabled = true;
            cboCliente.SelectedIndex = -1;
            txtMascota.Enabled = false;
            txtEdad.Enabled = false;
            cboTipo.Enabled = false;
            dtPicker.Enabled = false;
            txtImporte.Enabled = false;
            txtDescripcion.Enabled = false;
        }



        public void CargarComboCliente()
        {
            List<Clientes> lst = _gestor.ObtenerClientes();

            cboCliente.DataSource = lst;
            cboCliente.ValueMember = "Codigo";
            cboCliente.DisplayMember = "Nombre";

        }

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

        public void CargarListBox(int indice)
        {

            List<Mascota> mascota = _gestor.ObtenerMascotaCliente(indice);
            int j = 0;
            for (int i = 0; i < mascota.Count; i++)
            {
                lstBoxMascota.Items.Add(mascota[j].Nombre);
                j++;
            }
        }



        private void CargarCampos(string nom)
        {
            limpiar();
            dgvAtencion.Rows.Clear();

            DataTable tabla = new DataTable();
            tabla = _gestor.MascotaNombre(nom);

            //Carga Campos
            foreach (DataRow item in tabla.Rows)
            {
                int cod = Convert.ToInt32(item[0].ToString());
                txtMascota.Text = item[1].ToString();
                txtEdad.Text = item[2].ToString();
                cboTipo.SelectedIndex = Convert.ToInt32(item[3]) - 1;
            }
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

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            int id = _gestor.GetIdMascota(cboCliente.SelectedIndex + 1, txtMascota.Text);
            nuevo = false;


            oMascota.Nombre = txtMascota.Text;
            oMascota.Edad = Convert.ToInt32(txtEdad.Text);
            oMascota.TipoMascota = cboTipo.SelectedIndex + 1;


            int filas = _gestor.MascotaNombre(txtMascota.Text).Rows.Count;

            if (filas == 0)
            {
                Validaciones();
                for (int i = 0; i < dgvAtencion.Rows.Count; i++)
                {
                    Atencion atencion = new Atencion();
                    atencion.Fecha = Convert.ToDateTime(dgvAtencion.Rows[i].Cells["Fecha"].Value);
                    atencion.Descripcion = dgvAtencion.Rows[i].Cells["Descripcion"].Value.ToString();
                    atencion.Importe = Convert.ToDouble(dgvAtencion.Rows[i].Cells["Importe"].Value);
                    oMascota.AgregarAtencion(atencion);
                }
                //falta if
                _gestor.AgregarMascotaAtencion(oMascota, cboCliente.SelectedIndex + 1);
               // _gestor.AgregarMascotaAtencion(oMascota);
                lstBoxMascota.Items.Clear();
                CargarListBox(cboCliente.SelectedIndex + 1);
                //CargarDGV();
                dtPicker.Value = DateTime.Today;
                txtDescripcion.Text = string.Empty;
                cboTipo.SelectedIndex = -1;
                txtImporte.Text = string.Empty;
                txtMascota.Enabled = false;
                txtEdad.Enabled = false;
                cboTipo.Enabled = false;
                dtPicker.Focus();



                return;
            }
            //        {
            //            int codigo = Convert.ToInt32(dgvAtencion.CurrentRow.Cells[0].Value);
            //            DateTime fecha = Convert.ToDateTime(dgvAtencion.CurrentRow.Cells[1].Value);
            //            string descrp = dgvAtencion.CurrentRow.Cells[2].Value.ToString();
            //            double importe = Convert.ToDouble(dgvAtencion.CurrentRow.Cells[3].Value);
            //            if (_gestor.UpdateAtencion(id, codigo, fecha, importe, descrp) == true)
            //            {
            //                MessageBox.Show("Se Actualizo Detalle Atencion", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //                editar = false;
            //                //return;
            //            }
            //            else
            //            {
            //                MessageBox.Show("Problemas al Actualizar", "Precaucion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //                return;
            //            }
            //        }
            //    }

            else if (filas > 0)
            {

                //int det = prox - 1;
                //int j = dgvAtencion.Rows.Count - det;

                int prox = _gestor.ProximoDetalle(id);
                for (int i = 0; i < dgvAtencion.Rows.Count; i++)
                {

                    int k = Convert.ToInt32(dgvAtencion.Rows[i].Cells["id"].Value);
                    if (k == prox)
                    {
                        Atencion oAtencion = new Atencion();
                        oAtencion.Fecha = Convert.ToDateTime(dgvAtencion.Rows[i].Cells["Fecha"].Value);
                        oAtencion.Descripcion = dgvAtencion.Rows[i].Cells["Descripcion"].Value.ToString();
                        oAtencion.Importe = Convert.ToDouble(dgvAtencion.Rows[i].Cells["Importe"].Value);
                        oMascota.AgregarAtencion(oAtencion);
                        _gestor.InsertarAtencion(prox, id, oAtencion.Fecha, oAtencion.Descripcion, oAtencion.Importe);
                        prox++;
                    }
                }
                MessageBox.Show("Se Agrego con Exito!", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

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


        private void lstBoxMascota_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAgregar.Enabled = true;
            CargarCampos(lstBoxMascota.Text);
            CargarDGV();
            btnAgregarDetalle.Enabled = true;
            txtDescripcion.Enabled = true;
            txtImporte.Enabled = true;
            dtPicker.Enabled = true;

        }

        private void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (!cb.Focused)
            {
                return;
            }
            else
            {
                limpiar();
                dgvAtencion.Rows.Clear();
                int indice = cboCliente.SelectedIndex + 1;
                lstBoxMascota.Items.Clear();
                CargarListBox(indice);
            }
            //if (cboCliente.SelectedIndex==0)
            //{
            //    txtMascota.Enabled = true;
            //    txtEdad.Enabled = true;
            //    cboTipo.Enabled = true;
            //}
            //if (cboCliente.SelectedIndex == 1)
            //{
            //    txtMascota.Enabled = true;
            //    txtEdad.Enabled = true;
            //    cboTipo.Enabled = true;
            //}
            //if (cboCliente.SelectedIndex == 2)
            //{
            //    txtMascota.Enabled = true;
            //    txtEdad.Enabled = true;
            //    cboTipo.Enabled = true;
            //}
            //if (cboCliente.SelectedIndex == 3)
            //{
            //    txtMascota.Enabled = true;
            //    txtEdad.Enabled = true;
            //    cboTipo.Enabled = true;
            //}
        }

        private void limpiar()
        {
            txtDescripcion.Text = string.Empty;
            txtEdad.Text = string.Empty;
            txtImporte.Text = string.Empty;
            txtMascota.Text = string.Empty;
            //cboCliente.SelectedIndex = -1;
            cboTipo.SelectedIndex = -1;

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

        private void txtMascota_TextChanged(object sender, EventArgs e)
        {

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

        private void dgvAtencion_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
