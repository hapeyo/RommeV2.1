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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml.Linq;
using System.Diagnostics;

namespace Romme_V2
{

    public partial class PrintAnalyseForm : Form
    {
        private NbrJug mainForm;
        private List<Spieler> spielerListe;
        private List<Spiel> spieleListe;
        private string spielZeile;
       
        private bool programmaticSelection = false; // Variable zum Nachverfolgen, ob die Auswahl programmgesteuert vorgenommen wurde
        public string SpielCode { get; set; }
        public string SpielerID { get; set; }
        public int Punkte { get; set; }


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
            spieleListe = LadeSpiele(spieleDateiPfad); // Spiele laden



        }
        public List<Spiel> LadeSpiele(string dateiPfad)
        {
            if (!File.Exists(dateiPfad))
                return new List<Spiel>();

            return File.ReadAllLines(dateiPfad)
                .Skip(1)
                .Select(line =>
                {
                    var daten = line.Split(',');
                    return new Spiel
                    {
                        Spielnummer = daten[0],
                        SpielerID = daten[1],
                        Punkte = string.IsNullOrWhiteSpace(daten[2]) ? 0 : int.Parse(daten[2])
                    };
                })
                .ToList();
        }

        private void druckenBestimmtePartie()
        {
            // 1️⃣ Letzte Spielnummer bestimmen
            // string spielZeile = spieleListe.Last().Spielnummer;
            string spielPrefix = spielZeile.Substring(0, 8);

            // 2️⃣ Nur Spieler aus dem letzten Spiel herausfiltern
            var aktuelleSpiele = spieleListe
                .Where(spiel => spiel.Spielnummer.StartsWith(spielPrefix))
                .OrderBy(spiel => int.Parse(spiel.Spielnummer.Substring(8, 2)))
                .ToList();

            // 3️⃣ Liste der Spieler im letzten Spiel
            var teilnehmendeSpieler = aktuelleSpiele.Select(spiel => spiel.SpielerID).Distinct().ToList();

            // 4️⃣ Punkte für das letzte Spiel berechnen
            var spielGruppiert = aktuelleSpiele
                .GroupBy(spiel => spiel.Spielnummer)
                .Select(gruppe => new
                {
                    Spielnummer = gruppe.Key,
                    PunkteMap = gruppe.Where(spiel => teilnehmendeSpieler.Contains(spiel.SpielerID))
                                     .ToDictionary(spiel => spiel.SpielerID, spiel => spiel.Punkte)
                })
                .ToList();

            // 5️⃣ DataTable für die DataGridView erstellen
            DataTable dt = new DataTable();
            dt.Columns.Add("Spielnummer", typeof(string));

            // Nur teilnehmende Spieler-Spalten hinzufügen
            foreach (var spieler in spielerListe.Where(spieler => teilnehmendeSpieler.Contains(spieler.ID)))
            {
                dt.Columns.Add(spieler.Spitzname, typeof(int));
            }

            // 6️⃣ Daten für DataGridView einfügen
            foreach (var spiel in spielGruppiert)
            {
                DataRow row = dt.NewRow();
                row["Spielnummer"] = spiel.Spielnummer.Substring(8, 2);

                foreach (var spieler in spielerListe.Where(spieler => teilnehmendeSpieler.Contains(spieler.ID)))
                {
                    row[spieler.Spitzname] = spiel.PunkteMap.ContainsKey(spieler.ID) ? spiel.PunkteMap[spieler.ID] : 0;
                }

                dt.Rows.Add(row);
            }

            // 7️⃣ Summe der Punkte berechnen und als letzte Zeile einfügen
            DataRow sumRow = dt.NewRow();
            sumRow["Spielnummer"] = "Gesamt"; // Letzte Zeile für die Summen

            foreach (var spieler in spielerListe.Where(spieler => teilnehmendeSpieler.Contains(spieler.ID)))
            {
                sumRow[spieler.Spitzname] = dt.AsEnumerable()
                    .Where(row => row["Spielnummer"].ToString() != "Gesamt") // Summen nicht mitberechnen
                    .Sum(row => Convert.ToInt32(row[spieler.Spitzname]));
            }

            dt.Rows.Add(sumRow); // Summenzeile hinzufügen

            // 8️⃣ DataTable an DataGridView übergeben
            dGVLastPartie.DataSource = dt;

            // Spaltenüberschriften setzen
            dGVLastPartie.Columns["Spielnummer"].HeaderText = "Spielnummer";
            foreach (var spieler in spielerListe.Where(spieler => teilnehmendeSpieler.Contains(spieler.ID)))
            {
                dGVLastPartie.Columns[spieler.Spitzname].HeaderText = spieler.Spitzname;
            }
            DruckenAlsPDF();
        }

