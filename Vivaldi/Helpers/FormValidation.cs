using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivaldi.Models;
using Vivaldi.Models.Authentication;
using Vivaldi.Services;

namespace Vivaldi.Helpers
{
    public class FormValidation
    {
        ApiServiceAutenticacion autenticar = new ApiServiceAutenticacion();
        public async Task<Response> Login(string user, string pass, string token)
        {
            string mesaggesError = "";
            if (user != "" && pass != "" && token != "")
            {
                Authentication resultValidarUsuario = await autenticar.ValidarUsuarioActivo(user);
                if (resultValidarUsuario.UsuarioActivo == "true")
                {
                    return new Response
                    {
                        IsSuccess = true,
                        IsError = false,
                        IsUserActive = true,
                        Message = Messages.SessionExist,
                    };
                }
                else if (resultValidarUsuario.UsuarioActivo == "false")
                {
                    return new Response
                    {
                        IsSuccess = true,
                        IsError = false,
                        IsUserActive = false,
                        };
                    }
                else
                {
                    return new Response
                    {
                        IsSuccess = false,
                        IsError = true,
                        Message = resultValidarUsuario.Message,
                    };
                }
            }
            else
            {
                if (user == "")
                {
                    mesaggesError += "\n" + Messages.EmptyUser;
                }
                if (pass == "")
                {
                    mesaggesError += "\n" + Messages.EmptyPass;
                }
                if (token == "")
                {
                    mesaggesError += "\n" + Messages.EmptyToken;
                }
            }
            return new Response
            {
                IsSuccess = false,
                Message = mesaggesError,
            };
        }

        public Response FormCapturarDatos(string huella)
        {
            string mesaggesError = "";
            if (huella != "")
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = mesaggesError,
                };
            }
            else
            {
                if (huella == "")
                {
                    mesaggesError += "\n" + Messages.EmptyFinger;
                }
            }
            return new Response
            {
                IsSuccess = false,
                Message = mesaggesError,
            };
        }
    }
}
