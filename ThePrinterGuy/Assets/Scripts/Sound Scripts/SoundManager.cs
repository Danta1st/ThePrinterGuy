using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    private static InGameSounds _inGameSounds;
    private static MainMenuSounds _mainMenuSounds;

    private static PaperSounds _paperSounds;
    private static InkSounds _inkSounds;
    private static RodSounds _rodSounds;
    private static BaroSounds _baroSounds;

    private static VoiceSounds _voiceSounds;

    void Awake()
    {
        _inGameSounds = transform.FindChild("In Game").GetComponent<InGameSounds>();
        _mainMenuSounds = transform.FindChild("Main Menu").GetComponent<MainMenuSounds>();

        _paperSounds = transform.FindChild("PaperTray").GetComponent<PaperSounds>();
        _inkSounds = transform.FindChild("Ink").GetComponent<InkSounds>();
        _rodSounds = transform.FindChild("Uranium Rods").GetComponent<RodSounds>();
        _baroSounds = transform.FindChild("BaroMeter").GetComponent<BaroSounds>();
    }

    public static void thisTest(GameObject go, Vector2 thisPos)
    {
        Debug.Log("Successfull subscription to static ");
    }

    #region Music
    public static void Music_InGame_Main()
    {
        _inGameSounds.Music_InGame_Main();
    }

    public static void Music_Menu_Main()
    {
        _mainMenuSounds.Music_Menu_Main();
    }
    #endregion

    #region Paper Sounds
    public static void Effect_PaperTray_MoveUp()
    {
        _paperSounds.Effect_PaperTray_MoveUp();
    }

    public static void Effect_PaperTray_MoveDown()
    {
        _paperSounds.Effect_PaperTray_MoveDown();
    }

    public static void Effect_PaperTray_ColorChange1()
    {
        _paperSounds.Effect_PaperTray_ColorChange1();
    }

    public static void Effect_PaperTray_ColorChange2()
    {
        _paperSounds.Effect_PaperTray_ColorChange2();
    }

    public static void Effect_PaperTray_Swipe1()
    {
        _paperSounds.Effect_PaperTray_Swipe1();
    }

    public static void Effect_PaperTray_Swipe2()
    {
        _paperSounds.Effect_PaperTray_Swipe2();
    }

    public static void Effect_PaperTray_Swipe3()
    {
        _paperSounds.Effect_PaperTray_Swipe3();
    }

    public static void Effect_PaperTray_Swipe4()
    {
        _paperSounds.Effect_PaperTray_Swipe4();
    }
    #endregion

    #region Ink Sounds
    public static void Effect_Ink_SlotOpen1()
    {
        _inkSounds.Effect_Ink_SlotOpen1();
    }

    public static void Effect_Ink_SlotOpen2()
    {
        _inkSounds.Effect_Ink_SlotOpen2();
    }

    public static void Effect_Ink_SlotOpen3()
    {
        _inkSounds.Effect_Ink_SlotOpen3();
    }

    public static void Effect_Ink_SlotOpen4()
    {
        _inkSounds.Effect_Ink_SlotOpen4();
    }
    #endregion

    #region Uranium Sounds
    public static void Effect_UraniumRods_Popup1()
    {
        _rodSounds.Effect_UraniumRods_Popup1();
    }

    public static void Effect_UraniumRods_Popup2()
    {
        _rodSounds.Effect_UraniumRods_Popup2();
    }

    public static void Effect_UraniumRods_Popup3()
    {
        _rodSounds.Effect_UraniumRods_Popup3();
    }

    public static void Effect_UraniumRods_Hammer()
    {
        _rodSounds.Effect_UraniumRods_Hammer();
    }
    #endregion

    #region Barometer Sounds
    public static void Effect_Barometer_Tap1()
    {
        _baroSounds.Effect_Barometer_Tap1();
    }

    public static void Effect_Barometer_Tap2()
    {
        _baroSounds.Effect_Barometer_Tap2();
    }

    public static void Effect_Barometer_Tap3()
    {
        _baroSounds.Effect_Barometer_Tap3();
    }
    #endregion

    #region Voice
    public static void Voice_Boss_1()
    {
        _voiceSounds.Voice_Boss_1();
    }

    public static void Voice_Boss_2()
    {
        _voiceSounds.Voice_Boss_2();
    }

    public static void Voice_Boss_3()
    {
        _voiceSounds.Voice_Boss_3();
    }

    public static void Voice_Boss_7()
    {
        _voiceSounds.Voice_Boss_7();
    }

    public static void Voice_Boss_8()
    {
        _voiceSounds.Voice_Boss_8();
    }

    public static void Voice_Boss_9()
    {
        _voiceSounds.Voice_Boss_9();
    }

    #endregion
}
