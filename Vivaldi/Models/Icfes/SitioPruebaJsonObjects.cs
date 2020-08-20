using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.Icfes
{
    public class SitioPruebaJsonObjects
    {
        private String idSitio;

        [JsonProperty("idSitio")]

        public string IdSitio
        {
            get { return idSitio; }
            set { idSitio = value; }
        }

        private String idPrueba;

        [JsonProperty("idPrueba")]

        public String IdPrueba
        {
            get { return idPrueba; }
            set { idPrueba = value; }
        }
    }
}
