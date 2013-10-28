using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public GameObject printer;

    OnStart(){


}

    void OnEnable()
    {
        GestureManager.OnSwipeRight += CameraRotation;

    }

    public void CameraRotation()
    {
        iTween.MoveTo(GameObject, "lookTarget", printer.transform.position, "position", new Vector3(-10,3,0));
    }
}