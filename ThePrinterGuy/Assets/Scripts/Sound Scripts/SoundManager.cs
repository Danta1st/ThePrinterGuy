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

    private static MachineSounds _machineSounds;

    void Awake()
    {
        _inGameSounds = transform.FindChild("In Game").GetComponent<InGameSounds>();
        _mainMenuSounds = transform.FindChild("Main Menu").GetComponent<MainMenuSounds>();

        _paperSounds = transform.FindChild("PaperTray").GetComponent<PaperSounds>();
        _inkSounds = transform.FindChild("Ink").GetComponent<InkSounds>();
        _rodSounds = transform.FindChild("Uranium Rods").GetComponent<RodSounds>();
        _baroSounds = transform.FindChild("BaroMeter").GetComponent<BaroSounds>();

        _voiceSounds = transform.FindChild("Voice").GetComponent<VoiceSounds>();

        _machineSounds = transform.FindChild("Machine").GetComponent<MachineSounds>();
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
    public static void PaperTray_RaiseVolume()
    {
        _paperSounds.RaiseVolume();
    }

    public static void PaperTray_LowerVolume()
    {
        _paperSounds.LowerVolume();
    }

    public static void Effect_PaperTray_MoveUp()
    {
        _paperSounds.Effect_PaperTray_MoveUp();
    }

    public static void Effect_PaperTray_MoveDown()
    {
        _paperSounds.Effect_PaperTray_MoveDown();
    }

    public static void Effect_PaperTray_ConveyorBelt()
    {
        _paperSounds.Effect_PaperTray_ConveyorBelt();
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

    public static void Effect_PaperTray_WrongSwipe()
    {
        _paperSounds.Effect_PaperTray_WrongSwipe();
    }
    #endregion

    #region Ink Sounds
    public static void Ink_RaiseVolume()
    {
        _inkSounds.RaiseVolume();
    }

    public static void Ink_LowerVolume()
    {
        _inkSounds.LowerVolume();
    }

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

    public static void Effect_Ink_RightSlot()
    {
        _inkSounds.Effect_Ink_RightSlot();
    }

    public static void Effect_Ink_WrongSlot()
    {
        _inkSounds.Effect_Ink_WrongSlot();
    }

    public static void Effect_Ink_SlotClose1()
    {
        _inkSounds.Effect_Ink_SlotClose1();
    }

    public static void Effect_Ink_SlotClose2()
    {
        _inkSounds.Effect_Ink_SlotClose2();
    }

    public static void Effect_Ink_SlotClose3()
    {
        _inkSounds.Effect_Ink_SlotClose3();
    }

    public static void Effect_Ink_SlotClose4()
    {
        _inkSounds.Effect_Ink_SlotClose4();
    }
    #endregion

    #region Uranium Sounds
    public static void UraniumRods_RaiseVolume()
    {
        _rodSounds.RaiseVolume();
    }

    public static void UraniumRods_LowerVolume()
    {
        _rodSounds.LowerVolume();
    }

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

    public static void Effect_UraniumRods_Popup4()
    {
        _rodSounds.Effect_UraniumRods_Popup4();
    }

    public static void Effect_UraniumRods_Hammer()
    {
        _rodSounds.Effect_UraniumRods_Hammer();
    }
    #endregion

    #region Barometer Sounds
    public static void Barometer_RaiseVolume()
    {
        _baroSounds.RaiseVolume();
    }

    public static void Barometer_LowerVolume()
    {
        _baroSounds.LowerVolume();
    }

    public static void Effect_Barometer_NormSpin1()
    {
        _baroSounds.Effect_Barometer_NormSpin1();
    }

    public static void Effect_Barometer_NormSpin2()
    {
        _baroSounds.Effect_Barometer_NormSpin2();
    }

    public static void Effect_Barometer_NormSpin3()
    {
        _baroSounds.Effect_Barometer_NormSpin3();
    }

    public static void Effect_Barometer_NokOkSpin1()
    {
        _baroSounds.Effect_Barometer_NokOkSpin1();
    }

    public static void Effect_Barometer_NokOkSpin2()
    {
        _baroSounds.Effect_Barometer_NokOkSpin2();
    }

    public static void Effect_Barometer_NokOkSpin3()
    {
        _baroSounds.Effect_Barometer_NokOkSpin3();
    }

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

    #region Voice Samples
    public static void Voice_Boos_Random_Happy()
    {
        _voiceSounds.Voice_Boss_Random_Happy();
    }

    public static void Voice_Boos_Random_Angry()
    {
        _voiceSounds.Voice_Boss_Random_Angry();
    }

    public static void Voice_Boss_1()
    {
        _voiceSounds.Voice_Boss_1();
    }

    public static void Voice_Boss_2()
    {
        _voiceSounds.Voice_Boss_2();
    }

    public static void Voice_Boss_6()
    {
        _voiceSounds.Voice_Boss_6();
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

    public static void Voice_Boss_11()
    {
        _voiceSounds.Voice_Boss_11();
    }

    public static void Voice_Boss_12()
    {
        _voiceSounds.Voice_Boss_12();
    }

    public static void Voice_Boss_13()
    {
        _voiceSounds.Voice_Boss_13();
    }

    public static void Voice_Boss_14()
    {
        _voiceSounds.Voice_Boss_14();
    }

    public static void Voice_Boss_15()
    {
        _voiceSounds.Voice_Boss_15();
    }

    public static void Voice_Boss_16()
    {
        _voiceSounds.Voice_Boss_16();
    }

    public static void Voice_Boss_17()
    {
        _voiceSounds.Voice_Boss_17();
    }

    public static void Voice_Boss_18()
    {
        _voiceSounds.Voice_Boss_18();
    }

    public static void Voice_Boss_19()
    {
        _voiceSounds.Voice_Boss_19();
    }

    public static void Voice_Boss_Idle()
    {
        _voiceSounds.Voice_Boss_Idle();
    }

    public static void Voice_Boss_ForwardsMovement()
    {
        _voiceSounds.Voice_Boss_ForwardsMovement();
    }

    public static void Voice_Boss_BackwardsMovement()
    {
        _voiceSounds.Voice_Boss_BackwardsMovement();
    }
   #endregion

    #region Machine Sounds
    public static void Effect_Machine_Cogwheels1()
    {
        _machineSounds.Effect_Machine_Cogwheels1();
    }

    public static void Effect_Machine_Cogwheels2()
    {
        _machineSounds.Effect_Machine_Cogwheels2();
    }

    public static void Effect_Machine_Cogwheels3()
    {
        _machineSounds.Effect_Machine_Cogwheels3();
    }

    public static void Effect_Machine_Electricity()
    {
        _machineSounds.Effect_Machine_Electricity();
    }

    public static void Effect_Machine_Smoke1()
    {
        _machineSounds.Effect_Machine_Smoke1();
    }

    public static void Effect_Machine_Smoke2()
    {
        _machineSounds.Effect_Machine_Smoke2();
    }

    public static void Effect_Machine_Smoke3()
    {
        _machineSounds.Effect_Machine_Smoke3();
    }
    #endregion

    #region In Game Sounds
    public static void Effect_InGame_Win()
    {
        _inGameSounds.Effect_InGame_Win();
    }

    public static void Effect_InGame_Lose()
    {
        _inGameSounds.Effect_InGame_Lose();
    }
    #endregion

    #region Menu Sounds
    public static void Effect_Menu_Intro()
    {
        _mainMenuSounds.Effect_Menu_Intro();
    }

    public static void Effect_Menu_Click()
    {
        _mainMenuSounds.Effect_Menu_Click();
    }

    public static void Effect_Menu_Stinger()
    {
        _mainMenuSounds.Effect_Menu_Stinger();
    }

    public static void Effect_Menu_Appear()
    {
        _mainMenuSounds.Effect_Menu_Appear();
    }

    public static void Effect_Menu_Disappear()
    {
        _mainMenuSounds.Effect_Menu_Disappear();
    }

    public static void Effect_Menu_Footsteps()
    {
        _mainMenuSounds.Effect_Menu_Footsteps();
    }

    public static void Effect_Menu_Credits()
    {
        _mainMenuSounds.Effect_Menu_Credits();
    }

    public static void Effect_Menu_Options()
    {
        _mainMenuSounds.Effect_Menu_Options();
    }
    #endregion
}
