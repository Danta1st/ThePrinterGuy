using UnityEngine;
using System.Collections;

public class StressOMeter : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _outOfZonePoints = -10.0f;
    [SerializeField]
    private float _inZonePoints = 10.0f;
    [SerializeField]
    private float _failedPoints = -20.0f;
    #endregion

    #region Privates#
    private float _rotationScale = 0.0f;
    private float _stressMIN = -40.0f;
    private float _stressMAX = 60.0f;
    private Vector3 _thisRotation;
    private float _rotationTime = 0.2f;
    private Vector3 _shakeRotation;
    private float _shakeTime = 0.05f;
    #endregion

    public delegate void GameFailed();
    public static event GameFailed OnGameFailed;

    void Start()
    {
        _thisRotation = new Vector3(0.0f, 0.0f, 0.0f);
        _shakeRotation = new Vector3(0.0f, 0.0f, 1.0f);
    }

    #region MonoBehaviour
    void OnEnable()
    {
        ActionSequencerItem.OnFailed += ReductPointsFailed;
        ScoreManager.OnTaskGreen += GivePointsGreen;
        ScoreManager.OnTaskRed += GivePointsGreen;
        ScoreManager.OnTaskYellow += GivePointsGreen;
        ScoreManager.OnTaskZone += ReductPointsZone;
    }

    void OnDisable()
    {
        ActionSequencerItem.OnFailed -= ReductPointsFailed;
        ScoreManager.OnTaskGreen -= GivePointsGreen;
        ScoreManager.OnTaskRed -= GivePointsGreen;
        ScoreManager.OnTaskYellow -= GivePointsGreen;
        ScoreManager.OnTaskZone -= ReductPointsZone;
    }
    #endregion

    //ToDO: Make the functions give a small boost over time / Subtract the needle over time, instead of doing instantly

    #region Give Points
    void GivePointsGreen()
    {
        _rotationScale -= _inZonePoints;
        UpdateRotation();
    }
    #endregion

    #region Reduct Point
    void ReductPointsZone()
    {
        _rotationScale -= _outOfZonePoints;
        UpdateRotation();
    }

    void ReductPointsFailed()
    {
        _rotationScale -= _failedPoints;
        UpdateRotation();
    }
    #endregion

    #region Needlemovement functions
    void UpdateRotation()
    {
        _rotationScale = Mathf.Clamp(_rotationScale, _stressMIN, _stressMAX);

        float thisRotationScale = 360.0f + _rotationScale;

        _thisRotation.z = thisRotationScale;

        iTween.Stop(gameObject);

        iTween.RotateTo(gameObject, iTween.Hash("rotation", _thisRotation, "time", _rotationTime, "easetype", iTween.EaseType.linear, "islocal", true,
            "oncomplete", "SlightMovement", "oncompletetarget", gameObject));
    }

    void SlightMovement()
    {
        if(OnGameFailed != null && _rotationScale == _stressMAX)
        {
            OnGameFailed();
        }

        iTween.ShakeRotation(gameObject, iTween.Hash("amount", _shakeRotation, "time", _shakeTime, "oncomplete", "SlightMovementBack", "oncompletetarget", gameObject));
    }

    void SlightMovementBack()
    {
        iTween.RotateTo(gameObject, iTween.Hash("rotation", _thisRotation, "time", _shakeTime, "oncomplete", "SlightMovement", "oncompletetarget", gameObject));
    }
    #endregion
}
