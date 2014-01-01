using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;


namespace Bubbles
{

    public class Bubble : GameObject, INotifyPropertyChanged
    {




        private double _gas;
        private double _maxGas;

        public static readonly DependencyProperty GasPressureProperty = DependencyProperty.Register("GasPressure", typeof(double), typeof(Bubble), new PropertyMetadata(""));
        public double GasPressure
        {
            get { return (double)GetValue(GasPressureProperty); }
            set { SetValue(GasPressureProperty, value); }
        }


        public Bubble()
        {
            Random r = new Random();
            int diameter = r.Next(40, 200);
            Size = new Size(diameter, diameter);

            _maxGas = Math.Pow(diameter, 2); // TODO: It should be related to size. Experiment to the right amount.
            Gas = _maxGas / 3 + r.NextDouble() * _maxGas / 3;
        }


#if USE_DEPENDENCY_PROPERTY
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(GameObject), new PropertyMetadata(""));
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
#else
        /*
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
         * */
#endif

        public double Gas
        {
            get
            {
                return _gas;
            }
            set
            {
                if (value != _gas)
                {
                    _gas = value;
                    if (_gas > _maxGas)
                        GasPressure = _maxGas;
                    else if (_gas < 0)
                        GasPressure = 0;
                    else
                        GasPressure = _gas / _maxGas;
                }
            }
        }


#if USE_STROKE
        public Brush Stroke
        {
            get
            {
                return _stroke;
            }
            set
            {
                if (value != _stroke)
                {
                    value = _stroke;
                    OnPropertyChanged("Stroke");
                }
            }
        }
#endif


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
    }
}
