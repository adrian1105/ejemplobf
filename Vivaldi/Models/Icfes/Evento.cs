using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivaldi.Models.Icfes
{
    public static class Evento
    {
        public static int eventoId;
        public static string nombre;
        public static string horaInicial;
        public static string horaFinal;

    }

    public class Eventos
    {
        public int eventoId { get; set; }
        public string nombre { get; set; }
        public string horaInicial { get; set; }
        public string horaFinal { get; set; }
    }
}
