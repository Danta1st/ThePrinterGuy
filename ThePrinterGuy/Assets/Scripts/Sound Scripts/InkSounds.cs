using UnityEngine;
using System.Collections;

public class InkSounds : MonoBehaviour
{
    #region Editor Publics
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private float _lowVolume = 0.8f;
    [SerializeField] private float _highVolume = 1.0f;
    [SerializeField] private float _fadeTime = 2.0f;
    #endregion

    #region Privates
    private GameObject _effectObject;
    private GenericSoundScript _soundFx;
    private GenericSoundScript _gestures;
    #endregion

    #region MoboBehavior
    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Ink").
            GetComponent<GenericSoundScript>();
        _gestures = transform.FindChild("Gestures_Ink").
            GetComponent<GenericSoundScript>();
        _effectObject = transform.FindChild("SoundFx_Ink").gameObject;
    }

    void Start()
    {
        _soundFx.audio.volume = _lowVolume;
    }
    #endregion

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

    #region Sounds
    public void Effect_Ink_SlotOpen()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_Ink_RightSlot1()
    {
        _gestures.PlayClip(0);
    }

    public void Effect_Ink_RightSlot2()
    {
        _gestures.PlayClip(1);
    }

    public void Effect_Ink_RightSlot3()
    {
        _gestures.PlayClip(2);
    }

    public void Effect_Ink_RightSlot4()
    {
        _gestures.PlayClip(3);
    }

    public void Effect_Ink_WrongSlot()
    {
        _gestures.PlayClip(4);
    }
    #endregion

    public GenericSoundScript GetEffectScript()
    {
        return _soundFx;
    }
}
