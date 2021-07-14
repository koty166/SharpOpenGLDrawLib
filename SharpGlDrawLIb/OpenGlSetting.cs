using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SharpGL;
using System.Windows.Forms;

namespace SharpGLDrawLib
{
    public static class OpenGlSetting
    {
        /// <summary>
        /// Обновляет настройки OpenGl. 3D проекция, камера смотрит в проекцию своих координат на глобальные оси XY, перпендикулярно им
        /// </summary>
        /// <param name="gl">Объект OpenGl, который необходимо обновить</param>
        /// <param name="X">X Координата камеры</param>
        /// <param name="Y">Y Координата камеры</param>
        /// <param name="Z">Z Координата камеры</param>
        /// <param name="Height">Высота рабочей области OpenGL</param>
        /// <param name="Width">Ширина рабочей области OpenGL</param>
        public static void UpdateGL(OpenGL gl,float X, float Y, float Z, float Width, float Height)
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 200.0);

            gl.LookAt(X, Y, Z,    // Позиция самой камеры (x, y, z)
                        X, Y, 0,     // Направление, куда мы смотрим
                        0, 2, 0);    // Верх камеры

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }
        /// <summary>
        /// Устанавливает 2D вид сцены
        /// </summary>
        /// <param name="gl">Объект OpenGl, который необходимо обновить</param>
        /// <param name="X">X Координата точки отсчёта</param>
        /// <param name="Y">Y Координата точки отсчёта</param>
        /// <param name="Height">Высота рабочей области OpenGL</param>
        /// <param name="Width">Ширина рабочей области OpenGL</param>
        public static void Set2d(OpenGL gl, int X, int Y, int Width, int Height)
        {
            gl.Viewport(X,Y, Width, Height);
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Ortho2D(0, Width, 0, Height);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
        }


    }
}
