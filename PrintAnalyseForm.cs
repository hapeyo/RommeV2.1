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
       foreach (var zeile in File.ReadAllLines("spielerdaten.csv").Skip(1))
{
    var teile = zeile.Split(',');
    //MessageBox.Show($"SpielerID aus Spielerdaten.csv: '{teile[0]}' Länge: {teile[0].Length}");
}

            if (!File.Exists(dateiPfad))
            return new List<Spiel>();
            foreach (var teile in File.ReadAllLines(NbrJug.dateiPfad).Skip(1))
            {
                var daten = teile.Split(',');
               // MessageBox.Show($"Eingelesene SpielerID: '{daten[1]}' - Länge: {daten[1].Length}");
            }

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
            // 1️⃣ Letzte Spielnummer bestimmen
            string letzteZeile = spieleListe.Last().Spielnummer;
            string spielPrefix = letzteZeile.Substring(0, 8);

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
                row["Spielnummer"] = spiel.Spielnummer.Substring(8,2);

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
            string pfad = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "LetztePartie.pdf");

            using (FileStream fs = new FileStream(pfad, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Überschrift hinzufügen
                doc.Add(new Paragraph("Rommé", FontFactory.GetFont(FontFactory.HELVETICA, 20, iTextSharp.text.Font.BOLD)));

                // 🗓️ Datum aus Spielnummer extrahieren
                string spielDatumRaw = spieleListe.Last().Spielnummer.Substring(0, 6);
                string spielDatum = $"{spielDatumRaw.Substring(4, 2)}.{spielDatumRaw.Substring(2, 2)}.{spielDatumRaw.Substring(0, 2)}";

                // Gesamtpartie aus Spielnummer extrahieren
                string gesamtpartie = spieleListe.Last().Spielnummer.Substring(6, 2);

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


            MessageBox.Show($"PDF gespeichert unter: {pfad}");
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