        private void DruckenAlsPDF()//diese Methode gibt ein verbessertes PDF aus
        {
            Document doc = new Document();
            string dateiNameRaw = spielZeile.Substring(0, 8);
            string dateiName = $"Romme{dateiNameRaw}.pdf";
            string pfad = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), dateiName);

            using (FileStream fs = new FileStream(pfad, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Überschrift hinzufügen
                doc.Add(new Paragraph("Rommé", FontFactory.GetFont(FontFactory.HELVETICA, 20, iTextSharp.text.Font.BOLD)));

                // 🗓️ Datum aus Spielnummer extrahieren
                string spielDatumRaw = spielZeile.Substring(0, 6);
                string spielDatum = $"{spielDatumRaw.Substring(4, 2)}.{spielDatumRaw.Substring(2, 2)}.{spielDatumRaw.Substring(0, 2)}";

                // Gesamtpartie aus Spielnummer extrahieren
                string gesamtpartie = spielZeile.Substring(6, 2);

                // Datum und Gesamtpartie hinzufügen
                doc.Add(new Paragraph($"Fecha del partido: {spielDatum}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                doc.Add(new Paragraph($"Partido: {gesamtpartie}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));

                // 🎭 Daten aus DataGridView übernehmen
                PdfPTable table = new PdfPTable(dGVLastPartie.Columns.Count);

                // Spaltenüberschriften hinzufügen
                foreach (DataGridViewColumn column in dGVLastPartie.Columns)
                {
                    string headerText = column.HeaderText == "Spielnummer" ? "Spiel" : column.HeaderText;
                    table.AddCell(new PdfPCell(new Phrase(headerText)));
                }

                // Zeilen hinzufügen
                foreach (DataGridViewRow row in dGVLastPartie.Rows)
                {
                    if (row.IsNewRow) continue; // Überspringe die neue Zeile

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        table.AddCell(new PdfPCell(new Phrase(cell.Value?.ToString() ?? string.Empty)));
                    }
                }

                doc.Add(table);

                // Spieler mit niedrigsten Punktzahlen Symbole zuweisen (Gold, Silber, Bronze)
                var relevanteSpieler = dGVLastPartie.Columns.Cast<DataGridViewColumn>()
                    .Where(col => col.Name != "Spielnummer" && col.Name != "Gesamt")
                    .Select(col => col.Name)
                    .ToList();

                var spielerSummen = relevanteSpieler?
                     .Select(spitzname => new
                     {
                         Spieler = spitzname,
                         Summe = dGVLastPartie?.Rows?.Cast<DataGridViewRow>()
                        .Where(row => row.Cells["Spielnummer"]?.Value?.ToString() != "Gesamt")
                        .Sum(row => Convert.ToInt32(row.Cells[spitzname]?.Value ?? 0))
                     }
                     )
                .OrderBy(spiel => spiel.Summe)
                .ToList();

                // Tabelle für das Siegertreppchen mit drei Spalten
                PdfPTable podiumTable = new PdfPTable(new float[] { 1f, 0.7f, 1f }); // Mitte schmaler für engeres Layout
                podiumTable.WidthPercentage = 40; // Engeres Layout
                podiumTable.DefaultCell.Border = PdfPCell.NO_BORDER; // Keine Ränder!

                // Medaillenbilder-Pfade
                string[] medalPaths = { "resources/goldmedallie.jpg", "resources/silbermedallie.jpg", "resources/bronzemedallie.jpg" };

                // **Erste Zeile: Der Sieger alleine in der Mitte**
                PdfPCell winnerCell = new PdfPCell();
                winnerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                winnerCell.Border = PdfPCell.NO_BORDER; // Rand entfernt!

                // Siegername zentriert
                Paragraph winnerText = new Paragraph(spielerSummen[0].Spieler);
                winnerText.Alignment = Element.ALIGN_CENTER;
                winnerCell.AddElement(winnerText);

                // Goldmedaille darunter zentrieren
                if (File.Exists(medalPaths[0]))
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(medalPaths[0]);
                    image.ScaleToFit(40f, 40f);
                    image.Alignment = Element.ALIGN_CENTER;
                    winnerCell.AddElement(image);
                }

                // Leere Zellen links und rechts für Balance (jetzt schmaler)
                PdfPCell emptyCell1 = new PdfPCell(new Phrase(" "));
                PdfPCell emptyCell2 = new PdfPCell(new Phrase(" "));
                emptyCell1.Border = PdfPCell.NO_BORDER;
                emptyCell2.Border = PdfPCell.NO_BORDER;

                podiumTable.AddCell(emptyCell1);
                podiumTable.AddCell(winnerCell);
                podiumTable.AddCell(emptyCell2);

                // **Zweite Zeile: Silber und Bronze näher an die Mitte bringen**
                PdfPCell secondCell = new PdfPCell();
                PdfPCell thirdCell = new PdfPCell();
                secondCell.HorizontalAlignment = Element.ALIGN_CENTER;
                thirdCell.HorizontalAlignment = Element.ALIGN_CENTER;
                secondCell.Border = PdfPCell.NO_BORDER;
                thirdCell.Border = PdfPCell.NO_BORDER;

                // Namen und Medaillen für Silber und Bronze
                for (int i = 1; i < 3 && i < spielerSummen.Count; i++)
                {
                    Paragraph playerText = new Paragraph(spielerSummen[i].Spieler);
                    playerText.Alignment = Element.ALIGN_CENTER;

                    PdfPCell currentCell = (i == 1) ? secondCell : thirdCell;
                    currentCell.AddElement(playerText);

                    if (File.Exists(medalPaths[i]))
                    {
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(medalPaths[i]);
                        image.ScaleToFit(30f, 30f);
                        image.Alignment = Element.ALIGN_CENTER;
                        currentCell.AddElement(image);
                    }
                }

                // Zweite Zeile enger machen durch eine kleinere Mittelspalte
                podiumTable.AddCell(secondCell);
                podiumTable.AddCell(new PdfPCell { Border = PdfPCell.NO_BORDER, FixedHeight = 10f }); // Schmalere Mittelbox für engeren Abstand
                podiumTable.AddCell(thirdCell);

                // Podium unter die Haupttabelle setzen
                doc.Add(podiumTable);
                doc.Close();


            }

            MessageBox.Show($"PDF grabado en: {pfad}. Para imprimir usa un pdf Reader");
        }

        private void btnBackToMain_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Form1 wieder sichtbar machen
            this.Hide(); // PrintAnalyseForm ausblenden

        }

        private void btnLastGame_Click(object sender, EventArgs e)
        {
            //Letzte Spielnummer bestimmen
            spielZeile = spieleListe.Last().Spielnummer;
            // string spielPrefix = spielZeile.Substring(0, 8);
            druckenBestimmtePartie();

        }
        private List<string> HoleAlleSpielPrefixe()
        {
            return spieleListe
                .Select(spiel => spiel.Spielnummer.Substring(0, 8)) // Nur die ersten 8 Zeichen der Spielnummer
                .Distinct() // Doppelte Spielnummern-Präfixe entfernen
                .OrderByDescending(spielPrefix => spielPrefix) // Sortierung nach Datum (neueste zuerst)
                .ToList();
        }
        private void btnOtherGame_Click(object sender, EventArgs e)
        {
            // Liste mit allen verfügbaren Spielnummern-Präfixen abrufen
            List<string> spielPrefixe = HoleAlleSpielPrefixe();

            // ComboBox leeren und neu befüllen
            cbBSpielnummer.Items.Clear();
            cbBSpielnummer.Items.AddRange(spielPrefixe.ToArray());

            // Erste Option auswählen (optional)
            if (cbBSpielnummer.Items.Count > 0)
            {
                programmaticSelection = true; // Setzen Sie die Variable vor der Auswahl
                cbBSpielnummer.SelectedIndex = 0;
                programmaticSelection = false; // Setzen Sie die Variable nach der Auswahl zurück
            }

        }
        /***********************+
                private void btIPrintPdf_Click(object sender, EventArgs e)
                {

                }
        ********************/
        private void cbBSpielnummer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!programmaticSelection && cbBSpielnummer.SelectedIndex != -1)
            {
                spielZeile = cbBSpielnummer.SelectedItem.ToString();
                MessageBox.Show($"Gewählte Spielnummer: {spielZeile}");
                druckenBestimmtePartie();
            }
        }
        public static List<SpielerStatistik> BerechneRanking(List<Spiel> spieleListe)
        {
            var spielerGruppiert = spieleListe.GroupBy(s => s.SpielerID);
            List<SpielerStatistik> rankingListe = new List<SpielerStatistik>();

            // Wörterbuch, um für jeden Spieler die Anzahl der gewonnenen Partien zu speichern
            var gewonnenPartien = new Dictionary<string, int>();
            var gewonnenSpielFaktoren = new Dictionary<string, double>();

            // Gruppiere Spiele nach Partien
            var partien = spieleListe.GroupBy(s => s.Spielnummer.Substring(0, 8));

            foreach (var partie in partien)
            {
                int anzahlSpielerInPartie = partie.Select(s => s.SpielerID).Distinct().Count(); // Anzahl der Spieler in der Partie
                double spielerAnzahlFaktor;
                switch(anzahlSpielerInPartie)
                {
                    case 5:
                        spielerAnzahlFaktor = 1.0;
                        break;
                    case 4:
                        spielerAnzahlFaktor = 0.9;
                        break;
                    case 3:
                        spielerAnzahlFaktor = 0.8;
                        break;
                    case 2:
                        spielerAnzahlFaktor = 0.7;
                        break;
                    default:
                        spielerAnzahlFaktor = 0.0; // Sicherheitswert für unbekannte Spieleranzahl
                        break;
                      
                }

                // Berechne die Gesamtpunktzahl jedes Spielers in der Partie
                var punkteProSpieler = partie.GroupBy(sp => sp.SpielerID)
                                             .Select(gr => new { SpielerID = gr.Key, Gesamtpunkte = gr.Sum(sp => sp.Punkte) });

                // Bestimme die niedrigste Gesamtpunktzahl in der Partie
                var niedrigstePunktzahl = punkteProSpieler.Min(p => p.Gesamtpunkte);

                // Finde den Spieler mit der niedrigsten Punktzahl und zähle die Partie für ihn
                foreach (var spieler in punkteProSpieler.Where(p => p.Gesamtpunkte == niedrigstePunktzahl))
                {
                    double gewinnFaktor = (double)1 / partie.Count() * spielerAnzahlFaktor; // Gewinnfaktor berechnen

                    if (!gewonnenPartien.ContainsKey(spieler.SpielerID))
                    {
                        gewonnenPartien[spieler.SpielerID] = 0; // Initialisiere den Zähler
                    }
                    gewonnenPartien[spieler.SpielerID]++;

                    if (!gewonnenSpielFaktoren.ContainsKey(spieler.SpielerID))
                    {
                        gewonnenSpielFaktoren[spieler.SpielerID] = 0.0; // Initialisiere den Gewinnfaktor
                    }
                    gewonnenSpielFaktoren[spieler.SpielerID] += gewinnFaktor; // Addiere den Gewinnfaktor für die Partie
                }
            }

            // Durchschnittswerte berechnen
            foreach (var spielerId in gewonnenSpielFaktoren.Keys.ToList())
            {
                gewonnenSpielFaktoren[spielerId] /= gewonnenPartien[spielerId]; // Durchschnitt berechnen
            }

            foreach (var spieler in spielerGruppiert)
            {
                var spiele = spieler.ToList();
                int ap = spiele.Select(s => s.Spielnummer.Substring(0, 8)).Distinct().Count();  // Anzahl Partien
                int ans = spiele.Count; // Anzahl Spiele
                int gpz = spiele.Sum(s => s.Punkte); // Gesamtpunktzahl
                string SpielerId = spieler.Key;
                int gp = gewonnenPartien.ContainsKey(SpielerId) ? gewonnenPartien[SpielerId] : 0; // Anzahl der gewonnenen Partien übernehmen
                int gs = spiele.Count(s => s.Punkte == 0); // Gewonnene Spiele

                double gewinnSpielFaktor = gewonnenSpielFaktoren.ContainsKey(SpielerId) ? gewonnenSpielFaktoren[SpielerId] : 0.0; // Gewinnfaktor aus Spielen übernehmen
               // MessageBox.Show($"Spieler {spieler.Key} hat einen Gewinnfaktor von {gewinnSpielFaktor}.");  
                rankingListe.Add(new SpielerStatistik
                {
                    SpielerID = spieler.Key,
                    Name = "Spieler " + spieler.Key,
                    AP = ap,
                    AS = ans,
                    GP = gp,
                    GS = gs,
                    GPZ = gpz,
                    GewinnSpielFaktor = gewinnSpielFaktor // Hinzufügen des Gewinnfaktors aus Spielen
                });
            }

            MessageBox.Show("Ranking erfolgreich berechnet!");
            return rankingListe.OrderBy(s => s.BerechneRanking()).ToList(); // Sortieren nach Ranking
        }


        public static void ErstelleRankingPDF(List<SpielerStatistik> rankingListe, List<Spieler> spielerListe, string pfad)
        {
            
            Document document = new Document();
            

            PdfWriter.GetInstance(document, new FileStream(pfad, FileMode.Create));
            document.Open();

            PdfPTable table = new PdfPTable(6); // 6 Spalten
            table.AddCell("Platz"); table.AddCell("Name"); table.AddCell("Ranking");
            table.AddCell("Gew. Partien"); table.AddCell("Gew. Spiele"); table.AddCell("Ges. Punkte");

            int platz = 1;
            foreach (var spieler in rankingListe)
            {
                
                table.AddCell(platz.ToString());
                  
                  string spielerID = spieler.Name.Split(' ').Last(); // Nimmt das letzte Element nach Leerzeichen
                  var spielerObjekt = spielerListe.FirstOrDefault(s => s.ID == spielerID);
                  if (spielerObjekt != null)
                  {
                      string spielerNachVorname = $"{spielerObjekt.Nachname}, {spielerObjekt.Vorname}";
                      table.AddCell(spielerNachVorname);
                  }
                  else
                  {
                      table.AddCell("Unbekannter Spieler"); // Falls die ID nicht gefunden wird
                  }
                  
               // MessageBox.Show($"Spielername: {spieler.Name}");
               // table.AddCell(spieler.Name);
                table.AddCell(spieler.BerechneRanking().ToString("0.00"));
                table.AddCell($"{spieler.GP}:{spieler.AP}");
                table.AddCell($"{spieler.GS}:{spieler.AS}");
                table.AddCell(spieler.GPZ.ToString());
                platz++;
            }

            document.Add(table);
            document.Close();
            MessageBox.Show("Pdf erstellen durchlaufen");
        }

        private void btnRanking_Click(object sender, EventArgs e)
        {
            List<SpielerStatistik> rankingListe = BerechneRanking(spieleListe);
            string datum = DateTime.Now.ToString("yyMMdd");
            string dateiName = $"RommeRanking{datum}.pdf";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, dateiName);

            ErstelleRankingPDF(rankingListe, spielerListe, filePath);

            // BerechneRanking(spieleListe);
            // ErstelleRankingPDF(BerechneRanking(spieleListe), "Ranking.pdf");
        }


    }
    public class Spiel
    {
        public string SpielerID { get; set; }
        public string Spielnummer { get; set; }
        public int Punkte { get; set; }
    }

    public class SpielerStatistik
    {
        public string SpielerID { get; set; }
        public string Name { get; set; }
        public int AP { get; set; }  // Anzahl Partien
        public int AS { get; set; }  // Anzahl Spiele
        public int GP { get; set; }  // Gewonnene Partien (kleinste Summe)
        public int GS { get; set; }  // Gewonnene Spiele (0 Punkte)
        public int GPZ { get; set; } // Gesamtpunktezahl
        public double GewinnSpielFaktor { get; set; } // Gewinnfaktor aus gewonnenen Spielen


        // Methode zur Berechnung der durchschnittlichen Punkte pro Spiel
        private double BerechneGPZProSpiel()
        {
            double result = (double)GPZ / AS;
      //     MessageBox.Show($"GPZ: {GPZ}, AS: {AS}, GPZ pro Spiel: {result}", "Prüfausgabe - GPZProSpiel");
            return result;
        }

        // Methode zur Berechnung des Gewinnfaktors
        private double BerechneGewinnFaktor()
        {
            //double result = (double)(GP / AP + GS / AS) / 2;
            double result = (GP / (double)AP + (double)GewinnSpielFaktor) / 2;
         //   MessageBox.Show($"GP: {GP}, AP: {AP}, GS: {GS}, AS: {AS}, Gewinnfaktor: {result}", "Prüfausgabe - GewinnFaktor");
            return result;
        }

        // Methode zur Berechnung des Rankings
        public double BerechneRanking()
        {
            var gpzProSpiel = BerechneGPZProSpiel();
            var gewinnFaktor = BerechneGewinnFaktor();
            double result = gpzProSpiel - (gpzProSpiel * gewinnFaktor);
           // MessageBox.Show($"GPZ pro Spiel: {gpzProSpiel}, Gewinnfaktor: {gewinnFaktor}, Ranking: {result}", "Prüfausgabe - Ranking");
            return result;
        }
    }

    /*****************
    public class SpielerStatistik
    {
        public string SpielerID { get; set; }
        public string Name { get; set; }
        public int AP { get; set; }  // Anzahl Partien
        public int AS { get; set; }  // Anzahl Spiele
        public int GP { get; set; }  // Gewonnene Partien (kleinste Summe)
        public int GS { get; set; }  // Gewonnene Spiele (0 Punkte)
        public int GPZ { get; set; } // Gesamtpunktezahl

        public double BerechneRanking()
        {
            return (double)GPZ / AS - ((double)GPZ / AS - (double)(GP / AP + GS / AS) / 2);
        }
    }
    **************/
}