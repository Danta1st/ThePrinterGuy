using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericSoundScript : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private List<AudioClip> _audioClips = new List<AudioClip>();
    [SerializeField]
    private bool _randomPitchOnPlay;
    [SerializeField]
    private float _minPitch = 0.5f;
    [SerializeField]
    private float _maxPitch = 1.5f;
    #endregion

    #region Privates
    private AudioSource _audioSource;

    private bool _shouldLoop = false;

    private float _currentPitch = 1.0f;
    private float _currentVolume = 1.0f;

    private float _pitchMIN = 0.0f;
    private float _pitchMAX = 3.0f;

    private float _volumeMIN = 0.0f;
    private float _volumeMAX = 1.0f;
    #endregion

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _audioSource.clip = _audioClips[0];
        _audioSource.ignoreListenerPause = true;
    }

    void Update()
    {
        audio.ignoreListenerPause = true;
    }

    #region Set Clip
    public void AssignClip()
    {
        if(!_audioSource.isPlaying)
        {
            _audioSource.clip = _audioClips[0];
        }
    }

    public void AssignClip(int index)
    {
        if(!_audioSource.isPlaying)
        {
            _audioSource.clip = _audioClips[index];
        }
    }
    #endregion

    #region Get Clip
    public AudioClip GetClip()
    {
        return _audioSource.clip;
    }
    #endregion

    #region Loop Clip
    public void LoopClipStart()
    {
        _shouldLoop = true;
        StartCoroutine(PlayLoopingClip(_audioClips[0]));
    }

    public void LoopClipStart(int index)
    {
        _shouldLoop = true;
        StartCoroutine(PlayLoopingClip(_audioClips[index]));
    }

    public void LoopCurrentClipStart()
    {
        _shouldLoop = true;
        StartCoroutine(PlayLoopingClip(_audioSource.clip));
    }

    public void LoopClipStop()
    {
        _shouldLoop = false;
    }
    #endregion

    #region PlayClip
    public void PlayClip()
    {
        if(_randomPitchOnPlay)
        {
            RandomPitch();
        }

        _audioSource.PlayOneShot(_audioClips[0]);
    }

    public void PlayClip(int index)
    {
        if(_audioSource != null)
        {
            if(_randomPitchOnPlay)
            {
                RandomPitch();
            }

            _audioSource.PlayOneShot(_audioClips[index]);
        }
    }

    private IEnumerator PlayLoopingClip(AudioClip thisClip)
    {
        _audioSource.Stop();

        if(_shouldLoop)// && !_audioSource.isPlaying)
        {
            _audioSource.clip = thisClip;
            _audioSource.Play();

            yield return new WaitForSeconds(_audioSource.clip.length);
            PlayLoopingClip(_audioSource.clip);
        }
    }
    #endregion

    #region Random Pitch
    public void RandomPitch()
    {
        _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
    }

    public void RandomPitch(float minPitch, float maxPitch)
    {
        float thisMinPitch = Mathf.Clamp(minPitch, _pitchMIN, _pitchMAX);
        float thisMaxPitch = Mathf.Clamp(maxPitch, _pitchMIN, _pitchMAX);

        _audioSource.pitch = Random.Range(thisMinPitch, thisMaxPitch);
    }
    #endregion

    #region Set Pitch
    public void SetPitch(float endPitch)
    {
        float thisEndPitch = Mathf.Clamp(endPitch, _pitchMIN, _pitchMAX);

        _audioSource.pitch = thisEndPitch;
    }

//    public void SetPitch(float endPitch, float time)
//    {
//        float thisEndPitch = Mathf.Clamp(endPitch, _pitchMIN, _pitchMAX);
//
//        float i = 0.0f;
//        float rate = 1.0f/time;
//        float currentPitch = _currentPitch;
//
//        while (i < 1.0f)
//        {
//             i += Time.deltaTime * rate;
//             _audioSource.pitch = Mathf.Lerp(currentPitch, thisEndPitch, i);
//             _currentPitch = _audioSource.pitch;
//             break;
//        }
//
//        Debug.Log(_audioSource.pitch);
//    }

    public void SetPitch(float startPitch, float endPitch, float time)
    {
        float thisStartPitch = Mathf.Clamp(startPitch, _pitchMIN, _pitchMAX);
        float thisEndPitch = Mathf.Clamp(endPitch, _pitchMIN, _pitchMAX);

        float i = 0.0f;
        float rate = 1.0f/time;

        while (i < 1.0f)
        {
             i += Time.deltaTime * rate;
             _audioSource.pitch = Mathf.Lerp(thisStartPitch, thisEndPitch, i);
             _currentPitch = _audioSource.pitch;
             break;
        }
    }
    #endregion

    #region Get Pitch
    public float GetPitch()
    {
        return _audioSource.pitch;
    }
    #endregion

    #region Set Volume
    public void SetVolume(float newVolume)
    {
        float thisNewVolume = Mathf.Clamp(newVolume, _volumeMIN, _volumeMAX);

        _audioSource.volume = thisNewVolume;
    }

//    public void SetVolume(float newVolume, float time)
//    {
//        float thisNewVolume = Mathf.Clamp(newVolume, _volumeMIN, _volumeMAX);
//
//        float i = 0.0f;
//        float rate = 1.0f/time;
//        float currentVolume = _currentVolume;
//
//        while (i < 1.0f)
//        {
//             i += Time.deltaTime * rate;
//             _audioSource.volume = Mathf.Lerp(currentVolume, thisNewVolume, i);
//             _currentVolume = _audioSource.volume;
//             break;
//        }
//
//        Debug.Log(_audioSource.volume);
//    }

    public void SetVolume(float startVolume, float endVolume, float time)
    {
        float thisStartVolume = Mathf.Clamp(startVolume, _volumeMIN, _volumeMAX);
        float thisEndVolume = Mathf.Clamp(endVolume, _volumeMIN, _volumeMAX);

        float i = 0.0f;
        float rate = 1.0f/time;

        while (i < 1.0f)
        {
             i += Time.deltaTime * rate;
             _audioSource.volume = Mathf.Lerp(thisStartVolume, thisEndVolume, i);
             _currentVolume = _audioSource.volume;
             break;
        }
    }
    #endregion

    #region Get Volume
    public float GetVolume()
    {
        return _audioSource.volume;
    }
    #endregion

    #region Set Pan
    public void SetPanLevel(float newPanLevel)
    {
        _audioSource.panLevel = newPanLevel;
    }
    #endregion

    #region General Method
    public void FadeVolume(float newVolume, float fadeTime)
    {
        iTween.AudioTo(gameObject, iTween.Hash("audiosource", gameObject.audio, "volume", newVolume,
            "time", fadeTime, "easetype", iTween.EaseType.linear));
            //, "oncomplete", "Pause", "oncompletetarget", gameObject));
    }

    public void Pause()
    {
        //Debug.Log("DONNNEEE + " + Time.time);
    }
    #endregion
}
