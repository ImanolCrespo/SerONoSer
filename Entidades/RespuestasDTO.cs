using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RespuestasDTO
    {
        public int NumRespuesta { get; set; }
        public string PosibleRespuesta { get; set; }
        public Boolean Valida { get; set; }
        public string Explicacion { get; set; }
    }
}
