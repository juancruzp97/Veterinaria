using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBackend.Dominio;

namespace VeterinariaBackend.Acceso_a_Datos.Interfaces
{
    interface IMascotaDao
    {

        bool AgregarMascotaAtencion(Mascota oMascota, int id);
        public bool InsertarMascota(Mascota oMascota, int cod);
        public bool InsertarAtencion(int codAtencion, int codMascota, DateTime fecha, string descp, double importe);
        public bool UpdateMascota(Mascota mascota);
        List<Clientes> ConsultarClientes();
        DataTable ConsultarMascotaNombre(string nombre);
        List<Mascota> ConsultaMascotaCliente(int cod);
        int GetIdMascota(int cliente, string nombre);
        public List<int> GetIdAtencion(int idMascota);
        public int ProximoDetalle(int idMascota);
        List<Atencion> ObtenerAtencion(int cod);
        public bool DeleteMascota(int idMascota);
        public bool DeleteAtencion(int idMascota);
        public bool DeleteDetalleAtencion(int idMascota, int idDetalle);
        public bool UpdateAtencion(Atencion oAtencion, int id);
    }
}
