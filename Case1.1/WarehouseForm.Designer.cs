namespace Case1._1
{
    partial class WarehouseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtOre = new TextBox();
            txtNikel = new TextBox();
            txtMarganec = new TextBox();
            txtChrome = new TextBox();
            txtTimeFurnace = new TextBox();
            txtTimeConverter = new TextBox();
            txtTimeRollingMachine = new TextBox();
            btnExit = new Button();
            btnSave = new Button();
            SuspendLayout();
            // 
            // txtOre
            // 
            txtOre.Location = new Point(216, 107);
            txtOre.Margin = new Padding(3, 4, 3, 4);
            txtOre.Name = "txtOre";
            txtOre.Size = new Size(199, 27);
            txtOre.TabIndex = 0;
            // 
            // txtNikel
            // 
            txtNikel.Location = new Point(424, 107);
            txtNikel.Margin = new Padding(3, 4, 3, 4);
            txtNikel.Name = "txtNikel";
            txtNikel.Size = new Size(199, 27);
            txtNikel.TabIndex = 0;
            // 
            // txtMarganec
            // 
            txtMarganec.Location = new Point(12, 233);
            txtMarganec.Margin = new Padding(3, 4, 3, 4);
            txtMarganec.Name = "txtMarganec";
            txtMarganec.Size = new Size(199, 27);
            txtMarganec.TabIndex = 0;
            // 
            // txtChrome
            // 
            txtChrome.Location = new Point(630, 107);
            txtChrome.Margin = new Padding(3, 4, 3, 4);
            txtChrome.Name = "txtChrome";
            txtChrome.Size = new Size(199, 27);
            txtChrome.TabIndex = 0;
            // 
            // txtTimeFurnace
            // 
            txtTimeFurnace.Location = new Point(216, 233);
            txtTimeFurnace.Margin = new Padding(3, 4, 3, 4);
            txtTimeFurnace.Name = "txtTimeFurnace";
            txtTimeFurnace.Size = new Size(199, 27);
            txtTimeFurnace.TabIndex = 0;
            // 
            // txtTimeConverter
            // 
            txtTimeConverter.Location = new Point(425, 233);
            txtTimeConverter.Margin = new Padding(3, 4, 3, 4);
            txtTimeConverter.Name = "txtTimeConverter";
            txtTimeConverter.Size = new Size(199, 27);
            txtTimeConverter.TabIndex = 0;
            // 
            // txtTimeRollingMachine
            // 
            txtTimeRollingMachine.Location = new Point(630, 233);
            txtTimeRollingMachine.Margin = new Padding(3, 4, 3, 4);
            txtTimeRollingMachine.Name = "txtTimeRollingMachine";
            txtTimeRollingMachine.Size = new Size(203, 27);
            txtTimeRollingMachine.TabIndex = 0;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(9, 479);
            btnExit.Margin = new Padding(3, 4, 3, 4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(170, 45);
            btnExit.TabIndex = 1;
            btnExit.Text = "Назад";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(453, 479);
            btnSave.Margin = new Padding(3, 4, 3, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(170, 45);
            btnSave.TabIndex = 1;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // WarehouseForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(btnSave);
            Controls.Add(btnExit);
            Controls.Add(txtTimeRollingMachine);
            Controls.Add(txtChrome);
            Controls.Add(txtMarganec);
            Controls.Add(txtTimeConverter);
            Controls.Add(txtTimeFurnace);
            Controls.Add(txtNikel);
            Controls.Add(txtOre);
            Margin = new Padding(3, 4, 3, 4);
            Name = "WarehouseForm";
            Text = "WarehouseForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtOre;
        private TextBox txtNikel;
        private TextBox txtMarganec;
        private TextBox txtChrome;
        private TextBox txtTimeFurnace;
        private TextBox txtTimeConverter;
        private TextBox txtTimeRollingMachine;
        private Button btnExit;
        private Button btnSave;
    }
}