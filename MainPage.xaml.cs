using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Bubbles
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            Loaded += (x, e) => CreateGame();
        }

        private void CreateGame()
        {
            ViewModel.GameArea = new Size(this.ActualWidth, this.ActualHeight);
            ViewModel.NewGame();
        }

        private void Ellipse_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Ellipse ellipse = (Ellipse) e.OriginalSource;
            if (ellipse.DataContext is Bubble)
            {
                Bubble b = (Bubble)ellipse.DataContext;
                b.Gas += e.GetCurrentPoint(ellipse).Properties.Pressure * 100;

                b.Top -= e.GetCurrentPoint(ellipse).Properties.Pressure; // TODO: Just for testing
            }
        }

    }
}
