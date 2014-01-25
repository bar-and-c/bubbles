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

        WasapiOutRT _waveOut;
        SampleToWaveProvider _sampleToWaveProvider;

        public SoundMachine()
        {
            _noisesizer = new Noisesizer();
            _noisesizer.Off();

        }

        public async void InitAudio()
        {
            _sampleToWaveProvider = new SampleToWaveProvider(_noisesizer);

            _waveOut = new WasapiOutRT(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 10);

            await _waveOut.Init(_sampleToWaveProvider);
            _waveOut.Play();
        }

        internal Noisesizer GetNoiseForObject(object p)
        {
            // TODO: Maybe set up a dictionary object -> noise? 
            return _noisesizer;
        }
    }
}
