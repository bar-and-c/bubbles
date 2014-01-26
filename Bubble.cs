

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
        private Size _gameArea;

        private double _fillRatio = 0;
        private const double _fillIncreaseFactor = 0.01;


        private double _radiusX;
        public double RadiusX
        {
            get { return _radiusX; }
            set
            {
                if (value != _radiusX)
                {
                    _radiusX = value;
                    ColorRadiusX = _fillRatio * _radiusX;
                    OnPropertyChanged("RadiusX");
                }
            }
        }

        private double _radiusY;
        public double RadiusY
        {
            get { return _radiusY; }
            set
            {
                if (value != _radiusY)
                {
                    _radiusY = value;
                    OnPropertyChanged("RadiusY");
                    ColorRadiusY = _fillRatio * _radiusY;
                }
            }
        }

        private double _colorRadiusX;
        public double ColorRadiusX
        {
            get { return _colorRadiusX; }
            set
            {
                if ((value != _colorRadiusX) && (!IsPopped))
                {
                    if (value < RadiusX)
                    {
                        _colorRadiusX = value;
                    }
                    else
                    {
                        IsPopped = true;
                        _colorRadiusX = RadiusX;
                    }
                    OnPropertyChanged("ColorRadiusX");
                }
            }
        }

        private double _colorRadiusY;
        public double ColorRadiusY
        {
            get { return _colorRadiusY; }
            set
            {
                if ((value != _colorRadiusY) && (!IsPopped))
                {
                    if (value < RadiusY)
                    {
                        _colorRadiusY = value;
                    }
                    else
                    {
                        IsPopped = true;
                        _colorRadiusY = RadiusY;
                    }
                    OnPropertyChanged("ColorRadiusY");
                }
            }
        }


        public Bubble(Size GameArea)
        {
            this._gameArea = GameArea;
            Random r = new Random();
            int radius = r.Next(40, 150);
            RadiusX = radius;
            RadiusY = radius;

            ColorRadiusX = 0;

            int margin = 100;
            X = r.Next(margin, (int)_gameArea.Width - margin);
            Y = r.Next(-radius, 200);
        }






        // Physics from http://buildnewgames.com/gamephysics/

        private double _vx = 0;
        private double _vy = 0;

        private double _ax = 0;
        private double _ay = 0;

        private double Mass { get { return (RadiusX * RadiusY); } }

        private double _dt = 0.02;

        private double _airResistance = 1000;

        private double _gravity = 9.81;



        public override void Update()
        {
            base.Update();


            double gravitationForce = Mass * _gravity;


            double airResistanceForce = -(0.5 * _airResistance * RadiusX * _vy * _vy);


            double forceY = gravitationForce + airResistanceForce;


            double dy = _vy * _dt + (0.5 * _ay * _dt * _dt);

            // Flattening the bubbles as they hit the ground. 
            /* TODO: As a bubble flattens completely, turn into a line and raise the bottom level. */
            double yIncrease = dy * 100;
            if ((Y + yIncrease + RadiusY) < _gameArea.Height)
            {
                Y += yIncrease;
            }
            else if ((RadiusY + yIncrease / 2) > 2)
            {
                Y += yIncrease / 2;
                RadiusY -= yIncrease / 2;
                RadiusX += yIncrease;
            }


            double newAccelerationY = forceY / Mass;
            double averageAccelerationY = 0.5 * (newAccelerationY + _ay);
            _vy += averageAccelerationY * _dt;


        }

        internal void AddColor(double pressure)
        {
            _fillRatio += pressure * _fillIncreaseFactor;
            ColorRadiusX = _fillRatio * RadiusX;
            ColorRadiusY = _fillRatio * RadiusY;
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
