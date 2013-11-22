using UnityEngine;
using System.Collections;

public class RodSounds : MonoBehaviour
{
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;

    void Awake()
    {
        _soundFx = transform.transform.FindChild("SoundFx_Uranium Rods").
            GetComponent<GenericSoundScript>();
        _music = transform.transform.FindChild("Music_Uranium Rods").
            GetComponent<GenericSoundScript>();
    }

    public void Effect_UraniumRods_Popup1()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_UraniumRods_Popup2()
    {
        _soundFx.PlayClip(1);
    }

    public void Effect_UraniumRods_Popup3()
    {
        _soundFx.PlayClip(2);
    }

    public void Effect_UraniumRods_Hammer()
    {
        _soundFx.PlayClip(3);
    }
}
