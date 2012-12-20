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
using ScatterTangram.Pages;

namespace ScatterTangram
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        private UserControl currentPage; //the page load in the interface
        private int level; //the level selected for the shape display
        private bool lockShape; //if the shapes should to be locked or not

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();
            // Add handlers for Application activation events
            AddActivationHandlers();

            // load the menu
            this.currentPage = new ScatterTangram.Pages.Menu();
            grid.Children.Clear();
            grid.Children.Add(currentPage);

            // Add the button who allows load mainScatterView 
            SurfaceButton goButton = (SurfaceButton)Resources["goButton"];
            grid.Children.Add(goButton);
        }


        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for Application activation events
            RemoveActivationHandlers();
        }

        /// <summary>
        /// Adds handlers for Application activation events.
        /// </summary>
        private void AddActivationHandlers()
        {
            // Subscribe to surface application activation events
            ApplicationLauncher.ApplicationActivated += OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed += OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated += OnApplicationDeactivated;
        }

        /// <summary>
        /// Removes handlers for Application activation events.
        /// </summary>
        private void RemoveActivationHandlers()
        {
            // Unsubscribe from surface application activation events
            ApplicationLauncher.ApplicationActivated -= OnApplicationActivated;
            ApplicationLauncher.ApplicationPreviewed -= OnApplicationPreviewed;
            ApplicationLauncher.ApplicationDeactivated -= OnApplicationDeactivated;
        }

        /// <summary>
        /// This is called when application has been activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationActivated(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when application is in preview mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationPreviewed(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        ///  This is called when application has been deactivated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationDeactivated(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        /// <summary>
        ///  This is called when user click on the go button, this load the mainScatterView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            // bring back the options selected by the user
            ScatterTangram.Pages.Menu menuPage = (ScatterTangram.Pages.Menu)this.currentPage;
            this.lockShape = (bool)menuPage.surfaceCheckBox1.IsChecked;
            this.level = menuPage.level();

            // load the MainScatterView
            this.currentPage = new MainScatterView(this.level);
            grid.Children.Clear();
            grid.Children.Add(this.currentPage);

            // Add the returnButton
            SurfaceButton returnButton = (SurfaceButton)Resources["returnButton"];
            grid.Children.Add(returnButton);

            // Subscribe to the ShapesSelectedEvent
            MainScatterView m = this.currentPage as MainScatterView;
            m.OnShapeSelected += this.shapeSelected_Click;
        }

        /// <summary>
        ///  This is called when user click on the return button, this load again the menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            // load the menu
            this.currentPage = new ScatterTangram.Pages.Menu();
            grid.Children.Clear();
            grid.Children.Add(currentPage);

            // Add the button who allows load mainScatterView 
            SurfaceButton goButton = (SurfaceButton)Resources["goButton"];
            grid.Children.Add(goButton);
        }

        /// <summary>
        ///  This is called when user click on a shape in the mainScatterView, this load the secondScatterView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shapeSelected_Click(object sender, EventArgs e)
        {
            // Unsubscribe to the ShapesSelectedEvent
            MainScatterView m = sender as MainScatterView;
            m.OnShapeSelected -= this.shapeSelected_Click;

            // load the SecondScatterView
            ShapeSelectedEventArgs es = e as ShapeSelectedEventArgs;
            this.currentPage = new SecondScatterView(this.lockShape, es.Shape);
            grid.Children.Clear();
            grid.Children.Add(this.currentPage);

            // Add the returnScButton
            SurfaceButton returnScButton = (SurfaceButton)Resources["returnScButton"];
            grid.Children.Add(returnScButton);
        }

        /// <summary>
        ///  This is called when user click on the return button in the secondScatterView, this load again the mainScatterView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnScButton_Click(object sender, RoutedEventArgs e)
        {
            // load the MainScatterView
            this.currentPage = new MainScatterView(this.level);
            grid.Children.Clear();
            grid.Children.Add(this.currentPage);

            // Subscribe to the ShapesSelectedEvent
            MainScatterView m = this.currentPage as MainScatterView;
            m.OnShapeSelected += this.shapeSelected_Click;

            // Add the returnButton
            SurfaceButton returnButton = (SurfaceButton)Resources["returnButton"];
            grid.Children.Add(returnButton);
        }
    }
}