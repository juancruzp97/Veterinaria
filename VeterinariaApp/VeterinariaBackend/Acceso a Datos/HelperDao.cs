using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBackend.Dominio;

namespace VeterinariaBackend.Acceso_a_Datos
{
    class HelperDao
    {
        private static HelperDao instancia;
        private string conexionString;

        private HelperDao()
        {
            conexionString = @"Data Source=localhost;Initial Catalog=db_Veterinaria;Integrated Security=True";
        }

        public static HelperDao GetInstance()
        {
            if (instancia == null)
            {
                instancia = new HelperDao();
            }
            return instancia;
        }
        public int GetIdMascota(string spId, int cliente, string nombre)
        {
            SqlConnection cnn = new SqlConnection(conexionString);
            int id = 0;
            try
            {

                cnn.Open();
                SqlCommand cmd = new SqlCommand(spId, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cliente", cliente);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@cod";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
                id = (int)param.Value;

            }
            catch
            {
                return id;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return id;

        }
        public List<int> GetIdAtencion(int idMascota)
        {
            SqlConnection cnn = new SqlConnection(conexionString);
            List<int> detalle = new List<int>();
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_COD_ATENCION", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mascota", idMascota);
                DataTable tabla = new DataTable();
                tabla.Load(cmd.ExecuteReader());

                foreach (DataRow fila in tabla.Rows)
                {
                    int i = Convert.ToInt32(fila["cod_atencion"].ToString());
                    detalle.Add(i);
                }

            }
            catch (SqlException)
            {
                return detalle;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return detalle;

        }
        public int ProximoDetalle(int idMascota)
        {
            SqlConnection cnn = new SqlConnection(conexionString);
            int detalle = 1;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_PROXIMO_DETALLE", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod", idMascota);
                SqlParameter param = new SqlParameter();
                param.Direction = ParameterDirection.Output;
                param.ParameterName = "@nro";
                param.SqlDbType = SqlDbType.Int;

                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();

                return (int)param.Value;
            }
            catch
            {
                return detalle;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
        }

