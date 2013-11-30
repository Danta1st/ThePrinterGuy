using UnityEngine;
using System.Collections;

public class CutsceneSounds : MonoBehaviour
{
    #region Privates
    private GenericSoundScript _music;
    #endregion

    #region MonoBehavior
    void Awake()
    {
        _music = transform.FindChild("Music_Cutscene").
            GetComponent<GenericSoundScript>();
    }

    public void CutScene_Main_Music()
    {
        _music.PlayClip(0);
    }
    #endregion
}
