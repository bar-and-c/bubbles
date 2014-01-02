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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

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



        public virtual void Update()
        {
            // TODO: Add movement 
        }
    }
}
