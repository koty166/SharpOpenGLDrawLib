using System;
using System.Drawing;
using System.Drawing.Imaging;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using System.Runtime.InteropServices;

namespace SharpGLDrawLib
{
    /// <summary>
    /// Представляет удобные методы для рисования графических примитивов OpenGl.
    /// Инкапсулирует методы OpenGL.
    /// </summary>
    public class Drawer
    {
        /// <summary>
        /// Рисует квадрат
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="X">X координата базовой точки</param>
        /// <param name="Y">Y координата базовой точки</param>
        /// <param name="Heigth">Высота квадрата</param>
        /// <param name="Width">Ширина квадрата</param>
        /// <param name="Z">Z координата примитива</param>
        public static void DrawQuard(OpenGL Drawer, Color Color, float X, float Y, float Heigth, float Width, float Z = 0)
        {
            Drawer.Begin(OpenGL.GL_LINE_LOOP);

            Drawer.Color(Color.R, Color.G, Color.B);

            Drawer.Vertex(X, Y, Z);
            Drawer.Vertex(X, Y + Heigth, Z);
            Drawer.Vertex(X + Width, Y + Heigth, Z);
            Drawer.Vertex(X + Width, Y, Z);

            Drawer.End();
        }

        /// <summary>
        /// Рисует четырёхугольники. Требует число точек, кратное 4.
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="Points">Массив вершин четырёхугольников</param>
        /// <param name="Z">Z координата примитива</param>
        public static void DrawRectangles(OpenGL Drawer, Color Color, PointF[] Points, float Z = 0)
        {
            if (Points.Length % 4 != 0)
                return;
            Drawer.Color(Color.R, Color.G, Color.B);
            for (int i = 0; i < Points.Length; i += 4)
            {
                Drawer.Begin(OpenGL.GL_LINE_LOOP);
                Drawer.Vertex(Points[i].X, Points[i].Y, Z);
                Drawer.Vertex(Points[i + 1].X, Points[i + 1].Y, Z);
                Drawer.Vertex(Points[i + 2].X, Points[i + 2].Y, Z);
                Drawer.Vertex(Points[i + 3].X, Points[i + 3].Y, Z);
                Drawer.End();
            }

        }

        /// <summary>
        /// Соеденяет все точки в замкнутый многоугольник
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="Points">Массив вершин многоугольника</param>
        /// <param name="Z">Z координата примитива</param>
        public static void DrawLinesLoop(OpenGL Drawer, Color Color, PointF[] Points, float Z = 0)
        {
            Drawer.Begin(OpenGL.GL_LINE_LOOP);
            Drawer.Color(Color.R, Color.G, Color.B);
            foreach (var item in Points)
            {
                Drawer.Vertex(item.X,item.Y,Z);
            }
            Drawer.End();
        }


        /// <summary>
        /// Рисует куб
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="X">X координата примитива</param>
        /// <param name="Y">Y координата примитива</param>
        /// <param name="Z">Z координата примитива</param>
        /// <param name="Heigth">Высота куба</param>
        /// <param name="Width">Ширина куба</param>
        /// <param name="Depth">Глубина(Z) куба</param>
        public static void DrawQube(OpenGL Drawer, Color Color, float X, float Y, float Z, float Heigth, float Width, float Depth)
        {
            Drawer.Begin(OpenGL.GL_QUADS);

            Drawer.Color(Color.R, Color.G, Color.B);

            Drawer.Vertex(X, Y, Z);
            Drawer.Vertex(X, Y + Heigth, Z);
            Drawer.Vertex(X + Width, Y + Heigth, Z);
            Drawer.Vertex(X + Width, Y, Z);

            Drawer.Vertex(X, Y, Z + Depth);
            Drawer.Vertex(X, Y + Heigth, Z + Depth);
            Drawer.Vertex(X + Width, Y + Heigth, Z + Depth);
            Drawer.Vertex(X + Width, Y, Z + Depth);


            Drawer.Vertex(X, Y, Z);
            Drawer.Vertex(X, Y, Z + Depth);
            Drawer.Vertex(X, Y + Heigth, Z + Depth);
            Drawer.Vertex(X, Y + Heigth, Z);


            Drawer.Vertex(X + Width, Y, Z);
            Drawer.Vertex(X + Width, Y + Heigth, Z);
            Drawer.Vertex(X + Width, Y + Width, Z + Depth);
            Drawer.Vertex(X + Width, Y, Z + Depth);


            Drawer.Vertex(X, Y, Z);
            Drawer.Vertex(X, Y, Z + Depth);
            Drawer.Vertex(X + Width, Y, Z + Depth);
            Drawer.Vertex(X + Width, Y, Z);


            Drawer.Vertex(X, Y + Heigth, Z);
            Drawer.Vertex(X, Y + Heigth, Z + Depth);
            Drawer.Vertex(X + Width, Y + Heigth, Z + Depth);
            Drawer.Vertex(X + Width, Y + Heigth, Z);


            Drawer.End();
        }

