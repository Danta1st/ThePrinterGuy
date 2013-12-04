using UnityEngine;
using System.Collections;

public class MachineSounds : MonoBehaviour
{
    #region Privates
    private GenericSoundScript _soundFxM;
    #endregion

    #region Monobehavior
    void Awake()
    {
        _soundFxM = transform.FindChild("SoundFx_Machine").
            GetComponent<GenericSoundScript>();
    }
    #endregion

    #region Sounds
    public void Effect_Machine_TaskMissed()
    {
        _soundFxM.PlayClip(1);
    }
    #endregion

    public GenericSoundScript GetEffectScriptMachine()
    {
        return _soundFxM;
    }
}
