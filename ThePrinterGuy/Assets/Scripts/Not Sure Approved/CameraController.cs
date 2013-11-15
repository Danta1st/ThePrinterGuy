using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private iTween.EaseType easeType = iTween.EaseType.easeOutBack;
    [SerializeField]
    private float rotationTime = 0.5f;
    [SerializeField]
    private GameObject cameraRotationPoint;
    #endregion
	

    #region Privates
    private bool RotationInProcess = false;
	private bool _inFreeRoam = true;
    #endregion

    #region OnEnableOnDisable
    void OnEnable()
    {
        GestureManager.OnSwipeRight += CameraRotationLeft;
        GestureManager.OnSwipeLeft += CameraRotationRight;
        GestureManager.OnSwipeDown += CameraRotationUp;
    }

    void OnDisable()
    {
        GestureManager.OnSwipeRight -= CameraRotationLeft;
        GestureManager.OnSwipeLeft -= CameraRotationRight;
        GestureManager.OnSwipeDown -= CameraRotationUp;
    }
    #endregion

    #region Methods
	public void FreeRoamMode()
	{
		GestureManager.OnSwipeRight += CameraRotationLeft;
        GestureManager.OnSwipeLeft += CameraRotationRight;
        GestureManager.OnSwipeDown += CameraRotationUp;
	}
	public void FocusedMode()
	{
		GestureManager.OnSwipeRight -= CameraRotationLeft;
        GestureManager.OnSwipeLeft -= CameraRotationRight;
        GestureManager.OnSwipeDown -= CameraRotationUp;
	}
	
    public void CameraRotationLeft(GameObject go)
    {
        if(RotationInProcess == false && _inFreeRoam)
        {
            RotateCamera(0, 90, 0);
        }
    }

    public void CameraRotationRight(GameObject go)
    {
        if(RotationInProcess == false && _inFreeRoam)
        {
            RotateCamera(0, -90, 0);
        }
    }

    public void CameraRotationUp(GameObject go)
    {
        if(RotationInProcess == false && _inFreeRoam)
        {
            GestureManager.OnSwipeRight -= CameraRotationLeft;
            GestureManager.OnSwipeLeft -= CameraRotationRight;
            GestureManager.OnSwipeDown -= CameraRotationUp;
            GestureManager.OnSwipeUp += CameraRotationDown;
            RotateCamera(60, 0, 0);
        }
    }

    public void CameraRotationDown(GameObject go)
    {
        if(RotationInProcess == false && _inFreeRoam)
        {
            GestureManager.OnSwipeRight += CameraRotationLeft;
            GestureManager.OnSwipeLeft += CameraRotationRight;
            GestureManager.OnSwipeUp -= CameraRotationDown;
            GestureManager.OnSwipeDown += CameraRotationUp;
            RotateCamera(-60, 0, 0);
        }
    }

    public void RotateCamera(float rotateX, float rotateY, float rotateZ)
    {
            RotationInProcess = true;
            iTween.RotateAdd(cameraRotationPoint, iTween.Hash("x", cameraRotationPoint.transform.position.x + rotateX, "y", cameraRotationPoint.transform.position.y + rotateY, "z", cameraRotationPoint.transform.position.y + rotateZ, "time", rotationTime, "easeType", easeType, "onComplete", "CameraRotationStopped", "onCompleteTarget", gameObject));
    }

    public void CameraRotationStopped()
    {
        RotationInProcess = false;
    }
    #endregion
}