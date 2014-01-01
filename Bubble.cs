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

    public class Bubble : GameObject
    {
        private double _gas;
        private double _maxGas;

#if DEPENDENCY_OBJECT
        public static readonly DependencyProperty GasPressureProperty = DependencyProperty.Register("GasPressure", typeof(double), typeof(Bubble), new PropertyMetadata(""));
        public double GasPressure
        {
            get { return (double)GetValue(GasPressureProperty); }
            set { SetValue(GasPressureProperty, value); }
        }
#else
        private double _gasPressure;
        public double GasPressure
        {
            get { return _gasPressure; }
            set 
            {
            if (value != _gasPressure)
            {
                _gasPressure = value;
                OnPropertyChanged("GasPressure");
            }
            }
        }
#endif

        public Bubble()
        {
            Random r = new Random();
            int diameter = r.Next(40, 200);
            Size = new Size(diameter, diameter);

            _maxGas = Math.Pow(diameter, 2); // TODO: It is related to size. Experiment to the right amount.
            Gas = _maxGas / 4 + r.NextDouble() * _maxGas / 2;
        }


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


    }
}
