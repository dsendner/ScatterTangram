using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;

namespace ScatterTangram
{
    class ShapesManager
    {
        List<TangramShape> shapes;

        public ShapesManager()
        {
            this.shapes = new List<TangramShape>();
            this.shapesGeneration();
        }

        public List<TangramShape> getShapes()
        {
            return this.shapes;
        }

        /// <summary>
        /// create the list of all the tangramShape from a xml file.
        /// </summary>
        public void shapesGeneration()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"../../shapes.xml"); //open the file
            int i = 0;
            foreach (XmlNode n in doc.DocumentElement.ChildNodes) //for each TangramShape node
            {
                i++;
                ScatterViewItem motherPiece = this.stringToScatterViewItem(n.Attributes["MotherPiece"].Value);
                int level = Convert.ToInt32(n.Attributes["Level"].Value);
                List<ScatterViewItem> pieces = new List<ScatterViewItem>();
                List<ScatterViewItem> locations = new List<ScatterViewItem>();

                foreach (XmlNode e in n.ChildNodes) //for each piece node
                {
                    ScatterViewItem p = this.stringToScatterViewItem(e.Attributes["Path"].Value);
                    pieces.Add(p);

                    ScatterViewItem l = this.stringToScatterViewItem(e.Attributes["Path"].Value);
                    double x = Convert.ToDouble(e.Attributes["PositionX"].Value);
                    double y = Convert.ToDouble(e.Attributes["PositionY"].Value);
                    l.Center = new Point(x, y);
                    l.Orientation = Convert.ToDouble(e.Attributes["Orientation"].Value);
                    locations.Add(l);
                }

                this.shapes.Add(new TangramShape(i, motherPiece, locations, pieces, level));
            }
        }

        /// <summary>
        /// transform a string in ScatterViewItem
        /// </summary>
        /// <param name="line"></param>
        public ScatterViewItem stringToScatterViewItem(String line)
        {
            List<int> tab = new List<int>();
            String num = "";

            foreach (char c in line)
            {
                if (c != ' ')
                {
                    num += c;
                }
                else
                {
                    tab.Add(Convert.ToInt32(num));
                    num = "";
                }
            }
            tab.Add(Convert.ToInt32(num));
            return this.createScatterViewItem(tab);
        }

        /// <summary>
        /// transform a list of coordinate in a scatterviewitem
        /// </summary>
        /// <param name="coord"></param>
        public ScatterViewItem createScatterViewItem(List<int> coord)
        {
            ScatterViewItem myShape = new ScatterViewItem();
            //create the pathGeometry
            if (coord != null)
            {
                PathFigure myPathFigure = new PathFigure();
                myPathFigure.StartPoint = new Point(coord[0], coord[1]);

                PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                for (int i = 2; i < coord.Count(); i = i + 2)
                {
                    LineSegment myLineSegment = new LineSegment();
                    myLineSegment.Point = new Point(coord[i], coord[i + 1]);
                    myPathSegmentCollection.Add(myLineSegment);
                }

                myPathFigure.Segments = myPathSegmentCollection;
                PathFigureCollection myPathFigureCollection = new PathFigureCollection();
                myPathFigureCollection.Add(myPathFigure);
                PathGeometry myPathGeometry = new PathGeometry();
                myPathGeometry.Figures = myPathFigureCollection;

                //create the path
                System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
                myPath.Stroke = Brushes.Black;
                myPath.StrokeThickness = 1;
                myPath.Data = myPathGeometry;
                myPath.Fill = new SolidColorBrush(Colors.Blue);

                //create the scatterViewItem
                myShape.Content = myPath;
                myShape.Width = this.max(coord);
                myShape.Height = this.max(coord);
            }
 
            return myShape;
        }

        /// <summary>
        /// return the max number in the list
        /// </summary>
        /// <param name="tab"></param>
        public int max(List<int> tab)
        {
            int max = 0;
            for (int i = 0; i < tab.Count(); i++)
            {
                if (max < tab[i])
                {
                    max = tab[i];
                }
            }

            return max;
        }

        /// <summary>
        /// divide the size of the scatterviewitem by two
        /// </summary>
        /// <param name="myShape"></param>
        public ScatterViewItem minimizeShape(ScatterViewItem myShape)
        {
            //extract the path
            System.Windows.Shapes.Path myPath = (System.Windows.Shapes.Path)myShape.Content;
            //extract the PathGeometry
            PathGeometry myPathGeometry = myPath.Data.GetFlattenedPathGeometry();
            //extract the PathFigure
            PathFigure myPathFigure = myPathGeometry.Figures[0];

            //create a new PathFigure
            PathFigure myNewPathFigure = new PathFigure();
            Point sp = myPathFigure.StartPoint;
            myNewPathFigure.StartPoint = new Point(sp.X / 2, sp.Y / 2);

            PathSegmentCollection myNewPathSegmentCollection = new PathSegmentCollection();
            foreach (PolyLineSegment l in myPathFigure.Segments)
            {
                PolyLineSegment myPolyLineSegment = new PolyLineSegment();
                foreach (Point p in l.Points)
                {
                    Point np = new Point(p.X/2, p.Y/2);
                    myPolyLineSegment.Points.Add(np);
                }
                myNewPathSegmentCollection.Add(myPolyLineSegment);
            }

            myNewPathFigure.Segments = myNewPathSegmentCollection;

            PathFigureCollection myNewPathFigureCollection = new PathFigureCollection();
            myNewPathFigureCollection.Add(myNewPathFigure);
            PathGeometry myNewPathGeometry = new PathGeometry();
            myNewPathGeometry.Figures = myNewPathFigureCollection;

            //create the new path
            System.Windows.Shapes.Path myNewPath = new System.Windows.Shapes.Path();
            myNewPath.Stroke = Brushes.Black;
            myNewPath.StrokeThickness = 1;
            myNewPath.Data = myNewPathGeometry;
            myNewPath.Fill = myPath.Fill;

            //create the new scatterViewItem
            ScatterViewItem myNewShape = new ScatterViewItem();
            myNewShape.Content = myNewPath;
            myNewShape.Width = myShape.Width/2;
            myNewShape.Height = myShape.Height/2;

            return myNewShape;
        }
    }
}
