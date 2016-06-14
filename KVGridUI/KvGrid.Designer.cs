namespace KVGridUI
{
    partial class KVGrid
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
            this.kvGridBlock1 = new KVGridUI.KVGridBlock();
            this.SuspendLayout();
            // 
            // kvGridBlock1
            // 
            this.kvGridBlock1.AutoSize = true;
            this.kvGridBlock1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kvGridBlock1.Location = new System.Drawing.Point(0, 0);
            this.kvGridBlock1.Name = "kvGridBlock1";
            this.kvGridBlock1.Size = new System.Drawing.Size(591, 250);
            this.kvGridBlock1.TabIndex = 0;
            // 
            // KVGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.kvGridBlock1);
            this.Name = "KVGrid";
            this.Size = new System.Drawing.Size(594, 253);
            this.SizeChanged += new System.EventHandler(this.KVGrid_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KVGridBlock kvGridBlock1;
    }
}
