using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace RayTracing
{
    public partial class Form1 : Form
    {
        Graphics gr;
        RayTracing scene;
        public Form1()
        {
            InitializeComponent();
            gr = new Graphics();
            scene = new RayTracing();
        }

        
private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            // gr.Update();
            scene.Update();

            glControl1.SwapBuffers();
            //gr.closeProgram();
            scene.closeProgram();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            scene.Resize(glControl1.Width, glControl1.Height);
            Application.Idle += Application_Idle;
            //gr.Resize(glControl1.Width, glControl1.Height);
        }


        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
                glControl1.Refresh();
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            float heightCoef = (-e.Y + glControl1.Height) / (float)glControl1.Height;
            float  widthCoef = (e.X - glControl1.Width * 0.5f) / (float)glControl1.Width;
            scene.latitude = widthCoef * 360; // Широта
            scene.longitude = heightCoef * 90; //Долгота
        }
    }
}
