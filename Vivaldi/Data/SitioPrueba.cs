using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivaldi.Domain;

namespace Vivaldi.Data
{
    public class SitioPrueba
    {
        public void InsertarInformacionSitioPrueba(string idSitio, string idPrueba, string porcentaje, string eventoId)
        {
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "INSERT INTO configuracion_sitio_prueba (idSitio, idPrueba, idEvento, porcentaje) VALUES ('" + idSitio + "', '" + idPrueba + "', '" + eventoId + "', '" + porcentaje + "')";

            try
            {
                conexionSQLite.Open();
                comandoSQLite.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al insertar configuracion_sitio_prueba", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }
        }

        // Consulta un registro que contiene la tabla configuracion_sitio_prueba
        public DataTable ObtenerInformacionSitioPrueba(string idSitio, string idPrueba, string eventoId)
        {
            DataTable obtenerPuestos = new DataTable();
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "Select * From configuracion_sitio_prueba Where idSitio = '" + idSitio + "' and idPrueba ='" + idPrueba + "' and idEvento ='" + eventoId + "'";

            try
            {
                conexionSQLite.Open();
                SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(comandoSQLite);
                sqliteDataAdapter.Fill(obtenerPuestos);
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al consultar configuracion_sitio_prueba.", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }
            return obtenerPuestos;
        }

        // Consulta un registro que contiene la tabla salon
        public DataTable ObtenerSalon(int idConfiguracion, string numero_salon)
        {
            DataTable obtenerPuestos = new DataTable();
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "Select * From salon Where idConfiguracion = " + idConfiguracion + " and numero_salon = '" + numero_salon + "'";

            try
            {
                conexionSQLite.Open();
                SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(comandoSQLite);
                sqliteDataAdapter.Fill(obtenerPuestos);
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al consultar salon.", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }
            return obtenerPuestos;
        }

        //Borra un registro de la tabla salon
        public void ActualizarSalon(int idSalon, int idConfiguracion, int filas, int columnas, int numero_asistentes, string posicion_puerta, string orientacion, string tablero)
        {
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "Update salon set filas = " + filas + ", columnas = " + columnas + ", numero_asistentes = " + numero_asistentes + ", posicion_puerta = '" + posicion_puerta + "', orientacion = '" + orientacion + "', tablero = '" + tablero + "' Where idSalon = " + idSalon + " and idConfiguracion = " + idConfiguracion;

            try
            {
                conexionSQLite.Open();
                comandoSQLite.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al actualizar salon", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }
        }

        //Borra todos los registros de la tabla puesto
        public void BorrarPuestos(int idSalon)
        {
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "Delete From puesto Where idSalon =" + idSalon;

            try
            {
                conexionSQLite.Open();
                comandoSQLite.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al borrar puesto", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }
        }

        // Inserta un registro en salon
        public void InsertarSalon(int idConfiguracion, string numero_salon, int filas, int columnas, int numero_asistentes, string posicion_puerta, string orientacion, string tablero)
        {
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "INSERT INTO salon (idConfiguracion, numero_salon, filas, columnas, numero_asistentes, posicion_puerta, orientacion, tablero) VALUES (" + idConfiguracion + ", '" + numero_salon + "', " + filas + ", " + columnas + ", " + numero_asistentes + ", '" + posicion_puerta + "', '" + orientacion + "', '" + tablero + "')";

            try
            {
                conexionSQLite.Open();
                comandoSQLite.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al insertar salon", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }
        }

        // Consulta todo lo que contiene la tabla puestos
        public DataTable ObtenerPuestos(int idSalon)
        {
            DataTable obtenerPuestos = new DataTable();
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "Select * From puesto Where idSalon =" + idSalon;

            try
            {
                conexionSQLite.Open();
                SQLiteDataAdapter sqliteDataAdapter = new SQLiteDataAdapter(comandoSQLite);
                sqliteDataAdapter.Fill(obtenerPuestos);
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al consultar puestos.", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }
            return obtenerPuestos;
        }

        // Consulta si se han validado puestos
        public bool PuestosVerificados(int idSalon)
        {
            bool vPuestos = false;
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "Select * From puesto Where estado <> 0 and idSalon =" + idSalon;

            try
            {
                conexionSQLite.Open();
                SQLiteDataAdapter DB = new SQLiteDataAdapter(comandoSQLite);
                DataSet ds = new DataSet();
                DB.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    vPuestos = true;
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al consultar puestos verificados.", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }

            return vPuestos;
        }

        //Actualiza un puesto
        public void ActualizarPuesto(int idSalon, int numero_puesto, int estado)
        {
            SQLiteConnection conexionSQLite = ConexionBD.EstablecerConexion();
            SQLiteCommand comandoSQLite = new SQLiteCommand();
            comandoSQLite.CommandType = CommandType.Text;
            comandoSQLite.Connection = conexionSQLite;
            comandoSQLite.CommandText = "Update puesto set estado =" + estado + "  Where idSalon =" + idSalon + " and numero_puesto =" + numero_puesto;

            try
            {
                conexionSQLite.Open();
                comandoSQLite.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw new Exception("Ocurrió un error al actualizar puesto", exception);
            }
            finally
            {
                conexionSQLite.Close();
                conexionSQLite.Dispose();
            }
        }
    }
}
