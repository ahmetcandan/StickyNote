using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note
{
    public static class DataAccess
    {
        public static Data Data;
    }

    public class Data
    {
        public List<Note> Notes { get; set; }
        public List<int> OpenNoteIds { get; set; }

        public Data()
        {
            Notes = new List<Note>();
            OpenNoteIds = new List<int>();
            fileExistCreate();
            read();
        }

        public void Save()
        {
            write();
        }

        private void write()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Data/Notes.json"))
                {
                    if (Notes == null || Notes.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject(from a in Notes select new { a.Description, a.NoteId, a.Opacity, a.Size, a.Location }));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kayıt yazma sırasında bir hata oluştu.", ex);
            }
        }

        private void read()
        {
            try
            {
                using (StreamReader sw = new StreamReader("Data/Notes.json"))
                {
                    Notes = JsonConvert.DeserializeObject<List<Note>>(sw.ReadToEnd());
                    if (Notes == null)
                        Notes = new List<Note>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kayıt okuma sırasında bir hata oluştu.", ex);
            }
        }

        private void fileExistCreate()
        {
            bool save = false;
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
                save = true;
            }

            if (!File.Exists("Data/Notes.json"))
            {
                using (var file = File.Create("Data/Accounts.json"))
                {
                    save = true;
                    Notes = new List<Note>();
                    file.Close();
                }
            }

            if (save) write();
        }
    }
}
