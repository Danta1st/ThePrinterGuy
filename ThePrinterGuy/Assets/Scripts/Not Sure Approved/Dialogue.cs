using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {
    #region Editor Publics
    [SerializeField]
    private Color _alertColor;
    #endregion

    private GameObject character;
    private Color oldColor;
    private bool _cameraMovement = false;

    void Start()
    {
        character = gameObject.transform.FindChild("bossChar").gameObject;
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
        character.animation.CrossFade("Selection");
        character.animation.CrossFadeQueued("Idle");
        StartCoroutine("CheckForDialogueEnd");
    }

    //Make some variables available to tweaking in editor.   
    private void AngryCharacter()
    {
        //OnDialogueStart();
        character.animation.CrossFade("Selection");
        iTween.ColorFrom(Camera.main.gameObject, Color.red, 2f);
        oldColor = Camera.main.backgroundColor;
        iTween.ValueTo(gameObject, iTween.Hash("from", oldColor, "to", _alertColor, "time", 0.1f, "onupdate", "changeSkyboxValue"));
        iTween.ValueTo(gameObject, iTween.Hash("from", _alertColor, "to", oldColor, "time", 0.1f, "onupdate", "changeSkyboxValue", "delay", 0.1f));
        character.animation.CrossFadeQueued("Idle");
        if(!_cameraMovement)
        {
            iTween.ShakeRotation(Camera.main.gameObject, iTween.Hash("amount", new Vector3(0.5f,0.5f,0.5f), "time", 0.2f));
        }
        StartCoroutine("CheckForDialogueEnd");
    }

    IEnumerator CheckForDialogueEnd()
    {
        while(character.animation.IsPlaying("Selection"))
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
