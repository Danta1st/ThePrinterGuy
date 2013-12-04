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

    public void Voice_Boss_Mumbling_Hrn_1()
    {
        _voiceBossMumbling.PlayClip(5);
    }

    public void Voice_Boss_Mumbling_Hrn2()
    {
        _voiceBossMumbling.PlayClip(6);
    }

    public void Voice_Boss_Mumbling_Hrn_3()
    {
        _voiceBossMumbling.PlayClip(7);
    }

    public void Voice_Boss_Mumbling_Mumble_1()
    {
        _voiceBossMumbling.PlayClip(8);
    }

    public void Voice_Boss_Mumbling_Mumble_2()
    {
        _voiceBossMumbling.PlayClip(9);
    }

    public void Voice_Boss_Mumbling_Mumble_3()
    {
        _voiceBossMumbling.PlayClip(10);
    }

    public void Voice_Boss_Mumbling_No_1()
    {
        _voiceBossMumbling.PlayClip(11);
    }

    public void Voice_Boss_Mumbling_Ohh_1()
    {
        _voiceBossMumbling.PlayClip(12);
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

    public void Voice_Boss_Angry_PrinterGuy_1()
    {
        _voiceBoss.PlayClip(20);
    }

    public void Voice_Boss_Angry_PrinterGuy_2()
    {
        _voiceBoss.PlayClip(21);
    }

    public void Voice_Boss_Angry_PrinterGuy_3()
    {
        _voiceBoss.PlayClip(22);
    }

    public void Voice_Boss_Angry_WhatIsTheMatter_1()
    {
        _voiceBoss.PlayClip(23);
    }

    public void Voice_Boss_Angry_WhatIsTheMatter_2()
    {
        _voiceBoss.PlayClip(24);
    }

    public void Voice_Boss_Angry_WhatTheHell_1()
    {
        _voiceBoss.PlayClip(25);
    }

    public void Voice_Boss_Angry_WhatTheHell_2()
    {
        _voiceBoss.PlayClip(26);
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

    public void Voice_Boss_Happy_Know_3()
    {
        _voiceBoss.PlayClip(17);
    }

    public void Voice_Boss_Happy_NoRaise_1()
    {
        _voiceBoss.PlayClip(31);
    }

    public void Voice_Boss_Happy_NotBad_1()
    {
        _voiceBoss.PlayClip(18);
    }

    public void Voice_Boss_Happy_NotBad_2()
    {
        _voiceBoss.PlayClip(19);
    }

    public void Voice_Boss_Happy_WellWell_1()
    {
        _voiceBoss.PlayClip(27);
    }

    public void Voice_Boss_Happy_WellWell_2()
    {
        _voiceBoss.PlayClip(28);
    }

    public void Voice_Boss_Happy_YouGetIt_1()
    {
        _voiceBoss.PlayClip(29);
    }

    public void Voice_Boss_Happy_YouGetIt_2()
    {
        _voiceBoss.PlayClip(30);
    }

    #endregion

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

    public void Voice_Boss_Random_FireYou()
    {
        _voiceBoss.PlayClip(Random.Range(0, 1));
    }

    public void Voice_Boss_Random_GiveUp()
    {
        _voiceBoss.PlayClip(Random.Range(2, 4));
    }

    public void Voice_Boss_Random_Idiot()
    {
        _voiceBoss.PlayClip(Random.Range(5, 6));
    }

    public void Voice_Boss_Random_Bravo()
    {
        _voiceBoss.PlayClip(Random.Range(9, 11));
    }

    public void Voice_Boss_Random_KeepGoing()
    {
        _voiceBoss.PlayClip(Random.Range(12, 14));
    }

    public void Voice_Boss_Random_Know()
    {
        _voiceBoss.PlayClip(Random.Range(15, 17));
    }

    public void Voice_Boss_Random_NotBad()
    {
        _voiceBoss.PlayClip(Random.Range(18, 19));
    }

    public void Voice_Boss_Random_PrinterGuy()
    {
        _voiceBoss.PlayClip(Random.Range(20, 22));
    }

    public void Voice_Boss_Random_WhatIsTheMatter()
    {
        _voiceBoss.PlayClip(Random.Range(23, 24));
    }

    public void Voice_Boss_Random_WhatTheHell()
    {
        _voiceBoss.PlayClip(Random.Range(25, 26));
    }

    public void Voice_Boss_Random_WellWell()
    {
        _voiceBoss.PlayClip(Random.Range(27, 28));
    }

    public void Voice_Boss_Random_YouGetIt()
    {
        _voiceBoss.PlayClip(Random.Range(29, 30));
    }

    public GenericSoundScript GetEffectScript()
    {
        return _voiceBoss;
    }
}
