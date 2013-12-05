﻿using UnityEngine;
using System.Collections;

public class InGameSounds : MonoBehaviour
{
    [SerializeField] private float _musicVolume = 0.5f;
    [SerializeField] private float _endVolume = 0.1f;


    #region Privates
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;
    #endregion

    #region MonoBehavior
    void Awake()
    {
        _soundFx = transform.FindChild("SoundFx_Game").
            GetComponent<GenericSoundScript>();
        _music = transform.FindChild("Music_Game").
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
    public void Music_InGame_Main()
    {
        _music.LoopClipStart();
    }

    public void Effect_InGame_Win()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_InGame_Lose()
    {
        _soundFx.PlayClip(1);
    }

    public void FadeMusic(float fadeTime)
    {
        _music.FadeVolume(_musicVolume, fadeTime);
    }

    public void FadeMusicEnd(float fadeTime)
    {
        _music.FadeVolume(_endVolume, fadeTime);
    }

    public void StopIngameMusic()
    {
        _music.SetVolume(0.0f);
    }

    public GenericSoundScript GetEffectScript()
    {
        return _soundFx;
    }

    public GenericSoundScript GetMusicScript()
    {
        return _music;
    }
    #endregion
}
