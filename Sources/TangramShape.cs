using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using SSC = Microsoft.Surface.Presentation.Controls;

namespace ScatterTangram
{
    public class TangramShape
    {
        private ScatterViewItem motherPiece;
        private List<ScatterViewItem> locations;
        private List<ScatterViewItem> pieces;
        private int level;
        public static int lockMargin = 30; //the error allow when you place the piece
        private int id;

        public TangramShape(int id, ScatterViewItem motherPiece, List<ScatterViewItem> locations, List<ScatterViewItem> pieces, int level)
        {
            this.motherPiece = motherPiece;
            this.locations = locations;
            this.pieces = pieces;
            this.level = level;
            this.id = id;

            if (this.level == 1)
            {
                Path p = (Path)this.motherPiece.Content;
                p.Fill = new SolidColorBrush(Colors.Green);
                foreach (ScatterViewItem s in this.pieces)
                {
                    Path pp = (Path)s.Content;
                    pp.Fill = new SolidColorBrush(Colors.Green);
                }
            }
            else if (this.level == 2)
            {
                Path p = (Path)this.motherPiece.Content;
                p.Fill = new SolidColorBrush(Colors.Blue);
                foreach (ScatterViewItem s in this.pieces)
                {
                    Path pp = (Path)s.Content;
                    pp.Fill = new SolidColorBrush(Colors.Blue);
                }
            }
            else if (this.level == 3)
            {
                Path p = (Path)this.motherPiece.Content;
                p.Fill = new SolidColorBrush(Colors.Red);
                foreach (ScatterViewItem s in this.pieces)
                {
                    Path pp = (Path)s.Content;
                    pp.Fill = new SolidColorBrush(Colors.Red);
                }
            }
        }

        public ScatterViewItem getMotherPiece()
        {
            return this.motherPiece;
        }

        public List<ScatterViewItem> getLocations()
        {
            return this.locations;
        }

        public List<ScatterViewItem> getPieces()
        {
            return this.pieces;
        }

        /// <summary>
        /// return true if the center of piece number "i" is at the same place than the center of this location. false otherwise.
        /// </summary>
        /// <param name="i"></param>
        public bool goodCenterPosition(int i)
        {
            bool infX, supX, infY, supY;
            if (this.pieces != null && this.pieces[i] != null)
            {
                infX = this.pieces[i].Center.X <= this.locations[i].Center.X + lockMargin;
                supX = this.pieces[i].Center.X >= this.locations[i].Center.X - lockMargin;
                infY = this.pieces[i].Center.Y <= this.locations[i].Center.Y + lockMargin;
                supY = this.pieces[i].Center.Y >= this.locations[i].Center.Y - lockMargin;
            }
            else
            {
                infX = supX = infY = supY = false;
            }

            return infX && supX && infY && supY;
        }

        /// <summary>
        /// return true if the piece number "i" has the same orientation than the location number "i". false otherwise.
        /// </summary>
        /// <param name="i"></param>
        public bool goodOrientation(int i)
        {
            bool inf, sup;
            if (this.pieces != null && this.pieces[i] != null)
            {
                inf = this.pieces[i].Orientation <= this.locations[i].Orientation + lockMargin;
                sup = this.pieces[i].Orientation >= this.locations[i].Orientation - lockMargin;
            }
            else
            {
                inf = sup = false;
            }
            return inf && sup;
        }

        /// <summary>
        /// return true if all the pieces of the shape are on the good place. false otherwise.
        /// </summary>
        public bool allPiecesOk()
        {
            int i = 0;
            if (this.pieces != null)
            {
                while (i < this.pieces.Count && this.okForPieceNumber(i))
                {
                    i++;
                }
            }
            return i == this.pieces.Count;
        }

        /// <summary>
        /// return true if the piece number "i" is at the good place that if this piece has the same orientation and the same center than this location. false otherwise. 
        /// </summary>
        /// <param name="i"></param>
        public bool okForPieceNumber(int i)
        {
            return this.goodCenterPosition(i) &&
            this.goodOrientation(i);
        }

        /// <summary>
        /// put the piece number "i" on the good location and lock it on this.
        /// </summary>
        /// <param name="i"></param>
        public void setGoodPlaceFor(int i)
        {
            if (this.pieces != null && this.pieces[i] != null)
            {
                this.pieces[i].Center = this.locations[i].Center;
                this.pieces[i].Orientation = this.locations[i].Orientation;
                this.pieces[i].CanMove = false;
                this.pieces[i].CanRotate = false;
                this.pieces[i].CanScale = false;
            }
        }

        public int getLevel
        {
            get
            {
                return this.level;
            }
        }

        public int getIndex
        {
            get
            {
                return this.id;
            }
        }

    }
}
