using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Romme_V2
{
    public partial class NewPlayerForm : Form
    {
        public NewPlayerForm()
        {
            InitializeComponent();
        }
       public Spieler Spieler { get; private set; } //Verbindung zur Spieler Klasse
        private void btnOK_Click(object sender, EventArgs e)
        {
            Spieler = new Spieler
            {
                Vorname = txtForename.Text,
                Nachname = txtLastName.Text,
                Geburtstag = dtpBirthday.Value,
                Spitzname = txtNickName.Text,
            };

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        
    }
}
