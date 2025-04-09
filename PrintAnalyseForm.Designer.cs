namespace Romme_V2
{
    partial class PrintAnalyseForm
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
            this.btnBackToMain = new System.Windows.Forms.Button();
            this.btnLastGame = new System.Windows.Forms.Button();
            this.lstErgebnisse = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnBackToMain
            // 
            this.btnBackToMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackToMain.Location = new System.Drawing.Point(578, 398);
            this.btnBackToMain.Name = "btnBackToMain";
            this.btnBackToMain.Size = new System.Drawing.Size(200, 40);
            this.btnBackToMain.TabIndex = 0;
            this.btnBackToMain.Text = "Regresar";
            this.btnBackToMain.UseVisualStyleBackColor = true;
            this.btnBackToMain.Click += new System.EventHandler(this.btnBackToMain_Click);
            // 
            // btnLastGame
            // 
            this.btnLastGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLastGame.Location = new System.Drawing.Point(29, 23);
            this.btnLastGame.Name = "btnLastGame";
            this.btnLastGame.Size = new System.Drawing.Size(300, 35);
            this.btnLastGame.TabIndex = 1;
            this.btnLastGame.Text = "Imprimir ultimo partido";
            this.btnLastGame.UseVisualStyleBackColor = true;
            this.btnLastGame.Click += new System.EventHandler(this.btnLastGame_Click);
            // 
            // lstErgebnisse
            // 
            this.lstErgebnisse.FormattingEnabled = true;
            this.lstErgebnisse.ItemHeight = 20;
            this.lstErgebnisse.Location = new System.Drawing.Point(527, 23);
            this.lstErgebnisse.Name = "lstErgebnisse";
            this.lstErgebnisse.Size = new System.Drawing.Size(120, 84);
            this.lstErgebnisse.TabIndex = 2;
            // 
            // PrintAnalyseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstErgebnisse);
            this.Controls.Add(this.btnLastGame);
            this.Controls.Add(this.btnBackToMain);
            this.Name = "PrintAnalyseForm";
            this.Text = "Imprimir y Analizar";
            this.Load += new System.EventHandler(this.PrintAnalyseForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBackToMain;
        private System.Windows.Forms.Button btnLastGame;
        private System.Windows.Forms.ListBox lstErgebnisse;
    }
}