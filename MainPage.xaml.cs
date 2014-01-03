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
            Ellipse ellipse = (Ellipse) e.OriginalSource;
            if (ellipse.DataContext is Bubble)
            {
                Bubble b = (Bubble)ellipse.DataContext;

                /* TODO: Boosting the value just to get a visible change on the opacity. 
                 * As stated elsewhere - think it through! Maybe it's better done elsewhere. Etc. */
                b.BoostGas(ConvertToGamePressure(e.GetCurrentPoint(ellipse).Properties.Pressure));
            }
        }

        // For a better game experience, transform the incoming pressure to something more exciting.
        private double ConvertToGamePressure(float p)
        {
            // Minimum pressure is about 0.53, maximum is 1.0 
            double offset = 0.53;
            double scaledIncomingPressure = (p - offset) * 2; // TODO: Think it through properly, after some sleep!!!
            return Math.Pow(100, scaledIncomingPressure);
        }

        private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Ellipse ellipse = (Ellipse)e.OriginalSource;
            if (ellipse.DataContext is Bubble)
            {
                Bubble b = (Bubble)ellipse.DataContext;
                b.HoldBubble = true;
            }
        }

        private void Ellipse_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Ellipse ellipse = (Ellipse)e.OriginalSource;
            if (ellipse.DataContext is Bubble)
            {
                Bubble b = (Bubble)ellipse.DataContext;
                b.HoldBubble = false;
            }
        }

    }
}
