#define USE_RELATIVE_COLOR_INCREASE


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
        private double _colorDiameter;
        private Size _gameArea;

#if USE_RELATIVE_COLOR_INCREASE
        private const double _diameterIncreaseFactor = 0.015;
#else
        private const double _diameterIncreaseFactor = 1;
#endif


        public Bubble(Size GameArea)
        {
            this._gameArea = GameArea;
            Random r = new Random();
            int diameter = r.Next(40, 200);
            Size = new Size(diameter, diameter);
            ColorDiameter = 0;

            int margin = 100;
            X = r.Next(margin, (int)_gameArea.Width - margin);
            Y = 200;
        }

        public double ColorDiameter
        {
            get { return _colorDiameter; }
            set 
            {
                if ((value != _colorDiameter) && (!IsPopped))
                {
                    if (value < Size.Width)
                    {
                        _colorDiameter = value;
                    }
                    else
                    {
                        IsPopped = true;
                        _colorDiameter = Size.Width;
                    }
                    OnPropertyChanged("ColorDiameter");
                }
            }
        }





        // Physics from http://buildnewgames.com/gamephysics/

        private double _vx = 0;
        private double _vy = 0;

        private double _ax = 0;
        private double _ay = 0;

        private double Mass { get { return (Size.Width * Size.Height); } }

        private double _dt = 0.02;

        private double _airResistance = 1000;

        private double _gravity = 9.81;



        public override void Update()
        {
            base.Update();


            double gravitationForce = Size.Width * Size.Height * _gravity;


            double airResistanceForce = -(0.5 * _airResistance * Size.Width * _vy * _vy);


            double forceY = gravitationForce + airResistanceForce;


            double dy = _vy * _dt + (0.5 * _ay * _dt * _dt);
            Y += dy * 100;

            double newAccelerationY = forceY / Mass;
            double averageAccelerationY = 0.5 * (newAccelerationY + _ay);
            _vy += averageAccelerationY * _dt;
        }

        internal void AddColor(double pressure)
        {
#if USE_RELATIVE_COLOR_INCREASE
            ColorDiameter += pressure * Size.Width * _diameterIncreaseFactor;
#else
            ColorDiameter += pressure * _diameterIncreaseFactor;
#endif
        }

        private bool _isPopped = false;
        public bool IsPopped
        {
            get
            {
                return _isPopped;
            }
            set
            {
                if (value != _isPopped)
                {
                    _isPopped = value;
                    OnPropertyChanged("IsPopped");
                }
            }
        }
    }
}
