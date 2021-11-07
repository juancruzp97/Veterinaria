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

        //                    MASCOTA

        //INSERT
        public bool AgregarMascotaAtencion(Mascota oMascota, int id);
        public bool InsertarMascota(Mascota oMascota, int cod);

        //SELECT
       
        DataTable ConsultarMascotaNombre(string nombre);
        List<Mascota> ConsultaMascotaCliente(int cod);
        int GetIdMascota(int cliente, string nombre);
        
        //UPDATE
        public bool UpdateMascota(Mascota mascota);

        //DELETE
        public bool DeleteMascota(int idMascota);



        //                   CLIENTE
        List<Clientes> ConsultarClientes();

        //                   ATENCION

        //INSERT
        public bool InsertarDetalleAtencion(List<Atencion> oAtencion, int idMascota);
       
        public bool InsertarAtencion(Mascota oMascota);

        //SELECT
    
        public List<int> GetIdAtencion(int idMascota);
        public int ProximoDetalle(int idMascota);
        List<Atencion> ObtenerAtencion(int cod);  
        
        //DELETE
        public bool DeleteAtencion(int idMascota);
        public bool DeleteDetalleAtencion(int idDetalle, int idMascota);

        //UPDATE
        public bool UpdateAtencion(Atencion oAtencion, int id);
    }
}
