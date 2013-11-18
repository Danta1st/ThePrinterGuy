using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour {

    #region Editor Publics
    [SerializeField] private float _transitionTime = 2.0f;
    [SerializeField] private iTween.EaseType _easeType = iTween.EaseType.easeInExpo;

    [SerializeField] private Transform _uraniumFocus;
    [SerializeField] private Transform _inkFocus;
    [SerializeField] private Transform _barometerFocus;
    [SerializeField] private Transform _paperFocus;
	
	[SerializeField] private Transform _overviewFocus;
    #endregion

    #region Privates
    private float _lookTargetDelay;
    private Transform _lookingAt;
    private bool _isMoving = false;
    #endregion

    //TODO: Make Proper Connectivity to whatever it needs to connect to
    void OnEnable()
    {
//        GestureManager.OnSwipeRight += TriggerMoveBarometer;
//        GestureManager.OnSwipeLeft += TriggerMovePaper;
//        GestureManager.OnSwipeUp += TriggerMoveInk;
//        GestureManager.OnSwipeDown += TriggerMoveUranium;
		ActionSequencerManager.OnBarometerNode += TriggerMoveBarometer;
		ActionSequencerManager.OnInkNode += TriggerMoveInk;
		ActionSequencerManager.OnPaperNode += TriggerMovePaper;
		ActionSequencerManager.OnUraniumRodNode += TriggerMoveUranium;
    }
    void OnDisable()
    {
//        GestureManager.OnSwipeRight -= TriggerMoveBarometer;
//        GestureManager.OnSwipeLeft -= TriggerMovePaper;
//        GestureManager.OnSwipeUp -= TriggerMoveInk;
//        GestureManager.OnSwipeDown -= TriggerMoveUranium;
		ActionSequencerManager.OnBarometerNode -= TriggerMoveBarometer;
		ActionSequencerManager.OnInkNode -= TriggerMoveInk;
		ActionSequencerManager.OnPaperNode -= TriggerMovePaper;
		ActionSequencerManager.OnUraniumRodNode -= TriggerMoveUranium;
    }

    #region Monohevaiour Methods
    void Start()
    {
        _lookTargetDelay = _transitionTime / 2;
    }
    #endregion

    #region Class Methods
	private void TriggerMoveBegin()
	{
		if(_lookingAt != null)
		{
			if(_lookingAt == _paperFocus)
				MoveReversed("BeginPaper", _overviewFocus);
			else if(_lookingAt == _uraniumFocus)
				MoveReversed("BeginUranium", _overviewFocus);
			else if(_lookingAt == _barometerFocus)
				MoveReversed("BeginBarometer", _overviewFocus);
			else if(_lookingAt == _inkFocus)
				MoveReversed("BeginInk", _overviewFocus);
		}
	}
	
    private void TriggerMoveUranium(int itemNumber)
    {
		if(_lookingAt == null || _lookingAt == _overviewFocus)
			Move("BeginUranium",_uraniumFocus);
        else if(_lookingAt == _paperFocus)
            Move("PaperUranium", _uraniumFocus);
        else if(_lookingAt == _barometerFocus)
            MoveReversed("UraniumBarometer", _uraniumFocus);
        else if(_lookingAt == _inkFocus)
            MoveReversed("UraniumInk", _uraniumFocus);
    }

    private void TriggerMoveInk(int itemNumber)
    {
		if(_lookingAt == null || _lookingAt == _overviewFocus)
			Move("BeginInk",_inkFocus);
        else if(_lookingAt == _paperFocus)
            Move("PaperInk", _inkFocus);
        else if(_lookingAt == _uraniumFocus)
            Move("UraniumInk", _inkFocus);
        else if(_lookingAt == _barometerFocus)
            Move("BarometerInk", _inkFocus);
    }

    private void TriggerMoveBarometer(int itemNumber)
    {
		if(_lookingAt == null || _lookingAt == _overviewFocus)
			Move("BeginBarometer",_barometerFocus);
        else if(_lookingAt == _paperFocus)
            Move("PaperBarometer", _barometerFocus);
        else if(_lookingAt == _uraniumFocus)
            Move("UraniumBarometer", _barometerFocus);
        else if(_lookingAt == _inkFocus)
            MoveReversed("BarometerInk", _barometerFocus);
    }

    private void TriggerMovePaper(int itemNumber)
    {
		if(_lookingAt == null || _lookingAt == _overviewFocus)
			Move("BeginPaper",_paperFocus);
        else if(_lookingAt == _uraniumFocus)
            MoveReversed("PaperUranium",_paperFocus);
        else if(_lookingAt == _barometerFocus)
            MoveReversed("PaperBarometer",_paperFocus);
        else if(_lookingAt == _inkFocus)
            MoveReversed("PaperInk",_paperFocus);
    }

    //Move Functions
	private void Move(string pathName, Transform lookTarget)
    {
        if(_isMoving == false)
        {
            _isMoving = true;
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(pathName),
                                                    "time", _transitionTime,
                                                    "easetype", _easeType,
                                                    "looktarget", lookTarget,
                                                    "looktime", _lookTargetDelay,
                                                    "oncomplete", "AdjustLookingAt",
                                                    "oncompleteparams", lookTarget,
                                                    "oncompletetarget", gameObject));
        }
    }

    private void MoveReversed(string pathName, Transform lookTarget)
    {
        if(_isMoving == false)
        {
            _isMoving = true;
            iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPathReversed(pathName),
                                                    "time", _transitionTime,
                                                    "easetype", _easeType,
                                                    "looktarget", lookTarget,
                                                    "looktime", _lookTargetDelay,
                                                    "oncomplete", "AdjustLookingAt",
                                                    "oncompleteparams", lookTarget,
                                                    "oncompletetarget", gameObject));
        }
    }

    private void AdjustLookingAt(Transform tf)
    {
        _isMoving = false;
        _lookingAt = tf;
    }
    #endregion
}
