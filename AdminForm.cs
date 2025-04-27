using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Romme_V2
{
    public partial class AdminForm : Form
    {
        private NbrJug mainForm;
        private List<Spieler> spielerListe;
        private List<Spiel> spieleListe;
        bool programmaticSelection = false;
        public AdminForm(NbrJug form1)
        {
            InitializeComponent();
            mainForm = form1;
            spielerListe = mainForm.GetSpielerListe();
           spieleListe = LadeSpiele(NbrJug.dateiPfad);
           this.Load += AdminForm_Load; // OnLoad verknüpft
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // Form1 wieder sichtbar machen
            this.Hide(); // AdminForm ausblenden
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
        
       
        private void AdminForm_Load(object sender, EventArgs e)
        {
            string spieleDateiPfad = NbrJug.dateiPfad; // Pfad aus NbrJug holen
            spieleListe = LadeSpiele(spieleDateiPfad); // Spiele laden
        }
        private void SpielerVorZuName()
        {
            // SpielerListe in ComboBox füllen
            cbBPlayerList.Items.Clear();

            foreach (var spieler in spielerListe)
            {
                // Hinzufügen von "Nachname, Vorname" zur ComboBox
                cbBPlayerList.Items.Add($"{spieler.Nachname}, {spieler.Vorname}");
            }

            if (cbBPlayerList.Items.Count > 0)
            {
                programmaticSelection = true; // Flag setzen, um zu verhindern, dass die Auswahl erneut ausgelöst wird
                cbBPlayerList.SelectedIndex = 0; // Optionale Vorauswahl
                programmaticSelection = false; // Flag zurücksetzen
            }
        }

        private void cbBPlayerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!programmaticSelection && cbBPlayerList.SelectedIndex != -1)
            {
                // Den ausgewählten Spieler aus der Liste holen
                Spieler ausgewaehlterSpieler = spielerListe[cbBPlayerList.SelectedIndex];
                MessageBox.Show($"Jugador/a: {ausgewaehlterSpieler.Vorname} {ausgewaehlterSpieler.Nachname}\nApodo: {ausgewaehlterSpieler.Spitzname}, ID: {ausgewaehlterSpieler.ID}\nGeburtstag: {ausgewaehlterSpieler.Geburtstag.ToShortDateString()}", "Informacion Jugadora", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Beispiel: Fehler in Vorname oder Geburtstag korrigieren
                string neuerVorname = PromptUserForInput("Neuer Vorname:", ausgewaehlterSpieler.Vorname);
                string neuerNachname = PromptUserForInput("Neuer Nachname:", ausgewaehlterSpieler.Nachname);
                string neuerGeburtstag = PromptUserForInput("Neues Geburtsdatum (yyyy-MM-dd):", ausgewaehlterSpieler.Geburtstag.ToString("yyyy-MM-dd"));
                string neuerSpitzname = PromptUserForInput("Neuer Spitzname:", ausgewaehlterSpieler.Spitzname);
                
                ausgewaehlterSpieler.Vorname = neuerVorname;
                ausgewaehlterSpieler.Nachname = neuerNachname;
                 if  (DateTime.TryParse(neuerGeburtstag, out DateTime geburtstag))
                {
                    ausgewaehlterSpieler.Geburtstag = geburtstag;
                    
                }
                else
                {
                    MessageBox.Show("Ungültiges Geburtsdatum eingegeben!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (neuerSpitzname == ausgewaehlterSpieler.Spitzname)
                {
                    // Kein Wechsel nötig, einfach weiter machen.
                    return;
                }
                if (!ErrorHandling.IsNicknameTaken(neuerSpitzname, spielerListe))
                {
                    ausgewaehlterSpieler.Spitzname = neuerSpitzname;
                }
                else
                {
                    MessageBox.Show("Der Spitzname ist bereits vergeben! Bitte einen anderen Spitznamen wählen.", "Warnung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
        public static string PromptUserForInput(string title, string defaultText)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 10, Top = 10, Text = title, AutoSize = true };
            TextBox inputBox = new TextBox() { Left = 10, Top = 30, Width = 260, Text = defaultText };
            Button confirmation = new Button() { Text = "OK", Left = 200, Top = 70, Width = 70 };

            confirmation.Click += (sender, e) => { prompt.DialogResult = DialogResult.OK; prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : defaultText;
        }



        /********************
        private string PromptUserForInput(string message, string defaultValue)
        {
            // Zeigt eine Eingabeaufforderung an, um Werte zu bearbeiten
            using (var dialog = new Form())
            {
                dialog.Text = message;
                var textBox = new TextBox { Text = defaultValue, Dock = DockStyle.Fill };
                dialog.Controls.Add(textBox);
                dialog.ShowDialog();
                return textBox.Text;
            }
        }
       *********************/


        private void btnUpdatePlayer_Click(object sender, EventArgs e)
        {
            SpielerVorZuName();
        }
    }
}
