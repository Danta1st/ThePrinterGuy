using UnityEngine;
using System.Collections;

public class RodSounds : MonoBehaviour
{
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private float _lowVolume = 0.8f;
    [SerializeField] private float _highVolume = 1.0f;
    [SerializeField] private float _fadeTime = 2.0f;

    private GameObject _effectObject;

    private GenericSoundScript _soundFx;

    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Uranium Rods").
            GetComponent<GenericSoundScript>();
        _effectObject = transform.FindChild("SoundFx_Uranium Rods").gameObject;
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

    public void Effect_UraniumRods_Popup1()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_UraniumRods_Popup2()
    {
        _soundFx.PlayClip(1);
    }

    public void Effect_UraniumRods_Popup3()
    {
        _soundFx.PlayClip(2);
    }

    public void Effect_UraniumRods_Popup4()
    {
        _soundFx.PlayClip(3);
    }

    public void Effect_UraniumRods_Hammer()
    {
        _soundFx.PlayClip(4);
    }

    public GenericSoundScript GetEffectScript()
    {
        return _soundFx;
    }
}
