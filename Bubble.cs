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
        private double _gasPressure;
        private Size _gameArea;


        public Bubble(Size GameArea)
        {
            this._gameArea = GameArea;
            Random r = new Random();
            int diameter = r.Next(40, 200);
            Size = new Size(diameter, diameter);

            _maxGas = Math.Pow(diameter, 2); // TODO: It is related to size. Experiment to the right amount.

            _gas = 0;
            double minInitalGasAmount = _maxGas / 4;
            double maxInitialGasAmount = _maxGas - minInitalGasAmount;
            double randomFactor = r.NextDouble() * (maxInitialGasAmount - minInitalGasAmount);
            AddGas(minInitalGasAmount + randomFactor);

            int margin = 100;
            Left = r.Next(margin, (int)_gameArea.Width - margin);
            Top = GetHorizontalPositionFromGasPressure();
        }

        // This is basically just a relative gas metric, normalized for GUI usage. TODO: A bit smelly.
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

        public void AddGas(double gasAmount)
        {
            _gas += gasAmount;

            UpdateRelativeGasPressure();
        }

        private void UpdateRelativeGasPressure()
        {
            // TODO: Think this through. This is where the touch pressure gets in - how shall it be represented in the model?
            if (_gas > _maxGas)
                GasPressure = 1.0;
            else if (_gas < 0)
                GasPressure = 0;
            else
                GasPressure = _gas / _maxGas;
        }

        private double GetHorizontalPositionFromGasPressure()
        {
            return (1 - GasPressure) * _gameArea.Height; // The y-level is supposed to be related to the "gas pressure". 
        }


        public override void Update()
        {
            base.Update();
            Top = GetHorizontalPositionFromGasPressure();

//            Gas -= GetGasLeakage();
        }

    }
}
