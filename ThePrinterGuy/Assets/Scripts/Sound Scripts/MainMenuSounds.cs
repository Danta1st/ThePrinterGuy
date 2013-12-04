using UnityEngine;
using System.Collections;

public class MainMenuSounds : MonoBehaviour
{
    [SerializeField] private float _musicVolume = 0.5f;
    [SerializeField] private float _endVolume = 0.1f;

    #region Privates
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;
    private GenericSoundScript _tasks;
    #endregion

    #region MonoBehavior
    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Menu").
            GetComponent<GenericSoundScript>();
        _music = transform.FindChild("Music_Menu").
            GetComponent<GenericSoundScript>();
        _tasks = transform.FindChild("Tasks_Menu").
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

    public void Effect_Task_Unmatched()
    {
        _tasks.PlayClip(0);
    }

    public void FadeMusic(float fadeTime)
    {
        _music.FadeVolume(_musicVolume, fadeTime);
    }

    public void FadeMusicEnd(float fadeTime)
    {
        _music.FadeVolume(_endVolume, fadeTime);
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