        /// <summary>
        /// Рисует круг
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="X">X координата примитива</param>
        /// <param name="Y">Y координата примитива</param>
        /// <param name="Z">Z координата примитива</param>
        /// <param name="R">Радиус круга</param>
        public static void DrawCircle(OpenGL Drawer, Color Color, float X, float Y, float Z, float R)
        {
            Drawer.Begin(OpenGL.GL_LINE_LOOP);


            Drawer.Color(Color.R, Color.G, Color.B);
            for (float i = (float)(Math.PI / 2); i < (Math.PI * 2 + Math.PI / 2); i += (float)Math.PI / 180)
            {
                Drawer.Vertex(X + Math.Cos(i) * R, Y + Math.Sin(i) * R, Z);
            }
            Drawer.End();

        }

        /// <summary>
        /// Соеденяет точки в незамкнутый многоугольник
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="Points">Массив вершин многоугольника</param>
        public static void DrawPolygon(OpenGL Drawer, Color Color, PointF[] Points)
        {
            Drawer.Begin(OpenGL.GL_LINE_STRIP);

            Drawer.Color(Color.R, Color.G, Color.B);
            foreach (var i in Points)
            {
                Drawer.Vertex(i.X, i.Y);
            }

            Drawer.End();
        }

        /// <summary>
        /// Рисует координатную систему с возможностью подписи промежуточных значений.
        /// Максимальные значения по осям приняты за их длину и высоту. 
        /// Рисует только 1 координатную четверть. 
        /// <para>
        /// Подписывает промежуточные значения и не пишет ноль. 
        /// Начало координат распоожено в (0,0). 
        /// Длина чёрточек принята за 6. 
        /// Колл-во промежуточных значений равно 4.
        /// </para>
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Width">Ширина системы координат. Макс длина в координатах opengl</param>
        /// <param name="Height">Высота системы координат. Макс высота в координатах opengl</param>

        public static void DrawCoordSystem(OpenGL Drawer, float Width, float Height)
        {
            DrawCoordSystem(Drawer, Color.White, Width, Height, 4, 4, 6, Width, Height, 0, 0, true, false,true);
        }

        /// <summary>
        /// Рисует координатную систему с возможностью подписи промежуточных значений.
        /// Максимальные значения по осям приняты за их длину и высоту. 
        /// Рисует только 1 координатную четверть. 
        /// <para>
        /// Подписывает промежуточные значения и не пишет ноль. 
        /// Начало координат распоожено в (0,0). 
        /// Длина чёрточек принята за 6.
        /// </para>
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Width">Ширина системы координат. Макс длина в координатах opengl</param>
        /// <param name="Height">Высота системы координат. Макс высота в координатах opengl</param>
        /// <param name="XSteps">Колл-во промежуточных значений по оси OX</param>
        /// <param name="YSteps">Колл-во промежуточных значений по оси OН</param>

        public static void DrawCoordSystem(OpenGL Drawer, float Width, float Height, int XSteps, int YSteps)
        {
            DrawCoordSystem(Drawer, Color.White, Width, Height, XSteps, YSteps, 6, Width, Height, 0, 0, true, false,true);
        }

