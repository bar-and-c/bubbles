using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Bubbles
{
    public class MainViewModel
    {
        private int _numberOfBubbles = 10;
        private static Random _random = new Random();
        private DispatcherTimer _dispatcherTimer;

        public MainViewModel()
        {
            GameObjects = new ObservableCollection<GameObject>();
            
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
        }

        void _dispatcherTimer_Tick(object sender, object e)
        {
            if (GameObjects.Count < _numberOfBubbles)
            {
                Bubble bubble = GetRandomBubble();
                GameObjects.Add(bubble);
            }
            if (GameObjects.Count < _numberOfBubbles)
            {
                _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(_random.Next(500, 2000));
            }
            else
            {
                _dispatcherTimer.Stop();
            }
        }

        public ObservableCollection<GameObject> GameObjects { get; private set; }

        public Size GameArea { get; set; }

        

        internal void NewGame()
        {

            GameObjects.Clear();

            // DEBUG: Adding another GameObject to test the GUI stuff

            GameObjects.Add(new GameObject() { Left = GameArea.Width/2 - 250, Top = 900 });
            
            _dispatcherTimer.Start();

//            await Task.Run(() => CreateBubbles());
        }

        private Bubble GetRandomBubble()
        {
            int margin = 100;
            double left = _random.Next(margin, (int)GameArea.Width - margin);
            double top = _random.Next(margin, (int)GameArea.Height - margin);
            //Task.Delay(TimeSpan.FromMilliseconds(_random.Next(500, 2000)));
            return new Bubble() { Left = left, Top = top };
        }


        private void CreateBubbles()
        {
            for (int i = 0; i < _numberOfBubbles; i++)
            {
                Point location = new Point
                {
                    X = _random.Next(10, (int)GameArea.Width - 10),
                    Y = _random.Next(10, (int)GameArea.Height - 10)
                };
                Bubble bubble = new Bubble() { Left = location.X, Top = location.Y };
                GameObjects.Add(bubble);

                Task.Delay(TimeSpan.FromMilliseconds(_random.Next(500, 2000)));
            }

            // DEBUG: Adding another GameObject to test the GUI stuff
            GameObjects.Add(new GameObject() { Left = 400, Top = 600 });
        }


    }
}
