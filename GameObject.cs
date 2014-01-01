using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Bubbles
{
    public class GameObject : DependencyObject, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Size), typeof(GameObject), new PropertyMetadata(""));
        public Size Size
        {
            get { return (Size)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Point), typeof(GameObject), new PropertyMetadata(""));
        public Point Location
        {
            get { return (Point)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }


















        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(name);
                handler(this, e);
            }
        }

        private double _x;
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    OnPropertyChanged("X");
                }
            }
        }

    }
}
