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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : SurfaceUserControl
    {
        public Menu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Return the level selected by the user.
        /// </summary>
        public int level()
        {
            int lvl;

            if ((bool)this.surfaceRadioButton1.IsChecked)
            {
                lvl = 1;
            }
            else if ((bool)this.surfaceRadioButton2.IsChecked)
            {
                lvl = 2;
            }
            else if ((bool)this.surfaceRadioButton3.IsChecked)
            {
                lvl = 3;
            }
            else
            {
                lvl = 0;
            }
            return lvl;
        }
    }
}
