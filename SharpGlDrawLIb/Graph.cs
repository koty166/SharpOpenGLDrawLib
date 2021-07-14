using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpGL;
using System.Windows.Forms;

namespace SharpGLDrawLib
{
    public class Graph : Scene2D
    {
        /// <summary>
        /// Прибавление к максимальным значениям графика для увелечения осей.
        /// </summary>
        const float MaxModifier = 0;
        /// <summary>
        /// Модификатор количества отметок по осям.
        /// </summary>
        const float GraphStemModifier = 1;
        /// <summary>
        /// Смещение всех точке по оси OX, необходимо для более наглядного представления.
        /// </summary>
        const float DX = 0;
        /// <summary>
        /// Скорость увеличения и уменьшения.
        /// </summary>
        public float ScaleSpeed = 0.1f;
        /// <summary>
        /// Отношение размеров отрисованной картинки к к размеру самого изображения.
        /// </summary>
        float scale;
        /// <summary>
        /// Базовое значение увеличения.
        /// </summary>
        float BaseScale=1;
        /// <summary>
        /// Максимальные значения графика.
        /// </summary>
        float MaxX, MaxY;
        float MaxXValue, MaxYValue;
        /// <summary>
        /// Базовый показатель количества промежуточных значений графика
        /// </summary>
        int BaseXSteps, BaseYSteps;
        /// <summary>
        /// Данные о точках для отрисовки.
        /// </summary>
        PointF[] Points;
        /// <summary>
        /// Данные о Y координатах точек.
        /// </summary>
        float[] YPosData;
        /// <summary>
        /// Ширина и высота графика
        /// </summary>
        float x,y;
        /// <summary>
        /// Материнский объект OpenGL
        /// </summary>
        OpenGL gl;
        /// <summary>
        /// Последняя позиция мыши, позиция -1;0 означает отсутсвие последней позиции (Point не допускает null ¯\_(ツ)_/¯)
        /// </summary>
        Point LastPos = new Point(-1, 0);
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Graph() : base()
        {
            gl = Main.OpenGL;
            Main.MouseWheel += Main_MouseWheel;
            Main.MouseLeave += Main_MouseLeave;
            Main.OpenGLDraw += Main_OpenGLDraw;
            Main.MouseUp += Main_MouseUp;
        }
        /// <summary>
        /// Инициализует график. За максимальное число по оси X принято количество точек. По оси Y - максимельное значение среди точек.
        /// </summary>
        /// <param name="_Data">Данные о y координатах точек</param>
        /// <param name="_BaseXSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_BaseYSteps">Базовое количество промежуточных значений по оси X</param>
        public void Init(PointF[] _Data, int _BaseXSteps = 4, int _BaseYSteps = 4)
        {

            float max = 0;
            foreach (var item in _Data)
            {
                if (item.Y > max)
                    max = item.Y;
            }
            Init(_Data, _Data.Length, max,0,0, _BaseXSteps, _BaseYSteps);
        }
        /// <summary>
        /// Инициализует график. За максимальное число по оси X принято количество точек.
        /// </summary>
        /// <param name="_Data">Данные о y координатах точек</param>
        /// <param name="_MaxY">Максимальное значение на графике по оси Y</param>
        /// <param name="_BaseXSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_BaseYSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_scale">Увеличение</param>
        public void Init(PointF[] _Data, float _MaxY, int _BaseXSteps = 4, int _BaseYSteps = 4, float _scale = 2.5f)
        {
            Init(_Data, _Data.Length, _MaxY,0,0, _BaseXSteps, _BaseYSteps, _scale);
        }
        /// <summary>
        /// Инициализует график
        /// </summary>
        /// <param name="_Data">Данные о y координатах точек</param>
        /// <param name="_MaxX">Максимальное значение на графике по оси X</param>
        /// <param name="_MaxY">Максимальное значение на графике по оси Y</param>
        /// <param name="_BaseXSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_BaseYSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_scale">Увеличение</param>
        public void Init(PointF[] _Data, float _MaxX, float _MaxY, int _MaxXValue = 0, int _MaxYValue = 0, int _BaseXSteps = 4, int _BaseYSteps = 4, float _scale = 2.5f)
        {
            float[] PData = new float[_Data.Length];
            for (int i = 0; i < _Data.Length; i++)
            {
                PData[i] = _Data[i].Y;
            }
            YPosData = PData;



            //А хуй его знает, почему он не хочет работать в конструкторе класса
            InitOpenGL(0, 0, this.Width, this.Height);
            Points = _Data;
            MaxX = _MaxX+MaxModifier;
            MaxY = _MaxY+ MaxModifier;
            BaseXSteps = _BaseXSteps;
            BaseYSteps = _BaseYSteps;
            MaxXValue = (_MaxXValue == 0 ? MaxX : _MaxXValue);
            MaxYValue = (_MaxYValue == 0 ? MaxY : _MaxYValue);
            scale = _scale;
            BaseScale = _scale;
            Move(0, 0);
        }
        /// <summary>
        /// Инициализет график.
        /// </summary>
        /// <param name="_Data">Данные о y координатах точек</param>
        /// <param name="_MaxX">Максимальное значение на графике по оси X</param>
        /// <param name="_MaxY">Максимальное значение на графике по оси Y</param>
        /// <param name="_BaseXSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_BaseYSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_scale">Увеличение</param>
        //public void Init(float[] _Data, int _MaxXValue = 0, int _MaxYValue = 0, int _BaseXSteps = 4, int _BaseYSteps = 4, float _scale = 2.5f)
        //{
        //    float max = 0;
        //    foreach (var item in _Data)
        //    {
        //        if (item > max)
        //            max = item;
        //    }
        //    Init(_Data, _Data.Length, max,  _MaxXValue,  _MaxYValue,  _BaseXSteps,  _BaseYSteps,  _scale);
        //}
        /// <summary>
        /// Инициализет график.
        /// </summary>
        /// <param name="_Data">Данные о y координатах точек</param>
        /// <param name="_MaxX">Максимальное значение на графике по оси X</param>
        /// <param name="_MaxY">Максимальное значение на графике по оси Y</param>
        /// <param name="_BaseXSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_BaseYSteps">Базовое количество промежуточных значений по оси X</param>
        /// <param name="_scale">Увеличение</param>
        public void Init(float[] _Data, float _MaxX, float _MaxY, float _MaxXValue=0, float _MaxYValue=0, int _BaseXSteps = 4, int _BaseYSteps = 4,float _scale = 2.5f)
        {
            InitOpenGL(0, 0, this.Width, this.Height);
            YPosData = _Data;
            MaxX = _MaxX + MaxModifier;
            MaxY = _MaxY + MaxModifier;
            BaseXSteps = _BaseXSteps;
            BaseYSteps = _BaseYSteps;
            scale = _scale;
            BaseScale = _scale;
            MaxXValue = (_MaxXValue == 0 ? MaxX : _MaxXValue);
            MaxYValue = (_MaxYValue == 0 ? MaxY : _MaxYValue);
            Move(0, 0);
        }
        /// <summary>
        /// Обновляет точки в соответствии с координатами смещения.
        /// </summary>
        void PointsUpdate()
        {
            if (YPosData == null)
                return;
            PointF[] PData = new PointF[YPosData.Length];
            for (int i = 0; i < YPosData.Length; i++)
            {
                PData[i] = new PointF((i + DX + x)*scale, (YPosData[i] + y)*scale);
            }
            Points = PData;
        }
        
        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            LastPos = new Point(-1, 0);
        }

