﻿using Newtonsoft.Json;
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

namespace VeterinariaApp
{
    public partial class FrmAltaMascota : Form
    {
        Mascota oMascota;
        public FrmAltaMascota()
        {
            InitializeComponent();
            oMascota = new Mascota();
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

        private async void btnAgregar_Click(object sender, EventArgs e)
        {
            oMascota.Nombre = txtNombre.Text;
            oMascota.Edad = Convert.ToInt32(txtEdad.Text);
            oMascota.TipoMascota = cboTipo.SelectedIndex +1;
            int id = cboCliente.SelectedIndex +1;

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
            cboCliente.SelectedIndex = -1;
            txtNombre.Text = string.Empty;
            txtEdad.Text = string.Empty;
            cboTipo.SelectedIndex = -1;
        }

        private async Task<bool> GrabarMascotaAsync(string data, int id)
        {
            string url = "https://localhost:44310/api/Veterinaria/AgregarMascota" + "/" + id.ToString();
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                string response = await result.Content.ReadAsStringAsync();
                return response.Equals("Ok");
            }
        }
    }
}