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
        //Play ConveyorBelt sound
    }

    public void Effect_PaperTray_ColorChange1()
    {
        //Play colorchange sound 1
    }

    public void Effect_PaperTray_ColorChange2()
    {
        //Play colorchange sound 2
    }

    public void Effect_PaperTray_Swipe1()
    {
        //Play swipe sound 1
    }

    public void Effect_PaperTray_Swipe2()
    {
        //Play swipe sound 2
    }

    public void Effect_PaperTray_Swipe3()
    {
        //Play swipe sound 3
    }

    public void Effect_PaperTray_Swipe4()
    {
        //Play swipe sound 4
    }

    public void Effect_PaperTray_WrongSwipe()
    {

    }
}
