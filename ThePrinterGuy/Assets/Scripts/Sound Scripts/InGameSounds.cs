using UnityEngine;
using System.Collections;

public class InGameSounds : MonoBehaviour
{
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;

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

    public GenericSoundScript GetEffectScript()
    {
        return _soundFx;
    }

    public GenericSoundScript GetMusicScript()
    {
        return _music;
    }
}
