using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGLDrawLib;
using SharpGL.SceneGraph.Assets;

namespace SharpGLDrawLib
{
    public partial class Scene2D : UserControl
    {
        public Scene2D()
        {
            InitializeComponent();
        }
        protected bool IsGLInit = false;
        protected bool IsMouseOnOpenGL = false;
        protected bool IsMouseDown = false;
        public MouseButtons MouseButtonToMove = MouseButtons.Middle;
        protected void InitOpenGL(int x,int y,int Width,int Height)
        {
            OpenGlSetting.Set2d(Main.OpenGL,x,y,Width,Height);
            IsGLInit = true;
        }

        private void Main_MouseLeave(object sender, EventArgs e)
        { 
            IsMouseOnOpenGL = false;
        }

        private void Main_MouseEnter(object sender, EventArgs e)
        {
            IsMouseOnOpenGL = true;
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            if(MouseButtonToMove == e.Button)
            IsMouseDown = true;
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseButtonToMove == e.Button)
                IsMouseDown = false;
        }

        private void Scene2D_Resize(object sender, EventArgs e)
        {
            Main.Refresh();
        }

        private void Scene2D_SizeChanged(object sender, EventArgs e)
        {
            InitOpenGL(0, 0, Width, Height);
        }
    }
}