        /// <summary>
        /// Рисует координатную систему с возможностью подписи промежуточных значений.
        /// Максимальные значения по осям приняты за их длину и высоту. 
        /// Рисует только 1 координатную четверть.
        /// <para>
        /// Подписывает промежуточные значения и не пишет ноль. 
        /// Начало координат распоожено в (0,0).
        /// </para>
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Width">Ширина системы координат. Макс длина в координатах opengl</param>
        /// <param name="Height">Высота системы координат. Макс высота в координатах opengl</param>
        /// <param name="XSteps">Колл-во промежуточных значений по оси OX</param>
        /// <param name="YSteps">Колл-во промежуточных значений по оси OН</param>
        /// <param name="Length">Длина чёрточки промужуточных значений</param>

        public static void DrawCoordSystem(OpenGL Drawer, float Width, float Height, int XSteps, int YSteps, float Length)
        {
            DrawCoordSystem(Drawer, Color.White, Width, Height, XSteps, YSteps, Length, Width, Height, 0, 0, true, false,true);
        }

        /// <summary>
        /// Рисует координатную систему с возможностью подписи промежуточных значений. 
        /// Максимальные значения по осям приняты за их длину и высоту. 
        /// Рисует только 1 координатную четверть.
        /// <para>
        /// Подписывает промежуточные значения и не пишет ноль.
        /// </para>
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Width">Ширина системы координат. Макс длина в координатах opengl</param>
        /// <param name="Height">Высота системы координат. Макс высота в координатах opengl</param>
        /// <param name="XSteps">Колл-во промежуточных значений по оси OX</param>
        /// <param name="YSteps">Колл-во промежуточных значений по оси OН</param>
        /// <param name="Length">Длина чёрточки промужуточных значений</param>
        /// <param name="X">X координата начала системы координат</param>
        /// <param name="Y">Y координата начала системы координат</param>

        public static void DrawCoordSystem(OpenGL Drawer, float Width, float Height, int XSteps, int YSteps, float Length, float X, float Y)
        {
            DrawCoordSystem(Drawer,Color.White,Width,Height,XSteps,YSteps,Length,Width,Height,X,Y,true,false,true);
        }

        /// <summary>
        /// Рисует координатную систему с возможностью подписи промежуточных значений. 
        /// Максимальные значения по осям приняты за их длину и высоту. 
        /// Рисует только 1 координатную четверть.
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет системы координат</param>
        /// <param name="Width">Ширина системы координат. Макс длина в координатах opengl</param>
        /// <param name="Height">Высота системы координат. Макс высота в координатах opengl</param>
        /// <param name="XSteps">Колл-во промежуточных значений по оси OX</param>
        /// <param name="YSteps">Колл-во промежуточных значений по оси OН</param>
        /// <param name="Length">Длина чёрточки промужуточных значений</param>
        /// <param name="X">X координата начала системы координат</param>
        /// <param name="Y">Y координата начала системы координат</param>
        /// <param name="IsWriteSteps">Подписывать ли промежуточные значения</param>
        /// <param name="IsWriteZero">Попискать ли ноль системы координат</param>
        public static void DrawCoordSystem(OpenGL Drawer, Color Color, float Width, float Height, int XSteps, int YSteps, float Length, float X, float Y, bool IsWriteSteps, bool IsWriteZero)
        {
            DrawCoordSystem(Drawer, Color, Width, Height, XSteps, YSteps, Length, Width, Height, X, Y, IsWriteSteps, IsWriteZero,true);
        }
       
        /// <summary>
        /// Рисует координатную систему с возможностью подписи промежуточных значений. 
        /// Максимальные значения по осям приняты за их длину и высоту.
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет системы координат</param>
        /// <param name="Width">Ширина системы координат. Макс длина в координатах opengl</param>
        /// <param name="Height">Высота системы координат. Макс высота в координатах opengl</param>
        /// <param name="XSteps">Колл-во промежуточных значений по оси OX</param>
        /// <param name="YSteps">Колл-во промежуточных значений по оси OН</param>
        /// <param name="Length">Длина чёрточки промужуточных значений</param>
        /// <param name="X">X координата начала системы координат</param>
        /// <param name="Y">Y координата начала системы координат</param>
        /// <param name="IsWriteSteps">Подписывать ли промежуточные значения</param>
        /// <param name="IsWriteZero">Попискать ли ноль системы координат</param>
        /// <param name="IsOnlyPositiveCoordValue">Рисовать ли только 1 координатную четверть</param>
        public static void DrawCoordSystem(OpenGL Drawer, Color Color, float Width, float Height, int XSteps, int YSteps, float Length, float X, float Y, bool IsWriteSteps, bool IsWriteZero, bool IsOnlyPositiveCoordValue)
        {
            DrawCoordSystem(Drawer, Color, Width, Height, XSteps, YSteps, Length, Width, Height, X, Y, IsWriteSteps, IsWriteZero, IsOnlyPositiveCoordValue);
        }

