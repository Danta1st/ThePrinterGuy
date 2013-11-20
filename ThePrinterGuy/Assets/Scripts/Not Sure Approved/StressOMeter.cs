﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StressOMeter : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _inZonePoints = 7.0f;
    [SerializeField]
    private float _failedPoints = -15.0f;
    [SerializeField]
    private float _quoteWaitTime = 10.0f;

    /*Paper
     *Ink
     *Uranium Rods
     *(Barometer)
     */
//    [SerializeField]
//    private List<GameObject> _audioObjects = new List<GameObject>();
    #endregion

    #region Privates#
    private float _rotationScale = 0.0f;
    private float _stressMIN = -60.0f;
    private float _stressMAX = 60.0f;
    private float _angryZone = 30.0f;
    private float _happyZone = -30.0f;
    private Vector3 _thisRotation;
    private float _rotationTime = 0.5f;
    private Vector3 _shakeRotation;
    private float _shakeTime = 0.05f;
    private bool _canDie = false;
    private bool _canTriggerQuote = true;

    private bool _paperCounter = false;
    private bool _inkCounter = false;
    private bool _uraniumCounter = false;
    private bool _barometerCounter = false;

    private float _paperAudioStreak = 0.0f;
    private float _inkAudioStreak = 0.0f;
    private float _rodAudioStreak = 0.0f;
    private float _barometerAudioStreak = 0.0f;

    private float _paperStreak = 0.0f;
    private float _inkStreak = 0.0f;
    private float _rodStreak = 0.0f;
    private float _barometerStreak = 0.0f;

    private float _audioStreak = 0.0f;
    private float _audioPanLevel = 0.0f;
    private int _panLevelIndex = 0;

    private List<float> _streakArray = new List<float>();

    private GUIGameCamera _guiGameCamScript;
    #endregion
    
    public delegate void GameFailed();
    public static event GameFailed OnGameFailed;

    public delegate void AngryZoneEntered();
    public static event AngryZoneEntered OnAngryZoneEntered;

    public delegate void HappyZoneEntered();
    public static event HappyZoneEntered OnHappyZoneEntered;

    void Start()
    {
        //ToDo: Find out if colorHit is an outdated variable or if it is renamed.
        _guiGameCamScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
        _thisRotation = new Vector3(0.0f, 0.0f, 0.0f);
        _shakeRotation = new Vector3(0.0f, 0.0f, 1.0f);
        SlightMovement();
    }

    void Update()
    {

    }

    #region MonoBehaviour
    void OnEnable()
    {
        Ink.OnCorrectInkInserted += UpdateInkCounts;
        PaperInsertion.OnCorrectPaperInserted += UpdatePaperCounts;
        Barometer.OnBarometerFixed += UpdateBaroCounts;
        UraniumRods.OnRodHammered += UpdateRodCounts;

//        PathManager.OnCamPosChangedBeginAction += GiveNewPanLevel;

        ActionSequencerItem.OnFailed += ReductPointsFailed;
    }

    void OnDisable()
    {
        Ink.OnCorrectInkInserted -= UpdateInkCounts;
        PaperInsertion.OnCorrectPaperInserted -= UpdatePaperCounts;
        Barometer.OnBarometerFixed -= UpdateBaroCounts;
        UraniumRods.OnRodHammered -= UpdateRodCounts;

//        PathManager.OnCamPosChangedBeginAction -= GiveNewPanLevel;

        ActionSequencerItem.OnFailed -= ReductPointsFailed;
    }
    #endregion

    //ToDO: Make the functions give a small boost over time / Subtract the needle over time, instead of doing instantly

    #region Give Points
    void GivePoints()
    {
        _rotationScale -= _inZonePoints;
        UpdateRotation();
    }
    #endregion

    #region Reduct Point
    void ReductPointsFailed()
    {
         _paperAudioStreak = 0.0f;
        _rotationScale -= _failedPoints;
        UpdateRotation();
    }
    #endregion

    #region Counting CRAP
    private void StreakReset()
    {
        _paperCounter = false;
        _inkCounter = false;
        _uraniumCounter = false;
        _barometerCounter = false;

        _paperAudioStreak = 0.0f;
        _inkAudioStreak = 0.0f;
        _rodAudioStreak = 0.0f;
        _barometerAudioStreak = 0.0f;
    }

    private void UpdatePaperCounts()
    {
        int colorHit = _guiGameCamScript.GetZone();

        switch(colorHit)
        {
        case 0:
                //Bad
            break;
        case 1:
                //almost bad
            break;
        case 2:
                //Almost perfect
            break;
        case 3:
                //Perfect

            if(!_paperCounter)
            {
                StreakReset();
            }

            _paperCounter = true;
            _paperAudioStreak++;
            GivePoints();

            break;
        default:
            break;
        }

        _paperStreak++;
        _inkStreak = 0.0f;
        _rodStreak = 0.0f;
        _barometerStreak = 0.0f;
    }

    private void UpdateInkCounts()
    {
        int colorHit = _guiGameCamScript.GetZone();

        switch(colorHit)
        {
        case 0:
                //Bad
            break;
        case 1:
                //almost bad
            break;
        case 2:
                //Almost perfect
            break;
        case 3:
                //Perfect

            if(!_inkCounter)
            {
                StreakReset();
            }

            _inkCounter = true;
            _inkAudioStreak++;
            GivePoints();

            break;
        default:
            break;
        }

        _paperStreak = 0.0f;
        _inkStreak++;
        _rodStreak = 0.0f;
        _barometerStreak = 0.0f;
    }

    private void UpdateRodCounts()
    {
        int colorHit = _guiGameCamScript.GetZone();

        switch(colorHit)
        {
        case 0:
                //Bad
            break;
        case 1:
                //almost bad
            break;
        case 2:
                //Almost perfect
            break;
        case 3:
                //Perfect

            if(!_uraniumCounter)
            {
                StreakReset();
            }

            _uraniumCounter = true;
            _rodAudioStreak++;
            GivePoints();

            break;
        default:
            break;
        }

        _paperStreak = 0.0f;
        _inkStreak = 0.0f;
        _rodStreak++;
        _barometerStreak = 0.0f;
    }

    private void UpdateBaroCounts()
    {
        int colorHit = _guiGameCamScript.GetZone();

        switch(colorHit)
        {
        case 0:
                //Bad
            break;
        case 1:
                //almost bad
            break;
        case 2:
                //Almost perfect
            break;
        case 3:
                //Perfect

            if(!_barometerCounter)
            {
                StreakReset();
            }

            _barometerCounter = true;
            _barometerAudioStreak++;
            GivePoints();

            break;
        default:
            break;
        }

        _paperStreak = 0.0f;
        _inkStreak = 0.0f;
        _rodStreak = 0.0f;
        _barometerStreak++;
    }

    private void ResetAudioCount()
    {
        _audioStreak = 0.0f;
    }

    private void AddAudioCount()
    {
        _audioStreak++;
    }


