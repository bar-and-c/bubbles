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


namespace Bubbles
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel _viewModel;

        public MainPage()
        {
            this.InitializeComponent();

            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            Loaded += (x, e) => CreateGame();
        }

        private void CreateGame()
        {
            _viewModel.GameArea = new Size(this.ActualWidth, this.ActualHeight);
            _viewModel.NewGame();
        }

        private void Ellipse_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource is Windows.UI.Xaml.Shapes.Path)
            {
                Windows.UI.Xaml.Shapes.Path ellipse = (Windows.UI.Xaml.Shapes.Path)e.OriginalSource;
                if (ellipse.DataContext is Bubble)
                {
                    Bubble b = (Bubble)ellipse.DataContext;

                    /* TODO: Boosting the value just to get a visible change on the opacity. 
                     * As stated elsewhere - think it through! Maybe it's better done elsewhere. Etc. */
                    b.AddColor(ConvertToGamePressure(e.GetCurrentPoint(ellipse).Properties.Pressure));
                }
            }
        }

        // For a better game experience, transform the incoming pressure to something more exciting.
        private double ConvertToGamePressure(float p)
        {
            // Minimum pressure is about 0.53, maximum is 1.0 
            double offset = 0.53;
            double scaledIncomingPressure;
            scaledIncomingPressure = Math.Pow(2.15 * (p - offset), 2);
//            scaledIncomingPressure = Math.Pow(4.4, p - offset) - 1;
            scaledIncomingPressure = Math.Min(scaledIncomingPressure, 1);
            scaledIncomingPressure = Math.Max(scaledIncomingPressure, 0);
            System.Diagnostics.Debug.WriteLine("P: {0}, scaled: {1}", p, scaledIncomingPressure);
            return scaledIncomingPressure;
        }

        private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource is Windows.UI.Xaml.Shapes.Path)
            {
                Windows.UI.Xaml.Shapes.Path ellipse = (Windows.UI.Xaml.Shapes.Path)e.OriginalSource;

                if (ellipse.DataContext is Bubble)
                {
                    Bubble b = (Bubble)ellipse.DataContext;
                }
            }
        }

        private void Ellipse_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource is Windows.UI.Xaml.Shapes.Path)
            {
                Windows.UI.Xaml.Shapes.Path ellipse = (Windows.UI.Xaml.Shapes.Path)e.OriginalSource;
                if (ellipse.DataContext is Bubble)
                {
                    Bubble b = (Bubble)ellipse.DataContext;
                }
            }
        }

    }
}
