using UnityEngine;
using System.Collections;

public class PrinterDrag : MonoBehaviour {

    #region Editor Publics
    [SerializeField]
    private iTween.EaseType _easeType = iTween.EaseType.easeOutBack;
    [SerializeField]
    private Vector3 _destination = Vector3.zero;
    [SerializeField]
    private float _duration = 0.0f;
    #endregion

    #region Privates
    private Camera _camera;
    private RaycastHit _hit;
    private Vector3 _startPos;
    private bool _isBack = false;
    #endregion

    void OnEnable(){
        GestureManager.OnTap += SmoothDrag;
    }

    void OnDisable(){
        GestureManager.OnTap -= SmoothDrag;
    }

    #region Monobehaviour Functions
    void Start()
    {
        _camera = GameObject.Find("Main Camera").camera;
        _startPos = transform.position;
    }

    void Update()
    {

    }
    #endregion

    #region Class Methods
    public void SmoothDrag(GameObject go, Vector2 _screenPosition)
    {
        if(go != null && gameObject.Equals(go))
        {
                if(go != null)
                {
                    if(!_isBack)
                    {
                        iTween.MoveAdd(go, iTween.Hash("amount", _destination,
                                                                    "time", _duration));
                        _isBack = true;
                    }
                    else
                    {
                        iTween.MoveAdd(go, iTween.Hash("amount", -_destination,
                                                                    "time", _duration));
                        _isBack = false;
                    }
                }
                else
                {
                    Debug.Log("Missing object");
                }
        }
    }
    #endregion
}
