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
            this.dGVLastPartie = new System.Windows.Forms.DataGridView();
            this.btnOtherGame = new System.Windows.Forms.Button();
            this.cbBSpielnummer = new System.Windows.Forms.ComboBox();
            this.btnRanking = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dGVLastPartie)).BeginInit();
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
            this.btnLastGame.Text = "Mostrar ultimo partido";
            this.btnLastGame.UseVisualStyleBackColor = true;
            this.btnLastGame.Click += new System.EventHandler(this.btnLastGame_Click);
            // 
            // dGVLastPartie
            // 
            this.dGVLastPartie.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dGVLastPartie.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVLastPartie.Location = new System.Drawing.Point(12, 64);
            this.dGVLastPartie.Name = "dGVLastPartie";
            this.dGVLastPartie.RowHeadersWidth = 62;
            this.dGVLastPartie.RowTemplate.Height = 28;
            this.dGVLastPartie.Size = new System.Drawing.Size(766, 300);
            this.dGVLastPartie.TabIndex = 3;
            // 
            // btnOtherGame
            // 
            this.btnOtherGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOtherGame.Location = new System.Drawing.Point(29, 398);
            this.btnOtherGame.Name = "btnOtherGame";
            this.btnOtherGame.Size = new System.Drawing.Size(300, 40);
            this.btnOtherGame.TabIndex = 4;
            this.btnOtherGame.Text = "Mostrar otros partidos";
            this.btnOtherGame.UseVisualStyleBackColor = true;
            this.btnOtherGame.Click += new System.EventHandler(this.btnOtherGame_Click);
            // 
            // cbBSpielnummer
            // 
            this.cbBSpielnummer.FormattingEnabled = true;
            this.cbBSpielnummer.Location = new System.Drawing.Point(355, 398);
            this.cbBSpielnummer.Name = "cbBSpielnummer";
            this.cbBSpielnummer.Size = new System.Drawing.Size(200, 28);
            this.cbBSpielnummer.TabIndex = 5;
            this.cbBSpielnummer.SelectedIndexChanged += new System.EventHandler(this.cbBSpielnummer_SelectedIndexChanged);
            // 
            // btnRanking
            // 
            this.btnRanking.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRanking.Location = new System.Drawing.Point(628, 23);
            this.btnRanking.Name = "btnRanking";
            this.btnRanking.Size = new System.Drawing.Size(150, 35);
            this.btnRanking.TabIndex = 6;
            this.btnRanking.Text = "Ranking";
            this.btnRanking.UseVisualStyleBackColor = true;
            this.btnRanking.Click += new System.EventHandler(this.btnRanking_Click);
            // 
            // PrintAnalyseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRanking);
            this.Controls.Add(this.cbBSpielnummer);
            this.Controls.Add(this.btnOtherGame);
            this.Controls.Add(this.dGVLastPartie);
            this.Controls.Add(this.btnLastGame);
            this.Controls.Add(this.btnBackToMain);
            this.Name = "PrintAnalyseForm";
            this.Text = "Imprimir y Analizar";
            this.Load += new System.EventHandler(this.PrintAnalyseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dGVLastPartie)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBackToMain;
        private System.Windows.Forms.Button btnLastGame;
        private System.Windows.Forms.DataGridView dGVLastPartie;
        private System.Windows.Forms.Button btnOtherGame;
        private System.Windows.Forms.ComboBox cbBSpielnummer;
        private System.Windows.Forms.Button btnRanking;
    }
}