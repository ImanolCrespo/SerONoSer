using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PreguntasDTO
    {
        public int NumPregunta { get; set; }
        public string Enunciado { get; set; }
        public int Nivel { get; set; }

        public List<RespuestasDTO> Respuestas { get; set; }
    }
}
