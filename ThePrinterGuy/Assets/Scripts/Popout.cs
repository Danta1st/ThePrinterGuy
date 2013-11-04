using UnityEngine;
using System.Collections;

public class Popout: MonoBehaviour {
    #region Editor Publics
    [SerializeField]
    private iTween.EaseType _popoutEaseType = iTween.EaseType.easeOutBack;
    [SerializeField]
    private float _popoutLength = 5;
    [SerializeField]
    private float _popoutDuration = 0.4f;
    [SerializeField]
    private iTween.EaseType _hammerInEaseType = iTween.EaseType.easeOutExpo;
    [SerializeField]
    private float _hammerHitsReq = 5f;
    [SerializeField]
    private float _hammerInDuration = 0.1f;
    #endregion

    #region Privates
    private float _hammerHitsTaken = 0;
    private bool _animationInProcess = false;
    private bool _isOut = false;
    #endregion


    #region Delegates & Events
    public delegate void CylinderHammeredIn(GameObject go);
    public static event CylinderHammeredIn OnCylinderHammeredIn;
    #endregion


    #region Methods
    void OnEnable(){
        GestureManager.OnTap += HitCylinder;
    }

    void OnDisable(){
        GestureManager.OnTap -= HitCylinder;
    }


    //Smoke/damp when it goes up - continue with small damp!
    //dust/force cloud when hammering
    //Handle hammer too

    public void HitCylinder(GameObject go, Vector2 _screenPosition)
    {
        if(go != null && gameObject.Equals(go) && _animationInProcess == false && _isOut == true && _hammerHitsReq != _hammerHitsTaken)
        {
            _animationInProcess = true;
            float _hammerInAmount = -(_popoutLength / _hammerHitsReq);
            iTween.MoveAdd(go, iTween.Hash("y", _hammerInAmount, "time", _hammerInDuration, "easeType", _hammerInEaseType, "onComplete", "AnimationStopped"));
            _hammerHitsTaken++;
        }
        if(_hammerHitsReq == _hammerHitsTaken)
        {
            _hammerHitsTaken = 0;
            _isOut = false;
            if(OnCylinderHammeredIn != null)
            {
                OnCylinderHammeredIn(gameObject.transform.root.gameObject);
            }
        }
    }

    public void PopoutCylinder()
    {
        if(_animationInProcess == false && _isOut == false)
        {
            _animationInProcess = true;
            _isOut = true;
            iTween.MoveAdd(gameObject, iTween.Hash("y", _popoutLength, "time", _popoutDuration, "easeType", _popoutEaseType, "onComplete", "AnimationStopped"));
        }
    }

    public void AnimationStopped()
    {
        _animationInProcess = false;
    }

    public bool GetIsOut()
    {
        return _isOut;
    }
    #endregion
}
