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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Romme_V2
{
    public partial class PrintAnalyseForm : Form
    {
        private NbrJug mainForm;
        private List<Spieler> spielerListe;
        private List<Spiel> spieleListe;
        public PrintAnalyseForm(NbrJug form1)
        {
            InitializeComponent();
            mainForm = form1;
            spielerListe = mainForm.GetSpielerListe();
            spieleListe = LadeSpiele(NbrJug.dateiPfad);
            this.Load += PrintAnalyseForm_Load; // OnLoad verknüpft
        }
        
        private void PrintAnalyseForm_Load(object sender, EventArgs e)
        {
            string spieleDateiPfad = NbrJug.dateiPfad; // Pfad aus NbrJug holen
            List<Spiel> spieleListe = LadeSpiele(spieleDateiPfad); // Spiele laden
        }
        public List<Spiel> LadeSpiele(string dateiPfad)
        {
            if (!File.Exists(dateiPfad))
                return new List<Spiel>();

            return File.ReadAllLines(dateiPfad)
                       .Skip(1)
                       .Select(line =>
                       {
                           var teile = line.Split(',');
                           return new Spiel
                           {
                               Spielnummer = teile[0],
                               SpielerID = teile[1],
                               Punkte = string.IsNullOrWhiteSpace(teile[2]) ? 0 : int.Parse(teile[2])
                           };
                       })
                       .ToList();
        }

        private void DruckenLetztePartie()
        {
            // 1️⃣ Letzte Zeile aus Spiele-Liste holen
            string letzteZeile = spieleListe.Last().Spielnummer;
            string spielPrefix = letzteZeile.Substring(0, 8); // Erste 8 Zeichen der Spielnummer

            // 2️⃣ Alle Spiele mit dieser Spielnummer herausfiltern
            var aktuelleSpiele = spieleListe
                .Where(spiel => spiel.Spielnummer.StartsWith(spielPrefix))
                .OrderBy(spiel => int.Parse(spiel.Spielnummer.Substring(8, 2))) // Nach letzten beiden Zahlen sortieren
                .ToList();

            // 3️⃣ Punkte für jeden Spieler berechnen
            Dictionary<string, int> spielerPunkte = new Dictionary<string, int>();

            foreach (var spiel in aktuelleSpiele)
            {
                if (!spielerPunkte.ContainsKey(spiel.SpielerID))
                    spielerPunkte[spiel.SpielerID] = 0;

                // Falls ein Spieler keine Punkte hat, setzen wir ein X (Gewinner)
                spielerPunkte[spiel.SpielerID] += string.IsNullOrWhiteSpace(spiel.Punkte.ToString()) ? 0 : spiel.Punkte;
            }

            // 4️⃣ Spieler-IDs in Spitznamen umwandeln
            var spielerErgebnisse = spielerPunkte
                .Select(spieler => new
                {
                    Spitzname = spielerListe.FirstOrDefault(s => s.ID == spieler.Key)?.Spitzname ?? "Unbekannt",
                    Punkte = spieler.Value
                })
                .OrderByDescending(spieler => spieler.Punkte) // Gewinner zuerst
                .ToList();

            // 5️⃣ Sieger festlegen (Gold, Silber, Bronze)
            string[] platzierungen = { "Gold", "Silber", "Bronze" };
            for (int i = 0; i < Math.Min(spielerErgebnisse.Count, platzierungen.Length); i++)
            {
                lstErgebnisse.Items.Clear();
                foreach (var spieler in spielerErgebnisse)
                {
                    lstErgebnisse.Items.Add($"{spieler.Spitzname}: {spieler.Punkte} Punkte");
                }
                
            }

            // 🖨️ Hier könnte eine Funktion `DruckeErgebnis(spielerErgebnisse)` kommen
        }


        private void btnBackToMain_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Form1 wieder sichtbar machen
            this.Hide(); // PrintAnalyseForm ausblenden
        }

        private void btnLastGame_Click(object sender, EventArgs e)
        {
            DruckenLetztePartie();
        }
    }

}
public class Spiel
{
    public string SpielerID { get; set; }
    public string Spielnummer { get; set; }
    public int Punkte { get; set; }
}