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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;

namespace ScatterTangram.Pages
{
    /// <summary>
    /// Interaction logic for MainScatterView.xaml
    /// </summary>
    public partial class MainScatterView : SurfaceUserControl
    {
        private ShapesManager mng; //create and manage TangramShapes
        private List<TangramShape> shapes; //all the shapes
        private TangramShape selectedShape; //the shape selected by the user
        public event ShapeSelectedEventHandler OnShapeSelected;

        /// <summary>
        /// delegate for the "onShapeSelected" event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ShapeSelectedEventHandler(object sender, ShapeSelectedEventArgs e);

        /// <summary>
        /// Create shapes and load them in the scatterview.
        /// </summary>
        /// <param name="level">the level selected by the user</param>
        public MainScatterView(int level)
        {
            InitializeComponent();
            this.mng = new ShapesManager();
            this.shapes = this.mng.getShapes();

            //Shapes generation on the ScatterView
            for (int i = 0; i < this.shapes.Count; i++)
            {
                ScatterViewItem m = mng.minimizeShape(this.shapes[i].getMotherPiece());
                this.scatter.Items.Add(m);
                m.Visibility = (Visibility)1;
                if (this.shapes[i].getLevel == level || level == 0)
                {
                    m.Visibility = (Visibility)0;
                }
            }
        }

        /// <summary>
        /// Occurs when the user tap on a shape.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scatter_ContactTapGesture(object sender, ContactEventArgs e)
        {
            for (int i = 0; i < this.scatter.Items.Count; i++)
            {
                ScatterViewItem svi = (ScatterViewItem)this.scatter.Items[i];
                if (svi.IsActive) //if a shape is selected
                {
                    this.selectedShape = this.shapes[i];
                    // trigger an event with this selected shape in parameter
                    ShapeSelectedEventArgs eventShapeSelection = new ShapeSelectedEventArgs(this.selectedShape);
                    if (eventShapeSelection != null) this.OnShapeSelected(this, eventShapeSelection);
                }
            }
        }

        /// <summary>
        /// Occurs when the user double click on a shape.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scatter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < this.scatter.Items.Count; i++)
            {
                ScatterViewItem svi = (ScatterViewItem)this.scatter.Items[i];
                if (svi.IsActive) //if a shape is selected
                {
                    this.selectedShape = this.shapes[i];
                    // trigger an event with this selected shape in parameter
                    ShapeSelectedEventArgs eventShapeSelection = new ShapeSelectedEventArgs(this.selectedShape);
                    if (eventShapeSelection != null) this.OnShapeSelected(this, eventShapeSelection);
                }
            }
        }
    }
}
