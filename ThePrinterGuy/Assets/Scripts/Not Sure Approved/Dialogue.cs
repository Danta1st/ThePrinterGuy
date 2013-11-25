using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour {
    #region Editor Publics
    [SerializeField]
    private Color _alertColor;
    #endregion

    private Animation _characterAnimation;
    private Color _oldColor;
    private bool _cameraMovement = false;
    private GameObject _textBG;
    private GameObject _text;
    private string _localizationKey;
    private string[] happyTuple;
    private string[] angryTuple;
    //animation, sound, localzation key
    private List<string[]> happyCollection;
    private List<string[]> angryCollection;
    //1 AARGH, 2 næve, 3 smoke

    void Awake()
    {
        happyCollection = new List<string[]>();
        happyCollection.Add(new string[] {"Good 01","6","InGameNotBad"});
        angryCollection = new List<string[]>();
        angryCollection.Add(new string[] {"Very Angry 01","11","InGameIdiot"});
        _characterAnimation = gameObject.GetComponentInChildren<Animation>();
    }

    #region Delegates & Events
    public delegate void DialogueStart(string localizationKey);
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
        happyTuple = happyCollection[Random.Range(0, happyCollection.Count)];
        _characterAnimation.CrossFade(happyTuple[0]);
        PlaySound(happyTuple[1]);
        if(OnDialogueStart != null)
            OnDialogueStart(happyTuple[2]);
        _characterAnimation.CrossFadeQueued("Idle");
    }

    private void AngryCharacter()
    {
        angryTuple = angryCollection[Random.Range(0, angryCollection.Count)];
        _characterAnimation.CrossFade(angryTuple[0]);
        PlaySound(angryTuple[1]);
        _localizationKey = angryTuple[2];
        if(OnDialogueStart != null){
            OnDialogueStart(_localizationKey);
        }
        iTween.ColorFrom(Camera.main.gameObject, Color.red, 2f);
        _oldColor = Camera.main.backgroundColor;
        iTween.ValueTo(gameObject, iTween.Hash("from", _oldColor, "to", _alertColor, "time", 0.1f, "onupdate", "changeSkyboxValue"));
        iTween.ValueTo(gameObject, iTween.Hash("from", _alertColor, "to", _oldColor, "time", 0.1f, "onupdate", "changeSkyboxValue", "delay", 0.1f));
        if(!_cameraMovement)
        {
            iTween.ShakeRotation(Camera.main.gameObject, iTween.Hash("amount", new Vector3(0.5f,0.5f,0.5f), "time", 0.2f));
        }
        _characterAnimation.CrossFadeQueued("Idle");
        StartCoroutine(CheckIfAnimationStopped(angryTuple[0]));
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

    private void PlaySound(string sound)
    {
        switch (sound) {
        case "6":
            SoundManager.Voice_Boss_6();
            break;
        case "11":
            SoundManager.Voice_Boss_11();
            break;
        default:
        break;
        }
    }

    IEnumerator CheckIfAnimationStopped(string animation)
    {
        while(_characterAnimation.IsPlaying(angryTuple[0]))
        {
            yield return new WaitForSeconds(1f);
        }
        if(OnDialogueEnd != null)
            OnDialogueEnd();
    }

}
