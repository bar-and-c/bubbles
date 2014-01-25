using NAudio.Dsp;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubbles
{
    public class Noisesizer : ISampleProvider
    {

        private BiQuadFilter _filter;

        private SignalGenerator _signalGenerator;

        private float _amplitude = 0.1f;

        public float FilterBaseFrequency { get; set; }

        private float _relativeFrequency;
        public float RelativeFrequency 
        {
            get { return _relativeFrequency; } 
            set
            {
                if (value != _relativeFrequency)
                {
                    _relativeFrequency = value;
                    _filterBankIndex = (int) (value * (_numberOfFilterBanks - 1));
                }
            }
        }

        private float _filterQ = 20;

        private float[] _buffer;
        private int _bufferSize;
        
        private const int _numberOfFilterBanks = 50;
        private List<BiQuadFilter> _filterBank;
        private const int _minFrequency = 100; // Hz
        private const int _maxFrequency = 5000; // Hz

        private int _filterBankIndex = 0;

        public Noisesizer()
        {
            _signalGenerator = new SignalGenerator(48000, 1); // TODO: Consider stereo!
            _signalGenerator.Type = SignalGeneratorType.White;

            GetDefaultDeviceInfo();
            
            _filter = BiQuadFilter.BandPassFilterConstantPeakGain(_signalGenerator.WaveFormat.SampleRate, 400, _filterQ);
          //  _filter = BiQuadFilter.LowPassFilter(_signalGenerator.WaveFormat.SampleRate, 400, 20);

            _filterBank = new List<BiQuadFilter>();
            for (int i = 0; i < _numberOfFilterBanks; i++)
            {
                float frequency = _maxFrequency * (i + 1) / _numberOfFilterBanks;
                frequency = _minFrequency * (float)Math.Pow(_maxFrequency / _minFrequency, i / (float)_numberOfFilterBanks);
                _filterBank.Add(BiQuadFilter.BandPassFilterConstantPeakGain(_signalGenerator.WaveFormat.SampleRate, frequency, _filterQ));
            }


            _bufferSize = _signalGenerator.WaveFormat.SampleRate * 2; // I think two seconds should be enough
            _buffer = new float[_bufferSize]; 
        }

        private async static void GetDefaultDeviceInfo()
        {
            String defaultDevice = WasapiCaptureRT.GetDefaultCaptureDevice();
            Windows.Devices.Enumeration.DeviceInformation x = await Windows.Devices.Enumeration.DeviceInformation.CreateFromIdAsync(defaultDevice);
            foreach (String key in x.Properties.Keys)
            {
                System.Diagnostics.Debug.WriteLine("{0} : {1}", key, x.Properties[key]);
            }
           
        }


        public int Read(float[] buffer, int offset, int count)
        {
            if (count > _bufferSize)
            {
                _bufferSize = 2 * count;
                _buffer = new float[_bufferSize];
            }

            _signalGenerator.Read(_buffer, 0, count);
            for (int i = 0; i < count; i++)
            {
                float sample = _buffer[i];
                for (int j = 0; j < _numberOfFilterBanks; j++)
                {
                    if (j == _filterBankIndex)
                    {
                        sample = _filterBank[_filterBankIndex].Transform(_buffer[i]);
                    }
                    else
                    {
                        // Just run the input through each filter - I believe it affects the filter state
                        _filterBank[j].Transform(_buffer[i]);
                    }
                }
                buffer[i + offset] = sample * _amplitude;
            }
            return count;
        }

        public WaveFormat WaveFormat
        {
            get { return _signalGenerator.WaveFormat; }
        }

        internal void On()
        {
            /* When starting a noise, it's better not to have a really bright filter to start with, as it initially 
             * typically is low (i.e. if not starting at a low filter setting a high pitched glitch is heard). 
             * An alternative could be to lock the RelativeFrequency when not on. */
            RelativeFrequency = 0;
            _amplitude = 0.1f;
        }

        internal void Off()
        {
            _amplitude = 0;
            RelativeFrequency = 0;
        }
    }
}
