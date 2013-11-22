using UnityEngine;
using System.Collections;

public class InGameSounds : MonoBehaviour
{
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;

    void Awake()
    {
        _soundFx = transform.transform.FindChild("SoundFx_Game").
            GetComponent<GenericSoundScript>();
        _music = transform.transform.FindChild("Music_Game").
            GetComponent<GenericSoundScript>();
    }

    public void Music_InGame_Main()
    {
        _music.LoopClipStart(0);
    }

    public void Effect_InGame_Win()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_InGame_Lose()
    {
        _soundFx.PlayClip(1);
    }
}
