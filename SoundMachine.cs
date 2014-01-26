using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Win8.Wave.WaveOutputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bubbles
{
    public class SoundMachine
    {
        const int _numberOfSimultaneousSounds = 10; // Ten finger touch, I believe

        Dictionary<object, Noisesizer> _activeNoisesizers;
        object _noisesizerLock = new object();
        List<SignalGenerator> _activeSweepers;
        int _sweeperIndex;
        object _sweeperLock = new object();

        List<ISampleProvider> _sampleProviders;

        WasapiOutRT _waveOut;
        SampleToWaveProvider _sampleToWaveProvider;

        MixingSampleProvider _mixer;

        float _noiseAmplitude = 0.04f;
        float _sweepAmplitude = 0.005f;
        int _sampleRate = 48000;

        public SoundMachine()
        {
            _sampleProviders = new List<ISampleProvider>();

            _activeNoisesizers = new Dictionary<object, Noisesizer>();
            _activeSweepers = new List<SignalGenerator>();
            for (int i = 0; i < _numberOfSimultaneousSounds; i++)
            {
                Noisesizer n = CreateNoiseGenerator();
                _sampleProviders.Add(n);

                SignalGenerator s = CreateSweeper();
                _activeSweepers.Add(s);
                _sampleProviders.Add(s);
            }
            _sweeperIndex = 0;

            _mixer = new MixingSampleProvider(_sampleProviders);
        }

        private Noisesizer CreateNoiseGenerator()
        {
            Noisesizer n;
            n = new Noisesizer(_sampleRate, _noiseAmplitude);
            n.Off();
            return n;
        }

        private SignalGenerator CreateSweeper()
        {
            SignalGenerator s;
            s = new SignalGenerator(_sampleRate, 1);
            s.Type = SignalGeneratorType.SweepSingleShot;
            s.SweepLengthSecs = 0.03f;
            s.Frequency = 400;
            s.FrequencyEnd = 15000;// 00;
            s.Gain = _sweepAmplitude;
            return s;
        }

        public async void InitAudio()
        {
            _sampleToWaveProvider = new SampleToWaveProvider(_mixer);

            _waveOut = new WasapiOutRT(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 10);

            await _waveOut.Init(_sampleToWaveProvider);
            _waveOut.Play();

        }

        internal void StartNoiseForObject(object p)
        {
            lock (_noisesizerLock)
            {
                if (!_activeNoisesizers.ContainsKey(p))
                {
                    ActivateNoisesizerForObject(p);
                }
                _activeNoisesizers[p].On();
            }
        }

        internal void ModulateNoiseForObject(object p, double pressure)
        {
            lock (_noisesizerLock)
            {
                if (!_activeNoisesizers.ContainsKey(p))
                {
                    ActivateNoisesizerForObject(p);
                    _activeNoisesizers[p].On(); // If not already active it needs to be On()
                }
                _activeNoisesizers[p].Modulate((float)pressure);
            }
        }

        internal void StopNoiseForObject(object p)
        {
            lock (_noisesizerLock)
            {
                if (_activeNoisesizers.ContainsKey(p))
                {
                    _activeNoisesizers[p].Off();
                    _activeNoisesizers.Remove(_activeNoisesizers[p]);
                }
            }
        }



        internal void KickSweepForObject(object p)
        {
            int sweeperIndex;
            lock (_sweeperLock)
            {
                sweeperIndex = _sweeperIndex++;
                if (_sweeperIndex == _numberOfSimultaneousSounds)
                {
                    _sweeperIndex = 0;
                }
            }
            _activeSweepers[sweeperIndex].KickSweep();
        }


        private void ActivateNoisesizerForObject(object p)
        {
            Noisesizer objectNoisesizer = null;
            foreach (ISampleProvider sampleProvider in _sampleProviders)
            {
                if (sampleProvider is Noisesizer)
                {
                    Noisesizer noisesizer = (Noisesizer)sampleProvider;
                    if (!_activeNoisesizers.ContainsValue(noisesizer))
                    {
                        objectNoisesizer = noisesizer;
                    }
                }
            }
            if (objectNoisesizer == null)
            {
                // There really should be enogh sound generators for all fingers, but if not, take someone elses
                objectNoisesizer = _activeNoisesizers.First().Value;
                _activeNoisesizers.Remove(objectNoisesizer);
            }
            _activeNoisesizers[p] = objectNoisesizer;
        }
        

    }
}
