
namespace Vivaldi.Helpers
{
    using System;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;

    class Security
    {
        private static X509Certificate2 verifyCert;
        // Llame a este método solo una vez en todo el ciclo de vida
        public void setSSLCertificate()
        {
            try
            {
                // Crear una instancia de verificar certificado para verificar el certificado del servidor
                String AUTHEN_CERT_FILE = @"D:\ProyectoNet\Biometria\BiometriaBanco\BiometriaBanco\Certificates\gd_bundle-g2-g1.crt";
                verifyCert = new X509Certificate2(AUTHEN_CERT_FILE);


                // Establece ServerCertificateValidationCallback en un método
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);
            }
            catch (Exception)
            {
                //capturar excepción
            }
        }

        // Esto se llamará automáticamente en cada solicitud enviada. Acepta el certificado del servidor y verifica
        public bool customXertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPoicyErrors)
        {
            switch (sslPoicyErrors)
            {
                case System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors:
                case System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch:
                case System.Net.Security.SslPolicyErrors.RemoteCertificateNotAvailable:
                    break;
            }

            // Verifique elertCert con el certificado del servidor y devuelva el valor de verificación
            return verifyCert.Verify();
        }
    }
}
