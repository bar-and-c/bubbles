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

            _maxGas = Math.Pow(diameter, 2); // TODO: It is related to size. Experiment to the right amount.
            Gas = _maxGas / 3 + r.NextDouble() * _maxGas / 3;
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
