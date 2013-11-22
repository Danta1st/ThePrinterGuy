using UnityEngine;
using System.Collections;

public class BaroSounds : MonoBehaviour
{
    private GenericSoundScript _soundFx;
    private GenericSoundScript _music;

    void Awake()
    {
        _soundFx = transform.transform.FindChild("SoundFx_Barometers").
            GetComponent<GenericSoundScript>();
        _music = transform.transform.FindChild("SoundFx_Barometers").
            GetComponent<GenericSoundScript>();
    }

    public void Effect_Barometer_NormSpin1()
    {
        _soundFx.PlayClip(0);
    }

    public void Effect_Barometer_NormSpin2()
    {
        _soundFx.PlayClip(1);
    }

    public void Effect_Barometer_NormSpin3()
    {
        _soundFx.PlayClip(2);
    }

    public void Effect_Barometer_NokOkSpin1()
    {
        _soundFx.PlayClip(3);
    }

    public void Effect_Barometer_NokOkSpin2()
    {
        _soundFx.PlayClip(4);
    }

    public void Effect_Barometer_NokOkSpin3()
    {
        _soundFx.PlayClip(5);
    }

    public void Effect_Barometer_Tap1()
    {
        _soundFx.PlayClip(6);
    }

    public void Effect_Barometer_Tap2()
    {
        _soundFx.PlayClip(7);
    }

    public void Effect_Barometer_Tap3()
    {
        _soundFx.PlayClip(8);
    }
}
