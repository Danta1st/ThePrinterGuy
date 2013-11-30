using UnityEngine;
using System.Collections;

public class VoiceSounds : MonoBehaviour
{
    private GenericSoundScript _voiceBoss;
    private GenericSoundScript _voiceBossEnd;
    private GenericSoundScript _voiceBossMumbling;

    void Awake()
    {
        _voiceBoss = transform.FindChild("Boss_Oneliners").
            GetComponent<GenericSoundScript>();
        _voiceBossEnd = transform.FindChild("Boss_EndScene").
            GetComponent<GenericSoundScript>();
        _voiceBossMumbling = transform.FindChild("Boss_Mumbling").
            GetComponent<GenericSoundScript>();
    }

    #region EndScene
    public void Voice_Boss_EndScene_Fired1()
    {
        _voiceBossEnd.PlayClip(0);
    }

    public void Voice_Boss_EndScene_Fired2()
    {
        _voiceBossEnd.PlayClip(1);
    }

    public void Voice_Boss_EndScene_Fired3()
    {
        _voiceBossEnd.PlayClip(2);
    }

    public void Voice_Boss_EndScene_Fired4()
    {
        _voiceBossEnd.PlayClip(3);
    }

    public void Voice_Boss_EndScene_NotFired1()
    {
        _voiceBossEnd.PlayClip(4);
    }

    public void Voice_Boss_EndScene_NotFired2()
    {
        _voiceBossEnd.PlayClip(5);
    }

    public void Voice_Boss_EndScene_NotFired3()
    {
        _voiceBossEnd.PlayClip(6);
    }

    public void Voice_Boss_EndScene_NotFired4()
    {
        _voiceBossEnd.PlayClip(7);
    }
    #endregion

    #region Mumbling
    public void Voice_Boss_Mumbling_Arrww_1()
    {
        _voiceBossMumbling.PlayClip(0);
    }

    public void Voice_Boss_Mumbling_Arrww_2()
    {
        _voiceBossMumbling.PlayClip(1);
    }

    public void Voice_Boss_Mumbling_Arrww_3()
    {
        _voiceBossMumbling.PlayClip(2);
    }

    public void Voice_Boss_Mumbling_Hmm_1()
    {
        _voiceBossMumbling.PlayClip(3);
    }

    public void Voice_Boss_Mumbling_Hmm_2()
    {
        _voiceBossMumbling.PlayClip(4);
    }

    public void Voice_Boss_Mumbling_Hmm_3()
    {
        _voiceBossMumbling.PlayClip(5);
    }

    public void Voice_Boss_Mumbling_Hrn_1()
    {
        _voiceBossMumbling.PlayClip(6);
    }

    public void Voice_Boss_Mumbling_Hrn2()
    {
        _voiceBossMumbling.PlayClip(7);
    }

    public void Voice_Boss_Mumbling_Hrn_3()
    {
        _voiceBossMumbling.PlayClip(8);
    }

    public void Voice_Boss_Mumbling_Mumble_1()
    {
        _voiceBossMumbling.PlayClip(9);
    }

    public void Voice_Boss_Mumbling_Mumble_2()
    {
        _voiceBossMumbling.PlayClip(10);
    }

    public void Voice_Boss_Mumbling_Mumble_3()
    {
        _voiceBossMumbling.PlayClip(11);
    }

    public void Voice_Boss_Mumbling_No_1()
    {
        _voiceBossMumbling.PlayClip(12);
    }

    public void Voice_Boss_Mumbling_Ohh_1()
    {
        _voiceBossMumbling.PlayClip(13);
    }
    #endregion

    #region Angry Oneliners
    public void Voice_Boss_Angry_FireYou_1()
    {
        _voiceBoss.PlayClip(0);
    }

    public void Voice_Boss_Angry_FireYou_2()
    {
        _voiceBoss.PlayClip(1);
    }

    public void Voice_Boss_Angry_GiveUp_1()
    {
        _voiceBoss.PlayClip(2);
    }

    public void Voice_Boss_Angry_GiveUp_2()
    {
        _voiceBoss.PlayClip(3);
    }

    public void Voice_Boss_Angry_GiveUp_3()
    {
        _voiceBoss.PlayClip(4);
    }

    public void Voice_Boss_Angry_Idiot_1()
    {
        _voiceBoss.PlayClip(5);
    }

    public void Voice_Boss_Angry_Idiot_2()
    {
        _voiceBoss.PlayClip(6);
    }

    public void Voice_Boss_Angry_Idiot_3()
    {
        _voiceBoss.PlayClip(7);
    }

    public void Voice_Boss_Angry_Idiot_4()
    {
        _voiceBoss.PlayClip(8);
    }
    #endregion

    #region Happy Oneliners
    public void Voice_Boss_Happy_Bravo_1()
    {
        _voiceBoss.PlayClip(9);
    }

    public void Voice_Boss_Happy_Bravo_2()
    {
        _voiceBoss.PlayClip(10);
    }

    public void Voice_Boss_Happy_Bravo_3()
    {
        _voiceBoss.PlayClip(11);
    }

    public void Voice_Boss_Happy_KeepGoing_1()
    {
        _voiceBoss.PlayClip(12);
    }

    public void Voice_Boss_Happy_KeepGoing_2()
    {
        _voiceBoss.PlayClip(13);
    }

    public void Voice_Boss_Happy_KeepGoing_3()
    {
        _voiceBoss.PlayClip(14);
    }

    public void Voice_Boss_Happy_Know_1()
    {
        _voiceBoss.PlayClip(15);
    }

    public void Voice_Boss_Happy_Know_2()
    {
        _voiceBoss.PlayClip(16);
    }

    public void Voice_Boss_Happy_NoRaise_1()
    {
        _voiceBoss.PlayClip(17);
    }

    public void Voice_Boss_Happy_NotBad_1()
    {
        _voiceBoss.PlayClip(18);
    }

    public void Voice_Boss_Happy_NotBad_2()
    {
        _voiceBoss.PlayClip(19);
    }

    #endregion

    public void Voice_Boss_Random_Happy()
    {
        _voiceBoss.PlayClip(Random.Range(9, 19));
    }

    public void Voice_Boss_Random_Angry()
    {
        _voiceBoss.PlayClip(Random.Range(0, 8));
    }

    public void Voice_Boss_Random_Mumbling()
    {
        _voiceBossMumbling.PlayClip(Random.Range(0, 13));
    }

    public void Voice_Boss_Random_WinEnd()
    {
        _voiceBossEnd.PlayClip(Random.Range(4, 7));
    }

    public void Voice_Boss_Random_LoseEnd()
    {
        _voiceBossEnd.PlayClip(Random.Range(0, 3));
    }

    public GenericSoundScript GetEffectScript()
    {
        return _voiceBoss;
    }
}
