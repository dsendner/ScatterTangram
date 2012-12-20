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

namespace ScatterTangram.Pages
{
    /// <summary>
    /// Interaction logic for SecondScatterView.xaml
    /// </summary>
    public partial class SecondScatterView : SurfaceUserControl
    {
        private TangramShape currentShape; //the shape selected by the user
        private bool lockShapes; // if the piece of the shape should to be lock
        private bool win = false; // true if the user had win.
        private SurfaceTextBox name; // for allow the ser to enter his name
        private DateTime t1; // the begin time and the final time
        private TimeSpan duration; // the duration of the party

        /// <summary>
        /// Load the shape and its pieces
        /// </summary>
        /// <param name="lockShapes"></param>
        /// <param name="shape"></param>
        public SecondScatterView(bool lockShapes, TangramShape shape)
        {
            InitializeComponent();
            this.lockShapes = lockShapes;
            this.currentShape = shape;

            //Load the motherpiece for the location
            this.currentShape.getMotherPiece().Center = new Point(512, 384);
            this.currentShape.getMotherPiece().Orientation = 0;
            this.currentShape.getMotherPiece().IsEnabled = false;
            System.Windows.Shapes.Path p = (System.Windows.Shapes.Path)this.currentShape.getMotherPiece().Content;
            p.Fill = new SolidColorBrush(Colors.Black);
            p.Opacity = 0.5;
            this.currentShape.getMotherPiece().CanMove = false;
            this.currentShape.getMotherPiece().CanRotate = false;
            this.currentShape.getMotherPiece().CanScale = false;
            this.scatter2.Items.Add(this.currentShape.getMotherPiece());

            //Load all pieces for the shape
            for (int j = 0; j < this.currentShape.getPieces().Count; j++)
            {
                this.scatter2.Items.Add(this.currentShape.getPieces()[j]);
            }

            //Create the surfaceTextBlock
            this.name = (SurfaceTextBox)Resources["name"];

            //Check the time
            this.t1 = DateTime.Now;
        }

        /// <summary>
        /// Check if the piece is at the good position, if the user win and calculate the score.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scatter2_ContactChanged(object sender, ContactEventArgs e)
        {
            if (!win) //if the player don't end the game
            {
                for (int i = 1; i < (this.scatter2.Items.Count); i++) // for all piece
                {
                    ScatterViewItem v = (ScatterViewItem)this.scatter2.Items[i];
                    if (v.IsActive) //if this piece is selected
                    {
                        //check the place of the piece, if it's ok lock it
                        if (this.lockShapes && this.currentShape.okForPieceNumber(i - 1))
                        {
                            this.currentShape.setGoodPlaceFor(i - 1);
                        }
                    }
                }
                if (this.currentShape.allPiecesOk()) //if all the piece are on their places
                {
                    // lock all pieces
                    for (int i = 1; i < (this.scatter2.Items.Count); i++)
                    {
                        this.currentShape.setGoodPlaceFor(i - 1);
                    }

                    //Create the winner logo
                    Label winner = (Label)Resources["winner"];
                    ScatterViewItem svWinner = new ScatterViewItem();
                    svWinner.Content = winner;
                    svWinner.Center = new Point(512, 134);
                    svWinner.Orientation = 0;
                    svWinner.CanMove = false;
                    svWinner.CanRotate = false;
                    svWinner.CanScale = false;
                    //Add the winner logo
                    scatter2.Items.Add(svWinner);

                    //Check the time
                    DateTime t2 = DateTime.Now;

                    //calculates the duration
                    this.duration = t2.Subtract(t1);

                    //Create the time logo
                    TextBlock time = (TextBlock)Resources["time"];
                    time.Text = "Score: " + duration.Minutes.ToString() + " min and " + duration.Seconds.ToString() + "s";
                    ScatterViewItem svTime = new ScatterViewItem();
                    svTime.Width = 400;
                    svTime.Content = time;
                    svTime.Center = new Point(512, 184);
                    svTime.Orientation = 0;
                    svTime.CanMove = false;
                    svTime.CanRotate = false;
                    svTime.CanScale = false;
                    scatter2.Items.Add(svTime);



                    // add the textbox
                    grid.Children.Add(this.name);
                    this.win = true;
                }
            }
        }

        /// <summary>
        /// Check if the piece is at the good position, if the user win and calculate the score.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scatter2_MouseMove(object sender, MouseEventArgs e)
        {
            if (!win) //if the player don't end the game
            {
                for (int i = 1; i < (this.scatter2.Items.Count); i++) // for all piece
                {
                    ScatterViewItem v = (ScatterViewItem)this.scatter2.Items[i];
                    if (v.IsActive) //if this piece is selected
                    {
                        //check the place of the piece, if it's ok lock it
                        if (this.lockShapes && this.currentShape.okForPieceNumber(i - 1))
                        {
                            this.currentShape.setGoodPlaceFor(i - 1);
                        }
                    }
                }
                if (this.currentShape.allPiecesOk()) //if all the piece are on their places
                {
                    // lock all pieces
                    for (int i = 1; i < (this.scatter2.Items.Count); i++)
                    {
                        this.currentShape.setGoodPlaceFor(i - 1);
                    }

                    //Create the winner logo
                    Label winner = (Label)Resources["winner"];
                    ScatterViewItem svWinner = new ScatterViewItem();
                    svWinner.Content = winner;
                    svWinner.Center = new Point(512, 134);
                    svWinner.Orientation = 0;
                    svWinner.CanMove = false;
                    svWinner.CanRotate = false;
                    svWinner.CanScale = false;
                    //Add the winner logo
                    scatter2.Items.Add(svWinner);

                    //Check the time
                    DateTime t2 = DateTime.Now;

                    //calculates the duration
                    this.duration = t2.Subtract(t1);

                    //Create the time logo
                    TextBlock time = (TextBlock)Resources["time"];
                    time.Text = "Score: " + duration.Minutes.ToString() + " min and " + duration.Seconds.ToString() + "s";
                    ScatterViewItem svTime = new ScatterViewItem();
                    svTime.Width = 400;
                    svTime.Content = time;
                    svTime.Center = new Point(512, 184);
                    svTime.Orientation = 0;
                    svTime.CanMove = false;
                    svTime.CanRotate = false;
                    svTime.CanScale = false;
                    scatter2.Items.Add(svTime);



                    // add the textbox
                    grid.Children.Add(this.name);
                    this.win = true;
                }
            }
        }

        /// <summary>
        /// Occurs when the player enter his name and type on the enter key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SurfaceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Key enter = Key.Return;
            if (e.Key.Equals(enter))
            {
                // Create the score
                Score s = new Score(this.name.Text, this.duration, this.currentShape.getIndex);

                //undisplay the SurfaceTextBox for enter the name 
                this.name.Visibility = (Visibility)1;

                //display all the scores for the shape
                SurfaceListBox score = (SurfaceListBox)Resources["top"];
                String[] scores = Score.scoreTable(this.currentShape.getIndex);
                foreach (String c in scores)
                {
                    SurfaceListBoxItem item = new SurfaceListBoxItem();
                    item.Content = c;
                    score.Items.Add(item);
                }
                this.grid.Children.Add(score);
            }
        }
    }
}
