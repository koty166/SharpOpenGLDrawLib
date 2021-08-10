using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SharpGLDrawLib
{
    public class BitmapProvider:IDisposable
    {
        public Bitmap CurrentBitmap { get; private set; }
        public delegate Bitmap DLoadImg(int x, int y, int width, int height,int scale);
        public DLoadImg LoadImgMetod { get; private set; }
        public delegate void DLoadImgMetaInf(string Path, out Size size);
        /// <summary>
        /// Указывает, заблокирован ли текущий участок изображения в памяти.
        /// </summary>
        public bool IsLock { get; private set; }
        public bool IsBitmapLoaded { get{ if (CurrentBitmap == null) return false; return true; } private set { } }
        public bool IsInit { get { if (AllBitmapSize == null) return false; return true; } private set { } }
        public bool IsNeedToSwap;
        public bool IsSwaped { get; private set; }
        int _Scale;
        public int Scale { get { return _Scale; } set { if ((value & (value - 1)) == 0 && value != 0) _Scale = value; else _Scale = 1; } } 
        public IntPtr Addr { get; private set; }
        /// <summary>
        /// Содержит некритичные ошибки.
        /// </summary>
        public String Errors { get; private set; }
        public BitmapData CurrentBitmapData { get; private set; }
        /// <summary>
        /// Текущее положение левого верхнего угла выреза.
        /// </summary>
        public Point CurrentPosLeftTop;
        /// <summary>
        /// Общий размер картинки в файле.
        /// </summary>
        public Size AllBitmapSize { get; private set; }
        public Size CurrentBSize { get; private set; }
        public string Path { get; private set; }

        public void SwapImageInMemory()
        {
            if ((!IsInit || !IsLock) && IsNeedToSwap)
                return;
            byte[] byf = new byte[CurrentBitmapData.Stride * CurrentBSize.Height];
            Marshal.Copy(Addr, byf, 0, Math.Abs(CurrentBitmapData.Stride) * CurrentBSize.Height);

            byf = Drawer.SwapInArr(byf, CurrentBSize.Height, CurrentBitmapData.Stride);

            Marshal.Copy(byf, 0, Addr, byf.Length);
            IsSwaped = !IsSwaped;
        }

        /// <summary>
        /// Обновляет <see cref="CurrentBitmap"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public void UpdatePart(int x,int y,int width,int height)
        {
            if (AllBitmapSize == null)
                throw new Exception("На текущий момент провайдер не инициализирован.");
            if (x + width > AllBitmapSize.Width || y + height > AllBitmapSize.Height)
                throw new ArgumentOutOfRangeException("Данный участок не может быть отображен, поскольку он находится за границей изображения.");
            if (x < 0 || y < 0 || height <= 0 || width <= 0)
                throw new ArgumentOutOfRangeException("Измерения находятся вне допустимых пределов");

            Bitmap buf = null;
            try
            {
                buf = LoadImgMetod(x, y, width, height, _Scale);
                if (buf == null)
                    throw new Exception("Некорректная загрузка изображения.");
            }
            catch (Exception ex) { throw new Exception("Во время загрузки изображения произошла ошибка " + ex.Message); }

            if (buf.Width > AllBitmapSize.Width || buf.Height > AllBitmapSize.Height)
                throw new Exception("Изображение больше исходного. Этого не может быть. Это ошибка проектирования, сообщите разработчику");
            if (!UnLockBits())
                Errors += "\tНе удалось разблокировать текущий экземпляр участка изображения\t";
            CurrentBitmap = buf;
            
            CurrentBSize = buf.Size;
            

            //buf = null;
            // GC.Collect();
        }
        public void Init(string _Path,DLoadImg LMetod,DLoadImgMetaInf LInfMetod)
        {

            LoadImgMetod = LMetod;
            LInfMetod(_Path,out Size Bsize);
            if (Bsize.IsEmpty)
                throw new Exception("Ошибка при загрузке информации об изображении.");
            AllBitmapSize = Bsize;
        }
        public bool LockBits()
        {
            if (!IsBitmapLoaded)
                return false;
            try
            {
                CurrentBitmapData =  CurrentBitmap.LockBits(new Rectangle(Point.Empty,CurrentBitmap.Size),ImageLockMode.ReadWrite,CurrentBitmap.PixelFormat);
            }
            catch(Exception ex) { Console.Error.WriteLine(ex.Message); return false; }
            Addr = CurrentBitmapData.Scan0;
            IsLock = true;
            return true;
        }
        public bool UnLockBits()
        {
            if (!IsBitmapLoaded)
                return false;
            try
            {
                CurrentBitmap.UnlockBits(CurrentBitmapData);
            }
            catch { return false; }
            Addr = IntPtr.Zero;
            IsLock = false;
            return true;
        }
        public BitmapProvider()
        {
            this.Addr = IntPtr.Zero;
            this.IsInit = false;
            this.IsLock = false;
            IsNeedToSwap = false;
            IsSwaped = false;
            _Scale = 1;
            CurrentPosLeftTop = new Point(20000, 15000);
        }
        public void Dispose()
        {

        }

    }
}
