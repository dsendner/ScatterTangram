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
    class Score
    {
        private String name;
        private TimeSpan time;
        private int shapeNumber;
        private XmlDocument doc;

        public Score(String name, TimeSpan time, int shapeNumber)
        {
            this.time = time;
            this.shapeNumber = shapeNumber;
            this.name = name;

            this.doc = new XmlDocument();
            if (File.Exists(@"../../score.xml"))
            {
                doc.Load(@"../../score.xml"); //open the file
                XmlNode n = doc.DocumentElement.ChildNodes[0].Clone(); //create the score
                n.Attributes["name"].Value = this.name;
                n.Attributes["shape"].Value = this.shapeNumber.ToString();
                n.Attributes["minutes"].Value = time.Minutes.ToString();
                n.Attributes["seconds"].Value = time.Seconds.ToString();

                this.place(n); //place the score

                doc.Save(@"../../score.xml"); //save the file
            }
        }

        /// <summary>
        ///  find the place of the score in the document
        /// </summary>
        /// <param name="n"></param>
        public void place(XmlNode n)
        {
            int i = 0;
            int j = 0;
            //On parcours tous les scores
             while (i < doc.DocumentElement.ChildNodes.Count && j < 10)
            {
                XmlNode m = doc.DocumentElement.ChildNodes[i];

                if (m.Attributes["shape"].Value.Equals(n.Attributes["shape"].Value))
                {
                    j++;
                    if (!this.isGreater(n, m))
                    {
                        doc.DocumentElement.InsertBefore(n, m);
                        break;
                    }
                }
                i++;
            }

            if (i == doc.DocumentElement.ChildNodes.Count && this.scoresNumberForOneShape(this.shapeNumber) < 10)
            {
                doc.DocumentElement.AppendChild(n);
            }

            if (this.scoresNumberForOneShape(this.shapeNumber) > 10)
            {
                int k = doc.DocumentElement.ChildNodes.Count - 1;
                while (doc.DocumentElement.ChildNodes[k].Attributes["shape"].Value != this.shapeNumber.ToString())
                {
                    k--;
                }
                doc.DocumentElement.RemoveChild(doc.DocumentElement.ChildNodes[k]);
            }
        }

        /// <summary>
        /// return true if the time of a is greater than the time of b, else false 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public bool isGreater(XmlNode a, XmlNode b)
        {
            int min1 = Convert.ToInt32(a.Attributes["minutes"].Value);
            int min2 = Convert.ToInt32(b.Attributes["minutes"].Value);
            int sec1 = Convert.ToInt32(a.Attributes["seconds"].Value);
            int sec2 = Convert.ToInt32(b.Attributes["seconds"].Value);

            if (min1 > min2)
            {
                return true; 
            }
            else if (min1 < min2)
            {
                return false;
            }
            else
            {
                if (sec1 > sec2)
                {
                    return true;
                }
                else if (sec1 < sec2)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///  return the number of scores for one shape with the number i
        /// </summary>
        /// <param name="i"></param>
        public int scoresNumberForOneShape(int i)
        {
            int num = 0;

            foreach (XmlNode n in doc.DocumentElement.ChildNodes)
            {
                if (Convert.ToInt32(n.Attributes["shape"].Value) == i)
                {
                    num++;
                }
            }

            return num;
        }

        /// <summary>
        ///  return a table of string with all the score for the shape with the number shapeNumber
        /// </summary>
        /// <param name="shapeNumber"></param>
        public static String[] scoreTable(int shapeNumber)
        {
            String[] result = new String[10];
            int i = 0;
            XmlDocument doc = new XmlDocument();

            if (File.Exists(@"../../score.xml"))
            {
                doc.Load(@"../../score.xml");
                //On parcours la liste des scores 
                foreach (XmlNode n in doc.DocumentElement.ChildNodes)
                {
                    if (n.Attributes["shape"].Value == shapeNumber.ToString() && i < 10)
                    {
                        result[i] = (i+1) + " Name: " + n.Attributes["name"].Value + " Time: " + n.Attributes["minutes"].Value + " minutes " + n.Attributes["seconds"].Value + " seconds";
                        i++;
                    }
                }
            }
            return result;
        }
    }
}
