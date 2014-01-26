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


        // For a better game experience, transform the incoming pressure to something more exciting.
        private double ConvertToGamePressure(float p)
        {
            // NOTE: I have experimented with exponential conversion, but after all I think linear is allright
            return Math.Min(Math.Max((p-0.53)/0.47, 0), 1); // Clamp the value
        }

        private void Ellipse_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource is Windows.UI.Xaml.Shapes.Path)
            {
                Windows.UI.Xaml.Shapes.Path ellipse = (Windows.UI.Xaml.Shapes.Path)e.OriginalSource;
                if (ellipse.DataContext is Bubble)
                {
                    Bubble b = (Bubble)ellipse.DataContext;

                    _viewModel.Pressure(b, ConvertToGamePressure(e.GetCurrentPoint(ellipse).Properties.Pressure));
                }
            }
        }


        private void Ellipse_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource is Windows.UI.Xaml.Shapes.Path)
            {
                Windows.UI.Xaml.Shapes.Path ellipse = (Windows.UI.Xaml.Shapes.Path)e.OriginalSource;

                if (ellipse.DataContext is Bubble)
                {
                    Bubble b = (Bubble)ellipse.DataContext;
                    _viewModel.Pressed(b);
                }
            }
        }

        private void Ellipse_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ReleaseBubble(sender);
        }

        private void Ellipse_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            ReleaseBubble(sender);
        }

        private void Path_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            ReleaseBubble(sender);
        }

        private void Path_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ReleaseBubble(sender);
        }

        private void ReleaseBubble(object sender)
        {
            if (sender is Windows.UI.Xaml.Shapes.Path)
            {
                Windows.UI.Xaml.Shapes.Path ellipse = (Windows.UI.Xaml.Shapes.Path)sender;
                if (ellipse.DataContext is Bubble)
                {
                    Bubble b = (Bubble)ellipse.DataContext;
                    _viewModel.Released(b);
                }
            }
        }

    }
}
