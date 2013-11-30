using UnityEngine;
using System.Collections;

public class MainMenuSounds : MonoBehaviour
{
    #region Privates
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;
    #endregion

    #region MonoBehavior
    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Menu").
            GetComponent<GenericSoundScript>();
        _music = transform.FindChild("Music_Menu").
            GetComponent<GenericSoundScript>();
    }

    void Start()
    {
        _music.audio.ignoreListenerPause = true;
        _soundFx.audio.ignoreListenerPause = true;
        _music.audio.ignoreListenerVolume = true;
        _soundFx.audio.ignoreListenerVolume = true;
    }
    #endregion

    #region Sounds
    public void Music_Menu_Main()
    {
        _music.LoopClipStart();
    }

    public void Effect_Menu_Intro()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_Menu_Click()
    {
        _soundFx.PlayClip(1);
    }

    public void Effect_Menu_Stinger()
    {
        _soundFx.PlayClip(2);
    }

    public void Effect_Menu_Appear()
    {
        _soundFx.PlayClip(3);
    }

    public void Effect_Menu_Disappear()
    {
        _soundFx.PlayClip(4);
    }

    public void Effect_Menu_Footsteps()
    {
        _soundFx.PlayClip(5);
    }

    public void Effect_Menu_Credits()
    {
        _soundFx.PlayClip(6);
    }

    public void Effect_Task_Unmatched()
    {
        _soundFx.PlayClip(7);
    }
    #endregion

    public GenericSoundScript GetEffectScript()
    {
        return _soundFx;
    }

    public GenericSoundScript GetMusicScript()
    {
        return _music;
    }
}
