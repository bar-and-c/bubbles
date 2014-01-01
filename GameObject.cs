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
    public class GameObject : INotifyPropertyChanged
    {
#if SIZE_AS_DEPENDENCY_OBJECT
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Size), typeof(GameObject), new PropertyMetadata(""));
        public Size Size
        {
            get { return (Size)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
#else
        private Size _size;
        public Size Size
        {
            get { return _size; }
            set
            {
                if (value != _size)
                {
                    _size = value;
                    OnPropertyChanged("Size");
                }
            }
        }

#endif

#if TOP_AS_DEPENDENCY_OBJECT
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(GameObject), new PropertyMetadata(""));
        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); OnPropertyChanged("Top"); }
        }
#else
        private double _top;
        public double Top
        {
            get { return _top; }
            set 
            {
                if (value != _top)
                {
                    _top = value;
                    OnPropertyChanged("Top");
                }
            }
        }
#endif

#if LEFT_AS_DEPENDENCY_OBJECT
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(GameObject), new PropertyMetadata(""));
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
#else
        private double _left;
        public double Left
        {
            get { return _left; }
            set
            {
                if (value != _left)
                {
                    _left = value;
                    OnPropertyChanged("Left");
                }
            }
        }
#endif






        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
