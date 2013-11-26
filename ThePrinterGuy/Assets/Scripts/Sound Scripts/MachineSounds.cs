using UnityEngine;
using System.Collections;

public class MachineSounds : MonoBehaviour
{

    private GenericSoundScript _soundFxC;
    private GenericSoundScript _soundFxM;
    private GenericSoundScript _soundFxS;

    void Awake()
    {
        _soundFxC = transform.FindChild("SoundFx_Cogwheels").
            GetComponent<GenericSoundScript>();
        _soundFxM = transform.FindChild("SoundFx_Machine").
            GetComponent<GenericSoundScript>();
        _soundFxS = transform.FindChild("SoundFx_Smoke").
            GetComponent<GenericSoundScript>();
    }

    public void Effect_Machine_Cogwheels1()
    {
        _soundFxC.PlayClip(0);
    }

    public void Effect_Machine_Cogwheels2()
    {
        _soundFxC.PlayClip(1);
    }

    public void Effect_Machine_Cogwheels3()
    {
        _soundFxC.PlayClip(2);
    }

    public void Effect_Machine_Electricity()
    {
        _soundFxM.PlayClip(0);
    }

    public void Effect_Machine_TaskMissed()
    {
        _soundFxM.PlayClip(1);
    }

    public void Effect_Machine_Smoke1()
    {
        _soundFxS.PlayClip(0);
    }

    public void Effect_Machine_Smoke2()
    {
        _soundFxS.PlayClip(1);
    }

    public void Effect_Machine_Smoke3()
    {
        _soundFxS.PlayClip(2);
    }

    public GenericSoundScript GetEffectScriptCogwheels()
    {
        return _soundFxC;
    }

    public GenericSoundScript GetEffectScriptMachine()
    {
        return _soundFxM;
    }

    public GenericSoundScript GetEffectScriptSmoke()
    {
        return _soundFxS;
    }
}
