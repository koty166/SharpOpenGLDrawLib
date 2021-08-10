using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpGL.SceneGraph.Assets;
using SharpGL;
using SharpGLDrawLib;
using System.Windows.Forms;
using System.Drawing.Imaging;

using System.Diagnostics;

namespace SharpGLDrawLib
{
    /// <summary>
    /// Предоставляет базовые функции для отрисовки и перемещения 2d картинки в пространстве OpenGl
    /// </summary>
    public class Pic: SharpGLDrawLib.Scene2D,IDisposable
    {

        /// <summary>
        /// Скорость увелечения и уменьшения
        /// </summary>
        public float ScaleSpeed = 0.1f;
        /// <summary>
        /// Отношение размеров отрисованной картинки к к размеру самого изображения
        /// </summary>
        float scale;
        /// <summary>
        /// Позиция картинки на экране. Шаг смещения при увелечении/уменьшении
        /// </summary>
        float x,y;
        /// <summary>
        /// Материнский объект OpenGL
        /// </summary>
        OpenGL gl;
        /// <summary>
        /// Последняя позиция мыши, позиция -1;0 означает отсутсвие последней позиции (Point не допускает null ¯\_(ツ)_/¯)
        /// </summary>
        Point LastPos = new Point(-1,0);
        public BitmapProvider BProvider;
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public delegate void OpenGLDrawExpansion(OpenGL gl);
        public event OpenGLDrawExpansion Draw;
        
        /// <summary>
        /// Извещает всех подписчиков о клике. Передаёт координаты клика в с.к. картинки
        /// </summary>
        public event MouseEventHandler EPMouseClick;
        /// <summary>
        /// Извещает всех подписчиков о клике. Передаёт координаты клика в с.к. OpenGL
        /// </summary>
        public event EventHandler<PointF> EGlMouseClick;
        public Pic():base()
        {
            BProvider = new BitmapProvider();
            gl = Main.OpenGL;
            Main.MouseWheel += Main_MouseWheel;
            Main.MouseLeave += Main_MouseLeave;
            Main.OpenGLDraw += Main_OpenGLDraw;
            Main.MouseUp += Main_MouseUp;
            Main.MouseClick += Main_MouseClick;
            Main.MouseClick += Main_MouseClick1;
        }

        private void Main_MouseClick1(object sender, MouseEventArgs e)
        {
            
            PointF ImgPos = ToOpenGLSpace(e.Location);
            EGlMouseClick?.Invoke(e, ImgPos);
        }
        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            Point ImgPos = GetMouseClickPos(e.Location);
            EPMouseClick?.Invoke(this,new MouseEventArgs(e.Button,e.Clicks,ImgPos.X,ImgPos.Y,e.Delta));
        }
        /// <summary>
        /// Инициализует текстуру картинки и её положение в экранном пространстве
        /// </summary>
        /// <param name="Data">Картинка</param>
        /// <param name="_scale">Увелечение картинки (0..1)</param>
        /// <param name="_x">X координата картинки</param>
        /// <param name="_y">Y координата картинки</param>
        /// <param name="_step">Шаг смешения при изменении маштаба</param>
        public void Init(float _scale = 1.0f, int _x = 0, int _y = 0)
        {
            if (BProvider == null)
                throw new Exception("Провайдер изображения не инициализован");
            //А хуй его знает, почему он не хочет работать в конструкторе класса
            InitOpenGL(0, 0, this.Width, this.Height);
            if (!BProvider.IsInit)
                return;            
            x = _x;
            y = _y;
            scale = _scale;
         //   GC.Collect();
            DrawImage();
        }

