﻿using GestorEmpleados.API.Models;
using MiWebAPI.Models;
using GestorEmpleados.API.Models;
using Microsoft.EntityFrameworkCore;
using MiWebAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWebAPI.Data
{
    public class ProveedorData
    {

        private readonly string conexion;
        public ProveedorData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        ///// <summary>
        ///// Consulta lista de empleados
        ///// </summary>
        ///// <returns></returns>
        public async Task<List<Proveedor>> GetProveedor (string filtro)
        {
            List<Proveedor> lista = new List<Proveedor>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_proveedor_selecciona", con);
                cmd.Parameters.AddWithValue("@filtro", filtro);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Proveedor
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["NombreCompleto"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Edad = Convert.ToInt32(reader["Edad"]),
                            Empresa = reader["Empresa"].ToString()
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
        public async Task<RespuestaDB> AddProveedor(Proveedor objeto)
        {
            var resultado = new RespuestaDB();

            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_proveedor_agrega", con);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@apellidoPaterno", objeto.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@apellidoMaterno", objeto.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@direccion", objeto.Direccion);
                cmd.Parameters.AddWithValue("@telefono", objeto.Telefono);
                cmd.Parameters.AddWithValue("@edad", objeto.Edad);
                cmd.Parameters.AddWithValue("@empresa", objeto.Empresa);
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
        public async Task<RespuestaDB> UpdateProveedor(Proveedor objeto)
        {
            var resultado = new RespuestaDB();

            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_proveedor_actualiza", con);
                cmd.Parameters.AddWithValue("@id", objeto.Id);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@apellidoPaterno", objeto.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@apellidoMaterno", objeto.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@direccion", objeto.Direccion);
                cmd.Parameters.AddWithValue("@telefono", objeto.Telefono);
                cmd.Parameters.AddWithValue("@edad", objeto.Edad);
                cmd.Parameters.AddWithValue("@empresa", objeto.Empresa);
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
        public async Task<RespuestaDB> DeleteProveedor(int id)
        {
            var resultado = new RespuestaDB();

            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("sp_proveedor_elimina", con);
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