        private void Main_MouseLeave(object sender, EventArgs e)
        {
            LastPos = new Point(-1, 0);
        }

        private void Main_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                Increase(ScaleSpeed);
            else if (e.Delta < 0)
                Decrease(ScaleSpeed);
        }

        /// <summary>
        /// Выполняет отрисовку сцены, отрисовка manual
        /// </summary>
        private void Main_OpenGLDraw(object sender, RenderEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.ClearColor(0, 0, 0, 0);

           
            if (Points != null)
            {
                Drawer.DrawPolygon(gl, Color.Red, Points);
                Drawer.DrawCoordSystem(gl, Color.White, MaxX * scale, MaxY * scale, (int)(BaseXSteps * scale / BaseScale * GraphStemModifier), (int)(BaseYSteps * scale / BaseScale * GraphStemModifier), 6, MaxXValue, MaxYValue, x * scale, y * scale, true, false, true);
            }            

            gl.Flush();
        }

        /// <summary>
        /// Рисует картинку
        /// </summary>
        public void DrawGraph()
        {

            if (!IsGLInit)
            {
                LogError("Объект OpenGL не инициализирован, это внутрення ошибка, сообщите разработчику");
                return;
            }
            if(YPosData == null || Points == null)
            {
                LogError("Элемент не инициализирован, это внутрення ошибка, сообщите разработчику");
                return;
            }
            Main.Refresh();


        }

        /// <summary>
        /// Рисует картинку. Обновляет параметр scale
        /// </summary>
        public void DrawGraph(float scale)
        {
            this.scale = scale;
            DrawGraph();
        }

        /// <summary>
        /// Увеличение картинки
        /// </summary>
        /// <param name="IncreaseStep"></param>
        public void Increase(float IncreaseStep)
        {
            scale += IncreaseStep;

            //  x -= step;
            //   y -= step;
            PointsUpdate();
            DrawGraph();
        }

        /// <summary>
        /// Уменьшение картинки
        /// </summary>
        /// <param name="DecreaseStep"></param>
        public void Decrease(float DecreaseStep)
        {
            if (scale - DecreaseStep >= 0)
                scale -= DecreaseStep;
            //   x += step;
            //   y += step;
            PointsUpdate();
            DrawGraph();
        }

        /// <summary>
        /// Перемещение картинки
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public new void Move(float dx, float dy)
        {
            x += dx;
            y += dy;
            PointsUpdate();
            DrawGraph();
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
                LastPos = CurrentPos;
            else
            {
                int dx = CurrentPos.X - LastPos.X, dy = CurrentPos.Y - LastPos.Y;
                Move(dx*1/scale, -dy*1/scale);
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
    }
}
