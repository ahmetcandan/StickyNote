using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note
{
    public class Note
    {
        public Note()
        {
            Opacity = 0.5;
            Size = new Point(200, 100);
            Location = new Point(200, 200);
            TopMost = false;
        }

        public int NoteId { get; set; }
        public string Description { get; set; }
        public double Opacity { get; set; }
        public Point Size { get; set; }
        public Point Location { get; set; }
        public bool TopMost { get; set; }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public Point()
        {

        }
    }
}
