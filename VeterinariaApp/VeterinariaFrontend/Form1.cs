using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VeterinariaFrontend
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void nuevaAtencionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAgregarMascota frmAgregarMascota = new FrmAgregarMascota();
            frmAgregarMascota.ShowDialog();
        }

        private void consultarMascotaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmConsultaMascota frmConsultaMascota = new FrmConsultaMascota();
            frmConsultaMascota.ShowDialog();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void altaMascotaAtencionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAltaAtencion frmAltaAtencion = new FrmAltaAtencion();
            frmAltaAtencion.ShowDialog();
        }

        private void consultarMascotaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmConsultaMascota frmConsultaMascota = new FrmConsultaMascota();
            frmConsultaMascota.ShowDialog();
        }
    }
}
