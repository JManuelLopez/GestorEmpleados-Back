
using GestorEmpleados.API.Models;
using Microsoft.EntityFrameworkCore;
using MiWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWebAPI.Data
{
    public class EmpleadoData
    {

        private readonly string conexion;
        public EmpleadoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        ///// <summary>
        ///// Consulta lista de empleados
        ///// </summary>
        ///// <returns></returns>
        public async Task<List<Empleado>> GetEmpleados(string filtro)
        {
            List<Empleado> lista = new List<Empleado>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_empleado_selecciona", con);
                cmd.Parameters.AddWithValue("@filtro", filtro);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) {
                        lista.Add(new Empleado
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["NombreCompleto"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            FechaContrato = reader["FechaContrato"].ToString()
                        });
                   }
                }
            }
            return lista;
        }


        /// <summary>
        /// Agrega un empleado
        /// </summary>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public async Task<RespuestaDB> AddEmpleado(Empleado objeto)
        {
            var resultado = new RespuestaDB();

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_empleado_agrega", con);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@apellidoPaterno", objeto.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@apellidoMaterno", objeto.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@fechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        resultado.TipoError = Convert.ToInt32(reader["TipoError"]);
                        resultado.Mensaje = reader["Mensaje"].ToString();
                    }
                }
            }
            return resultado;
        }

        ///// <summary>
        ///// Actualiza un empleado
        ///// </summary>
        ///// <param name="objeto"></param>
        ///// <returns></returns>
        public async Task<RespuestaDB> UpdateEmpleado(Empleado objeto)
        {
            var resultado = new RespuestaDB();

            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_empleado_actualiza", con);
                cmd.Parameters.AddWithValue("@id", objeto.Id);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@apellidoPaterno", objeto.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@apellidoMaterno", objeto.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        resultado.TipoError = Convert.ToInt32(reader["TipoError"]);
                        resultado.Mensaje = reader["Mensaje"].ToString();
                    }
                }
            }
            return resultado;
        }

        ///// <summary>
        ///// Elimina un empleado
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        public async Task<RespuestaDB> DeleteEmpleado(int id)
        {
            var resultado = new RespuestaDB();

            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_empleado_eliminar", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        resultado.TipoError = Convert.ToInt32(reader["TipoError"]);
                        resultado.Mensaje = reader["Mensaje"].ToString();
                    }
                }
            }
            return resultado;
        }
    }
}
