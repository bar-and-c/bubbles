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

        SoundMachine _soundMachine;

        public MainViewModel()
        {
            GameObjects = new ObservableCollection<GameObject>();
            
            _bubbleTimer = new DispatcherTimer();
            _bubbleTimer.Interval = TimeSpan.FromMilliseconds(500);
            _bubbleTimer.Tick += _dispatcherTimer_Tick;

            _gameLoopTimer = new DispatcherTimer();
            _gameLoopTimer.Interval = TimeSpan.FromMilliseconds(20); // A 20 ms period => 50 frames per second. TODO: Is that too frequent? Profile the loop now and then.
            _gameLoopTimer.Tick += _gameLoopTimer_Tick;

            _soundMachine = new SoundMachine();
            _soundMachine.InitAudio();
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
                Bubble b = new Bubble(GameArea);
                b.PropertyChanged += b_PropertyChanged;
                GameObjects.Add(b);
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

        void b_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((sender is Bubble) && (e.PropertyName == "IsPopped"))
            {
                // Stop noise
                Noisesizer n = _soundMachine.GetNoiseForObject(sender);
                n.Off();

                // TODO: Play sound
                

                // TODO: Add cool graphics

                GameObjects.Remove((Bubble)sender);
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


        internal void Pressed(Bubble b, double p)
        {
            Noisesizer n = _soundMachine.GetNoiseForObject((object)b);
            n.On();
        }

        internal void Pressure(Bubble b, double p)
        {
            // Add some color to the bubble
            b.AddColor(p);

            // And change the noise
            /* TODO:
             * Get the Noiseziser associated with this bubble, if any (if not, get a free one). 
             * Send pressure info to it.
             */
            Noisesizer n = _soundMachine.GetNoiseForObject((object)b);
            n.RelativeFrequency = (float)p;
        }

        internal void Released(Bubble b)
        {
            Noisesizer n = _soundMachine.GetNoiseForObject((object)b);
            n.Off();
        }
    }
}
