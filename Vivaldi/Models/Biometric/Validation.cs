using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.Biometric
{
    public class Validation
    {
        public static string Nut;
        public static string primerNombre;
        public static string segundoNombre;
        public static string particula;
        public static string descripcionParticula;
        public static string primerApellido;
        public static string segundoApellido;
        public static string lugarExpDocumento;
        public static string fechaExpDocumento;
        public static string codigoVigDocumento;
        public static string descripVigDocumento;
        public static string resultadoBusqueda;
        public static string resultadoCotejoHuella1;
        public static string resultadoCotejoHuella2;
        public static string nuipAplicante;
        public static string destinoCancelacion;
        public static int codigoResultado;
        public static string mensajeOperacion;

        private String status;

        public String Status
        {
            get { return status; }
            set { status = value; }
        }

        private String message;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
