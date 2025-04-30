namespace Case1._1
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnQueue = new Button();
            btnMakeOrder = new Button();
            btnWarehouse = new Button();
            SuspendLayout();
            // 
            // btnQueue
            // 
            btnQueue.Location = new Point(12, 128);
            btnQueue.Name = "btnQueue";
            btnQueue.Size = new Size(257, 188);
            btnQueue.TabIndex = 0;
            btnQueue.Text = "Очередь заказов";
            btnQueue.UseVisualStyleBackColor = true;
            btnQueue.Click += btnQueue_Click;
            // 
            // btnMakeOrder
            // 
            btnMakeOrder.Location = new Point(275, 128);
            btnMakeOrder.Name = "btnMakeOrder";
            btnMakeOrder.Size = new Size(257, 188);
            btnMakeOrder.TabIndex = 0;
            btnMakeOrder.Text = "Сделать заказ";
            btnMakeOrder.UseVisualStyleBackColor = true;
            btnMakeOrder.Click += btnMakeOrder_Click;
            // 
            // btnWarehouse
            // 
            btnWarehouse.Location = new Point(538, 128);
            btnWarehouse.Name = "btnWarehouse";
            btnWarehouse.Size = new Size(257, 188);
            btnWarehouse.TabIndex = 0;
            btnWarehouse.Text = "Склад";
            btnWarehouse.UseVisualStyleBackColor = true;
            btnWarehouse.Click += btnWarehouse_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnWarehouse);
            Controls.Add(btnMakeOrder);
            Controls.Add(btnQueue);
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnQueue;
        private Button btnMakeOrder;
        private Button btnWarehouse;
    }
}
