using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBackend.Dominio;

namespace VeterinariaBackend.Negocio
{
    public interface IGestorVeterinaria
    {
        public bool InsertarMascota(Mascota oMascota, int cod);
        public bool AgregarMascotaAtencion(Mascota mascota, int id);
        public bool InsertarAtencion(int codAtencion, int codMascota, DateTime fecha, string descp, double importe);
        public List<Clientes> ObtenerClientes();
        public List<Mascota> ObtenerMascotaCliente(int cod);
        public DataTable MascotaNombre(string nombre);
        public int GetIdMascota(int cliente, string nombre);
        public List<int> GetIdAtencion(int idMascota);
        public int ProximoDetalle(int idMascota);
        public List<Atencion> ObtenerAtencion(int cod);
        public bool DeleteMascota(int idMascota);
        public bool DeleteAtencion(int idMascota);
        public bool DeleteDetalleAtencion(int idMascota, int idDetalle);
        public bool UpdateAtencion(int codMascota, int codDetalle, DateTime fecha, double importe, string descrp);

    }
}
