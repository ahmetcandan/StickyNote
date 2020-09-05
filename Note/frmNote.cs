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
    public partial class frmNote : Form
    {
        Note Note;

        public frmNote(int noteId)
        {
            InitializeComponent();
            Note = DataAccess.Data.Notes.FirstOrDefault(c => c.NoteId == noteId);
            DataAccess.Data.OpenNoteIds.Add(noteId);
            load();
        }

        private void lblNote_DoubleClick(object sender, EventArgs e)
        {
            TopMost = false;
            frmEdit frm = new frmEdit(Note.NoteId, Note.Location, Note.Size);
            frm.ShowDialog();
            load();
            TopMost = Note.TopMost;
        }

        bool mouseClick = false;
        System.Drawing.Point lastLocation;

        private void lblNote_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClick = true;
            lastLocation = e.Location;
        }

        private void lblNote_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClick = false;
            Note.Location = new Point(Location.X, Location.Y);
            DataAccess.Data.Save();
        }

        private void lblNote_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseClick)
                Location = new System.Drawing.Point((Location.X - lastLocation.X) + e.X, (Location.Y - lastLocation.Y) + e.Y);
        }

        private void load()
        {
            lblNote.Text = Note.Description;
            Opacity = Note.Opacity;
            Width = Note.Size.X;
            Height = Note.Size.Y;
        }

        private void yeniNoteToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void kapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataAccess.Data.Notes.Count == 1)
                Application.Exit();
            else
            {
                DataAccess.Data.Notes.Remove(Note);
                DataAccess.Data.OpenNoteIds.Remove(Note.NoteId);
                DataAccess.Data.Save();
                Close();
            }
        }

        private void cikisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmNote_Load(object sender, EventArgs e)
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            var screenSize = Screen.PrimaryScreen.Bounds.Size;
            bool isMultiScreen = Screen.AllScreens.Length > 1;
            Location = new System.Drawing.Point((isMultiScreen || (Note.Location.X >= 0 && Note.Location.X <= screenSize.Width)) ? Note.Location.X : screenSize.Width - Note.Size.X, (isMultiScreen || (Note.Location.Y >= 0 && Note.Location.Y <= screenSize.Height)) ? Note.Location.Y : screenSize.Height - Note.Size.Y);

            Size = new Size(Note.Size.X, Note.Size.Y);
            TopMost = Note.TopMost;
            topMostToolStripMenuItem.Checked = TopMost;
        }

        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, ((this.ClientSize.Height - cGrip) < cGrip ? cGrip + 10 : this.ClientSize.Height - cGrip), cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            { 
                System.Drawing.Point pos = new System.Drawing.Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void frmNote_ResizeEnd(object sender, EventArgs e)
        {
            Note.Size = new Point(Size.Width, Size.Height);
            DataAccess.Data.Save();
        }

        private void frmNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            Forms.NoteForms.Remove(this);
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Note.TopMost = !Note.TopMost;
            TopMost = Note.TopMost;
            topMostToolStripMenuItem.Checked = Note.TopMost;
            DataAccess.Data.Save();
        }
    }
}
