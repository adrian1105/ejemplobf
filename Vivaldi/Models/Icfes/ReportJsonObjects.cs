using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.Icfes
{
    public class ReportJsonObjects
    {
        private String sitioId;

        [JsonProperty("sitioId")]

        public string SitioId
        {
            get { return sitioId; }
            set { sitioId = value; }
        }

        private String pruebaId;

        [JsonProperty("pruebaId")]

        public String PruebaId
        {
            get { return pruebaId; }
            set { pruebaId = value; }
        }

        private String tipoDocumentoDeclarado;

        [JsonProperty("tipoDocumentoDeclarado")]

        public String TipoDocumentoDeclarado
        {
            get { return tipoDocumentoDeclarado; }
            set { tipoDocumentoDeclarado = value; }
        }

        private String nroDocumentoDeclarado;

        [JsonProperty("nroDocumentoDeclarado")]

        public String NroDocumentoDeclarado
        {
            get { return nroDocumentoDeclarado; }
            set { nroDocumentoDeclarado = value; }
        }

        private String salon;

        [JsonProperty("salon")]

        public String Salon
        {
            get { return salon; }
            set { salon = value; }
        }

        private String citaSnee;

        [JsonProperty("citaSnee")]

        public String CitaSnee
        {
            get { return citaSnee; }
            set { citaSnee = value; }
        }

        private String motivo;

        [JsonProperty("motivo")]

        public String Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }

        private String observacion;

        [JsonProperty("observacion")]

        public String Observacion
        {
            get { return observacion; }
            set { observacion = value; }
        }

        private String codigoResultado;

        [JsonProperty("codigoResultado")]

        public String CodigoResultado
        {
            get { return codigoResultado; }
            set { codigoResultado = value; }
        }

        private String fechaCapturaHuella;

        [JsonProperty("fechaCapturaHuella")]

        public String FechaCapturaHuella
        {
            get { return fechaCapturaHuella; }
            set { fechaCapturaHuella = value; }
        }

        private String primerNombre;

        [JsonProperty("primerNombre")]

        public String PrimerNombre
        {
            get { return primerNombre; }
            set { primerNombre = value; }
        }

        private String segundoNombre;

        [JsonProperty("segundoNombre")]

        public String SegundoNombre
        {
            get { return segundoNombre; }
            set { segundoNombre = value; }
        }

        private String primerApellido;

        [JsonProperty("primerApellido")]

        public String PrimerApellido
        {
            get { return primerApellido; }
            set { primerApellido = value; }
        }

        private String segundoApellido;

        [JsonProperty("segundoApellido")]

        public String SegundoApellido
        {
            get { return segundoApellido; }
            set { segundoApellido = value; }
        }

        private String particula;

        [JsonProperty("particula")]

        public String Particula
        {
            get { return particula; }
            set { particula = value; }
        }

        private String descripcionParticula;

        [JsonProperty("descripcionParticula")]

        public String DescripcionParticula
        {
            get { return descripcionParticula; }
            set { descripcionParticula = value; }
        }

        private String lugarExpedicionDocumento;

        [JsonProperty("lugarExpedicionDocumento")]

        public String LugarExpedicionDocumento
        {
            get { return lugarExpedicionDocumento; }
            set { lugarExpedicionDocumento = value; }
        }

        private String fechaExpedicionDocumento;

        [JsonProperty("fechaExpedicionDocumento")]

        public String FechaExpedicionDocumento
        {
            get { return fechaExpedicionDocumento; }
            set { fechaExpedicionDocumento = value; }
        }

        private String codigoVigenciaDocumento;

        [JsonProperty("codigoVigenciaDocumento")]

        public String CodigoVigenciaDocumento
        {
            get { return codigoVigenciaDocumento; }
            set { codigoVigenciaDocumento = value; }
        }

        private String descripcionVigenciaDocumento;

        [JsonProperty("descripcionVigenciaDocumento")]

        public String DescripcionVigenciaDocumento
        {
            get { return descripcionVigenciaDocumento; }
            set { descripcionVigenciaDocumento = value; }
        }

        private String resultadoBusqueda;

        [JsonProperty("resultadoBusqueda")]

        public String ResultadoBusqueda
        {
            get { return resultadoBusqueda; }
            set { resultadoBusqueda = value; }
        }

        private String resultadoCotejoHuella1;

        [JsonProperty("resultadoCotejoHuella1")]

        public String ResultadoCotejoHuella1
        {
            get { return resultadoCotejoHuella1; }
            set { resultadoCotejoHuella1 = value; }
        }

        private String resultadoCotejoHuella2;

        [JsonProperty("resultadoCotejoHuella2")]

        public String ResultadoCotejoHuella2
        {
            get { return resultadoCotejoHuella2; }
            set { resultadoCotejoHuella2 = value; }
        }

        private String nuipAplicante;

        [JsonProperty("nuipAplicante")]

        public String NuipAplicante
        {
            get { return nuipAplicante; }
            set { nuipAplicante = value; }
        }

        private String nut;

        [JsonProperty("nut")]

        public String Nut
        {
            get { return nut; }
            set { nut = value; }
        }

        private int eventoId;

        [JsonProperty("eventoId")]

        public int EventoId
        {
            get { return eventoId; }
            set { eventoId = value; }
        }
    }
}
