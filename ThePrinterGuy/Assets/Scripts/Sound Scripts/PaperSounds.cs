using UnityEngine;
using System.Collections;

public class PaperSounds : MonoBehaviour
{
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;

    void Awake()
    {
        _soundFx = transform.transform.FindChild("SoundFx_Papertray").
            GetComponent<GenericSoundScript>();
        _music = transform.transform.FindChild("Music_Papertray").
            GetComponent<GenericSoundScript>();
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
}
