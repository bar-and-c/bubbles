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
        private static Random _random = new Random();
        private DispatcherTimer _bubbleTimer;
        private DispatcherTimer _gameLoopTimer;

        private SoundMachine _soundMachine;

        private readonly object _gameObjectLock = new object();

        public MainViewModel()
        {
            GameObjects = new ObservableCollection<GameObject>();
            
            _bubbleTimer = new DispatcherTimer();
            _bubbleTimer.Interval = TimeSpan.FromMilliseconds(500);
            _bubbleTimer.Tick += _dispatcherTimer_Tick;

            _gameLoopTimer = new DispatcherTimer();
            _gameLoopTimer.Interval = TimeSpan.FromMilliseconds(20);
            _gameLoopTimer.Tick += _gameLoopTimer_Tick;

            _soundMachine = new SoundMachine();
            _soundMachine.InitAudio();
        }



        public ObservableCollection<GameObject> GameObjects { get; private set; }

        public Size GameArea { get; set; }

        

        internal void NewGame()
        {
            lock (_gameObjectLock)
            {
                GameObjects.Clear();
            }
            // Dropping in the bubbles at random intervals
            _bubbleTimer.Start();

            // Start the game loop. 
            _gameLoopTimer.Start();
        }

        void _dispatcherTimer_Tick(object sender, object e)
        {
            lock (_gameObjectLock)
            {
                Bubble b = new Bubble(GameArea);
                b.PropertyChanged += b_PropertyChanged;
                GameObjects.Add(b);
            }
            _bubbleTimer.Interval = TimeSpan.FromMilliseconds(_random.Next(500, 2000));
        }

        void b_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((sender is Bubble) && (e.PropertyName == "IsPopped"))
            {
                // Stop noise
                _soundMachine.StopNoiseForObject(sender);

                // Play sound
                _soundMachine.KickSweepForObject(sender);

                // TODO: Add cool graphics...?


                lock (_gameObjectLock)
                {
                    GameObjects.Remove((Bubble)sender);
                }
            }
        }

        void _gameLoopTimer_Tick(object sender, object e)
        {
            lock (_gameObjectLock)
            {
                int numberOfGameObjects = GameObjects.Count;
                for (int i = 0; i < numberOfGameObjects; i++)
                {
                    GameObjects[i].Update();
                }
            }
        }


        internal void Pressed(Bubble bubble)
        {
            _soundMachine.StartNoiseForObject(bubble);
        }

        internal void Pressure(Bubble bubble, double pressure)
        {
            // Add some color to the bubble
            bubble.AddColor(pressure);

            // And change the noise
            _soundMachine.ModulateNoiseForObject(bubble, pressure);
        }

        internal void Released(Bubble bubble)
        {
            _soundMachine.StopNoiseForObject(bubble);
        }
    }
}
