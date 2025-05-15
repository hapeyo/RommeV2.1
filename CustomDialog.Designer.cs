namespace Romme_V2
{
    partial class Abfrage
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
            this.btnView = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCreatePdf = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnView
            // 
            this.btnView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.Location = new System.Drawing.Point(3, 1);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(200, 40);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "Solamente ver";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(3, 93);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(200, 40);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Borrar Partido";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCreatePdf
            // 
            this.btnCreatePdf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreatePdf.Location = new System.Drawing.Point(3, 47);
            this.btnCreatePdf.Name = "btnCreatePdf";
            this.btnCreatePdf.Size = new System.Drawing.Size(200, 40);
            this.btnCreatePdf.TabIndex = 2;
            this.btnCreatePdf.Text = "Crear PDF";
            this.btnCreatePdf.UseVisualStyleBackColor = true;
            this.btnCreatePdf.Click += new System.EventHandler(this.btnCreatePdf_Click);
            // 
            // Abfrage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 139);
            this.Controls.Add(this.btnCreatePdf);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnView);
            this.Name = "Abfrage";
            this.Text = "CustomDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCreatePdf;
    }
}