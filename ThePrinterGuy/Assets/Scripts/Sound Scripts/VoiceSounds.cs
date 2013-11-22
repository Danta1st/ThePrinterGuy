using UnityEngine;
using System.Collections;

public class VoiceSounds : MonoBehaviour
{
    private GenericSoundScript _voiceBoss;

    void Awake()
    {
        _voiceBoss = transform.FindChild("Voice_Boss").
            GetComponent<GenericSoundScript>();
    }

    #region Happy
    public void Voice_Boss_1()
    {
        _voiceBoss.PlayClip(0);
    }

    public void Voice_Boss_2()
    {
        _voiceBoss.PlayClip(1);
    }

    public void Voice_Boss_3()
    {
        _voiceBoss.PlayClip(2);
    }

    public void Voice_Boss_4()
    {
        _voiceBoss.PlayClip(3);
    }

    public void Voice_Boss_5()
    {
        _voiceBoss.PlayClip(4);
    }

    public void Voice_Boss_6()
    {
        _voiceBoss.PlayClip(5);
    }

    public void Voice_Boss_7()
    {
        _voiceBoss.PlayClip(6);
    }

    public void Voice_Boss_8()
    {
        _voiceBoss.PlayClip(7);
    }

    public void Voice_Boss_9()
    {
        _voiceBoss.PlayClip(8);
    }
    #endregion

    #region Angry
    public void Voice_Boss_11()
    {
        _voiceBoss.PlayClip(9);
    }

    public void Voice_Boss_12()
    {
        _voiceBoss.PlayClip(10);
    }

    public void Voice_Boss_13()
    {
        _voiceBoss.PlayClip(11);
    }

    public void Voice_Boss_14()
    {
        _voiceBoss.PlayClip(12);
    }

    public void Voice_Boss_15()
    {
        _voiceBoss.PlayClip(13);
    }

    public void Voice_Boss_16()
    {
        _voiceBoss.PlayClip(14);
    }

    public void Voice_Boss_17()
    {
        _voiceBoss.PlayClip(15);
    }

    public void Voice_Boss_18()
    {
        _voiceBoss.PlayClip(16);
    }

    public void Voice_Boss_19()
    {
        _voiceBoss.PlayClip(17);
    }
    #endregion

    public void Voice_Boss_Random_Happy()
    {
        _voiceBoss.PlayClip(Random.Range(0, 8));
    }

    public void Voice_Boss_Random_Angry()
    {
        _voiceBoss.PlayClip(Random.Range(9, 17));
    }
}