        public bool InsertarMascota(string spAltaM,Mascota mascota, int cod)
        {
            bool flag = true;

            SqlConnection cnn = new SqlConnection(conexionString);
            SqlTransaction transaccion = null;

            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();

                SqlCommand cmd = new SqlCommand(spAltaM, cnn, transaccion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nommascota",mascota.Nombre);
                cmd.Parameters.AddWithValue("@edad", mascota.Edad);
                cmd.Parameters.AddWithValue("@tipo",mascota.TipoMascota);
                cmd.Parameters.AddWithValue("@cliente", cod);

                cmd.ExecuteNonQuery();
                transaccion.Commit();


            }
            catch (Exception)
            {
                transaccion.Rollback();
                flag = false;

            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return flag;
        }

        public bool InsertarSql(Mascota oMascota, string spMascota, string spAtencion, int id)
        {
            bool flag = true;

            SqlConnection cnn = new SqlConnection(conexionString);
            SqlTransaction transaccion = null;


            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();

                SqlCommand cmd = new SqlCommand(spMascota, cnn, transaccion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nommascota", oMascota.Nombre);
                cmd.Parameters.AddWithValue("@edad", oMascota.Edad);
                cmd.Parameters.AddWithValue("@tipo", oMascota.TipoMascota);
                cmd.Parameters.AddWithValue("@cliente", id);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@cod_mascota";
                param.SqlDbType = SqlDbType.Int;

                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();

                int codMascota = (int)param.Value;
                //int codMascota = 15;
                int nroAtencion = 0;
                //transaction.Commit();


                foreach (Atencion aten in oMascota.ListaAtencion)
                {

                    SqlCommand cmd2 = new SqlCommand(spAtencion, cnn);

                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Transaction = transaccion;
                    cmd2.Parameters.AddWithValue("@cod_atencion", ++nroAtencion);
                    cmd2.Parameters.AddWithValue("@cod_mascota", codMascota);
                    cmd2.Parameters.AddWithValue("@fecha", aten.Fecha);
                    cmd2.Parameters.AddWithValue("@descripcion", aten.Descripcion);
                    cmd2.Parameters.AddWithValue("@importe", aten.Importe);
                    cmd2.ExecuteNonQuery();
                }

                transaccion.Commit();
        }
            catch
            {
                transaccion.Rollback();


                flag = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return flag;


        }
        public bool InsertarAtencion(int codAtencion, int codMascota, DateTime fecha, string descp, double importe)
        {
            SqlConnection cnn = new SqlConnection(conexionString);
            SqlTransaction transaccion = null;
            bool flag = true;

            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("SP_INSERTAR_ATENCION", cnn, transaccion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_atencion", codAtencion);
                cmd.Parameters.AddWithValue("cod_mascota", codMascota);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@descripcion", descp);
                cmd.Parameters.AddWithValue("@importe", importe);
                cmd.ExecuteNonQuery();

                transaccion.Commit();
            }
            catch (Exception)
            {
                transaccion.Rollback();
                flag = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return flag;
        }

        public bool UpdateMascota(string spUpdate, Mascota mascota)
        {
            bool flag = true;
            SqlConnection cnn = new SqlConnection(conexionString);
            SqlTransaction transaccion = null;

            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand(spUpdate, cnn, transaccion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod", mascota.CodigoMascota);
                cmd.Parameters.AddWithValue("@nom",mascota.Nombre);
                cmd.Parameters.AddWithValue("@edad",mascota.Edad);
                cmd.Parameters.AddWithValue("@tipo",mascota.TipoMascota);

                cmd.ExecuteNonQuery();

                transaccion.Commit();
            }
            catch (Exception)
            {
                transaccion.Rollback();
                flag = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return flag;

        }

        public List<Clientes> ObtenerClientes(string spCliente)
        {
            List<Clientes> lista = new List<Clientes>();
            SqlConnection cnn = new SqlConnection(conexionString);

            cnn.Open();
            SqlCommand cmd = new SqlCommand(spCliente, cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            DataTable tabla = new DataTable();
            tabla.Load(cmd.ExecuteReader());

            cnn.Close();

            foreach (DataRow row in tabla.Rows)
            {
                Clientes cliente = new Clientes();
                cliente.Codigo = Convert.ToInt32(row["cod_cliente"].ToString());
                cliente.Nombre = row["nombre"].ToString();
                cliente.Sexo = row["sexo"].ToString().Equals("M");

                lista.Add(cliente);
            }

            return lista;

        }
        public List<Atencion> ObtenerAtencion(string spAtencion, int cod)
        {
            List<Atencion> lista = new List<Atencion>();
            SqlConnection cnn = new SqlConnection(conexionString);

            cnn.Open();
            SqlCommand cmd = new SqlCommand(spAtencion, cnn);
            DataTable tabla = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cod", cod);
            tabla.Load(cmd.ExecuteReader());
            cnn.Close();

            foreach (DataRow filas in tabla.Rows)
            {
                Atencion oAtencion = new Atencion();
                oAtencion.CodAtencion = Convert.ToInt32(filas["cod_atencion"].ToString());
                oAtencion.Fecha = Convert.ToDateTime(filas["fecha"].ToString());
                oAtencion.Descripcion = filas["descripcion"].ToString();
                oAtencion.Importe = Convert.ToDouble(filas["importe"].ToString());

                lista.Add(oAtencion);
            }

            return lista;
        }

        public List<Mascota> ConsultarMascotaCliente(string spMasCliente, int cod)
        {
            List<Mascota> lista = new List<Mascota>();
            SqlConnection cnn = new SqlConnection(conexionString);

            cnn.Open();
            SqlCommand cmd = new SqlCommand(spMasCliente, cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cod", cod);

            DataTable tabla = new DataTable();
            tabla.Load(cmd.ExecuteReader());

            cnn.Close();


            foreach (DataRow row in tabla.Rows)
            {
                Mascota mascota = new Mascota();
                mascota.CodigoMascota = Convert.ToInt32(row["cod_mascota"].ToString());
                mascota.Nombre = row["nombre"].ToString();
                mascota.Edad = Convert.ToInt32(row["edad"].ToString());
                mascota.TipoMascota = Convert.ToInt32(row["tipo"].ToString());
                lista.Add(mascota);
            }

            return lista;
        }

        public DataTable MascotaPorNombre(string nombre)
        {
            // List<Mascota> lista = new List<Mascota>();
            SqlConnection cnn = new SqlConnection(conexionString);

            cnn.Open();
            SqlCommand cmd = new SqlCommand("SP_MASCOTA_NOMBRE", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nom", nombre);

            DataTable tabla = new DataTable();
            tabla.Load(cmd.ExecuteReader());

            cnn.Close();
            return tabla;
        }

        public bool DeleteMascota(int idMascota)
        {
            SqlConnection cnn = new SqlConnection(conexionString);
            SqlTransaction transaccion = null;
            bool flag = true;

            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("SP_DELETE_MASCOTA", cnn, transaccion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmascota", idMascota);
                cmd.ExecuteNonQuery();

                transaccion.Commit();
            }
            catch
            {
                transaccion.Rollback();
                flag = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return flag;
        }

        public bool DeleteAtencion(int idMascota)
        {
            SqlConnection cnn = new SqlConnection(conexionString);
            SqlTransaction transaccion = null;
            bool flag = true;

            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_ATENCION", cnn, transaccion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codmascota", idMascota);
                cmd.ExecuteNonQuery();

                transaccion.Commit();

            }
            catch (Exception)
            {

                transaccion.Rollback();
                flag = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return flag;
        }
        public bool DeleteDetalleAtencion(int idMascota, int idDetalle)
        {
            SqlConnection cnn = new SqlConnection(conexionString);
            SqlTransaction transaccion = null;
            bool flag = true;

            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_DETALLE_ATENCION", cnn, transaccion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@atencion", idDetalle);
                cmd.Parameters.AddWithValue("@mascota", idMascota);
                cmd.ExecuteNonQuery();

                transaccion.Commit();

            }
            catch (Exception)
            {
                transaccion.Rollback();
                flag = false;

            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return flag;
        }
        public bool UpdateAtencion(string spUpAt,Atencion atencion, int id)
        {
            SqlConnection cnn = new SqlConnection(conexionString);
            SqlTransaction transaccion = null;
            bool flag = true;

            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();
                SqlCommand cmd = new SqlCommand(spUpAt, cnn, transaccion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@atencion", atencion.CodAtencion);
                cmd.Parameters.AddWithValue("@mascota", id);
                cmd.Parameters.AddWithValue("@fecha", atencion.Fecha);
                cmd.Parameters.AddWithValue("@descripcion", atencion.Descripcion);
                cmd.Parameters.AddWithValue("@importe", atencion.Importe);
                cmd.ExecuteNonQuery();

                transaccion.Commit();
            }
            catch (Exception)
            {
                transaccion.Rollback();
                flag = false;

            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return flag;
        }

    
}
}
