using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.Authentication
{
    public class Authentication
    {
        private String status;

        public String Status
        {
            get { return status; }
            set { status = value; }
        }

        private String statusToken;

        public String StatusToken
        {
            get { return statusToken; }
            set { statusToken = value; }
        }

        private String message;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }

        private String token;

        public String Token
        {
            get { return token; }
            set { token = value; }
        }

        private String messageToken;

        public String MessageToken
        {
            get { return messageToken; }
            set { messageToken = value; }
        }

        private String statusCambio;

        public String StatusCambio
        {
            get { return statusCambio; }
            set { statusCambio = value; }
        }

        private String validacionValue;

        public String ValidacionValue
        {
            get { return validacionValue; }
            set { validacionValue = value; }
        }

        private String statusTokenOtp;

        public String StatusTokenOtp
        {
            get { return statusTokenOtp; }
            set { statusTokenOtp = value; }
        }

        private String usuarioActivo;

        public String UsuarioActivo
        {
            get { return usuarioActivo; }
            set { usuarioActivo = value; }
        }
    }
}
