namespace Romme_V2
{
    partial class NewPlayerForm
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
            this.lblForeName = new System.Windows.Forms.Label();
            this.txtForename = new System.Windows.Forms.TextBox();
            this.btnAddNewPl = new System.Windows.Forms.Button();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblBirthday = new System.Windows.Forms.Label();
            this.lblNickName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtNickName = new System.Windows.Forms.TextBox();
            this.lblTituloNewPLayerForm = new System.Windows.Forms.Label();
            this.dtpBirthday = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // lblForeName
            // 
            this.lblForeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForeName.Location = new System.Drawing.Point(50, 50);
            this.lblForeName.Name = "lblForeName";
            this.lblForeName.Size = new System.Drawing.Size(180, 40);
            this.lblForeName.TabIndex = 0;
            this.lblForeName.Text = "Nombre:";
            this.lblForeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtForename
            // 
            this.txtForename.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtForename.Location = new System.Drawing.Point(240, 50);
            this.txtForename.Name = "txtForename";
            this.txtForename.Size = new System.Drawing.Size(200, 35);
            this.txtForename.TabIndex = 1;
            // 
            // btnAddNewPl
            // 
            this.btnAddNewPl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNewPl.Location = new System.Drawing.Point(182, 257);
            this.btnAddNewPl.Name = "btnAddNewPl";
            this.btnAddNewPl.Size = new System.Drawing.Size(150, 40);
            this.btnAddNewPl.TabIndex = 2;
            this.btnAddNewPl.Text = "Agregar";
            this.btnAddNewPl.UseVisualStyleBackColor = true;
            this.btnAddNewPl.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblLastName
            // 
            this.lblLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastName.Location = new System.Drawing.Point(50, 100);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(180, 40);
            this.lblLastName.TabIndex = 3;
            this.lblLastName.Text = "Appellido:";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBirthday
            // 
            this.lblBirthday.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthday.Location = new System.Drawing.Point(50, 150);
            this.lblBirthday.Name = "lblBirthday";
            this.lblBirthday.Size = new System.Drawing.Size(180, 40);
            this.lblBirthday.TabIndex = 4;
            this.lblBirthday.Text = "Cumpleaños:";
            this.lblBirthday.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNickName
            // 
            this.lblNickName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNickName.Location = new System.Drawing.Point(50, 200);
            this.lblNickName.Name = "lblNickName";
            this.lblNickName.Size = new System.Drawing.Size(180, 40);
            this.lblNickName.TabIndex = 5;
            this.lblNickName.Text = "Apodo:";
            this.lblNickName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLastName
            // 
            this.txtLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastName.Location = new System.Drawing.Point(240, 100);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(200, 35);
            this.txtLastName.TabIndex = 6;
            // 
            // txtNickName
            // 
            this.txtNickName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNickName.Location = new System.Drawing.Point(240, 200);
            this.txtNickName.Name = "txtNickName";
            this.txtNickName.Size = new System.Drawing.Size(200, 35);
            this.txtNickName.TabIndex = 8;
            // 
            // lblTituloNewPLayerForm
            // 
            this.lblTituloNewPLayerForm.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTituloNewPLayerForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloNewPLayerForm.Location = new System.Drawing.Point(140, 7);
            this.lblTituloNewPLayerForm.Name = "lblTituloNewPLayerForm";
            this.lblTituloNewPLayerForm.Size = new System.Drawing.Size(300, 40);
            this.lblTituloNewPLayerForm.TabIndex = 9;
            this.lblTituloNewPLayerForm.Text = "Jugadores Nuevo/as";
            this.lblTituloNewPLayerForm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpBirthday
            // 
            this.dtpBirthday.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpBirthday.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBirthday.Location = new System.Drawing.Point(240, 160);
            this.dtpBirthday.Name = "dtpBirthday";
            this.dtpBirthday.Size = new System.Drawing.Size(200, 26);
            this.dtpBirthday.TabIndex = 10;
            this.dtpBirthday.Value = new System.DateTime(2025, 3, 25, 0, 0, 0, 0);
            // 
            // NewPlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dtpBirthday);
            this.Controls.Add(this.lblTituloNewPLayerForm);
            this.Controls.Add(this.txtNickName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.lblNickName);
            this.Controls.Add(this.lblBirthday);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.btnAddNewPl);
            this.Controls.Add(this.txtForename);
            this.Controls.Add(this.lblForeName);
            this.Name = "NewPlayerForm";
            this.Text = "NewPLayerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblForeName;
        private System.Windows.Forms.TextBox txtForename;
        private System.Windows.Forms.Button btnAddNewPl;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblBirthday;
        private System.Windows.Forms.Label lblNickName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtNickName;
        private System.Windows.Forms.Label lblTituloNewPLayerForm;
        private System.Windows.Forms.DateTimePicker dtpBirthday;
    }
}