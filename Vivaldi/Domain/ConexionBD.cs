namespace Vivaldi.Domain
{
    using System;
    using System.Data.SQLite;
    using System.IO;
    public class ConexionBD
    {
        public static SQLiteConnection EstablecerConexion()
        {
            string cadenaConexion = @"Data Source=cliente_icfes.db;Version=3;New=True;Compress=True";

            try
            {
                return new SQLiteConnection(cadenaConexion);
            }
            catch (Exception exception)
            {
                throw new Exception("La conexión a la BD no se pudo realizar.", exception);
            }
        }
    }
}
