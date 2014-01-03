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
        private DispatcherTimer _bubbleTimer;
        private DispatcherTimer _gameLoopTimer;


        public MainViewModel()
        {
            GameObjects = new ObservableCollection<GameObject>();
            
            _bubbleTimer = new DispatcherTimer();
            _bubbleTimer.Interval = TimeSpan.FromMilliseconds(500);
            _bubbleTimer.Tick += _dispatcherTimer_Tick;

            _gameLoopTimer = new DispatcherTimer();
            _gameLoopTimer.Interval = TimeSpan.FromMilliseconds(20); // A 20 ms period => 50 frames per second. TODO: Is that too frequent? Profile the loop now and then.
            _gameLoopTimer.Tick += _gameLoopTimer_Tick;
        }



        public ObservableCollection<GameObject> GameObjects { get; private set; }

        public Size GameArea { get; set; }

        

        internal void NewGame()
        {

            GameObjects.Clear();

            // TODO: Adding another GameObject to test the GUI stuff. Still, it looks pretty cool, might keep it in. 
      //      GameObjects.Add(new GameObject() { X = GameArea.Width/2 - 250, Y = 900 });

            // Dropping in the bubbles at random intervals
            _bubbleTimer.Start();

            // Start the game loop. 
            _gameLoopTimer.Start();
        }

        void _dispatcherTimer_Tick(object sender, object e)
        {
            if (GameObjects.Count < _numberOfBubbles)
            {
                GameObjects.Add(new Bubble(GameArea));
            }
            if (GameObjects.Count < _numberOfBubbles)
            {
                _bubbleTimer.Interval = TimeSpan.FromMilliseconds(_random.Next(500, 2000));
            }
            else
            {
                _bubbleTimer.Stop();
            }
        }

        void _gameLoopTimer_Tick(object sender, object e)
        {
            int numberOfGameObjects = GameObjects.Count;
            for (int i = 0; i < numberOfGameObjects; i++)
            {
                GameObjects[i].Update();
            }

        }

    }
}
