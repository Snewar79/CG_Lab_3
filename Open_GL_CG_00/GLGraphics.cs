using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;

namespace Open_GL_CG_00
{
    class GLGraphics
    {

        Vector3 cameraPosition = new Vector3(2, 3, 4);
        Vector3 cameraDirecton = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);


        public float latitude = 47.98f;
        public float longitude = 60.41f;
        public float radius = 5.385f;



        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;

        int vaoHandle;
        int[] vboHandlers = new int[2];

        Vector3[] vertdata = new Vector3[]
        {
            new Vector3(-1f, -1f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(1f, -1f, 0f)
        };



        public void Resize(int width, int height)
        {


            GL.ClearColor(System.Drawing.Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(
             MathHelper.PiOver4,
                width / (float)height,
                1,
                64);


            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);
            InitShaders();

        }

        public void Update()
        {


            cameraPosition = new Vector3(
                (float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Cos(Math.PI / 180.0f * longitude)),
                (float)(radius * Math.Cos(Math.PI / 180.0f * latitude) * Math.Sin(Math.PI / 180.0f * longitude)),
                (float)(radius * Math.Sin(Math.PI / 180.0f * latitude)));


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);
            Render();

        }

        private void drawTestQuad()
        {
            // string glVersion = GL.GetString(StringName.Version);
            // string glslVersion = GL.GetString(StringName.ShadingLanguageVersion);
          /* 
            GL.Begin(PrimitiveType.Triangles);

               
                GL.Color3(Color.Red);
                GL.Vertex3(-1.0f, 1.0f, -1.0f);
                GL.Color3(Color.Green);
                GL.Vertex3(1.0f, 1.0f, -1.0f);
                GL.Color3(Color.Blue);
                GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.End();
            */
            GL.UseProgram(BasicProgramID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
           // openGlControl.SwapBuffers();
            GL.UseProgram(0);
        }

        public void Render()
        {
            drawTestQuad();
            GL.PushMatrix();
            GL.Translate(1, 1, 1);
            GL.Rotate(45, Vector3.UnitZ);
            GL.Scale(0.5f, 0.5f, 0.5f);
            drawTestQuad();
            GL.PopMatrix();
        }



        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (System.IO.StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public void InitShaders()
        {
            // создание объекта программы
            BasicProgramID = GL.CreateProgram();
            loadShader("..\\..\\basic.vert.txt", ShaderType.VertexShader, BasicProgramID,
            out BasicVertexShader);
            loadShader("..\\..\\basic.frag.txt", ShaderType.FragmentShader, BasicProgramID,
            out BasicFragmentShader);
            //Компановка программы
            GL.LinkProgram(BasicProgramID);
            // Проверить успех компановки
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));

            float[] positionData = { -0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, 0.0f, 0.8f, 0.0f };
            float[] colorData = { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };

            GL.GenBuffers(2, vboHandlers);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);

            GL.BufferData(BufferTarget.ArrayBuffer,
                    (IntPtr)(sizeof(float) * positionData.Length),
                    positionData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);

            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(sizeof(float) * colorData.Length),
                colorData, BufferUsageHint.StaticDraw);

            vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            

        }




    }
}