        /// <summary>
        /// Рисует координатную систему с возможностью подписи промежуточных значений.
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет системы координат</param>
        /// <param name="Width">Ширина системы координат. Макс длина в координатах opengl</param>
        /// <param name="Height">Высота системы координат. Макс высота в координатах opengl</param>
        /// <param name="XSteps">Колл-во промежуточных значений по оси OX</param>
        /// <param name="YSteps">Колл-во промежуточных значений по оси OН</param>
        /// <param name="Length">Длина чёрточки промежуточных значений</param>
        /// <param name="XMaxValue">Максимальное значение по оси OX, относительно этого значения будут рассчитаны промежуточные значения подписей</param>
        /// <param name="YMaxValue">Максимальное значение по оси OY, относительно этого значения будут рассчитаны промежуточные значения подписей</param>
        /// <param name="X">X координата начала системы координат</param>
        /// <param name="Y">Y координата начала системы координат</param>
        /// <param name="IsWriteSteps">Подписывать ли промежуточные значения</param>
        /// <param name="IsWriteZero">Попискать ли ноль системы координат</param>
        /// <param name="IsOnlyPositiveCoordValue">Рисовать ли только 1 координатную четверть</param>
        public static void DrawCoordSystem(OpenGL Drawer, Color Color, float Width, float Height, int XSteps, int YSteps, float Length, float XMaxValue, float YMaxValue, float X, float Y, bool IsWriteSteps, bool IsWriteZero,bool IsOnlyPositiveCoordValue)
        {
            DrawBaseLines(Drawer, Color, Width, Height, X, Y,IsOnlyPositiveCoordValue);

            float[] XPos = new float[XSteps + 1];
            float[] YPos = new float[YSteps + 1];

            for (int i = 0; i < XPos.Length; i++)
                XPos[i] = (Width / XSteps) * i;
            for (int i = 0; i < YPos.Length; i++)
                YPos[i] = (Height / YSteps) * i;

            Drawer.Begin(OpenGL.GL_LINES);

            Drawer.Color(Color.R, Color.G, Color.B);
            for (int i = 1; i < YPos.Length; i++)
            {
                Drawer.Vertex(-Length / 2 + X, YPos[i] + Y);
                Drawer.Vertex(Length / 2 + X, YPos[i] + Y);
            }
            for (int i = 1; i < XPos.Length; i++)
            {
                Drawer.Vertex(XPos[i] + X, -Length / 2 + Y);
                Drawer.Vertex(XPos[i] + X, Length / 2 + Y);
            }
            Drawer.End();


            
            if (IsWriteSteps)
            {
                for (int i = 0; i < YPos.Length; i++)
                {

                    if (YPos[i] == 0)
                    {
                        if (IsWriteZero)
                            Drawer.DrawText((int)(-Length + X), (int)(YPos[i] + Y), Color.R, Color.G, Color.B, "Arial", 10, "0");
                        continue;
                    }
                    Drawer.DrawText((int)(-Length * 8 + X), (int)(YPos[i] + Y), Color.R, Color.G, Color.B, "Arial", 10, Math.Round((YMaxValue / YSteps * i), 3).ToString());
                }
                for (int i = 0; i < XPos.Length; i++)
                {
                    if (XPos[i] == 0)
                    {
                        if (IsWriteZero)
                            Drawer.DrawText((int)(XPos[i] + X), (int)(-Length + Y), Color.R, Color.G, Color.B, "Arial", 10, "0");
                        continue;
                    }

                    Drawer.DrawText((int)(XPos[i] + X), (int)(-Length * 3 + Y), Color.R, Color.G, Color.B, "Arial", 10, Math.Round((XMaxValue / XSteps * i), 3).ToString());
                }
            }


        }

