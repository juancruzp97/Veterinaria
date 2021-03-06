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


        //                 MASCOTA

        //INSERT
        public bool AgregarMascotaAtencion(Mascota oMascota, int id)
        {
            return HelperDao.GetInstance().InsertarSql(oMascota, "SP_ALTA_MASCOTAS", "SP_INSERTAR_ATENCION", id);

        }
        public bool InsertarMascota(Mascota oMascota, int cod)
        {
            return HelperDao.GetInstance().InsertarMascota("SP_ALTA_SOLO_MASCOTA", oMascota, cod);
        }

        //SELECT
        public List<Mascota> ConsultaMascotaCliente(int cod)
        {
            return HelperDao.GetInstance().ConsultarMascotaCliente("SP_CONSULTAR_MASCOTA_CLIENTE", cod);
        }

        public DataTable ConsultarMascotaNombre(string nombre)
        {
            return HelperDao.GetInstance().MascotaPorNombre(nombre);
        }
        public int GetIdMascota(int cliente, string nombre)
        {
            return HelperDao.GetInstance().GetIdMascota("SP_COD_MASCOTA", cliente, nombre);
        }

        //DELETE
        public bool DeleteMascota(int idMascota)
        {
            return HelperDao.GetInstance().DeleteMascota(idMascota);
        }
       
        //UPDATE
        public bool UpdateMascota(Mascota oMascota)
        {
            return HelperDao.GetInstance().UpdateMascota("SP_UPDATE_MASCOTA2", oMascota);
        }



        //                    CLIENTES

        public List<Clientes> ConsultarClientes()
        {
            return HelperDao.GetInstance().ObtenerClientes("SP_CONSULTAR_CLIENTES");
        }

        //UPDATE
        public bool UpdateCliente(Clientes cliente)
        {
            return HelperDao.GetInstance().UpdateCliente("SP_UPDATE_CLIENTE", cliente);
        }




        //                    ATENCION

        //INSERT
        public bool InsertarAtencion(Mascota oMascota)
        {
            return HelperDao.GetInstance().InsertarAtencion(oMascota);
        }
        public bool InsertarDetalleAtencion(List<Atencion> atencion, int id)
        {
            return HelperDao.GetInstance().InsertarDetalleAtencion(atencion, id, "SP_INSERTAR_DETALLE_ATENCION");
        }

        //SELECT
        public List<int> GetIdAtencion(int idMascota)
        {
            return HelperDao.GetInstance().GetIdAtencion(idMascota);
        }
        public List<Atencion> ObtenerAtencion(int cod)
        {
            return HelperDao.GetInstance().ObtenerAtencion("SP_ATENCION_CONSULTA", cod);
        }
        public int ProximoDetalle(int idMascota)
        {
            return HelperDao.GetInstance().ProximoDetalle(idMascota);
        }



        //DELETE

        public bool DeleteAtencion(int idMascota)
        {
            return HelperDao.GetInstance().DeleteAtencion(idMascota);
        }

        public bool DeleteDetalleAtencion(int idMascota, int idDetalle)
        {
            return HelperDao.GetInstance().DeleteDetalleAtencion(idMascota, idDetalle); ;
        }

     
        //UPDATE
        public bool UpdateAtencion(Atencion atencion, int id)
        {
            return HelperDao.GetInstance().UpdateAtencion("SP_UPDATE_ATENCION", atencion,id);
        }

        public bool InsertarCliente(Clientes cliente)
        {
            return HelperDao.GetInstance().InsertarCliente("SP_ALTA_CLIENTE", cliente);
        }

       
    }
}
