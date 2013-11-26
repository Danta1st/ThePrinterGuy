using UnityEngine;
using System.Collections;

public class PaperSounds : MonoBehaviour
{
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private float _lowVolume = 0.8f;
    [SerializeField] private float _highVolume = 1.0f;
    [SerializeField] private float _fadeTime = 2.0f;

    private GameObject _effectObject;

    private GenericSoundScript _soundFx;

    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Papertray").
            GetComponent<GenericSoundScript>();
        _effectObject = transform.FindChild("SoundFx_Papertray").gameObject;
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
        _soundFx.PlayClip(2);
    }

    public void Effect_PaperTray_ColorChange1()
    {
        _soundFx.PlayClip(3);
    }

    public void Effect_PaperTray_ColorChange2()
    {
        _soundFx.PlayClip(4);
    }

    public void Effect_PaperTray_Swipe1()
    {
        _soundFx.PlayClip(5);
    }

    public void Effect_PaperTray_Swipe2()
    {
        _soundFx.PlayClip(6);
    }

    public void Effect_PaperTray_Swipe3()
    {
        _soundFx.PlayClip(7);
    }

    public void Effect_PaperTray_Swipe4()
    {
        _soundFx.PlayClip(8);
    }

    public void Effect_PaperTray_WrongSwipe()
    {
        _soundFx.PlayClip(9);
    }

    public GenericSoundScript GetEffectScript()
    {
        return _soundFx;
    }
}
