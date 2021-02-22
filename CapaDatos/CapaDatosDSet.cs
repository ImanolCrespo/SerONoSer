using CapaDatos.DsSerONoSerTableAdapters;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CapaDatosDSet
    {
        public CapaDatosDSet(out string msjErrorLlamada)
        {
            msjErrorLlamada = LlenarTablas();
            
        }
        DsSerONoSer dsSerONoSer = new DsSerONoSer();

        private String LlenarTablas()
        {
            PreguntasTableAdapter daPreguntas = new PreguntasTableAdapter();
            RespuestasTableAdapter daRespuestas = new RespuestasTableAdapter();
            RespNoValidasTableAdapter daRespNoValidas = new RespNoValidasTableAdapter();

            try
            {
                daPreguntas.Fill(dsSerONoSer.Preguntas);
                daRespuestas.Fill(dsSerONoSer.Respuestas);
                daRespNoValidas.Fill(dsSerONoSer.RespNoValidas);

                foreach (var respNo in dsSerONoSer.RespNoValidas)
                {
                    if (respNo.IsExplicacionNull())
                    {
                        respNo.Explicacion = "Errónea, aunque no sabemos el motivo, debes investigarlo";
                    }
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            return "";
        }

        public List<PreguntasDTO> PreguntasDeNivel(int nivel,   out string msjError)
        {
            int max = dsSerONoSer.Preguntas.Max(res => res.Nivel);
            if (nivel>max)
            {
                msjError = $"No hay preguntas de este nivel, el nivel más alto es {max}";
                return null;
            }

            int nPreg = dsSerONoSer.Preguntas.Count(preg => preg.Nivel == nivel);
            if (nPreg==0)
            {
                msjError = $"No hay preguntas de ese nivel";
                return null;
            }
            List<PreguntasDTO> preguntas = dsSerONoSer.Preguntas.Where(preg=> preg.Nivel == nivel).Select(preg => new PreguntasDTO { Enunciado = preg.Enunciado, Nivel = preg.Nivel, NumPregunta = preg.NumPregunta, Respuestas = ListaRespuestas(preg.NumPregunta) }).ToList();



            String error = "Las respuestas están mal en: ";
            List<PreguntasDTO> preguntasRespMal = preguntas.Where(preg => preg.Respuestas.Count != 12).ToList();
            if (preguntasRespMal.Count!=0)
            {
                foreach (var p in preguntasRespMal)
                {
                    error = error + $", \"{p.NumPregunta}\" ";
                }
            }


            error = error + $"\n La relacion de validas-incorrectas está mal en: ";
            var preguntasRelMal = preguntas.Select(preg => preg.Respuestas).ToList();

            for (int i = 0; i < preguntasRelMal.Count(); i++)
            {
                int cnt=0;
                foreach (var res in preguntasRelMal[i])
                {
                    if (res.Valida)
                    {
                        cnt++;
                    }
                }
                if (cnt!=8)
                {
                    error = error + $", \"{preguntas[i].NumPregunta}\"";
                }
            }

            msjError = error;
            return preguntas;


        } 

        private List<RespuestasDTO> ListaRespuestas(int numPreg)
        {
            var respNoValidas = dsSerONoSer.RespNoValidas.Where(resNo => resNo.NumPregunta == numPreg).ToList();

            var respuestas = dsSerONoSer.Respuestas.Where(res => res.NumPregunta == numPreg).ToList();
             
            //return respuestas.Join(respNoValidas, resp => resp.NumRespuesta, resNo => resNo.NumRespuesta, (resp, resNo) => new RespuestasDTO { Explicacion = resNo.Explicacion, NumRespuesta = resp.NumRespuesta, PosibleRespuesta = resp.PosibleRespuesta, Valida = resp.Valida }).ToList();


            return dsSerONoSer.Respuestas.Where(res=> res.NumPregunta == numPreg).Select(resp=>  new RespuestasDTO { Explicacion = Explicacion(resp), NumRespuesta = resp.NumRespuesta, PosibleRespuesta = resp.PosibleRespuesta, Valida = resp.Valida }).Take(12).ToList();


        }
        private String Explicacion(DsSerONoSer.RespuestasRow respuestasRow)
        {
            //var res = respuestasRow.GetRespNoValidasRows
          
           var res = dsSerONoSer.RespNoValidas.Where(resNo => resNo.NumRespuesta == respuestasRow.NumRespuesta && resNo.NumPregunta == respuestasRow.NumPregunta).Select(resNo=> resNo.Explicacion).FirstOrDefault();
           return res; 
        }
    }
}
