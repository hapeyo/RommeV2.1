namespace Romme_V2
{
    partial class AdminForm
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
            this.btnRegresar = new System.Windows.Forms.Button();
            this.cbBPlayerList = new System.Windows.Forms.ComboBox();
            this.btnUpdatePlayer = new System.Windows.Forms.Button();
            this.btnOldGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRegresar
            // 
            this.btnRegresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegresar.Location = new System.Drawing.Point(618, 383);
            this.btnRegresar.Name = "btnRegresar";
            this.btnRegresar.Size = new System.Drawing.Size(150, 40);
            this.btnRegresar.TabIndex = 0;
            this.btnRegresar.Text = "Regresar";
            this.btnRegresar.UseVisualStyleBackColor = true;
            this.btnRegresar.Click += new System.EventHandler(this.btnRegresar_Click);
            // 
            // cbBPlayerList
            // 
            this.cbBPlayerList.FormattingEnabled = true;
            this.cbBPlayerList.Location = new System.Drawing.Point(46, 81);
            this.cbBPlayerList.Name = "cbBPlayerList";
            this.cbBPlayerList.Size = new System.Drawing.Size(150, 28);
            this.cbBPlayerList.TabIndex = 1;
            this.cbBPlayerList.SelectedIndexChanged += new System.EventHandler(this.cbBPlayerList_SelectedIndexChanged);
            // 
            // btnUpdatePlayer
            // 
            this.btnUpdatePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdatePlayer.Location = new System.Drawing.Point(46, 13);
            this.btnUpdatePlayer.Name = "btnUpdatePlayer";
            this.btnUpdatePlayer.Size = new System.Drawing.Size(200, 40);
            this.btnUpdatePlayer.TabIndex = 2;
            this.btnUpdatePlayer.Text = "Update Player";
            this.btnUpdatePlayer.UseVisualStyleBackColor = true;
            this.btnUpdatePlayer.Click += new System.EventHandler(this.btnUpdatePlayer_Click);
            // 
            // btnOldGame
            // 
            this.btnOldGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOldGame.Location = new System.Drawing.Point(270, 13);
            this.btnOldGame.Name = "btnOldGame";
            this.btnOldGame.Size = new System.Drawing.Size(200, 40);
            this.btnOldGame.TabIndex = 3;
            this.btnOldGame.Text = "Agregar Partido";
            this.btnOldGame.UseVisualStyleBackColor = true;
            this.btnOldGame.Click += new System.EventHandler(this.btnOldGame_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnOldGame);
            this.Controls.Add(this.btnUpdatePlayer);
            this.Controls.Add(this.cbBPlayerList);
            this.Controls.Add(this.btnRegresar);
            this.Name = "AdminForm";
            this.Text = "Administrar";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRegresar;
        private System.Windows.Forms.ComboBox cbBPlayerList;
        private System.Windows.Forms.Button btnUpdatePlayer;
        private System.Windows.Forms.Button btnOldGame;
    }
}