using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;

namespace GLL
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        public SharpGLForm()
        {
            InitializeComponent();
            timer1.Start();
        }

        const float clockRadius = 80.0f;
        const float clockVolume = 100.0f;
        const float angleMin = (float)Math.PI / 30.0f;
        const float minStart = 0.9f;
        const float minEnd = 1.0f;
        const float stepStart = 0.8f;
        const float stepEnd = 1.0f;

        float hourAngle = 0;
        float minAngle = 0;
        float secAngle = 0;

        void newLine(float rStart, float rEnd, float angle, OpenGL gl)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            gl.Vertex(clockRadius * rStart * cos, clockRadius * rStart * sin);
            gl.Vertex(clockRadius * rEnd * cos, clockRadius * rEnd * sin);
        }

        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Load the identity matrix.
            gl.LoadIdentity();

            
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.LineWidth(2.0f);
            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            gl.Enable(OpenGL.GL_POLYGON_SMOOTH);

            //Побудова циферблату годинника
            gl.Begin(OpenGL.GL_LINES);
            for (int i = 0; i < 60; i++)
            {
                if (i % 5 != 0)
                {
                    if (i % 5 == 1)
                        gl.Color(1.0f, 1.0f, 1.0f);
                    newLine(minStart, minEnd, i * angleMin, gl);
                }
                else
                {
                    gl.Color(1.0f, 0.0f, 0.0f);
                    newLine(stepStart, stepEnd, i * angleMin, gl);
                }
            }
            gl.End();

            // Малюємо стрілки
            gl.LineWidth(3.0f);
            gl.Begin(OpenGL.GL_LINES);
            newLine(0.0f, 0.5f, -hourAngle + (float)Math.PI / 2, gl);
            newLine(0.0f, 0.8f, -minAngle + (float)Math.PI / 2, gl);
            gl.End();

            //секундна
            gl.LineWidth(1.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Begin(OpenGL.GL_LINES);
            newLine(0.0f, 0.8f, -secAngle + (float)Math.PI / 2, gl);
            gl.End();
           
        }

        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);
        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  TODO: Set the projection matrix here.
            float aspectRatio;

            if (Height == 0)
                Height = 1;
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            gl.Viewport(0, 0, Width, Height);


            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            aspectRatio = Width / Height;

            if (Width <= Height)
                gl.Ortho(-clockVolume, clockVolume, -clockVolume / aspectRatio, clockVolume / aspectRatio, 1.0, -1.0);
            else
                gl.Ortho(-clockVolume * aspectRatio, clockVolume * aspectRatio, -clockVolume, clockVolume, 1.0, -1.0);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();       
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl.OpenGL;
            //secAngle -= 12.0f;
            secAngle += speedRate;
            minAngle += speedRate / 60;
            hourAngle += speedRate / 360;
        }

        private float speedRate = 0.1f;

        private void button1_Click(object sender, EventArgs e)
        {
            speedRate += 0.1f;
           // timer1.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (speedRate != 0.1f)
            {
                speedRate -= 0.1f;
            }
        }
    }
}
