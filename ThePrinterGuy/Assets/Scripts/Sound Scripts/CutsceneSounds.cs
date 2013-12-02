using UnityEngine;
using System.Collections;

public class CutsceneSounds : MonoBehaviour
{
    #region Privates
    private GenericSoundScript _music;
    private GenericSoundScript _soundFx;
    #endregion

    #region MonoBehavior
    void Awake()
    {
        _music = transform.FindChild("Music_Cutscene").
            GetComponent<GenericSoundScript>();
        _soundFx = transform.FindChild("SoundFx_Cutscene").
            GetComponent<GenericSoundScript>();
    }
    #endregion

    #region CutScene Sounds
    public void CutScene_Main_Music()
    {
        _music.PlayClip(0);
    }

    public void CutScene_Effect_Point()
    {
        _soundFx.PlayClip(0);
    }

    public void CutScene_Effect_Coffee_01()
    {
        _soundFx.PlayClip(1);
    }

    public void CutScene_Effect_Coffee_02()
    {
        _soundFx.PlayClip(2);
    }

    public void CutScene_Random_Coffee()
    {
        _soundFx.PlayClip(Random.Range(1, 2));
    }

    public GenericSoundScript GetMusicScript()
    {
        return _music;
    }

    public GenericSoundScript GetEffectsScript()
    {
        return _soundFx;
    }
    #endregion

}
