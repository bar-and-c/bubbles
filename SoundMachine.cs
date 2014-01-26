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
        Noisesizer _noisesizer;
        SignalGenerator _sweeper;

        List<ISampleProvider> _sampleProviders;

        WasapiOutRT _waveOut;
        SampleToWaveProvider _sampleToWaveProvider;

        MixingSampleProvider _mixer;

        public SoundMachine()
        {
            _sampleProviders = new List<ISampleProvider>();

            _noisesizer = new Noisesizer();
            _noisesizer.Off();

            _sampleProviders.Add(_noisesizer);

            _sweeper = new SignalGenerator(48000, 1);
            _sweeper.Type = SignalGeneratorType.SweepSingleShot;
            _sweeper.SweepLengthSecs = 0.03f;
            _sweeper.Frequency = 400;
            _sweeper.FrequencyEnd = 15000;// 00;
            _sweeper.Gain = 0.01;

            _sampleProviders.Add(_sweeper);

            _mixer = new MixingSampleProvider(_sampleProviders);
        }

        public async void InitAudio()
        {
            _sampleToWaveProvider = new SampleToWaveProvider(_mixer);

            _waveOut = new WasapiOutRT(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 10);

            await _waveOut.Init(_sampleToWaveProvider);
            _waveOut.Play();

            _sweeper.KickSweep();
        }

        internal Noisesizer GetNoiseForObject(object p)
        {
            // TODO: Maybe set up a dictionary object -> noise? 
            return _noisesizer;
        }

        internal void KickSweepForObject(object p)
        {
            // TODO: Maybe set up a dictionary object -> noise? 
            _sweeper.KickSweep();
        }
    }
}
