using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Romme_V2
{
    
    public partial class NbrJug : Form
    {
        private const int MaxPlayers = 5;
        private int spielNummer = 0;
        private int numPlayer = 0;
        private List<int[]> gesamtPunkte = new List<int[]>();
        private DateTime aktuellesDatum = DateTime.Today;
        private int partieZaehler = 1;
        public static string dateiPfad = "spiele.csv";
        private List<Spieler> spielerListe = new List<Spieler>();
        private string filePath = "spielerdaten.csv"; // Pfad zur CSV-Datei
        private PunktVerwaltung punktVerwaltung = new PunktVerwaltung();
        private List<(string spielNummerFinal, string spielerID, int [] punkteProSpiel)> spielPunkte = new List<(string, string, int[])>();// Liste von Tupeln, die die Spielnummer, ID des Spielers und Punkte enthalten
        private System.Windows.Forms.Label[] players;

        public NbrJug(bool isAdminMode = false)
        {
            InitializeComponent();
            LadeSpieler();
            InitializePlayers();
            if (isAdminMode)
            {
                ApplyAdminMode();
            }
            //btnSavePoints.Click += clcBtn_Click;
               

        }
        private void InitializePlayers()
        {
            players = new System.Windows.Forms.Label[] { player1, player2, player3, player4, player5 };
        }

        private void NbrJug_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToString("dd.MM.yy");
        }


       
        public string GeneriereSpielNummer(string spielNummerOldGame = null) //Generiert einen eindeutigen Spielnamen
        {
            if (spielNummerOldGame == null)
            {
                if (DateTime.Today != aktuellesDatum)
                {
                    aktuellesDatum = DateTime.Today;
                    partieZaehler = 1; // Zurücksetzen des Partiezählers für den neuen Tag
                }
                spielNummerOldGame = aktuellesDatum.ToString("yyMMdd"); // Spielnummer für den aktuellen Tag
            }
            string spielNummerFinal = $"{spielNummerOldGame}{partieZaehler:D2}{spielNummer:D2}";
            return spielNummerFinal;
        }
        private void PartieZaehler(string spielNummerOldGame = null)
        {
            string datumAktuell = spielNummerOldGame ?? DateTime.Now.ToString("yyMMdd"); // Falls kein Datum übergeben wird, verwende das aktuelle Datum
            partieZaehler = 1; // Standardwert für den ersten Eintrag

            if (File.Exists(dateiPfad))
            {
                string[] zeilen = File.ReadAllLines(dateiPfad);
                int maxPartiezaehler = 0;
                if (zeilen.Length > 0) //Beim ersten Start der app ist die Datei Spiele noch leer
                {
                    foreach (string zeile in zeilen)
                    {
                        string spielnummerID = zeile.Split(',')[0]; // Extrahiere die Spielnummer
                        if (spielnummerID.StartsWith(datumAktuell)) // Vergleiche die ersten sechs Zeichen (Datum)
                        {
                            string partiezaehlerTeil = spielnummerID.Substring(6, 2);
                            int aktuellerZaehler = Convert.ToInt32(partiezaehlerTeil);
                            maxPartiezaehler = Math.Max(maxPartiezaehler, aktuellerZaehler);
                        }
                    }

                    partieZaehler = maxPartiezaehler + 1; // Erhöhe den höchsten gefundenen Zähler um eins
                }
            }
        }
        /*********************************
         private void PartieZaehler(string spielNummerOldGame = null) // Zählt die Anzahl der Partien, die an einem Tag gespielt wurden und regelt den Partiezähler
         {
             string datumAktuell = spielNummerOldGame ?? DateTime.Now.ToString("yyMMdd"); // Falls kein Datum übergeben wird, verwende das aktuelle Datum

             if (File.Exists(dateiPfad))
             {
                 string[] zeilen = File.ReadAllLines(dateiPfad);
                 if (zeilen.Length > 0) //Beim ersten Start der app ist die Datei Spiele noch leer
                 {
                     string letzteZeile = zeilen[zeilen.Length - 1];
                     string spielnummerID = letzteZeile.Split(',')[0];
                     string datumTeil = spielnummerID.Substring(0, 6);
                     string partiezaehlerTeil = spielnummerID.Substring(6, 2);
                     int result = Convert.ToInt32(partiezaehlerTeil);
                     //string datumAktuell = DateTime.Now.ToString("yyMMdd");

                     if (datumTeil == datumAktuell)
                     {
                         partieZaehler = result + 1;
                     }
                 }
             }
         }
          ***********/
        // Speichert Spielinformationen in der CSV-Datei
        private void SpielSpeichern(string dateiPfad)
        {
            DialogResult result = MessageBox.Show("Guardar el partido?  Cuidado, si elige [no], los datos de este partido estan borrados!", "Verificación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Guardando partido!");
                using (StreamWriter writer = new StreamWriter(dateiPfad, true)) // 'true' fügt Daten an die Datei an
                {
                    foreach (var spiel in spielPunkte)
                    {
                      
                        for (int i = 0; i < spiel.punkteProSpiel.Length; i++)
                        {
                            // Format: Spielnummer, SpielerID, Punkte des Spielers
                            writer.WriteLine($"{spiel.spielNummerFinal},{spiel.spielerID.Trim()},{spiel.punkteProSpiel[i]}");
                        }
                    }
                }
                foreach (var zeile in File.ReadAllLines("spiele.csv").Skip(1))
                {
                    var teile = zeile.Split(',');
                    
                }

            }
            else
            {
                MessageBox.Show("OK, este partido no esta guardado y los datos estan borados.");
            }

            
        }
        
        //Das ist die Berechnung der Punkte, nach Eingabe der Punkte wird
        //die Summe aus allen Spielen angezeigt und die Eingabefelder werden wieder auf 
        //Null gesetzt.Ausserdem wird der Punkt fürs Kartenausteilen weitergesetzt
        private void clcBtn_Click(object sender, EventArgs e)
        {
            string spielNummerOldGame = btnSavePoints.Tag as string; // Spielnummer aus Tag holen
            string spielNummerFinal; 
            // Prüfen, ob ein altes Spiel behandelt wird
            if (!string.IsNullOrEmpty(spielNummerOldGame))
            {
                spielNummerFinal = GeneriereSpielNummer(spielNummerOldGame); // Altes Spiel nutzen
            }
            else
            {
                spielNummerFinal = GeneriereSpielNummer(); // Aktuelles Datum nutzen
            }

            MessageBox.Show($"Generierte Spielnummer: {spielNummerFinal}", "SpielNummer");
        



            string[] pointsFields = { pointsPl1.Text, pointsPl2.Text, pointsPl3.Text, pointsPl4.Text, pointsPl5.Text };
            string[] sumFields = { sumPl1.Text, sumPl2.Text, sumPl3.Text, sumPl4.Text, sumPl5.Text };
            string[] spielerIDs = {
                                    player1?.Tag != null ? player1.Tag.ToString() : string.Empty,
                                    player2?.Tag != null ? player2.Tag.ToString() : string.Empty,
                                    player3?.Tag != null ? player3.Tag.ToString() : string.Empty,
                                    player4?.Tag != null ? player4.Tag.ToString() : string.Empty,
                                    player5?.Tag != null ? player5.Tag.ToString() : string.Empty
};

            //string alleSpielerIDs = $"{spielerIDs[0]}, {spielerIDs[1]}, {spielerIDs[2]}";
            
            
            
            if (!ErrorHandling.FehlerbehandlungPunkte(pointsFields, numPlayer))
            {
                return; // Stoppe die Ausführung, wenn ein Fehler gefunden wird
            }

            // Punkte array für das aktuelle Spiel und spielerID
            int[] punkteProSpiel = new int[numPlayer]; // Punkte pro Spieler für das aktuelle Spiel



            for (int i = 0; i < numPlayer; i++)
            {
                string spielerID = spielerIDs[i];
                int punkte = 0;

                if (pointsFields[i] != " " && Int32.TryParse(pointsFields[i], out int v))
                {
                    
                    punkteProSpiel[i] = v;
                    punkte = v;
                    
                }
             //Füge das Array mit den Punkten des aktuellen Spiels zur Liste hinzu
                spielPunkte.Add((spielNummerFinal, spielerID, new int[]{ punkte }));

            }
            //string punkteAnzeigen = string.Join(", ", punkteProSpiel);


            string ausgabe = "";
            foreach (var spiel in spielPunkte)
            {
                for (int i = 0; i < spiel.punkteProSpiel.Length; i++)
                {
                    ausgabe += $"Spielnummer: {spiel.spielNummerFinal}, SpielerID: {spiel.spielerID}, Punkte: {spiel.punkteProSpiel[i]}{Environment.NewLine}";
                }
            }
            //MessageBox.Show(ausgabe, "Überprüfung der Liste");

            ResetPointsFieldsText(new System.Windows.Forms.TextBox[] { pointsPl1, pointsPl2, pointsPl3, pointsPl4, pointsPl5 });

            // Füge die Punkte für das aktuelle Spiel zum Array hinzu
            int[] rechnungGesamt = new int[numPlayer];
            for (int i = 0; i < numPlayer; i++)
            {
                if (Int32.TryParse(sumFields[i], out int v))
                {
                    v += punkteProSpiel[i]; ;
                    rechnungGesamt[i] = v;
                    sumFields[i] = v.ToString();
                }
            }
            // Füge das Array mit den aktuellen GesamtPunkten  zur Liste hinzu
            gesamtPunkte.Add(rechnungGesamt);

            sumPl1.Text = sumFields[0];
            sumPl2.Text = sumFields[1];
            sumPl3.Text = sumFields[2];
            sumPl4.Text = sumFields[3];
            sumPl5.Text = sumFields[4];

            // Dies ist der Spielezaehler
            ++spielNummer;
            gameCtr.Text = spielNummer.ToString();

            UpdateGeber();
        }
        private void UpdateGeber()
        {
            // array für Geber
            RadioButton[] cckBarajes = { cckBaraje1, cckBaraje2, cckBaraje3, cckBaraje4, cckBaraje5 };
            

            for (int k = 0; k < cckBarajes.Length - 1; k++)
            {
                if (cckBarajes[k].Checked)
                {
                    cckBarajes[k].Checked = false;
                    if (players[k + 1].Text != $"Jugador/a")
                    {
                        cckBarajes[k + 1].Checked = true;
                    }
                    else
                    {
                        cckBarajes[0].Checked = true;
                    }
                    return;
                }
                if (cckBarajes[cckBarajes.Length - 1].Checked)
                {
                    cckBarajes[cckBarajes.Length - 1].Checked = false;
                    cckBarajes[0].Checked = true;

                }
            }
        }
        
         private void strtJuego_Click(object sender, EventArgs e)
         {

            System.Windows.Forms.Label[] players = { player1, player2, player3, player4, player5 };
            System.Windows.Forms.TextBox[] pointsFields = { pointsPl1, pointsPl2, pointsPl3, pointsPl4, pointsPl5 };
            System.Windows.Forms.Label[] sumFields = { sumPl1, sumPl2, sumPl3, sumPl4, sumPl5 };
            string spielNummerOldGame = btnSavePoints.Tag as string; // Spielnummer aus Tag holen

            ++spielNummer;
            gameCtr.Text = spielNummer.ToString();
            CerrarApp.Enabled = false;
            //PartieZaehler();
            
            // Prüfen, ob ein altes Spiel behandelt wird
            if (!string.IsNullOrEmpty(spielNummerOldGame))
            {
                PartieZaehler(spielNummerOldGame); // Altes Spiel nutzen
            }
            else
            {
                PartieZaehler(); // Aktuelles Datum nutzen
            }

            MessageBox.Show($"Generierte Nummer: {partieZaehler}", "PartieZaheler");
            

            if (numPlayer <= 1)
            {
                MessageBox.Show("Por favor añade por lo menos dos jugadores antes de start.");
                return;
            }
            
            txtNewPl.Enabled = false;
            btnPrintAnalyse.Enabled = false;
            pointsPl1.Enabled = true;
            pointsPl2.Enabled = true;
            pointsPl3.Enabled = true;
            pointsPl4.Enabled = true;
            pointsPl5.Enabled = true;
            
            ResetPointsFieldsText(pointsFields);
           
            for (int j = players.Length-1; j >= numPlayer; j--)
            {
                players[j].ForeColor = this.BackColor;
                pointsFields[j].Enabled = false;
                sumFields[j].ForeColor = this.BackColor;
            }
            strtJuego.Enabled = false;
            cckBaraje1.Checked = true;
         }

         private void StopBtn_Click(object sender, EventArgs e)
         {
            SpielSpeichern(dateiPfad);
            pointsPl1.Enabled = false;
            pointsPl2.Enabled = false;
            pointsPl3.Enabled = false;
            pointsPl4.Enabled = false;
            pointsPl5.Enabled = false;
            txtNewPl.Enabled = true;
            ResetPointsFieldsText(new System.Windows.Forms.TextBox[]{ pointsPl1,pointsPl2,pointsPl3,pointsPl4,pointsPl5});
            btnPrintAnalyse.Enabled = true;
            CerrarApp.Enabled = true;
            gameCtr.Text = "0";
            ResetPlayerNames();
            ResetSumFields();
            punktVerwaltung.StringBuilderLeeren();
            spielPunkte.Clear();
            gesamtPunkte.Clear();

            cckBaraje1.Checked = true;
            cckBaraje2.Checked = false;
            cckBaraje3.Checked = false;
            cckBaraje4.Checked = false;
            cckBaraje5.Checked = false;
            numPlayer = 0;
            spielNummer = 0;

         }
         private void ResetPlayerNames()
         {
            player1.Text = "Jugador/a";
            player2.Text = "Jugador/a";
            player3.Text = "Jugador/a";
            player4.Text = "Jugador/a";
            player5.Text = "Jugador/a";
            player1.ForeColor = ForeColor;
            player2.ForeColor = ForeColor;
            player3.ForeColor = ForeColor;
            player4.ForeColor = ForeColor;
            player5.ForeColor = ForeColor;
            player1.Tag = " ";
            player2.Tag = " ";
            player3.Tag = " ";
            player4.Tag = " ";
            player5.Tag = " ";
         }

        private void ResetSumFields()
        {
            sumPl1.Text = "0";
            sumPl2.Text = "0";
            sumPl3.Text = "0";
            sumPl4.Text = "0";
            sumPl5.Text = "0";
            sumPl1.ForeColor = ForeColor;
            sumPl2.ForeColor = ForeColor;
            sumPl3.ForeColor = ForeColor;
            sumPl4.ForeColor = ForeColor;
            sumPl5.ForeColor = ForeColor;
        }

        private void ResetPointsFieldsText(System.Windows.Forms.TextBox[] pointsFields)
        {
            foreach (var field in pointsFields)
            {
                field.Text = " ";
                
            }
        }

        private void showPlaylist_Click(object sender, EventArgs e)
        {
            string[] spielerNamen = new string[numPlayer];
            for (int i = 0; i < numPlayer; i++)
            {
                Label playerLabel = this.Controls[$"Player{i + 1}"] as Label; // Dynamisch das Label finden
                if (playerLabel != null)
                {
                    spielerNamen[i] = playerLabel.Text; // Text des Labels in das Array übertragen
                }
                else
                {
                    spielerNamen[i] = "Unbekannt"; // Fallback, falls das Label nicht existiert
                }
            }
            string message = string.Join(Environment.NewLine, spielerNamen); // Neue Zeile pro Spielername
            
            
            punktVerwaltung.SpielerNamenAnzeigen(spielerNamen);
            punktVerwaltung.PunktListeErstellen(gesamtPunkte);
            MessageBox.Show(punktVerwaltung.gesamtPunkteString.ToString(), "Gesamtpunkte");
            punktVerwaltung.gesamtPunkteString.Clear();
        }

        private void Cerrar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hasta Pronto!", "Congratulations!");
            Application.Exit();
        }

        private void txtSpielerName_TextChanged(object sender, EventArgs e)
        {
            string eingabe = txtNewPl.Text;
            CerrarApp.Enabled = false;
            // Spieler filtern
            var gefilterteSpieler = spielerListe
                .Where(s => s.Vorname.StartsWith(eingabe, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            // Liste aktualisieren
            lstPlayer.Items.Clear();
            foreach (var spieler in gefilterteSpieler)
            {
                lstPlayer.Items.Add($"{spieler.Vorname} {spieler.Nachname}");
            }

            // Wenn keine Spieler mehr passen
            if (!gefilterteSpieler.Any() && !string.IsNullOrEmpty(eingabe))
            {
                btnNewPlayer.Text = "Nuevo/a Jugador/";
                btnNewPlayer.Enabled = true; // Button aktivieren
            }
            else
            {
                btnNewPlayer.Text = ""; 
                btnNewPlayer.Enabled = false; // Button deaktivieren
            }
        }
        private void lstPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Prüfen, ob ein Eintrag ausgewählt ist
            if (lstPlayer.SelectedItem != null)
            {
                // Den ausgewählten Spieler aus der ListBox abrufen
                var selectedSpieler = spielerListe.FirstOrDefault(s =>
                    $"{s.Vorname} {s.Nachname}" == lstPlayer.SelectedItem.ToString());
                
                 // Spitznamen im Label anzeigen
                if (selectedSpieler != null)
                {
                    Label[] playerLabels = { player1, player2, player3, player4, player5 };
                    // Fehlerbehandlung: Überprüfen, ob der Spieler bereits ausgewählt wurde
                    if (ErrorHandling.IsPlayerAlreadySelected(playerLabels, selectedSpieler.ID))
                    {
                        ErrorHandling.ShowPlayerAlreadySelectedError();
                        return; // Methode verlassen, da Spieler bereits ausgewählt wurde
                    }



                    foreach (var label in playerLabels)
                    {
                        if (label.Text == $"Jugador/a")
                        {
                            label.Text = $"{selectedSpieler.Spitzname}";
                            label.Tag = $"{selectedSpieler.ID}";
                            txtNewPl.Text = "";
                            numPlayer++;
                            lstPlayer.Items.Remove(lstPlayer.SelectedItem);
                            lstPlayer.Items.Clear();
                            break;
                        }

                    }
                    if (numPlayer >= 2)
                    {
                        strtJuego.Enabled = true;
                    }

                }
            }
        }
        private string GeneriereSpielerID()
        {
            int maxID = 0;

            // Überprüfen, ob die Datei existiert
            if (File.Exists(filePath))
            {
                // Datei zeilenweise lesen und die höchste ID ermitteln
                var lines = File.ReadAllLines(filePath).Skip(1); // Kopfzeile überspringen
                foreach (var line in lines)
                {
                    var teile = line.Split(',');
                    if (int.TryParse(teile[0], out int id) && id > maxID)
                    {
                        maxID = id; // MaxID aktualisieren
                    }
                }
            }

            // Neue ID erstellen (maxID + 1)
            int neueID = maxID + 1;

            // Rückgabe als 4-stellige Zahl
            return neueID.ToString("D4");
        }

        private void btnNeuerSpieler_Click(object sender, EventArgs e)
        {
            // Neueingabe eines Spielers
            using (var newPlayerForm = new NewPlayerForm(filePath, spielerListe))
            {
                if (newPlayerForm.ShowDialog() == DialogResult.OK)
                {
                    var neuerSpieler = newPlayerForm.Spieler;
                    neuerSpieler.ID = GeneriereSpielerID();
                    spielerListe.Add(neuerSpieler);
                    SpeichereSpieler();
                    MessageBox.Show("Spieler erfolgreich hinzugefügt!", "Info");
                    txtNewPl.Clear(); // Textbox zurücksetzen
                    btnNewPlayer.ForeColor = SystemColors.ControlLight;
                    btnNewPlayer.Enabled = false; // Button deaktivieren
                }
            }
        }

        // Hilfsmethoden für Spieler
        private void LadeSpieler()
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Datei {filePath} wurde nicht gefunden!", "Fehler");
            }

            if (File.Exists(filePath))
            {
                spielerListe = File.ReadAllLines(filePath)
                    .Skip(1)
                    .Select(line =>
                    {
                        var teile = line.Split(',');
                        return new Spieler
                        {
                            ID = teile[0],
                            Vorname = teile[1],
                            Nachname = teile[2],
                            Geburtstag = DateTime.Parse(teile[3]),
                            Spitzname = teile[4]
                        };
                    }).ToList();
            }

        }
        public List<Spieler> GetSpielerListe()
        {
            return spielerListe; // Gibt die vorhandene Liste zurück
        }

        private void SpeichereSpieler()
        {
            var lines = new List<string>
            {
                "ID,Vorname,Nachname,Geburtstag,Spitzname"
            };
            lines.AddRange(spielerListe.Select(s =>
                $"{s.ID},{s.Vorname},{s.Nachname},{s.Geburtstag:yyyy-MM-dd},{s.Spitzname}"));

            File.WriteAllLines(filePath, lines);
        }

       
        
        private void btnPrintAnalyse_Click(object sender, EventArgs e)
        {
            PrintAnalyseForm printForm = new PrintAnalyseForm(this); // Hauptform übergeben
            printForm.Show(); // PrintAnalyseForm anzeigen
            this.Hide(); // Form1 nur ausblenden
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm(this); // Hauptform übergeben
            adminForm.Show(); // AdminForm anzeigen
            this.Hide(); // Form1 nur ausblenden
        }

        private void ToggleEingabeModus()
        {
            // Liste aller betroffenen Elemente mit ihren "Enabled"- und "Visible"-Eigenschaften
            Control[] elements = { strtJuego, showPlaylist, StopBtn, btnPrintAnalyse, CerrarApp, ClcBtn, 
                           btnStrtOldGame, btnSaveOldGame, btnSavePoints, btnAdmin, lblSoloOldGames, dTPOldGameDate };

            foreach (var element in elements)
            {
                element.Enabled = !element.Enabled;  // Invertiere "Enabled"
                element.Visible = !element.Visible;  // Invertiere "Visible"
            }
        }
        
        private void ApplyAdminMode()
        {
            //Eingabe umbauen
            strtJuego.Enabled = false; // Deaktivieren Sie den Button    
            strtJuego.Visible = false; // Verstecken Sie den Button
            showPlaylist.Enabled = false; // Deaktivieren Sie den Button    
            showPlaylist.Visible = false; // Verstecken Sie den Button
            StopBtn.Enabled = false; // Deaktivieren Sie den Button
            StopBtn.Visible = false; // Verstecken Sie den Button
            btnPrintAnalyse.Enabled = false; // Deaktivieren Sie den Button
            btnPrintAnalyse.Visible = false; // Verstecken Sie den Button
            CerrarApp.Enabled = false; // Deaktivieren Sie den Button
            CerrarApp.Visible = false; // Verstecken Sie den Button
            ClcBtn.Enabled = false; // Deaktivieren Sie den Button
            ClcBtn.Visible = false; // Verstecken Sie den Button
            btnAdmin.Enabled = false; // Deaktivieren Sie den Button
            btnAdmin.Visible = false; // Verstecken Sie den Button
                                    // txtNewPl.Enabled = false; // Deaktivieren Sie die Textbox
            btnStrtOldGame.Enabled = true; // aktivieren Sie den Button
            btnStrtOldGame.Visible = true; // zeigen Sie den Button an
            btnSaveOldGame.Enabled = true; // aktivieren Sie den Button
            btnSaveOldGame.Visible = true; // zeigen Sie den Button an
            btnSavePoints.Enabled = true; // aktivieren Sie den Button  
            btnSavePoints.Visible = true; // zeigen Sie den Button an
            lblSoloOldGames.Visible = true; // zeigen Sie das Label an
            dTPOldGameDate.Visible = true; // zeigen Sie das Label an
            dTPOldGameDate.Enabled = true; // aktivieren Sie das Label
           
            
            // string eingabeDatum = PromptUserForInput("Datum für alte Spiele eingeben (yyMMdd):", DateTime.Now.ToString("yyMMdd"));
            //PartieZaehler(eingabeDatum);


        }
        /**********************************
        private void ApplyMainMode()
        {
            //Eingabe umbauen
            strtJuego.Enabled = false;     
            strtJuego.Visible = false; 
            showPlaylist.Enabled = false;     
            showPlaylist.Visible = false; 
            StopBtn.Enabled = false; 
            StopBtn.Visible = false; 
            btnPrintAnalyse.Enabled = false; 
            btnPrintAnalyse.Visible = false; 
            CerrarApp.Enabled = false; 
            CerrarApp.Visible = false; 
            ClcBtn.Enabled = false; 
            ClcBtn.Visible = false; // Verstecken Sie den Button
            txtNewPl.Enabled = false; // Deaktivieren Sie die Textbox
            btnStrtOldGame.Enabled = true; // aktivieren Sie den Button
            btnStrtOldGame.Visible = true; // zeigen Sie den Button an
            btnSaveOldGame.Enabled = true; // aktivieren Sie den Button
            btnSaveOldGame.Visible = true; // zeigen Sie den Button an
            btnSavePoints.Enabled = true; // aktivieren Sie den Button  
            btnSavePoints.Visible = true; // zeigen Sie den Button an
            lblSoloOldGames.Visible = true; // zeigen Sie das Label an
            dTPOldGameDate.Visible = true; // zeigen Sie das Label an
            dTPOldGameDate.Enabled = true; // aktivieren Sie das Label


            // string eingabeDatum = PromptUserForInput("Datum für alte Spiele eingeben (yyMMdd):", DateTime.Now.ToString("yyMMdd"));
            //PartieZaehler(eingabeDatum);


        }
        ******************/
        private void dTPOldGameDate_ValueChanged(object sender, EventArgs e)
        {
           // string formattedDate = dTPOldGameDate.Value.ToString("yyMMdd");
           // PartieZaehler(formattedDate);
            string spielNummerOldGame = dTPOldGameDate.Value.ToString("yyMMdd"); //$"{formattedDate}";
            btnSavePoints.Tag = spielNummerOldGame;
            // Jetzt übergeben wir spielNummerOldGame an die Methode
            string spielNummerFinal = GeneriereSpielNummer(spielNummerOldGame);

            MessageBox.Show($"Spielnummer: {spielNummerOldGame}", "Spielnummer");

            txtNewPl.Enabled = true;
            
        }

        private void dTPOldGameDate_CloseUp(object sender, EventArgs e)
        {
            string spielNummerOldGame = dTPOldGameDate.Value.ToString("yyMMdd"); //$"{formattedDate}";
            btnSavePoints.Tag = spielNummerOldGame;
            // Jetzt übergeben wir spielNummerOldGame an die Methode
            string spielNummerFinal = GeneriereSpielNummer(spielNummerOldGame);

            MessageBox.Show($"Spielnummer: {spielNummerOldGame}", "Spielnummer");

            txtNewPl.Enabled = true;
        }

        private void btnSaveOldGame_Click(object sender, EventArgs e)
        {
            StopBtn_Click(sender, e); // Bestehende Methode aufrufen
            btnPrintAnalyse.Enabled = false;
            ToggleEingabeModus();
            AdminForm adminForm = new AdminForm(this); // Hauptform übergeben
            adminForm.Show(); // AdminForm anzeigen
            this.Hide(); // Form1 nur ausblenden
        }

        /*****************************+   false
       private void btnStrtOldGame_Click(object sender, EventArgs e)
       {

           System.Windows.Forms.Label[] players = { player1, player2, player3, player4, player5 };
           System.Windows.Forms.TextBox[] pointsFields = { pointsPl1, pointsPl2, pointsPl3, pointsPl4, pointsPl5 };
           System.Windows.Forms.Label[] sumFields = { sumPl1, sumPl2, sumPl3, sumPl4, sumPl5 };
           ++spielNummer;
           gameCtr.Text = spielNummer.ToString();
           CerrarApp.Enabled = false;
           //PartieZaehler();

           if (numPlayer <= 1)
           {
               MessageBox.Show("Por favor añade por lo menos dos jugadores antes de start.");
               return;
           }

           txtNewPl.Enabled = false;
           btnPrintAnalyse.Enabled = false;
           pointsPl1.Enabled = true;
           pointsPl2.Enabled = true;
           pointsPl3.Enabled = true;
           pointsPl4.Enabled = true;
           pointsPl5.Enabled = true;

           ResetPointsFieldsText(pointsFields);

           for (int j = players.Length - 1; j >= numPlayer; j--)
           {
               players[j].ForeColor = this.BackColor;
               pointsFields[j].Enabled = false;
               sumFields[j].ForeColor = this.BackColor;
           }
           strtJuego.Enabled = false;
           cckBaraje1.Checked = true;

       }

       *****************************/
    }


    // Spieler-Klasse
    public class Spieler
    {
        public string ID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime Geburtstag { get; set; }
        public string Spitzname { get; set; }
    }
   

    public class PunktVerwaltung
    {
        // StringBuilder als Klassenvariable definieren
        public StringBuilder gesamtPunkteString = new StringBuilder();

       
        // Methode, um den StringBuilder zu leeren
        public void StringBuilderLeeren()
        {
            gesamtPunkteString.Clear();
        }
        
        // Spielernamen anzeigen
        public void SpielerNamenAnzeigen(string[]spielerNamen)
        {
           
            gesamtPunkteString.Append("Spiel:".PadRight(10));
            foreach (string spielerName in spielerNamen)
            {
                gesamtPunkteString.Append(spielerName.PadRight(10));
            }
            gesamtPunkteString.AppendLine(); Console.WriteLine(gesamtPunkteString.ToString());
        }
        public void PunktListeErstellen(List<int[]> gesamtPunkte)
        {
              //GesamtPunkte anzeigen und Gewinner mit x ausweisen
            for (int i = 0; i < gesamtPunkte.Count; i++)
            {
                gesamtPunkteString.Append($"Spiel {i + 1}".PadRight(10));
                for (int j = 0; j < gesamtPunkte[i].Length; j++)
                {
                    string punktAnzeige = gesamtPunkte[i][j].ToString();
                    // Gewinner, wenn 0 oder "" in erster Zeile
                    if (i == 0 && (gesamtPunkte[i][j] == 0 || string.IsNullOrEmpty(punktAnzeige)))
                    {
                        punktAnzeige = "X";
                    }
                    // Spieler gewinnt, wenn Punkte gleich sind in zwei aufeinanderfolgenden Zeilen
                    if (i > 0 && gesamtPunkte[i][j] == gesamtPunkte[i - 1][j])
                    {
                        punktAnzeige = "X";
                    }
                    gesamtPunkteString.Append(punktAnzeige.PadRight(10));
                }
                gesamtPunkteString.AppendLine();
            }
        }
    }
    public class ErrorHandling
    {
       
        public static bool IsNicknameTaken(string nickname, List<Spieler> spielerListe)
        {
            return spielerListe.Any(player => player.Spitzname == nickname);
        }


        public static bool FehlerbehandlungPunkte(string[] pointsFields, int numPlayer)
        {
            bool hasWinner = false;
            int ZeroOrEmptyCounter = 0;

            for (int i = 0; i < numPlayer; i++)
            {
                Console.WriteLine($"Überprüfung von Wert: '{pointsFields[i]}'");

                // Überprüfen, ob das Feld leer oder "0" ist
                if (string.IsNullOrWhiteSpace(pointsFields[i]) || pointsFields[i] == "0")
                {
                    ZeroOrEmptyCounter++;
                    hasWinner = true;
                }
                if (ZeroOrEmptyCounter > 1)
                {
                    MessageBox.Show("Dos jugadores con 0 puntos no es posible");
                    return false; // Fehler gefunden
                }

                // Prüfen auf gültige Eingaben
                if (!string.IsNullOrWhiteSpace(pointsFields[i]) && pointsFields[i] != "0")
                {
                    if (!int.TryParse(pointsFields[i], out _))
                    {
                        MessageBox.Show($"Hay un valor '{pointsFields[i]}' que no es numero.");
                        return false; // Fehler gefunden
                    }
                }
            }

            // Zusätzliche Prüfung, ob kein Gewinner existiert
            if (!hasWinner)
            {
                MessageBox.Show("Falta ganador! Uno de los jugadores tiene que tener 0 puntos.");
                return false; // Fehler gefunden
            }
            //MessageBox.Show("Alle Prüfungen erfolgreich!");
            return true; // Alle Prüfungen erfolgreich
            
        }
        public static bool IsPlayerAlreadySelected(Label[] labels, string playerId)
        {
            // Überprüfen, ob die Spieler-ID bereits in einem Label-Tag vorhanden ist
            foreach (var label in labels)
            {
                if (label.Tag != null && label.Tag.ToString() == playerId)
                {
                    return true; // Spieler ist bereits ausgewählt
                }
            }
            return false; // Spieler ist noch nicht ausgewählt
        }

        public static void ShowPlayerAlreadySelectedError()
        {
            MessageBox.Show("Spieler ist schon ausgewählt!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



    }
}
