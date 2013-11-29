using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    #region Editor Publics
    [SerializeField] private static float _fadeTime = 1.0f;
    [SerializeField] private static float _endMusicVolume = 0.2f;
    #endregion

    #region Privates
    private static bool _isFaded = false;
    private static bool _allSoundOn = false;

    private static InGameSounds _inGameSounds;
    private static MainMenuSounds _mainMenuSounds;
    private static PaperSounds _paperSounds;
    private static InkSounds _inkSounds;
    private static RodSounds _rodSounds;
    private static VoiceSounds _voiceSounds;
    private static MachineSounds _machineSounds;

    private static GenericSoundScript[] _audioScripts = new GenericSoundScript[30];
    private static List<GenericSoundScript> _audioScriptList = new List<GenericSoundScript>();

    private static List<float> _audioVolume = new List<float>();

    private static List<GenericSoundScript> _soundFxScripts = new List<GenericSoundScript>();
    private static List<GenericSoundScript> _musicScripts = new List<GenericSoundScript>();

    private static GameObject _audioRelay;
    #endregion

    #region MonoBehavior
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _audioRelay = GameObject.FindGameObjectWithTag("AudioRelay");

        if(_audioRelay != null && _audioRelay != gameObject)
        {
            Debug.Log("DELETING NEW SOUND SOURCE");
            Destroy(gameObject);
        }

        if(PlayerPrefs.HasKey("Sound"))
        {
            if(PlayerPrefs.GetString("Sound") == "On")
            {
                _allSoundOn = false;
            }

            else if(PlayerPrefs.GetString("Sound") == "Off")
            {
                _allSoundOn = true;
            }
        }

        _inGameSounds = transform.FindChild("In Game").GetComponent<InGameSounds>();
        _mainMenuSounds = transform.FindChild("Main Menu").GetComponent<MainMenuSounds>();
        _paperSounds = transform.FindChild("PaperTray").GetComponent<PaperSounds>();
        _inkSounds = transform.FindChild("Ink").GetComponent<InkSounds>();
        _rodSounds = transform.FindChild("Uranium Rods").GetComponent<RodSounds>();
        _voiceSounds = transform.FindChild("Voice").GetComponent<VoiceSounds>();
        _machineSounds = transform.FindChild("Machine").GetComponent<MachineSounds>();
    }

    void Start()
    {
        _audioScripts = FindObjectsOfType(typeof(GenericSoundScript)) as GenericSoundScript[];

        for(int i = 0; i <_audioScripts.Length; i++)
        {
            _audioScriptList.Add(_audioScripts[i]);
            _audioVolume.Add(_audioScripts[i].GetVolume());
        }

        _soundFxScripts.Add(_inkSounds.GetEffectScript());
        _soundFxScripts.Add(_machineSounds.GetEffectScriptCogwheels());
        _soundFxScripts.Add(_machineSounds.GetEffectScriptMachine());
        _soundFxScripts.Add(_machineSounds.GetEffectScriptSmoke());
        _soundFxScripts.Add(_paperSounds.GetEffectScript());
        _soundFxScripts.Add(_rodSounds.GetEffectScript());
        _soundFxScripts.Add(_mainMenuSounds.GetEffectScript());
        _soundFxScripts.Add(_voiceSounds.GetEffectScript());

        _musicScripts.Add(_inGameSounds.GetMusicScript());
        _musicScripts.Add(_mainMenuSounds.GetMusicScript());

        _mainMenuSounds.GetMusicScript().audio.ignoreListenerPause = true;
        _mainMenuSounds.GetEffectScript().audio.ignoreListenerPause = true;
        _inGameSounds.GetMusicScript().audio.ignoreListenerPause = true;

        ToogleAudio();
    }

    void Update()
    {
        if(Application.isLoadingLevel)
        {
            StopAllSoundSources();
            FadeAllSourcesUp();
        }
    }
    #endregion

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

    public static void Voice_Boss_4()
    {
        _voiceSounds.Voice_Boss_4();
    }

    public static void Voice_Boss_5()
    {
        _voiceSounds.Voice_Boss_5();
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

    public static void Effect_InGame_Task_Failed()
    {
        _machineSounds.Effect_Machine_TaskMissed();
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

    #region General Methods
    public static void FadeAllSourcesUp()
    {
        foreach(GenericSoundScript gss in _audioScripts)
        {
            int index = _audioScriptList.IndexOf(gss);
            gss.FadeVolume(_audioVolume[index], _fadeTime);
        }
    }

    public static void FadeAllSources(float volume)
    {
        foreach(GenericSoundScript gss in _audioScripts)
        {
            int index = _audioScriptList.IndexOf(gss);
            gss.FadeVolume(volume, _fadeTime);
        }
    }

    public static void StoreVolumes()
    {
        foreach(GenericSoundScript gss in _audioScripts)
        {
            int index = _audioScriptList.IndexOf(gss);
            _audioVolume[index] = gss.GetVolume();
            gss.SetVolume(0.0f);
        }

//        FadeAllMusic();
//        StopAllSoundEffects();
    }

    public static void StopAllSoundEffects()
    {
        foreach(GenericSoundScript gss in _soundFxScripts)
        {
            gss.SetVolume(0.0f);
        }
    }

    public static void FadeAllMusic()
    {
        foreach(GenericSoundScript gss in _musicScripts)
        {
            gss.FadeVolume(_endMusicVolume, _fadeTime);
        }
    }

    public static void ToogleAudio()
    {
        if(_allSoundOn)
        {
            _allSoundOn = false;
            foreach(GenericSoundScript gss in _audioScripts)
            {
                gss.audio.mute = true;
            }
        }

        else if(!_allSoundOn)
        {
            _allSoundOn = true;
            foreach(GenericSoundScript gss in _audioScripts)
            {
                gss.audio.mute = false;
            }
        }
    }

    public static void StopAllSoundSources()
    {
        foreach(GenericSoundScript gss in _audioScripts)
        {
            gss.audio.Stop();
            gss.audio.loop = false;
        }
    }

    public static void CheckAudioToogle()
    {
        if(PlayerPrefs.HasKey("Sound"))
        {
            if(PlayerPrefs.GetString("Sound") == "On")
            {
                _allSoundOn = false;
                ToogleAudio();
            }

            else if(PlayerPrefs.GetString("Sound") == "Off")
            {
                _allSoundOn = true;
                ToogleAudio();
            }
        }
    }
    #endregion
}
