using UnityEngine;
using System.Collections;

public class InkSounds : MonoBehaviour
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
        _soundFx = transform.FindChild("SoundFx_Ink").
            GetComponent<GenericSoundScript>();
        _music = transform.FindChild("Music_Ink").
            GetComponent<GenericSoundScript>();
        _effectObject = transform.FindChild("SoundFx_Ink").gameObject;
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

    public void Effect_Ink_SlotOpen1()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_Ink_SlotOpen2()
    {
        _soundFx.PlayClip(1);
    }

    public void Effect_Ink_SlotOpen3()
    {
        _soundFx.PlayClip(2);
    }

    public void Effect_Ink_SlotOpen4()
    {
        _soundFx.PlayClip(3);
    }

    public void Effect_Ink_RightSlot()
    {
        _soundFx.PlayClip(4);
    }

    public void Effect_Ink_WrongSlot()
    {
        _soundFx.PlayClip(5);
    }
}
