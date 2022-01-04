using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotaNotes.DTO.Context;
using DotaNotes.DTO.Models;

namespace DotaNotes.WinForm
{
    public partial class NoteDialogForm : Form
    {
        private DotaPlayer _dotaPlayer;

        public NoteDialogForm()
        {
            InitializeComponent();
        }

        public NoteDialogForm(DotaPlayer dotaPlayer)
        {
            InitializeComponent();
            
            _dotaPlayer = dotaPlayer;
            var textNote = _dotaPlayer.Note;

            if (!string.IsNullOrEmpty(textNote))
            {
                textBox1.Text = textNote;
            }
        }

        private  void button1_Click(object sender, EventArgs e)
        {
            var textNote = textBox1.Text;
                
            if (textNote.Length < 1024)
            {
                using (var _dc = new DatabaseContext())
                {
                    var createdUser = _dc.DotaPlayers.FirstOrDefault(u => u.SteamId64 == _dotaPlayer.SteamId64);
                    if (createdUser != null)
                    {
                        createdUser.Note = textNote;
                        _dc.SaveChanges();
                        this.Close();
                    }
                    else
                    {
                        _dotaPlayer.Note = textNote;
                        _dc.DotaPlayers.Add(_dotaPlayer);
                        _dc.SaveChanges();
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Text field cannot be less than 1024 symbols.", "Dota notes");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}