namespace KVGridUI
{
    partial class Component1
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
            this.kvGrid1 = new KVGridUI.KVGrid();
            // 
            // kvGrid1
            // 
            this.kvGrid1.AutoScroll = true;
            this.kvGrid1.AutoSize = true;
            this.kvGrid1.Location = new System.Drawing.Point(0, 0);
            this.kvGrid1.Name = "kvGrid1";
            this.kvGrid1.SelectedItem = null;
            this.kvGrid1.Size = new System.Drawing.Size(594, 253);
            this.kvGrid1.TabIndex = 0;

        }

        #endregion

        private KVGrid kvGrid1;
    }
}
