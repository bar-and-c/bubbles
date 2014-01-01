using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.Foundation;

namespace Bubbles
{
    public class MainViewModel
    {
        private int _numberOfBubbles = 2;

        public static Random random = new Random();

        private ObservableCollection<GameObject> _gameObjects;
        public ObservableCollection<GameObject> GameObjects
        {
            get { return _gameObjects ?? (_gameObjects = new ObservableCollection<GameObject>()); }
        }

        public Size GameArea { get; set; }

        internal void NewGame()
        {

            GameObjects.Clear();

            CreateBubbles();
        }

        private void CreateBubbles()
        {
            for (int i = 0; i < _numberOfBubbles; i++)
            {
                Point location = new Point
                {
                    X = random.Next(10, (int)GameArea.Width - 10),
                    Y = random.Next(10, (int)GameArea.Height - 10)
                };
                Bubble bubble = new Bubble() { Location = location, X = location.X };
                GameObjects.Add(bubble);
            }

            // DEBUG: Adding another GameObject to test the GUI stuff
            GameObjects.Add(new GameObject() { X = 400, Location = new Point(400, 600) });
        }


    }
}
