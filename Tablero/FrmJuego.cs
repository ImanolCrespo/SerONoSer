using CapaNegocio;
using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tablero
{
    public partial class FrmJuego : Form
    {
        int tiempo = 100;
        int cntCorrectas = 0;
        int cntErroneas = 0;
        PreguntasDTO pregunta;
        List<RespuestasDTO> respuestas;
        public FrmJuego()
        {
            InitializeComponent();
        }
        CapaNegocioDSet capaNegocio;
        private void FrmJuego_Load(object sender, EventArgs e)
        {
            foreach (var btn in this.Controls.OfType<Button>())
            {
                if (btn.Name.StartsWith("btnResp"))
                {
                    btn.Enabled = false;
                }
            }

            capaNegocio = new CapaNegocioDSet(out string msjError);
            if (!msjError.Equals("Las respuestas están mal en: \n La relacion de validas-incorrectas está mal en: "))
            {
                MessageBox.Show(msjError);
            }

        }

        private void timer_Tick()
        {
            throw new NotImplementedException();
        }

        private void btnComenzar_Click(object sender, EventArgs e)
        {
            
            pregunta = capaNegocio.PreguntaAleatoria(out string msjError);
            if (!msjError.Equals("Las respuestas están mal en: \n La relacion de validas-incorrectas está mal en: ") && !String.IsNullOrWhiteSpace(msjError))
            {
                MessageBox.Show(msjError);
                this.Close();
            }
            else
            { 
            if (pregunta != null)
            {
                respuestas = pregunta.Respuestas;
                int cnt = 0;
            foreach (var btn in this.Controls.OfType<Button>())
            {
                if (btn.Name.StartsWith("btnResp"))
                {
                    btn.Enabled = true;
                    btn.Text = pregunta.Respuestas[cnt].PosibleRespuesta;
                    btn.BackColor = DefaultBackColor;
                    cnt++;
                }
            }
                cntCorrectas = 0;
                cntErroneas = 0;
                lblEnunciado.Text = pregunta.Enunciado;
                lblNivel.Text = pregunta.Nivel.ToString();                
                lblTiempo.Text = tiempo.ToString();
                tmrTiempoTotal.Interval = 1000;
                tmrTiempoTotal.Start();
            }
            }
        }

        private void btnResp1_Click(object sender, EventArgs e)
        {
            Button btn = (Button) sender;
            if (btn.BackColor != Color.Green && btn.BackColor != Color.Red)
            {
                RespuestasDTO respuesta = respuestas.Where(res => res.PosibleRespuesta.Equals(btn.Text)).FirstOrDefault();

                if (respuesta.Valida)
                {
                    btn.BackColor = Color.Green;
                    lblRespuestaValida.Text = "";
                    cntCorrectas++;
                }
                else
                {
                    lblRespuestaValida.Text = respuesta.Explicacion;
                    btn.BackColor = Color.Red;
                    cntErroneas++;

                }

                if (cntErroneas == 4)
                {
                    tmrTiempoTotal.Stop();
                    DialogResult dr = MessageBox.Show("Has fallado 4 respuestas ¿Quieres seguir jugando?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    cntCorrectas = 0;
                    cntErroneas = 0;
                    foreach (var button in this.Controls.OfType<Button>())
                    {
                        if (btn.Name.StartsWith("btnResp"))
                        {
                            button.BackColor = DefaultBackColor;
                        }
                    }

                    if (dr == DialogResult.Yes)
                    {
                        btnComenzar_Click(sender, e);
                    }
                    else
                    {
                        this.Close();
                    }

                }
                if (cntCorrectas == 8)
                {
                    tmrTiempoTotal.Stop();
                    DialogResult dr = MessageBox.Show("Has acertado las 8 respuestas correctas, ¿Quieres seguir jugando con otra pregunta?", "Atención", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    cntCorrectas = 0;
                    cntErroneas = 0;
                    foreach (var button in this.Controls.OfType<Button>())
                    {
                        if (btn.Name.StartsWith("btnResp"))
                        {
                            button.BackColor = DefaultBackColor;
                        }
                    }
                    if (dr == DialogResult.Yes)
                    {
                        btnComenzar_Click(sender, e);
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }            
        }

        private void btnResp2_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp3_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp4_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp5_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp6_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp7_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp8_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp9_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp10_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp11_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnResp12_Click(object sender, EventArgs e)
        {
            btnResp1_Click(sender, e);
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tmrTiempoTotal_Tick(object sender, EventArgs e)
        {
            tiempo -= 1;
            lblTiempo.Text = tiempo.ToString();
            if (tiempo == 0)
            {
                tmrTiempoTotal.Stop();
                DialogResult dr = MessageBox.Show("Se ha acabado el tiempo. \n ¿Quieres volver a jugar con otra pregunta? El programa no acabará.", ":(", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

                if (dr == DialogResult.Yes)
                {
                    btnComenzar_Click(sender, e);
                }
            }
        }
    }
}