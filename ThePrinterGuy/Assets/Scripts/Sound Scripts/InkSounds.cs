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
    #endregion

    #region MoboBehavior
    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Ink").
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

    public void Effect_Ink_SlotClose1()
    {
        _soundFx.PlayClip(6);
    }

    public void Effect_Ink_SlotClose2()
    {
        _soundFx.PlayClip(7);
    }

    public void Effect_Ink_SlotClose3()
    {
        _soundFx.PlayClip(8);
    }

    public void Effect_Ink_SlotClose4()
    {
        _soundFx.PlayClip(9);
    }
    #endregion

    public GenericSoundScript GetEffectScript()
    {
        return _soundFx;
    }
}
