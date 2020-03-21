using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Note
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Hide();

            if (DataAccess.Data.Notes.Count == 0)
            {
                DataAccess.Data.Notes.Add(new Note()
                {
                    Description = "Bir not yazınız...",
                    NoteId = 1,
                    Opacity = 0.5,
                    Size = new Point(200, 100)
                });
                frmNote frm = new frmNote(1);
                frm.Show();
            }
            else
                foreach (var note in DataAccess.Data.Notes)
                {
                    frmNote frm = new frmNote(note.NoteId);
                    frm.Show();
                    Forms.NoteForms.Add(frm);
                }
            Hide();
        }

        private void showAllNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var frm in Forms.NoteForms)
            {
                frm.TopMost = true;
                frm.TopMost = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEdit frm = new frmEdit();
            frm.ShowDialog();

            var result = from note in DataAccess.Data.Notes
                         join openNote in DataAccess.Data.OpenNoteIds on note.NoteId equals openNote into iOpenNote
                         from dOpenNote in iOpenNote.DefaultIfEmpty()
                         where dOpenNote == 0
                         select new { Note = note, openNoteId = dOpenNote };

            foreach (var item in result)
            {
                frmNote frmNote = new frmNote(item.Note.NoteId);
                DataAccess.Data.OpenNoteIds.Add(item.Note.NoteId);
                frmNote.Show();
                Forms.NoteForms.Add(frmNote);
            }
        }
    }
}
