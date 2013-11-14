using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private float _charLockedDistance = 12.0f;
    [SerializeField]
    private float _charUnlockedDistance = 10.0f;
    [SerializeField]
    private float _charMoveTime = 2.0f;
    [SerializeField]
    private List<GameObject> _stageCharacters = new List<GameObject>();

    private GameObject _selectedStageChar;
    private GameObject _lookTarget;
    private GameObject _creditsLookTarget;
    private GameObject _optionsLookTarget;
    private GameObject _camPointDefault;
    private bool _isZoomed = false;
    private GameObject _camStartPos;

    void Awake()
    {
        _camPointDefault = GameObject.Find("CamPoint_Default");

        _creditsLookTarget = GameObject.Find("Television");

        _optionsLookTarget = GameObject.Find("OptionButtons");
    }

    // Use this for initialization
    void Start()
    {
        _lookTarget = new GameObject();
        _lookTarget.name = "LookTarget";
        _lookTarget.transform.position = _camPointDefault.transform.position;

        _camStartPos = new GameObject();
        _camStartPos.name = "CamStartPosition";
        _camStartPos.transform.position = Camera.main.transform.position;

        foreach(GameObject go in _stageCharacters)
        {
            go.transform.FindChild("stageLevelSelection").gameObject.SetActive(false);
            go.collider.enabled = false;
        }
    }
 
    // Update is called once per frame
    void Update()
    {
 
    }

    void OnEnable()
    {
        GestureManager.OnTap += SelectStage;
        GestureManager.OnTap += SelectLevel;
        GUIMainMenuCamera.OnCreditScreen += ChangeViewToCredits;
        GUIMainMenuCamera.OnMainScreen += ChangeViewToMain;
        GUIMainMenuCamera.OnOptionsScreen += ChangeViewToOptions;
    }

    void OnDisable()
    {
        GestureManager.OnTap -= SelectStage;
        GestureManager.OnTap -= SelectLevel;
        GUIMainMenuCamera.OnCreditScreen -= ChangeViewToCredits;
        GUIMainMenuCamera.OnMainScreen -= ChangeViewToMain;
        GUIMainMenuCamera.OnOptionsScreen -= ChangeViewToOptions;
    }

    //TODO:
     /*
      * Start - Language boxes appear, stageCharacters are visible as shadows in the background
      * OnTap - Select a language
      *         After the language is selected the stageCharacters appear
      *         The stages that are unlocked will appear lit, while the rest are silhouettes
      * OnTap - Selected stageCharacter moves forward (only on lit characters)
      *         Camera focus on selected Character
      *         An animation animates the character to move forward
      *         The level boxes appear above the character
      * OnTap - Go to selected levelbox's scene
      */

    public List<GameObject> GetStageCharacters()
    {
        return _stageCharacters;
    }

    void IlluminateCharacter(GameObject go)
    {

    }

    void SilhouetteCharacter(GameObject go)
    {

    }

    void SelectStage(GameObject go, Vector2 screenPos)
    {
        //It works, change at own risk
        if(go != null)
        {
            if(_stageCharacters.Contains(go))
            {
                if(_selectedStageChar != null)
                {
                    if(_selectedStageChar == go)
                    {
                        if(_isZoomed)
                        {
                            CameraZoomOut();
                            SilhouetteCharacter(_selectedStageChar);
                            BeginMoveBackAnimation(_selectedStageChar);

                        }
                        else if(!_isZoomed)
                        {
                            CameraFocusOnStage(go);
                            IlluminateCharacter(go);
                            BeginMoveForwardAnimation(go);
                        }
                    }
                    else if(_selectedStageChar != go)
                    {
                        CameraFocusOnStage(go);
                        IlluminateCharacter(go);
                        BeginMoveForwardAnimation(go);

                        SilhouetteCharacter(_selectedStageChar);
                        BeginMoveBackAnimation(_selectedStageChar);
                    }
                }
                else
                {
                    CameraFocusOnStage(go);
                    IlluminateCharacter(go);
                    BeginMoveForwardAnimation(go);
                }

                _selectedStageChar = go;
            }
        }
        else if(go == null && _selectedStageChar != null)
        {
            CameraZoomOut();
            SilhouetteCharacter(_selectedStageChar);
            BeginMoveBackAnimation(_selectedStageChar);
        }
    }

    void CameraFocusOnStage(GameObject go)
    {
        _isZoomed = true;

        Transform charCamPoint = go.transform.FindChild("CamPoint");

        iTween.MoveTo(_lookTarget, iTween.Hash("position", go.transform.position, "time", _charMoveTime));

        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", charCamPoint, "time", _charMoveTime, "looktarget", _lookTarget));
    }

    void CameraZoomOut()
    {
        _isZoomed = false;

        iTween.MoveTo(_lookTarget, iTween.Hash("position", _camPointDefault, "time", _charMoveTime));

        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", _camStartPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget));
    }

    void BeginMoveForwardAnimation(GameObject go)
    {
        Vector3 tmpPos = go.transform.position;
        tmpPos.z = _charUnlockedDistance;

        iTween.MoveTo(go, iTween.Hash("position", tmpPos, "time", _charMoveTime, "oncomplete", "OnMoveForwardAnimationEnd", "oncompletetarget", gameObject, "oncompleteparams", go));

        LevelBoxesAppear(go);
    }

    void OnMoveForwardAnimationEnd(GameObject go)
    {

    }

    void LevelBoxesAppear(GameObject go)
    {
        GameObject tmpGo = go.transform.FindChild("stageLevelSelection").gameObject;
        tmpGo.SetActive(true);
    }

    void BeginMoveBackAnimation(GameObject go)
    {
        Vector3 tmpPos = go.transform.position;
        tmpPos.z = _charLockedDistance;

        iTween.MoveTo(go, iTween.Hash("position", tmpPos, "time", _charMoveTime, "oncomplete", "OnMoveBackAnimationEnd", "oncompletetarget", gameObject, "oncompleteparams", go));

        LevelBoxesDisappear(go);
    }

    void OnMoveBackAnimationEnd(GameObject go)
    {

    }

    void LevelBoxesDisappear(GameObject go)
    {
        GameObject tmpGo = go.transform.FindChild("stageLevelSelection").gameObject;
        tmpGo.SetActive(false);
    }

    void SelectLevel(GameObject go, Vector2 screenPos)
    {
        if(go != null)
        {
            if(go.GetComponent<LevelSelection>() != null)
            {
                LevelSelection lvlSelect = go.GetComponent<LevelSelection>();
                string tmpSceneName = lvlSelect.GetSceneName();
                Application.LoadLevel(tmpSceneName);
            }
        }
    }

    void ChangeViewToCredits()
    {
//        GestureManager.OnTap -= SelectStage;
//        GestureManager.OnTap -= SelectLevel;

        GameObject creditsCamPos = _creditsLookTarget.transform.FindChild("CamPointWallW").gameObject;

        iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _creditsLookTarget.transform.position, "time", _charMoveTime));

        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", creditsCamPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform));
    }

    void ChangeViewToOptions()
    {
//        GestureManager.OnTap -= SelectStage;
//        GestureManager.OnTap -= SelectLevel;

        GameObject optionsCamPos = _optionsLookTarget.transform.FindChild("CamPointWallE").gameObject;

        iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _optionsLookTarget.transform.position, "time", _charMoveTime));

        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", optionsCamPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform));
    }

    void ChangeViewToMain()
    {
//        GestureManager.OnTap += SelectStage;
//        GestureManager.OnTap += SelectLevel;

        iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _camPointDefault.transform.position, "time", _charMoveTime));

        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", _camStartPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform));
    }
}
