using UnityEngine;
using System.Collections;

public class InkSounds : MonoBehaviour
{
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;

    void Awake()
    {
        _soundFx = transform.transform.FindChild("SoundFx_Ink").
            GetComponent<GenericSoundScript>();
        _music = transform.transform.FindChild("Music_Ink").
            GetComponent<GenericSoundScript>();
    }

    public void Effect_Ink_SlotOpen1()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_Ink_SlotOpen2()
    {
        _soundFx.PlayClip(1);
    }

    public void Effect_Ink_SlotOpen3()
    {
        _soundFx.PlayClip(2);
    }

    public void Effect_Ink_SlotOpen4()
    {
        _soundFx.PlayClip(3);
    }
}
