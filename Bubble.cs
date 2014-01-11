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

#if apa
        private double _dropSpeed;
        private double _dropSpeedIncrease;
#endif


        public Bubble(Size GameArea)
        {
            this._gameArea = GameArea;
            Random r = new Random();
            int diameter = r.Next(40, 200);
            Size = new Size(diameter, diameter);

            _maxGas = Math.Pow(diameter, 2); // TODO: It is related to size. Experiment to the right amount.

            _gas = 0;
            double minInitalGasAmount = _maxGas / 5;
            double maxInitialGasAmount = _maxGas - minInitalGasAmount;
            double randomFactor = r.NextDouble() * (maxInitialGasAmount - minInitalGasAmount);
            SetGas(minInitalGasAmount + randomFactor);

            int margin = 100;
            X = r.Next(margin, (int)_gameArea.Width - margin);
            Y = -300;

            _area = diameter * diameter / 100;

#if apa
            _dropSpeed = 5; // TODO: Experiment to a good value. This will be increased over time. 
            _dropSpeedIncrease = 0.05;
#endif
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

        private double _gasBoostAmount = 0;
        private double _gasBoostDecline = 2;

        private bool _boostBubble;
        public bool BoostBubble 
        {
            get { return _boostBubble; } 
            set
            {
                _boostBubble = value;
            }
        }

        private double _maxGasBoostFactor = 0.1;
        private double _gasBoost;
        public void BoostGas(double gasBoost)
        {
                _gasBoostAmount += gasBoost;
                System.Diagnostics.Debug.WriteLine("Boost: {0}", _gasBoostAmount);
                SetGas(_gas + gasBoost);
        }

        private void SetGas(double gasAmount)
        {
            _gas = gasAmount;
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






        // Physics from http://buildnewgames.com/gamephysics/

        private double _vx = 0;
        private double _vy = 0;

        private double _ax = 0;
        private double _ay = 0;

        private double Mass { get { return (_area + _gas) / 2; } }

        private double _dt = 0.02;

        private double _airResistance = 1500;

        private double _area;

        private double _gravity = 9.81;

        private double _gasLeakFactor = 0.0005;


        /* Todo: 
         * The gas stuff doesn't go too well. Maybe just have a tiny leak, let the boost fill it to near max, 
         * and give a brief upwards thrust? Maybe, maybe, maybe. 
         */

        int i;
        public override void Update()
        {
            base.Update();


            double gravitationForce = Mass * _gravity;


            double airResistanceForce = -(0.5 * _airResistance * _area * _vy * _vy);

            double gasBoostForce = 0;
            if (BoostBubble)
            {
                gasBoostForce = -(_gasBoostAmount * 200);
                _gasBoostAmount = 0;
                BoostBubble = false;



                _gasBoostAmount = _gasBoostAmount * 0.4;
                if (_gasBoostAmount < 5000)
                {
                    _gasBoostAmount = 0;
                    BoostBubble = false;
                }
            }

            // If the resulting force is upwards, the air resistance should be downwards
            if (Math.Sign(gravitationForce + gasBoostForce) < 0)
            {
                airResistanceForce = -airResistanceForce;
            }


            if (BoostBubble || ((i % 50) == 0))
            {
                System.Diagnostics.Debug.WriteLine("g: {0}, air: {1}, gas:{2}, b: {3}", gravitationForce, airResistanceForce, gasBoostForce, BoostBubble);
            }
            i++;

            double forceY = gravitationForce + airResistanceForce + gasBoostForce;


            double dy = _vy * _dt + (0.5 * _ay * _dt * _dt);
            Y += dy * 100;

            double newAccelerationY = forceY / Mass;
            double averageAccelerationY = 0.5 * (newAccelerationY + _ay);
            _vy += averageAccelerationY * _dt;

//            if (BoostBubble) _vy += -10;
            //BoostBubble = false;

            // Lose a little gas over time
            SetGas(_gas - _gasLeakFactor * _gas);
        }

#if apa

        public override void Update()
        {
            base.Update();
            AddGas(GetGasLeakage());
            _dropSpeed += _dropSpeedIncrease;
            Y = GetHorizontalPositionFromGasPressure();
        }

        private double GetGasLeakage()
        {
            /* A first shot at it. 
             * It should be based on size. And pressure? Yes, that sounds reasonable.
             * A smaller bubble leaks gas faster, I think. It also fills up faster. 
             * Higher pressure (i.e. higher altitude) means a larger leakage. 
             * And it should be negative, since it's a leakage.
             * 
             * Experimenting here, making it inversely proportional to the diameter, not area. 
             * And multiplying with some constant to keep the drop velocity reasonable.
             */

            /* A second thought, after experimenting a bit (see below).
             * I think I'd better go all in on the physics instead. Formulate these things 
             * in terms of mass/force/acceleration etc.:
             * 
             * - Pressing a ball could hold it still while we add gas (i.e. accumulate in a seperate variable).
             * 
             * - Releasing a ball could use that accumulated gas amount as a force upwards, resulting in acceleration upwards. 
             * 
             * - There is also acceleration downwards. Does mass come into this? Mass in our case is the amount of gas, innit?
             *   Not really, there's also the ball's size. But all that would imply that bigger balls fall faster, unlike my experiments. 
             *   Still, these experiments stink anyway, probably better off with physics... :-) 
             *   
             * - The ball's colour should be affected even when accumulating gas, i.e. altitude and colour should not be coupled. 
             *   
             * This might be a good guide to physics: http://gamedev.stackexchange.com/a/16466
             * Or keep googling for "game physics gravity force acceleration"
             * */
            double amount;

            amount = -(_dropSpeed * _gasPressure / Size.Width);

            amount = -Math.Pow(_dropSpeed, _gasPressure) / Size.Width;

            amount = -Math.Sqrt(_gasPressure) * _dropSpeed / Size.Width;

            amount = -Math.Sqrt(_gasPressure) * _dropSpeed / Math.Log(Size.Width);

            return amount;
            
        }
#endif
    }
}