        /// <summary>
        /// Рисует 1 четверть системы координат. Начало расположено в (0,0). Белый цвет.
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="MaxX">Максимальное значение X</param>
        /// <param name="MaxY">Максимальное значение Y</param>
        public static void DrawBaseLines(OpenGL Drawer, float MaxX, float MaxY)
        {
            DrawBaseLines(Drawer, Color.White, MaxX, MaxY, 0, 0, true);
        }

        /// <summary>
        /// Рисует 1 четверть системы координат. Начало расположено в (0,0)
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="MaxX">Максимальное значение X</param>
        /// <param name="MaxY">Максимальное значение Y</param>
        /// 
        public static void DrawBaseLines(OpenGL Drawer, Color Color, float MaxX, float MaxY)
        {
            DrawBaseLines(Drawer, Color, MaxX, MaxY, 0, 0, true);
        }

        /// <summary>
        /// Рисует координатные оси.
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="MaxX">Максимальное значение X</param>
        /// <param name="MaxY">Максимальное значение Y</param>
        /// <param name="X">X координата примитива</param>
        /// <param name="Y">Y координата примитива</param>
        /// <param name="IsOnlyPositiveCoordValue">Рисовать ли только 1 координатную четверть</param>
        public static void DrawBaseLines(OpenGL Drawer, Color Color, float MaxX, float MaxY, float X = 0, float Y = 0,bool IsOnlyPositiveCoordValue=false)
        {
            Drawer.Color(Color.R, Color.G, Color.B);

            Drawer.Begin(OpenGL.GL_LINES);
            if (IsOnlyPositiveCoordValue)
            {
                Drawer.Vertex(X, Y);
                Drawer.Vertex(X + MaxX, Y);
                Drawer.Vertex(X, Y);
                Drawer.Vertex(X, Y + MaxY);
            }
            else
            {
                Drawer.Vertex(X - MaxX, Y);
                Drawer.Vertex(X + MaxX, Y);
                Drawer.Vertex(X, Y - MaxY);
                Drawer.Vertex(X, Y + MaxY);
            }
            Drawer.End();
        }

        /// <summary>
        /// Рисует точеки.
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="Points">Массив точек</param>
        public static void DrawPoints(OpenGL Drawer, Color Color, PointF[] Points)
        {
            Drawer.Begin(OpenGL.GL_POINTS);

            Drawer.Color(Color.R, Color.G, Color.B);
            foreach (var i in Points)
            {
                Drawer.Vertex(i.X, i.Y);
            }

            Drawer.End();
        }

        /// <summary>
        /// Рисует и закрашивает многоугольник
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="Points">Массив вершин многоугольника</param>
        public static void FillPolygon(OpenGL Drawer, Color Color, PointF[] Points,float Z = 0)
        {
            Drawer.Begin(OpenGL.GL_POLYGON);


            Drawer.Color(Color.R, Color.G, Color.B);
            foreach (var i in Points)
                Drawer.Vertex(i.X, i.Y,Z);

            Drawer.End();
        }

        /// <summary>
        /// Рисует и закрашивает квадрат
        /// </summary>
        /// <param name="Drawer">Объект OpenGl</param>
        /// <param name="Color">Цвет примитива</param>
        /// <param name="X">X координата примитива</param>
        /// <param name="Y">Y координата примитива</param>
        /// <param name="Z">Z координата примитива</param>
        /// <param name="Heigth">Высота квадрата</param>
        /// <param name="Width">Ширина квадрата</param>
        public static void FillQuard(OpenGL Drawer, Color Color, float X, float Y, float Heigth, float Width, float Z = 0)
        {
            Drawer.Begin(OpenGL.GL_QUADS);

            Drawer.Color(Color.R, Color.G, Color.B);
            Drawer.Vertex(X, Y, Z);
            Drawer.Vertex(X, Y + Heigth, Z);
            Drawer.Vertex(X + Width, Y + Heigth, Z);
            Drawer.Vertex(X + Width, Y, Z);

            Drawer.End();
        }

