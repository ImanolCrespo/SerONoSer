using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CapaNegocioDSet
    {
        
        int nivel=0;
        CapaDatosDSet datosDSet;
        List<PreguntasDTO> preguntas;
        public CapaNegocioDSet(out string msjError)
        {
            datosDSet = new CapaDatosDSet(out string msjErrorLlamada);
            msjError = msjErrorLlamada;
            PreguntasDeNivel(1, out string msjErrorPreg);
            if (String.IsNullOrWhiteSpace(msjError))
            {
                msjError = msjErrorPreg;
            }
        }

        private void PreguntasDeNivel(int nivel, out string msjError)
        {
            this.nivel = nivel;
            preguntas = datosDSet.PreguntasDeNivel(nivel, out string msjErr);
            msjError = msjErr;
        }

        public PreguntasDTO PreguntaAleatoria(out string msjErr)
        {
            msjErr = "";
            if (preguntas.Count==0)
            {
                 PreguntasDeNivel(this.nivel + 1, out string msjError);
                 msjErr = msjError;
            }

            PreguntasDTO p=null;
            if (preguntas!=null) { 
                Random random = new Random();
                int aleatorio = random.Next(0, preguntas.Count - 1);
                p = preguntas[aleatorio];
                preguntas.RemoveAt(aleatorio);                          
            }
            return p;
        }


    }
}
