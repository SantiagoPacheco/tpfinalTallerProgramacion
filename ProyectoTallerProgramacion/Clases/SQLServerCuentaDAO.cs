﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DataTransferObject;
using System.Data;

namespace Clases.Persistencia.SQLServer
{
    /// <summary>
    /// Clase para interactuar con la informacion de la Base de Datos.
    /// </summary>
    public class SQLServerCuentaDAO : ICuentaDAO
    {
        /// <summary>
        /// Instancia de la clase SqlConnection.
        /// </summary>
        private SqlConnection iConexion;

        /// <summary>
        /// Instancia de la clase SqlTransaction.
        /// </summary>
        private SqlTransaction iTransaccion;

        /// <summary>
        /// Constructor de la clase SQLServerCuentaCorreoDAO.
        /// </summary>
        /// <param name="pConexion">Conexión a utilizar.</param>
        /// <param name="pTransaccion">Transacción a utilizar.</param>
        public SQLServerCuentaDAO(SqlConnection pConexion, SqlTransaction pTransaccion)
        {
            this.iConexion = pConexion;
            this.iTransaccion = pTransaccion;
        }

        /// <summary>
        /// Metodo para obtener la lista de cuentas de correo de la Base de Datos.
        /// </summary>
        /// <returns></returns>
        public IList<CuentaDTO> ObtenerCuentas()
        {
            List<CuentaDTO> mCuentasCorreo = new List<CuentaDTO>();
            try
            {
                SqlCommand comando = this.iConexion.CreateCommand();
                comando.CommandText = "select * from Cuenta";
                DataTable tabla = new DataTable();
                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                    foreach (DataRow fila in tabla.Rows)
                    {
                        mCuentasCorreo.Add(new CuentaDTO
                        {
                            Nombre = Convert.ToString(fila["Nombre"]),
                            Direccion = Convert.ToString(fila["Direccion"]),
                            Contraseña = Convert.ToString(fila["Contraseña"]),
                        });
                    }
                }
                return mCuentasCorreo;
            }
            catch (SqlException pSqlException)
            {
                throw new DAOException("Error en la obtención de datos de cuenta de correo", pSqlException);
            }
        }

        /// <summary>
        /// Metodo para insertar la informacion de una cuenta de correo en la Base de Datos.
        /// </summary>
        /// <param name="pCuentaCorreo"></param>
        public void AgregarCuenta(CuentaDTO pCuentaCorreo)
        {
            try
            {
                SqlCommand comando = this.iConexion.CreateCommand();
                comando.CommandText = @"insert into Cuenta(Nombre,Direccion,Contraseña)
                                                   values(@Nombre,@Direccion,@Contraseña)";
                comando.Parameters.AddWithValue("@Nombre", pCuentaCorreo.Nombre);
                comando.Parameters.AddWithValue("@Direccion", pCuentaCorreo.Direccion);
                comando.Parameters.AddWithValue("@Contraseña", pCuentaCorreo.Contraseña);
                comando.Transaction = iTransaccion;
                comando.ExecuteNonQuery();
            }
            catch (SqlException pSqlException)
            {
                new DAOException("Error en la inserción de datos de Cuenta de correo", pSqlException);
            }
        }

        /// <summary>
        /// Metodo para actualizar la informacion de una cuenta de correo de la Base de Datos.
        /// </summary>
        /// <param name="pCuentaCorreo"></param>
        public void ModificarCuenta(CuentaDTO pCuentaCorreo)
        {
            try
            {
                SqlCommand comando = this.iConexion.CreateCommand();
                comando.CommandText = @"update Cuenta set Direccion= @Direccion,
                                                                Contraseña= @Contraseña 
                                                                where Nombre = @Nombre";
                comando.Parameters.AddWithValue("@Nombre", pCuentaCorreo.Nombre);
                comando.Parameters.AddWithValue("@Direccion", pCuentaCorreo.Direccion);
                comando.Parameters.AddWithValue("@Contraseña", pCuentaCorreo.Contraseña);
                comando.Transaction = iTransaccion;
                comando.ExecuteNonQuery();
            }
            catch (SqlException pSqlException)
            {
                throw new DAOException("Error en la actualizacion de datos de cuenta de correo", pSqlException);
            }
        }

        /// <summary>
        /// Metodo para eliminar los datos de una Cuenta de Correo en la Base de Datos.
        /// </summary>
        /// <param name="pCuentaCorreo"></param>
        public void EliminarCuenta(CuentaDTO pCuentaCorreo)
        {
            try
            {
                SqlCommand comando = this.iConexion.CreateCommand();
                comando.CommandText = @"delete from Cuenta where Nombre = @Nombre";
                comando.Parameters.AddWithValue("@ID", pCuentaCorreo.Nombre);
                comando.Transaction = iTransaccion;
                comando.ExecuteNonQuery();
            }
            catch (SqlException pSqlException)
            {
                throw new DAOException("Error en la eliminacion de una cuenta de correo", pSqlException);
            }
        }
    }
}
