using UnityEngine;
using System.Collections;

public class BaroSounds : MonoBehaviour
{
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private float _lowVolume = 0.8f;
    [SerializeField] private float _highVolume = 1.0f;
    [SerializeField] private float _fadeTime = 2.0f;

    private GameObject _effectObject;

    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;

    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Barometers").
            GetComponent<GenericSoundScript>();
        _music = transform.FindChild("Music_Barometers").
            GetComponent<GenericSoundScript>();
        _effectObject = transform.FindChild("SoundFx_Barometers").gameObject;
    }

    public void LowerVolume()
    {
        iTween.AudioTo(_effectObject, iTween.Hash("audiosource", _effectObject.audio, "volume", _lowVolume,
            "time", _fadeTime, "easetype", _easeType));
    }

    public void RaiseVolume()
    {
        iTween.AudioTo(_effectObject, iTween.Hash("audiosource", _effectObject.audio, "volume", _highVolume,
            "time", _fadeTime, "easetype", _easeType));
    }
    
    public void Effect_Barometer_NormSpin1()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_Barometer_NormSpin2()
    {
        _soundFx.PlayClip(1);
    }

    public void Effect_Barometer_NormSpin3()
    {
        _soundFx.PlayClip(2);
    }

    public void Effect_Barometer_NokOkSpin1()
    {
        _soundFx.PlayClip(3);
    }

    public void Effect_Barometer_NokOkSpin2()
    {
        _soundFx.PlayClip(4);
    }

    public void Effect_Barometer_NokOkSpin3()
    {
        _soundFx.PlayClip(5);
    }

    public void Effect_Barometer_Tap1()
    {
        _soundFx.PlayClip(6);
    }

    public void Effect_Barometer_Tap2()
    {
        _soundFx.PlayClip(7);
    }

    public void Effect_Barometer_Tap3()
    {
        _soundFx.PlayClip(8);
    }
}
