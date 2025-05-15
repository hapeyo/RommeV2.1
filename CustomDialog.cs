using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Romme_V2
{
    public partial class Abfrage : Form
    {
        private PrintAnalyseForm parentForm;
        private string spielNummerDialog;
        public Abfrage(PrintAnalyseForm parent, string spielnummer)
        {
            InitializeComponent();
            parentForm = parent; parentForm = parent;
            spielNummerDialog = spielnummer;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            // "Anschauen" schließt nur das Fenster
            parentForm.druckenBestimmtePartie();
            this.Close();
        }


        private void btnCreatePdf_Click(object sender, EventArgs e)
        {
            // PDF-Erstellungsmethode in PrintAnalyseForm aufrufen
            parentForm.druckenBestimmtePartie();
            parentForm.DruckenAlsPDF();
            this.Close();
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            parentForm.druckenBestimmtePartie();
            // Warnung anzeigen, bevor gelöscht wird
            var result = MessageBox.Show("Ojo! Esta opcion es solamente para borrar tests o partidos," +
                " que son grabado equivocadamente! Realmente quieres borrar?", "Bestätigung", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                parentForm.LoescheSpiel(spielNummerDialog);
                this.Close();
            }
        }
    }
}
