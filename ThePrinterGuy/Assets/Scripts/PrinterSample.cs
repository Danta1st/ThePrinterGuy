using UnityEngine;
using System.Collections;

public class PrinterSample : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _zoomSpeed = 10;
    #endregion

    #region Privates
    private Vector3 _standardPos;
    private Vector3 _abovePrinterPos;
    private GameObject _printer;
    private bool _isAbove = false;
    #endregion

    void Start()
    {
        _printer = GameObject.Find("PrinterSample");
        _standardPos = transform.position;
        _abovePrinterPos = GameObject.Find("AbovePrinter").transform.position;
    }

    void OnEnable()
    {
        //GestureManager.OnTap += SmoothMoveTo;
        //GestureManager.OnDoubleTap += SmoothMoveTo;
        GestureManager.OnPress += SmoothMoveTo;
        GestureManager.OnDrag += Move;
        GestureManager.OnSwipeRight += RotateRight;
        GestureManager.OnSwipeLeft += RotateLeft;
        GestureManager.OnSwipeUp += SetCamStandard;
        GestureManager.OnSwipeDown += SetCamAbove;
        GestureManager.OnPinch += ZoomIn;
        GestureManager.OnSpread += ZoomOut;
    }

    void OnDisable()
    {
        //GestureManager.OnTap -= SmoothMoveTo;
        //GestureManager.OnDoubleTap -= SmoothMoveTo;
        GestureManager.OnPress -= SmoothMoveTo;
        GestureManager.OnDrag -= Move;
        GestureManager.OnSwipeRight -= RotateRight;
        GestureManager.OnSwipeLeft -= RotateLeft;
        GestureManager.OnSwipeUp -= SetCamStandard;
        GestureManager.OnSwipeDown -= SetCamAbove;
        GestureManager.OnPinch -= ZoomIn;
        GestureManager.OnSpread -= ZoomOut;
    }

    //FIXME: Rewrite this function to include several printers. Should also handle slide in /slide out behavior
    void SmoothMoveTo(GameObject go, Vector2 nothing)
    {
        var newpos = new Vector3(_printer.transform.position.x + 4,
                                 _printer.transform.position.y,
                                 _printer.transform.position.z);

        iTween.MoveTo(_printer, iTween.Hash("position", newpos,
                                                "time", 0.5f,
                                                "easeType", iTween.EaseType.easeOutBack));
    }

    void Move(GameObject go, Vector2 screenPosition, Vector2 deltaPosition)
    {
        var newPos = new Vector3(deltaPosition.x, deltaPosition.y, 0);
        go.transform.position += newPos * Time.deltaTime;
    }

    void RotateRight()
    {
        iTween.RotateBy(_printer, iTween.Hash("y", -0.25f, "time", 0.5f,
                                                "easeType", iTween.EaseType.easeOutBack));
    }

    void RotateLeft()
    {
        iTween.RotateBy(_printer, iTween.Hash("y", 0.25f, "time", 0.5f,
                                                "easeType", iTween.EaseType.easeOutBack));
    }

    void SetCamAbove()
    {
        if(_isAbove == false)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", _abovePrinterPos,
                                                     "time", 0.75f,
                                                     "easeType", iTween.EaseType.easeOutBack));

            iTween.RotateBy(gameObject, iTween.Hash("x", 0.25f, "time", 0.75f,
                                                     "easeType", iTween.EaseType.easeOutBack));
            _isAbove = true;
        }
    }

    void SetCamStandard()
    {
        if(_isAbove == true)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", _standardPos,
                                                     "time", 0.75f,
                                                     "easeType", iTween.EaseType.easeOutBack));

            iTween.RotateBy(gameObject, iTween.Hash("x", -0.25f, "time", 0.75f,
                                                     "easeType", iTween.EaseType.easeOutBack));
            _isAbove = false;
        }
    }

    void ZoomIn(float pinchDistance)
    {
        //FIXME: Insert boundaries
        camera.fieldOfView += pinchDistance * _zoomSpeed * Time.deltaTime;
    }

    void ZoomOut(float pinchDistance)
    {
        //FIXME: Insert boundaries
        camera.fieldOfView -= pinchDistance * _zoomSpeed * Time.deltaTime;
    }

}