        /// <summary>
        /// Рисует заданную текстуру.
        /// </summary>
        /// <param name="gl">Объект OpenGL</param>
        /// <param name="Im">Текстура, которая будет нарисованна</param>
        /// <param name="X">X координата базовой точки</param>
        /// <param name="Y">Y координата базовой точки</param>
        /// <param name="Height">Высота текстуры в пространстве OpenGL</param>
        /// <param name="Width">Ширина текстуры в пространстве OpenGL</param>
        /// <param name="Z">Z координата базовой точки</param>
        public static void DrawImage(OpenGL gl, Texture Im, float X, float Y, float Height, float Width, float Z = 0)
        {
            Im.Bind(gl);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
           
            gl.Begin(OpenGL.GL_QUADS);

            gl.TexCoord(0, 0); gl.Vertex(X, Y);
            gl.TexCoord(1, 0); gl.Vertex(X + Width, Y);
            gl.TexCoord(1, 1); gl.Vertex(X + Width, Y + Height);
            gl.TexCoord(0, 1); gl.Vertex(X, Y + Height);
            gl.End();
            gl.Disable(OpenGL.GL_TEXTURE_2D);

        }

        /// <summary>
        /// Рисует заданную текстуру.
        /// </summary>
        /// <param name="gl">Объект OpenGL</param>
        /// <param name="Im">Картинка, которая будет нарисованна</param>
        /// <param name="X">X координата базовой точки</param>
        /// <param name="Y">Y координата базовой точки</param>
        /// <param name="Heigth">Высота текстуры в пространстве OpenGL</param>
        /// <param name="Width">Ширина текстуры в пространстве OpenGL</param>
        /// <param name="Z">Z координата базовой точки</param>
        public static void DrawImage(OpenGL gl, Bitmap Im, float X, float Y, float Heigth, float Width, float Z = 0)
        { 
            Texture T = new Texture();
            T.Create(gl,Im);
            DrawImage(gl,T,X,Y,Heigth,Width,Z);

        }
        static void swap(ref byte a,ref byte b)
        {
            byte c=a;
            a = b;
            b = c;

        }
        public static byte[] SwapInArr(byte[] arr,int height,int L)
        {
            for (int k = 0; k < height / 2 * L; k++)
            {
                int i0 = k / L;
                int j0 = k % L;
                int add = (height-1 - i0) * L + j0;
                swap(ref arr[k], ref arr[add]);
            }
            return arr;
        }
        public static IntPtr GetPtr (BitmapData ImData)
        {
            byte[,] data = new byte[ImData.Height, ImData.Stride];
            byte[] byf = new byte[Math.Abs(ImData.Stride) * ImData.Height];

            Marshal.Copy(ImData.Scan0, byf, 0, Math.Abs(ImData.Stride) * ImData.Height);

            byf = SwapInArr(byf, ImData.Height, ImData.Stride);

            Marshal.Copy(byf, 0, ImData.Scan0, byf.Length);
            return ImData.Scan0;
        }
        public static void DrawImageByPixels(OpenGL gl, Bitmap Im, float X, float Y, float d, float aa, BitmapData ImData)
        {

            GetPtr(ImData);

            gl.RasterPos(X, Y,0);
            gl.PixelZoom(1, 1);
            gl.PixelStore(OpenGL.GL_UNPACK_ALIGNMENT, 1);
            gl.DrawPixels(ImData.Width,ImData.Height,OpenGL.GL_RGB,OpenGL.GL_UNSIGNED_BYTE, ImData.Scan0);

        }
        public static void DrawImageByPixels(OpenGL gl, IntPtr Addr, float X, float Y, int Width, int Height,float scale)
        {
            if (X < 0 || Y < 0)
            {
                gl.RasterPos(0, 0);
                gl.Bitmap(0, 0, 0, 0, X, Y, null);
            }
            else
                gl.RasterPos(X, Y);
            gl.PixelZoom(1*scale, 1*scale);
            gl.PixelStore(OpenGL.GL_UNPACK_ALIGNMENT, 4);
            gl.DrawPixels(Width, Height, OpenGL.GL_RGB, OpenGL.GL_UNSIGNED_BYTE, Addr);

        }
    }
}
