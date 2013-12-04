using UnityEngine;
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
    private float _quoteWaitTime = 7;
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
    private bool _canTrigger = false;
    private bool _canTriggerQuote = true;
	private bool _inHappyZone = false;
	private bool _inAngryZone = false;
    private bool _isDead = false;

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

    private GUIGameCamera _guiGameCamScript;
    #endregion
    
	#region Delegates & Events
    public delegate void GameFailed(int _score);
    public static event GameFailed OnGameFailed;

    public delegate void AngryZoneTriggered();
    public static event AngryZoneTriggered OnAngryZoneEntered;

    public delegate void HappyZoneTriggered();
    public static event HappyZoneTriggered OnHappyZoneEntered;
	
	public delegate void HappyZoneEntered();
    public static event HappyZoneEntered OnHappyZone;
	
	public delegate void AngryZoneEntered();
    public static event AngryZoneEntered OnAngryZone;
	
	public delegate void ZoneLeft();
	public static event ZoneLeft OnZoneLeft;

    public delegate void StressIncreaseAction();
    public static event StressIncreaseAction OnStressIncrease;

    public delegate void StressDecreaseAction();
    public static event StressIncreaseAction OnStressDecrease;
	#endregion

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

        BpmSequencerItem.OnFailed += ReductPointsFailed;
    }

    void OnDisable()
    {
        Ink.OnCorrectInkInserted -= UpdateInkCounts;
        PaperInsertion.OnCorrectPaperInserted -= UpdatePaperCounts;
        Barometer.OnBarometerFixed -= UpdateBaroCounts;
        UraniumRods.OnRodHammered -= UpdateRodCounts;

//        PathManager.OnCamPosChangedBeginAction -= GiveNewPanLevel;

        BpmSequencerItem.OnFailed -= ReductPointsFailed;
    }
    #endregion

    //ToDO: Make the functions give a small boost over time / Subtract the needle over time, instead of doing instantly

    #region Give Points
    void GivePoints()
    {
        if(OnStressIncrease != null)
            OnStressIncrease();
        _rotationScale -= _inZonePoints;
        UpdateRotation();
    }
    #endregion

    #region Reduct Point
    public void ReductPointsFailed()
    {
        SoundManager.Effect_InGame_Task_Unmatched();
        if(OnStressDecrease != null)
            OnStressDecrease();
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
    #endregion

    #region Public get methods for perfect score
    public float GetHappyZone()
    {
        return _happyZone;
    }

    public float GetZonePoints()
    {
        return _inZonePoints;
    }
    #endregion

    #region Needlemovement functions
    void UpdateRotation()
    {
        _canTrigger = true;

        _rotationScale = Mathf.Clamp(_rotationScale, _stressMIN, _stressMAX);

        float thisRotationScale = 360.0f + _rotationScale;

        _thisRotation.z = thisRotationScale;

        iTween.Stop(gameObject);

        iTween.RotateTo(gameObject, iTween.Hash("rotation", _thisRotation, "time", _rotationTime, "easetype", iTween.EaseType.easeOutBack, "islocal", true,
            "oncomplete", "SlightMovement", "oncompletetarget", gameObject));
    }

    void SlightMovement()
    {
        if(OnGameFailed != null && _rotationScale == _stressMAX && !_isDead)
        {
            _isDead = true;
            OnGameFailed(GameObject.Find("GUI List").GetComponent<GUIGameCamera>().GetScore());
        }

		if((_happyZone < _rotationScale && _inHappyZone) || (_rotationScale < _angryZone && _inAngryZone) && _canTrigger)
		{
			_inHappyZone = false;
			_inAngryZone = false;

			if(OnZoneLeft != null)
			{
				OnZoneLeft();
			}
		}
		else if(_rotationScale <= _happyZone && !_inHappyZone && _canTrigger)
		{
			_inHappyZone = true;
			
			if(OnHappyZone != null)
			{
				OnHappyZone();
			}
		}
		else if(_rotationScale >= _angryZone && !_inAngryZone && _canTrigger)
		{
			_inAngryZone = true;

			if(OnAngryZone != null)
			{
				OnAngryZone();
			}
		}
		
		_canTrigger = false;
		
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
        yield return new WaitForSeconds(_quoteWaitTime);
        _canTriggerQuote = true;
    }

    void SlightMovementBack()
    {
        iTween.RotateTo(gameObject, iTween.Hash("rotation", _thisRotation, "time", _shakeTime, "oncomplete", "SlightMovement", "oncompletetarget", gameObject));
    }
    #endregion
}
