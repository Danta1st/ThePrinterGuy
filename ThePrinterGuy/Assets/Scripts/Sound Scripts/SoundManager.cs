using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    #region Editor Publics
    [SerializeField] private static float _fadeTime = 1.0f;
    [SerializeField] private static float _endMusicVolume = 0.2f;
    [SerializeField] private static float _menuEffectsVolume = 0.8f;
    [SerializeField] private static float _voiceVolume = 1.0f;
    [SerializeField] private static float _gameMusic = 0.7f;
    [SerializeField] private static float _menuMusic = 0.5f;
    #endregion

    #region Privates
    private static bool _isFaded = false;
    private static bool _allSoundOn = false;
    private static bool _hasReset = false;

    private static InGameSounds _inGameSounds;
    private static MainMenuSounds _mainMenuSounds;
    private static PaperSounds _paperSounds;
    private static InkSounds _inkSounds;
    private static RodSounds _rodSounds;
    private static VoiceSounds _voiceSounds;
    private static MachineSounds _machineSounds;
    private static CutsceneSounds _cutSceneSounds;

    private static GenericSoundScript[] _audioScripts = new GenericSoundScript[30];
    private static List<GenericSoundScript> _audioScriptList = new List<GenericSoundScript>();

    private static List<float> _audioVolume = new List<float>();
    private static List<float> _musicVolume = new List<float>();

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
        _cutSceneSounds = transform.FindChild("CutScene").GetComponent<CutsceneSounds>();
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

        for(int i = 0; i < _musicScripts.Count; i++)
        {
            _musicVolume.Add(_audioScripts[i].GetVolume());
        }

        ToogleAudio();
    }

    void Update()
    {
        if(Application.isLoadingLevel && !_hasReset)
        {
            _hasReset = true;
            StopAllSoundSources();
        }

        if(!Application.isLoadingLevel)
        {
            _hasReset = false;
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

    public static void Music_CutScene_Main()
    {
        _cutSceneSounds.CutScene_Main_Music();
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

    public static void Effect_Ink_RightSlot1()
    {
        _inkSounds.Effect_Ink_RightSlot1();
    }

    public static void Effect_Ink_RightSlot2()
    {
        _inkSounds.Effect_Ink_RightSlot2();
    }

    public static void Effect_Ink_RightSlot3()
    {
        _inkSounds.Effect_Ink_RightSlot3();
    }

    public static void Effect_Ink_RightSlot4()
    {
        _inkSounds.Effect_Ink_RightSlot4();
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
    public static void Voice_Boss_EndScene_Fired1()
    {
        _voiceSounds.Voice_Boss_EndScene_Fired1();
    }

    public static void Voice_Boss_EndScene_Fired2()
    {
        _voiceSounds.Voice_Boss_EndScene_Fired2();
    }

    public static void Voice_Boss_EndScene_Fired3()
    {
        _voiceSounds.Voice_Boss_EndScene_Fired3();
    }

    public static void Voice_Boss_EndScene_Fired4()
    {
        _voiceSounds.Voice_Boss_EndScene_Fired4();
    }

    public static void Voice_Boss_EndScene_NotFired1()
    {
        _voiceSounds.Voice_Boss_EndScene_NotFired1();
    }

    public static void Voice_Boss_EndScene_NotFired2()
    {
        _voiceSounds.Voice_Boss_EndScene_NotFired2();
    }

    public static void Voice_Boss_EndScene_NotFired3()
    {
        _voiceSounds.Voice_Boss_EndScene_NotFired3();
    }

    public static void Voice_Boss_EndScene_NotFired4()
    {
        _voiceSounds.Voice_Boss_EndScene_NotFired4();
    }

    public static void Voice_Boss_Mumbling_Arrww_1()
    {
        _voiceSounds.Voice_Boss_Mumbling_Arrww_1();
    }

    public static void Voice_Boss_Mumbling_Arrww_2()
    {
        _voiceSounds.Voice_Boss_Mumbling_Arrww_2();
    }

    public static void Voice_Boss_Mumbling_Arrww_3()
    {
        _voiceSounds.Voice_Boss_Mumbling_Arrww_3();
    }

    public static void Voice_Boss_Mumbling_Hmm_1()
    {
        _voiceSounds.Voice_Boss_Mumbling_Hmm_1();
    }

    public static void Voice_Boss_Mumbling_Hmm_2()
    {
        _voiceSounds.Voice_Boss_Mumbling_Hmm_2();
    }

    public static void Voice_Boss_Mumbling_Hmm_3()
    {
        _voiceSounds.Voice_Boss_Mumbling_Hmm_3();
    }

    public static void Voice_Boss_Mumbling_Hrn_1()
    {
        _voiceSounds.Voice_Boss_Mumbling_Hrn_1();
    }

    public static void Voice_Boss_Mumbling_Hrn2()
    {
        _voiceSounds.Voice_Boss_Mumbling_Hrn2();
    }

    public static void Voice_Boss_Mumbling_Hrn_3()
    {
        _voiceSounds.Voice_Boss_Mumbling_Hrn_3();
    }

    public static void Voice_Boss_Mumbling_Mumble_1()
    {
        _voiceSounds.Voice_Boss_Mumbling_Mumble_1();
    }

    public static void Voice_Boss_Mumbling_Mumble_2()
    {
        _voiceSounds.Voice_Boss_Mumbling_Mumble_2();
    }

    public static void Voice_Boss_Mumbling_Mumble_3()
    {
        _voiceSounds.Voice_Boss_Mumbling_Mumble_3();
    }

    public static void Voice_Boss_Mumbling_No_1()
    {
        _voiceSounds.Voice_Boss_Mumbling_No_1();
    }

    public static void Voice_Boss_Mumbling_Ohh_1()
    {
        _voiceSounds.Voice_Boss_Mumbling_Ohh_1();
    }

    public static void Voice_Boss_Angry_FireYou_1()
    {
        _voiceSounds.Voice_Boss_Angry_FireYou_1();
    }

    public static void Voice_Boss_Angry_FireYou_2()
    {
        _voiceSounds.Voice_Boss_Angry_FireYou_2();
    }

    public static void Voice_Boss_Angry_GiveUp_1()
    {
        _voiceSounds.Voice_Boss_Angry_GiveUp_1();
    }

    public static void Voice_Boss_Angry_GiveUp_2()
    {
        _voiceSounds.Voice_Boss_Angry_GiveUp_2();
    }

    public static void Voice_Boss_Angry_GiveUp_3()
    {
        _voiceSounds.Voice_Boss_Angry_GiveUp_3();
    }

    public static void Voice_Boss_Angry_Idiot_1()
    {
        _voiceSounds.Voice_Boss_Angry_Idiot_1();
    }

    public static void Voice_Boss_Angry_Idiot_2()
    {
        _voiceSounds.Voice_Boss_Angry_Idiot_2();
    }

    public static void Voice_Boss_Angry_Idiot_3()
    {
        _voiceSounds.Voice_Boss_Angry_Idiot_3();
    }

    public static void Voice_Boss_Angry_Idiot_4()
    {
        _voiceSounds.Voice_Boss_Angry_Idiot_4();
    }

    public static void Voice_Boss_Happy_Bravo_1()
    {
        _voiceSounds.Voice_Boss_Happy_Bravo_1();
    }

    public static void Voice_Boss_Happy_Bravo_2()
    {
        _voiceSounds.Voice_Boss_Happy_Bravo_2();
    }

    public void Voice_Boss_Happy_Bravo_3()
    {
        _voiceSounds.Voice_Boss_Happy_Bravo_3();
    }

    public static void Voice_Boss_Happy_KeepGoing_1()
    {
        _voiceSounds.Voice_Boss_Happy_KeepGoing_1();
    }

    public static void Voice_Boss_Happy_KeepGoing_2()
    {
        _voiceSounds.Voice_Boss_Happy_KeepGoing_2();
    }

    public static void Voice_Boss_Happy_KeepGoing_3()
    {
        _voiceSounds.Voice_Boss_Happy_KeepGoing_3();
    }

    public static void Voice_Boss_Happy_Know_1()
    {
        _voiceSounds.Voice_Boss_Happy_Know_1();
    }

    public static void Voice_Boss_Happy_Know_2()
    {
        _voiceSounds.Voice_Boss_Happy_Know_2();
    }

    public static void Voice_Boss_Happy_NoRaise_1()
    {
        _voiceSounds.Voice_Boss_Happy_NoRaise_1();
    }

    public static void Voice_Boss_Happy_NotBad_1()
    {
        _voiceSounds.Voice_Boss_Happy_NotBad_1();
    }

    public static void Voice_Boss_Happy_NotBad_2()
    {
        _voiceSounds.Voice_Boss_Happy_NotBad_2();
    }

    public static void Voice_Boss_Random_Happy()
    {
        _voiceSounds.Voice_Boss_Random_Happy();
    }

    public static void Voice_Boss_Random_Angry()
    {
        _voiceSounds.Voice_Boss_Random_Angry();
    }

    public static void Voice_Boss_Random_Mumbling()
    {
        _voiceSounds.Voice_Boss_Random_Mumbling();
    }

    public static void Voice_Boss_Random_WinEnd()
    {
        _voiceSounds.Voice_Boss_Random_WinEnd();
    }

    public static void Voice_Boss_Random_LoseEnd()
    {
        _voiceSounds.Voice_Boss_Random_LoseEnd();
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

    public static void Effect_InGame_Task_Unmatched()
    {
        _mainMenuSounds.Effect_Task_Unmatched();
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

    public static void Effect_Task_Unmatched()
    {
        _mainMenuSounds.Effect_Task_Unmatched();
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

    public static void UnFadeAllMusic()
    {
//        foreach(GenericSoundScript gss in _musicScripts)
//        {
//            gss.FadeVolume(_startMusicVolume, _fadeTime);
//        }

//        for(int i = 0; i < _musicScripts.Count; i++)
//        {
//            _musicScripts[i].FadeVolume(_musicVolume[i], _fadeTime);
//        }

        _inGameSounds.GetMusicScript().FadeVolume(_gameMusic, _fadeTime);
        _mainMenuSounds.GetMusicScript().FadeVolume(_menuMusic, _fadeTime);
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

    public static void StopPointSound()
    {
        _cutSceneSounds.GetEffectsScript().LoopClipStop();
    }

    public static void TurnOnMenuSounds()
    {
        _mainMenuSounds.GetEffectScript().SetVolume(_menuEffectsVolume);
    }

    public static void TurnOnVoice()
    {
        _voiceSounds.GetEffectScript().SetVolume(_voiceVolume);
    }
    #endregion

    #region Dialogue Methods
    public static void Voice_Boss_Random_FireYou()
    {
        _voiceSounds.Voice_Boss_Random_FireYou();
    }

    public static void Voice_Boss_Random_GiveUp()
    {
        _voiceSounds.Voice_Boss_Random_GiveUp();
    }

    public static void Voice_Boss_Random_Idiot()
    {
        _voiceSounds.Voice_Boss_Random_Idiot();
    }

    public static void Voice_Boss_Random_Bravo()
    {
        _voiceSounds.Voice_Boss_Random_Bravo();
    }

    public static void Voice_Boss_Random_KeepGoing()
    {
        _voiceSounds.Voice_Boss_Random_KeepGoing();
    }

    public static void Voice_Boss_Random_Know()
    {
        _voiceSounds.Voice_Boss_Random_Know();
    }

    public static void Voice_Boss_Random_NotBad()
    {
        _voiceSounds.Voice_Boss_Random_NotBad();
    }
    #endregion

    #region Cutscene Sounds
    public static void CutScene_Effect_Coffee_01()
    {
        _cutSceneSounds.CutScene_Effect_Coffee_01();
    }

    public static void CutScene_Effect_Point()
    {
        _cutSceneSounds.CutScene_Effect_Point();
    }

    public static void CutScene_Effect_Coffee_02()
    {
        _cutSceneSounds.CutScene_Effect_Coffee_02();
    }

    public static void CutScene_Random_Coffee()
    {
        _cutSceneSounds.CutScene_Random_Coffee();
    }
    #endregion
}
