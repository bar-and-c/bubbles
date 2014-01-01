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
//                b.Gas += e.GetCurrentPoint(ellipse).Properties.Pressure;

                /* TODO: Boosting the value just to get a visible change on the opacity. As stated elsewhere - think it through! */
                b.Gas += e.GetCurrentPoint(ellipse).Properties.Pressure * 100;
            }
        }

    }
}
