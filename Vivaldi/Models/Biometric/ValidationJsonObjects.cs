using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.Biometric
{
    public class ValidationJsonObjects
    {
        private String idOperador;

        [JsonProperty("idOperador")]

        public string IdOperador
        {
            get { return idOperador; }
            set { idOperador = value; }
        }

        private String idCliente;

        [JsonProperty("idCliente")]

        public String IdCliente
        {
            get { return idCliente; }
            set { idCliente = value; }
        }

        private String nuipAplicante;

        [JsonProperty("nuipAplicante")]

        public String NuipAplicante
        {
            get { return nuipAplicante; }
            set { nuipAplicante = value; }
        }

        private String sede;

        [JsonProperty("sede")]

        public String Sede
        {
            get { return sede; }
            set { sede = value; }
        }

        private String fechaOperacion;

        [JsonProperty("fechaOperacion")]

        public String FechaOperacion
        {
            get { return fechaOperacion; }
            set { fechaOperacion = value; }
        }

        private String codigoCliente;

        [JsonProperty("codigoCliente")]

        public String CodigoCliente
        {
            get { return codigoCliente; }
            set { codigoCliente = value; }
        }

        private String ipCliente;

        [JsonProperty("ipCliente")]

        public String IpCliente
        {
            get { return ipCliente; }
            set { ipCliente = value; }
        }

        private String macCliente;

        [JsonProperty("macCliente")]

        public String MacCliente
        {
            get { return macCliente; }
            set { macCliente = value; }
        }

        private String consultaPorMovil;

        [JsonProperty("consultaPorMovil")]

        public String ConsultaPorMovil
        {
            get { return consultaPorMovil; }
            set { consultaPorMovil = value; }
        }

        private String latitud;

        [JsonProperty("latitud")]

        public String Latitud
        {
            get { return latitud; }
            set { latitud = value; }
        }

        private String longitud;

        [JsonProperty("longitud")]

        public String Longitud
        {
            get { return longitud; }
            set { longitud = value; }
        }

        private String pdfAutorizacion;

        [JsonProperty("pdfAutorizacion")]

        public String PdfAutorizacion
        {
            get { return pdfAutorizacion; }
            set { pdfAutorizacion = value; }
        }

        private String formatoMinucias;

        [JsonProperty("formatoMinucias")]

        public String FormatoMinucias
        {
            get { return formatoMinucias; }
            set { formatoMinucias = value; }
        }

        private String idDedo1;

        [JsonProperty("idDedo1")]

        public String IdDedo1
        {
            get { return idDedo1; }
            set { idDedo1 = value; }
        }

        private String idDedo2;

        [JsonProperty("idDedo2")]

        public String IdDedo2
        {
            get { return idDedo2; }
            set { idDedo2 = value; }
        }

        private String minuciasDedo1;

        [JsonProperty("minuciasDedo1")]

        public String MinuciasDedo1
        {
            get { return minuciasDedo1; }
            set { minuciasDedo1 = value; }
        }

        private String minuciasDedo2;

        [JsonProperty("minuciasDedo2")]

        public String MinuciasDedo2
        {
            get { return minuciasDedo2; }
            set { minuciasDedo2 = value; }
        }

        private int scoreDedo1;

        [JsonProperty("scoreDedo1")]

        public int ScoreDedo1
        {
            get { return scoreDedo1; }
            set { scoreDedo1 = value; }
        }

        private int scoreDedo2;

        [JsonProperty("scoreDedo2")]

        public int ScoreDedo2
        {
            get { return scoreDedo2; }
            set { scoreDedo2 = value; }
        }

        private String detalleOperacion;

        [JsonProperty("detalleOperacion")]

        public String DetalleOperacion
        {
            get { return detalleOperacion; }
            set { detalleOperacion = value; }
        }

        private Boolean aplicanteEsMenorEdad;

        [JsonProperty("aplicanteEsMenorEdad")]
        public Boolean AplicanteEsMenorEdad
        {
            get { return aplicanteEsMenorEdad; }
            set { aplicanteEsMenorEdad = value; }
        }
    }
}