//    private float CalculateAudioStreak()
//    {
//        _streakArray.Add(_paperStreak);
//        _streakArray.Add(_inkStreak);
//        _streakArray.Add(_rodStreak);
//        _streakArray.Add(_barometerStreak);
//
//        for(int i = 0; i < _streakArray.Count; i++)
//        {
//            if(_audioPanLevel < _streakArray[i])
//            {
//                _audioPanLevel = _streakArray[i];
//                _panLevelIndex = i;
//            }
//        }
//
//        _audioPanLevel = 1 - _audioPanLevel;
//
//        return _audioPanLevel;
//    }
//
//    //ToDO: HookUp with event from PathManager... && Update other related brances and merge with this...
//    private void GiveNewPanLevel()
//    {
//        GenericSoundScript thisAudioScript = _audioObjects[_panLevelIndex].GetComponent<GenericSoundScript>();
////        thisAudioScript.SetPanLevel()CalculateAudioStreak());
//
//        StreakReset();
//
//        _paperStreak = 0;
//        _inkStreak = 0;
//        _rodStreak = 0;
//        _barometerStreak = 0;
//    }
    #endregion

    #region Needlemovement functions
    void UpdateRotation()
    {
        _canDie = true;

        _rotationScale = Mathf.Clamp(_rotationScale, _stressMIN, _stressMAX);

        float thisRotationScale = 360.0f + _rotationScale;

        _thisRotation.z = thisRotationScale;

        iTween.Stop(gameObject);

        iTween.RotateTo(gameObject, iTween.Hash("rotation", _thisRotation, "time", _rotationTime, "easetype", iTween.EaseType.linear, "islocal", true,
            "oncomplete", "SlightMovement", "oncompletetarget", gameObject));
    }

    void SlightMovement()
    {
        if(OnGameFailed != null && _rotationScale == _stressMAX && _canDie)
        {
            OnGameFailed();
        }

        _canDie = false;

        if(_rotationScale >= _angryZone && _canTriggerQuote)
        {
            _canTriggerQuote = false;

            if(OnAngryZoneEntered != null)
            {
                OnAngryZoneEntered();
            }

            StartCoroutine(QuoteWaitTime());
        }

        if(_rotationScale <= _happyZone && _canTriggerQuote)
        {
            _canTriggerQuote = false;

            if(OnHappyZoneEntered != null)
            {
                OnHappyZoneEntered();
            }

            StartCoroutine(QuoteWaitTime());
        }

        iTween.ShakeRotation(gameObject, iTween.Hash("amount", _shakeRotation, "time", _shakeTime, "oncomplete", "SlightMovementBack", "oncompletetarget", gameObject));
    }

    IEnumerator QuoteWaitTime()
    {
        yield return new WaitForSeconds(2.0f);
        _canTriggerQuote = true;
    }

    void SlightMovementBack()
    {
        iTween.RotateTo(gameObject, iTween.Hash("rotation", _thisRotation, "time", _shakeTime, "oncomplete", "SlightMovement", "oncompletetarget", gameObject));
    }
    #endregion
}