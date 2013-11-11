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
    #endregion

    #region Privates
    private float _lookTargetDelay;
    private Transform _lookingAt;
    private bool _isMoving = false;
    #endregion

    //TODO: Make Proper Connectivity to whatever it needs to connect to
    void OnEnable()
    {
        GestureManager.OnSwipeRight += TriggerMoveBarometer;
        GestureManager.OnSwipeLeft += TriggerMovePaper;
        GestureManager.OnSwipeUp += TriggerMoveInk;
        GestureManager.OnSwipeDown += TriggerMoveUranium;
    }
    void OnDisable()
    {
        GestureManager.OnSwipeRight -= TriggerMoveBarometer;
        GestureManager.OnSwipeLeft -= TriggerMovePaper;
        GestureManager.OnSwipeUp -= TriggerMoveInk;
        GestureManager.OnSwipeDown -= TriggerMoveUranium;
    }

    #region Monohevaiour Methods
    void Start()
    {
        _lookTargetDelay = _transitionTime / 2;
        _lookingAt = _paperFocus;
    }
    #endregion

    #region Class Methods
    //Trigger Functions
    private void TriggerMoveUranium(GameObject go)
    {
        if(_lookingAt == _paperFocus)
            Move("PaperUranium", _uraniumFocus);
        else if(_lookingAt == _barometerFocus)
            MoveReversed("UraniumBarometer", _uraniumFocus);
        else if(_lookingAt == _inkFocus)
            MoveReversed("UraniumInk", _uraniumFocus);
    }

    private void TriggerMoveInk(GameObject go)
    {
       if(_lookingAt == _paperFocus)
            Move("PaperInk", _inkFocus);
        else if(_lookingAt == _uraniumFocus)
            Move("UraniumInk", _inkFocus);
        else if(_lookingAt == _barometerFocus)
            Move("BarometerInk", _inkFocus);
    }

    private void TriggerMoveBarometer(GameObject go)
    {
        if(_lookingAt == _paperFocus)
            Move("PaperBarometer", _barometerFocus);
        else if(_lookingAt == _uraniumFocus)
            Move("UraniumBarometer", _barometerFocus);
        else if(_lookingAt == _inkFocus)
            MoveReversed("BarometerInk", _inkFocus);
    }

    private void TriggerMovePaper(GameObject go)
    {
        if(_lookingAt == _uraniumFocus)
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
