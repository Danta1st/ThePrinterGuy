using UnityEngine;
using System.Collections;

public class MainMenuSounds : MonoBehaviour
{

    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;

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
    }

    public void Music_Menu_Main()
    {
        _music.LoopClipStart(0);
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

    public void Effect_Menu_Options()
    {
        _soundFx.PlayClip(7);
    }
}
