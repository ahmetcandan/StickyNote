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
    public partial class frmEdit : Form
    {
        Note Note;

        public frmEdit(int noteId = 0)
        {
            InitializeComponent();
            if (noteId == 0)
                Note = new Note();
            else
                Note = DataAccess.Data.Notes.FirstOrDefault(c => c.NoteId == noteId);
        }

        private void frmEdit_Load(object sender, EventArgs e)
        {
            txtDescription.Text = Note.Description;
            txtHeight.Text = Note.Size.Y.ToString();
            txtOpacity.Text = (Note.Opacity * 100).ToString();
            txtWidth.Text = Note.Size.X.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Note.Description = txtDescription.Text;
                Note.Size.Y = int.Parse(txtHeight.Text);
                Note.Size.X = int.Parse(txtWidth.Text);
                Note.Opacity = int.Parse(txtOpacity.Text) / 100.0;

                if (Note.NoteId == 0)
                {
                    Note.NoteId = DataAccess.Data.Notes.Max(c => c.NoteId) + 1;
                    DataAccess.Data.Notes.Add(Note);
                }
                DataAccess.Data.Save();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
