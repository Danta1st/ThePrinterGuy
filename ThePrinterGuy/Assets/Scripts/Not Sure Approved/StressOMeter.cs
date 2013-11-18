using UnityEngine;
using System.Collections;

public class StressOMeter : MonoBehaviour
{
    #region Privates#
    private float _rotationScale = 0.0f;
    #endregion

    #region MonoBehaviour
    void OnEnable()
    {
        ActionSequencerItem.OnFailed += ReductPointsFailed;
        ScoreManager.OnTaskGreen += GivePointsGreen;
        ScoreManager.OnTaskYellow += GivePointsYellow;
        ScoreManager.OnTaskRed += GivePointsRed;
        ScoreManager.OnTaskZone += ReductPointsZone;
    }

    void OnDisable()
    {
        ActionSequencerItem.OnFailed -= ReductPointsFailed;
        ScoreManager.OnTaskGreen -= GivePointsGreen;
        ScoreManager.OnTaskYellow -= GivePointsYellow;
        ScoreManager.OnTaskRed -= GivePointsRed;
        ScoreManager.OnTaskZone -= ReductPointsZone;
    }
    #endregion

    //ToDO: Make the functions give a small boost over time / Subtract the needle over time, instead of doing instantly

    #region Give Points
    void GivePointsRed()
    {
        _rotationScale += 2.5f;
        UpdateRotation();
    }

    void GivePointsYellow()
    {
        _rotationScale += 5.0f;
        UpdateRotation();
    }

    void GivePointsGreen()
    {
        _rotationScale += 10.0f;
        UpdateRotation();
    }
    #endregion

    #region Reduct Point
    void ReductPointsZone()
    {
        _rotationScale -= 7.5f;
        UpdateRotation();
    }

    void ReductPointsFailed()
    {
        _rotationScale -= 15.0f;
        UpdateRotation();
    }
    #endregion

    void UpdateRotation()
    {
        transform.rotation.Set(transform.rotation.x, transform.rotation.y, Mathf.Clamp(_rotationScale, -50.0f, 50.0f), transform.rotation.w);
    }
}
