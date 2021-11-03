using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBackend.Acceso_a_Datos.Interfaces;
using VeterinariaBackend.Dominio;

namespace VeterinariaBackend.Acceso_a_Datos.Implementaciones
{
    class MascotaDao : IMascotaDao
    {

        public bool AgregarMascotaAtencion(Mascota oMascota, int id)
        {
            return HelperDao.GetInstance().InsertarSql(oMascota, "SP_ALTA_MASCOTAS", "SP_INSERTAR_ATENCION", id);

        }

        public List<Mascota> ConsultaMascotaCliente(int cod)
        {
            return HelperDao.GetInstance().ConsultarMascotaCliente("SP_CONSULTAR_MASCOTA_CLIENTE", cod);
        }

        public List<Clientes> ConsultarClientes()
        {
            return HelperDao.GetInstance().ObtenerClientes("SP_CONSULTAR_CLIENTES");
        }

        public DataTable ConsultarMascotaNombre(string nombre)
        {
            return HelperDao.GetInstance().MascotaPorNombre(nombre);
        }

        public bool DeleteAtencion(int idMascota)
        {
            return HelperDao.GetInstance().DeleteAtencion(idMascota);
        }

        public bool DeleteDetalleAtencion(int idMascota, int idDetalle)
        {
            return HelperDao.GetInstance().DeleteDetalleAtencion(idMascota, idDetalle);
        }

        public bool DeleteMascota(int idMascota)
        {
            return HelperDao.GetInstance().DeleteMascota(idMascota);
        }

        public List<int> GetIdAtencion(int idMascota)
        {
            return HelperDao.GetInstance().GetIdAtencion(idMascota);
        }

        public int GetIdMascota(int cliente, string nombre)
        {
            return HelperDao.GetInstance().GetIdMascota("SP_COD_MASCOTA", cliente, nombre);
        }

        public bool InsertarAtencion(int codAtencion, int codMascota, DateTime fecha, string descp, double importe)
        {
            return HelperDao.GetInstance().InsertarAtencion(codAtencion, codMascota, fecha, descp, importe);
        }

        public bool InsertarMascota(Mascota oMascota, int cod)
        {
            return HelperDao.GetInstance().InsertarMascota("SP_ALTA_SOLO_MASCOTA", oMascota, cod);
        }

        public List<Atencion> ObtenerAtencion(int cod)
        {
            return HelperDao.GetInstance().ObtenerAtencion("SP_ATENCION_CONSULTA", cod);
        }

        public int ProximoDetalle(int idMascota)
        {
            return HelperDao.GetInstance().ProximoDetalle(idMascota);
        }

        public bool UpdateAtencion(int codMascota, int codDetalle, DateTime fecha, double importe, string descrp)
        {
            return HelperDao.GetInstance().UpdateAtencion(codMascota, codDetalle, fecha, importe, descrp);
        }

        public bool UpdateMascota(Mascota oMascota)
        {
            return HelperDao.GetInstance().UpdateMascota("SP_UPDATE_MASCOTA2", oMascota);
        }
    }
}
