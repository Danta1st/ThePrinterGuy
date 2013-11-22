using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {
    #region Editor Publics
    [SerializeField]
    private Color _alertColor;
    #endregion

    private Animation _characterAnimation;
    private Color _oldColor;
    private bool _cameraMovement = false;

    void Start()
    {
        _characterAnimation = gameObject.GetComponentInChildren<Animation>();
    }

    #region Delegates & Events
    public delegate void DialogueStart();
    public static event DialogueStart OnDialogueStart;

    public delegate void DialogueEnd();
    public static event DialogueEnd OnDialogueEnd;
    #endregion

    #region Enable and Disable
    void OnEnable()
    {
        StressOMeter.OnHappyZoneEntered += HappyCharacter;
        StressOMeter.OnAngryZoneEntered += AngryCharacter;
        PathManager.OnCamPosChangeBegan += CameraStartedMoving;
        PathManager.OnCamPosChangeEnded += CameraStoppedMoving;

    }

    void OnDisable()
    {
        StressOMeter.OnHappyZoneEntered -= HappyCharacter;
        StressOMeter.OnAngryZoneEntered -= AngryCharacter;
        PathManager.OnCamPosChangeBegan -= CameraStartedMoving;
        PathManager.OnCamPosChangeEnded -= CameraStoppedMoving;
    }
    #endregion

    private void HappyCharacter()
    {
       // OnDialogueStart();
        _characterAnimation.CrossFade("Good 01");
        StartCoroutine("CheckForDialogueEnd");
         _characterAnimation.CrossFadeQueued("Idle");
    }

    //Make some variables available to tweaking in editor.   
    private void AngryCharacter()
    {
        //OnDialogueStart();
        _characterAnimation.CrossFade("Very Angry 01");
        iTween.ColorFrom(Camera.main.gameObject, Color.red, 2f);
        _oldColor = Camera.main.backgroundColor;
        iTween.ValueTo(gameObject, iTween.Hash("from", _oldColor, "to", _alertColor, "time", 0.1f, "onupdate", "changeSkyboxValue"));
        iTween.ValueTo(gameObject, iTween.Hash("from", _alertColor, "to", _oldColor, "time", 0.1f, "onupdate", "changeSkyboxValue", "delay", 0.1f));
        if(!_cameraMovement)
        {
            iTween.ShakeRotation(Camera.main.gameObject, iTween.Hash("amount", new Vector3(0.5f,0.5f,0.5f), "time", 0.2f));
        }
        StartCoroutine("CheckForDialogueEnd");
        _characterAnimation.CrossFadeQueued("Idle");
    }

    IEnumerator CheckForDialogueEnd()
    {
        while(_characterAnimation.IsPlaying("Very Angry 01") || _characterAnimation.IsPlaying("Good 01"))
        {
            yield return new WaitForSeconds(1);
        }
        //OnDialogueEnd();
    }

    private void changeSkyboxValue(Color color)
    {
        Camera.main.backgroundColor = color;
    }

    private void CameraStartedMoving()
    {
        _cameraMovement = true;
    }

    private void CameraStoppedMoving()
    {
        _cameraMovement = false;
    }

}