        //обработчики перемещения мыши
        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                BProvider.UpdatePart(BProvider.CurrentPosLeftTop.X, BProvider.CurrentPosLeftTop.Y,Main.Width/100*100, Main.Height/100*100);
                x = 0;
                y = 0;
                DrawImage();
                
            }
            catch (Exception ex) { /*MessageBox.Show(ex.Message);*/ return; }
            LastPos = new Point(-1, 0);
        }

        private void Main_MouseLeave(object sender, EventArgs e)
        {
            LastPos = new Point(-1,0);
        }

        private void Main_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                Increase(ScaleSpeed);
                ScaleSpeed /=0.9f;
            }
            else if ((scale-ScaleSpeed) > 0 && e.Delta < 0)
            {
                
                Decrease(ScaleSpeed);
                ScaleSpeed *= 0.9f;
            }
        }

        /// <summary>
        /// Выполняет отрисовку сцены, отрисовка manual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Main_OpenGLDraw(object sender, RenderEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
             gl.LoadIdentity();
              gl.ClearColor(255, 255, 255, 255);


            try
            {
                if (BProvider?.CurrentBitmap != null)
                    Drawer.DrawImageByPixels(gl, BProvider.Addr, x, y, BProvider.CurrentBSize.Width, BProvider.CurrentBSize.Height, 1);
            }
            catch { throw new Exception(); }
              Draw?.Invoke(gl);
             gl.Flush();
        }

       //TODO: Добавить отрисовку по координатам
       
        /// <summary>
        /// Рисует картинку
        /// </summary>
        public void DrawImage()
        {

            if (!IsGLInit)
            {
                LogError("Объект OpenGL не инициализирован, это внутрення ошибка, сообщите разработчику");

                return;
            }
            if(!BProvider.IsLock)
            {
                bool tr = BProvider.LockBits();
                if (!tr)
                    return;
                            BProvider.SwapImageInMemory();
            }
            //if(Tex == null)
            //{
            //    LogError("Текстура неинициализированна, это может говорить о преждевременном вызове функции отрисовки изображения");
            //    return;
            //}
            Main.Refresh();
            

        }
        /// <summary>
        /// Рисует картинку
        /// </summary>
        public void DrawImage(float scale,bool SaveScale=false)
        {
            if (SaveScale)
                this.scale = scale;

            DrawImage();
        }
        /// <summary>
        /// Увелечение картинки
        /// </summary>
        /// <param name="IncreaseStep"></param>
        public void Increase(float IncreaseStep)
        {
            scale += IncreaseStep;
            //x -= scale*100;
            //y -= scale * 100;
            DrawImage();
        }
        /// <summary>
        /// Уменьшение картинки
        /// </summary>
        /// <param name="DecreaseStep"></param>
        public void Decrease(float DecreaseStep)
        {
            if(scale - DecreaseStep >=0)
            scale -= DecreaseStep;
            //x += scale * 100;
            //y += scale * 100;
            DrawImage();
        }
        /// <summary>
        /// Перемешение картинки
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public new void Move(float dx,float dy)
        {
            const int speedUp = 2;
            x += dx;
            y += dy;
            BProvider.CurrentPosLeftTop = new Point((int)(BProvider.CurrentPosLeftTop.X-dx* speedUp), (int)(BProvider.CurrentPosLeftTop.Y+dy* speedUp));
            DrawImage();
        }
        /// <summary>
        /// Проверка на позицию мыши и нажатие клавиши. перемешение картинки
        /// </summary>
        public void CheckAndMove()
        {

            if (!(IsMouseDown && IsMouseOnOpenGL))
                return;

            Point CurrentPos = PointToClient(MousePosition);
            if (LastPos.X == -1)
            {
                LastPos = CurrentPos;
                return;
            }
            else
            {
                int dx = CurrentPos.X - LastPos.X, dy = CurrentPos.Y - LastPos.Y;
                Move(dx, -dy);
            }
            LastPos = CurrentPos;
        }


        /// <summary>
        /// Логирование в stderr поток
        /// </summary>
        /// <param name="text"></param>
        private void LogError(string text)
        {
            Console.Error.WriteLine(DateTime.Now + "\t" + text + "\t" + (GetType().FullName ?? GetType().ToString()));
        }



        
        /// <summary>
        /// На основании координат мыши в пространстве элемента выдаёт координаты в пространстве картинки.
        /// </summary>
        /// <param name="MousePosition">Координаты мыши в пространстве элемента</param>
        /// <returns></returns>
        public Point GetMouseClickPos(Point MousePosition) =>
            FromOpenGLToPictureSpace(ToOpenGLSpace(MousePosition));

        /// <summary>
        /// Транслирует координаты точки из с.к. эелемента в с.к. Opengl.
        /// </summary>
        /// <param name="Point">Координаты точки в с.к. элемента</param>
        /// <returns></returns>
        public PointF ToOpenGLSpace(Point Point) =>
            new PointF(Point.X , Main.Height - (Point.Y));


        /// <summary>
        /// Транслирует координаты точки из с.к. OpenGl в с.к. картинки, закреплённой за этим элементом.
        /// </summary>
        /// <param name="Point">Координаты точки в с.к. Opengl</param>
        /// <returns></returns>
       public Point FromOpenGLToPictureSpace(PointF _Point)
            
        {
            if (!BProvider.IsInit)
                return Point.Empty;
            // Я бы мог без труда уместить вся это в одну строку, но тогда сложность логики увеличится. Если я забуду принцип, нарисуй прямоугольник
            // в прямоугольной системе координат и рассчитай отношение к стороне прямоугольника.
            float a = BProvider.CurrentBSize.Width  * scale, b = BProvider.CurrentBSize.Height * scale;
            int bx = (int)((_Point.X - x) / a * BProvider.CurrentBSize.Width);
            int by = (int)((b - (_Point.Y - y)) / b * BProvider.CurrentBSize.Height);
            return new Point(bx, by);
        }
        public PointF FromPictureToOpenGLSpace(Point Point)
        {
            if (!BProvider.IsInit)
                return PointF.Empty;
            // Я бы мог без труда уместить вся это в одну строку, но тогда сложность логики увеличится. Если я забуду принцип, нарисуй прямоугольник
            // в прямоугольной системе координат и рассчитай отношение к стороне прямоугольника.
            Point = new Point(Point.X,Point.Y);
            float a = BProvider.CurrentBSize.Width * scale, b = BProvider.CurrentBSize.Height * scale;
            float bx = Point.X * a / BProvider.CurrentBSize.Width + x;
            float by = b + y - (Point.Y * b / BProvider.CurrentBSize.Height);
            return new PointF(bx, by);
        }
        public new void Dispose()
        {
            BProvider.Dispose();
        }
        ~Pic()
        {
            Dispose();
        }


    }
}
