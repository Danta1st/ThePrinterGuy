using UnityEngine;
using System.Collections;

public class PaperSounds : MonoBehaviour
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

    #region MonoBehavior
    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Papertray").
            GetComponent<GenericSoundScript>();
        _gestures = transform.FindChild("Gestures_Papertray").
            GetComponent<GenericSoundScript>();
        _effectObject = transform.FindChild("SoundFx_Papertray").gameObject;
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
    public void Effect_PaperTray_MoveUp()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_PaperTray_MoveDown()
    {
        _soundFx.PlayClip(1);
    }

    public void Effect_PaperTray_ConveyorBelt()
    {
//        _soundFx.PlayClip(2);
        _soundFx.LoopClipStart(2);
    }

    public void Effect_PaperTray_StopConveyorBelt()
    {
        _soundFx.LoopClipStop();
    }

    public void Effect_PaperTray_Swipe1()
    {
//        _soundFx.PlayClip(5);
        _gestures.PlayClip(0);
    }

    public void Effect_PaperTray_Swipe2()
    {
//        _soundFx.PlayClip(6);
        _gestures.PlayClip(1);
    }

    public void Effect_PaperTray_Swipe3()
    {
//        _soundFx.PlayClip(7);
        _gestures.PlayClip(2);
    }

    public void Effect_PaperTray_Swipe4()
    {
//        _soundFx.PlayClip(8);
        _gestures.PlayClip(3);
    }

    public void Effect_PaperTray_WrongSwipe()
    {
//        _soundFx.PlayClip(9);
        _gestures.PlayClip(4);
    }
    #endregion

    public GenericSoundScript GetEffectScript()
    {
        return _soundFx;
    }
}
