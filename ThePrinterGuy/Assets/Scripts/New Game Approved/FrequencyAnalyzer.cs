using UnityEngine;
using System.Collections;

public class FrequencyAnalyzer : MonoBehaviour {

    #region Private Variables
    //Array size
    private int _qSamples = 1024;
    //RMS value for 0 dB
    private float _refValue = 0.1f;
    //Minimum amplitude to extract pitch
    private float _threshold = 0.02f;
    //Sound level - Hz
    private float _pitchValue;

    //Audio samples
    private float[] _samples;
    //Audio spectrum
    private float[] _spectrum;
    private float _fSample;
    #endregion

    #region Start and Update
	// Use this for initialization
	void Start () {
	    _samples = new float[_qSamples];
        _spectrum = new float[_qSamples];
        _fSample = AudioSettings.outputSampleRate;
	}
	
	// Update is called once per frame
	void Update () {
        AnalyzeSound();
	}
    #endregion

    #region Class methods
    public float GetPitch()
    {
        return _pitchValue;
    }

    private void AnalyzeSound()
    {
        //Fill array with samples
        audio.GetOutputData(_samples, 0);

        float _sum = 0.0f;

        for(int i = 0; i < _qSamples; i++)
        {
            // Sum squared samples
            _sum += _samples[i]*_samples[i];
        }

        audio.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float _maxV = 0.0f;
        int _maxN = 0;

        //Find max
        for(int i = 0; i < _qSamples; i++)
        {
            if(_spectrum[i] > _maxV && _spectrum[i] > _threshold)
            {
                _maxV = _spectrum[i];
                //maxN is the index of max
                _maxN = i;
            }
        }

        //Pass the index to a float variable
        float _fregN = _maxN;

        //Interpolate index using neighbors
        if(_maxN > 0 && _maxV < _qSamples-1)
        {
            float _dL =  _spectrum[_maxN-1]/_spectrum[_maxN];
            float _dR = _spectrum[_maxN+1]/_spectrum[_maxN];
            _fregN += 0.5f*(_dR*_dR - _dL*_dL);
        }

        //Convert index to frequency
        _pitchValue = _fregN * (_fSample/2) / _qSamples;
        Debug.Log(_pitchValue);
    }
    #endregion
}
