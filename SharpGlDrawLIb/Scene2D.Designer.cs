
namespace SharpGLDrawLib
{
    partial class Scene2D
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Main = new SharpGL.OpenGLControl();
            ((System.ComponentModel.ISupportInitialize)(this.Main)).BeginInit();
            this.SuspendLayout();
            // 
            // Main
            // 
            this.Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main.DrawFPS = false;
            this.Main.Location = new System.Drawing.Point(0, 0);
            this.Main.Name = "Main";
            this.Main.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL4_3;
            this.Main.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.Main.RenderTrigger = SharpGL.RenderTrigger.Manual;
            this.Main.Size = new System.Drawing.Size(201, 153);
            this.Main.TabIndex = 0;
            this.Main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.Main.MouseEnter += new System.EventHandler(this.Main_MouseEnter);
            this.Main.MouseLeave += new System.EventHandler(this.Main_MouseLeave);
            this.Main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Main_MouseUp);
            // 
            // Scene2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Main);
            this.Name = "Scene2D";
            this.Size = new System.Drawing.Size(201, 153);
            this.SizeChanged += new System.EventHandler(this.Scene2D_SizeChanged);
            this.Resize += new System.EventHandler(this.Scene2D_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected SharpGL.OpenGLControl Main;
    }
}
