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
    [SerializeField]
    private List<GameObject> _gameLevels = new List<GameObject>();
    [SerializeField]
    private List<bool> _gameLevelsUnlocked = new List<bool>();

    private GameObject _selectedStageChar;
    private GameObject _lookTarget;
    private GameObject _creditsLookTarget;
    private GameObject _optionsLookTarget;
    private GameObject _camPointDefault;
    private bool _isZoomed = false;
    private GameObject _camStartPos;
    private bool _camAtRest = true;


    public delegate void CreditsView();
    public static event CreditsView OnCreditsView;

    public delegate void OptionsView();
    public static event OptionsView OnOptionsView;

    public delegate void MainView();
    public static event MainView OnMainView;


    void Awake()
    {
        _creditsLookTarget = GameObject.Find("Television");
        _optionsLookTarget = GameObject.Find("OptionButtons");
    }

    // Use this for initialization
    void Start()
    {
        #region Camera Positioning Objects
        _camPointDefault = new GameObject();
        _camPointDefault.name = "CamLookPosDefault";
        _camPointDefault.transform.position = new Vector3(0, 0, 12);

        _lookTarget = new GameObject();
        _lookTarget.name = "LookTarget";
        _lookTarget.transform.position = _camPointDefault.transform.position;

        _camStartPos = new GameObject();
        _camStartPos.name = "CamStartPosition";
        _camStartPos.transform.position = Camera.main.transform.position;
        #endregion

        foreach(GameObject go in _stageCharacters)
        {
            go.collider.enabled = false;

            StageCharacter thisChar = go.GetComponent<StageCharacter>();

            GameObject thisProjectorHolder = go.transform.parent.FindChild("projectorHolder").gameObject;

            Projector thisProjector = thisProjectorHolder.transform.GetComponent<Projector>();

            Color thisColor = thisProjector.material.color;

            thisColor.a = 1.0f;

            thisProjector.material.SetColor("_Color", thisColor);

            if(thisChar.GetUnlocked())
            {
                SilhouetteCharacter(thisProjectorHolder);
            }
        }

        for(int i = 0; i < _gameLevels.Count; i++)
        {
            _gameLevels[i].SetActive(false);
        }
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

    public List<GameObject> GetStageCharacters()
    {
        return _stageCharacters;
    }

    void SilhouetteCharacter(GameObject go)
    {
        Projector thisProjector = go.transform.GetComponent<Projector>();

        Color thisColor = thisProjector.material.color;

        thisColor.a = 0.9f;

        thisProjector.material.SetColor("_Color", thisColor);
    }

    void SelectStage(GameObject go, Vector2 screenPos)
    {
        //It works, change at own risk
        if(go != null)
        {
            if(_stageCharacters.Contains(go) && go.GetComponent<StageCharacter>().GetUnlocked())
            {
                if(_selectedStageChar != null)
                {
                    if(_selectedStageChar == go)
                    {
                        if(_isZoomed)
                        {
                            CameraZoomOut();
                            BeginMoveBackAnimation(_selectedStageChar);

                        }
                        else if(!_isZoomed)
                        {
                            CameraFocusOnStage(go);
                            BeginMoveForwardAnimation(go);
                        }
                    }
                    else if(_selectedStageChar != go)
                    {
                        CameraFocusOnStage(go);
                        BeginMoveForwardAnimation(go);

                        BeginMoveBackAnimation(_selectedStageChar);
                    }
                }
                else
                {
                    CameraFocusOnStage(go);
                    BeginMoveForwardAnimation(go);
                }

                _selectedStageChar = go;
            }
        }
        else if(go == null && _selectedStageChar != null)
        {
            CameraZoomOut();
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
        int indexChar = _stageCharacters.IndexOf(go);

        int minIndex = indexChar * 5;

        for(int i = minIndex; i < (minIndex + 5); i++)
        {
            if(_gameLevelsUnlocked[i])
            {
                _gameLevels[i].SetActive(true);
            }
        }
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
//        GameObject tmpGo = go.transform.FindChild("stageLevelSelection").gameObject;
//        tmpGo.SetActive(false);

        int indexChar = _stageCharacters.IndexOf(go);

        int minIndex = indexChar * 5;

        for(int i = minIndex; i < (minIndex + 5); i++)
        {
            if(_gameLevelsUnlocked[i])
            {
                _gameLevels[i].SetActive(false);
            }
        }
    }

    void SelectLevel(GameObject go, Vector2 screenPos)
    {
        if(go != null && _gameLevels.Contains(go))
        {
            int index = _gameLevels.IndexOf(go);

            if(_gameLevelsUnlocked[index])
            {
                Application.LoadLevel(index);
            }
        }
    }

    void ChangeViewToCredits()
    {
        if(_camAtRest)
        {
            _camAtRest = false;

            GestureManager.OnTap -= SelectStage;
            GestureManager.OnTap -= SelectLevel;
    
            GameObject creditsCamPos = _creditsLookTarget.transform.FindChild("CamPointWallW").gameObject;
    
            iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _creditsLookTarget.transform.position, "time", _charMoveTime));

            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", creditsCamPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform,
                "oncomplete", "OnCreditCameraComplete", "oncompletetarget", gameObject));

            if(_selectedStageChar != null)
            {
                BeginMoveBackAnimation(_selectedStageChar);
            }
        }
    }

    void OnCreditCameraComplete()
    {
        SetCamAtRest();
        OnCreditsView();
    }

    void ChangeViewToOptions()
    {
        if(_camAtRest)
        {
            _camAtRest = false;

            GestureManager.OnTap -= SelectStage;
            GestureManager.OnTap -= SelectLevel;
    
            GameObject optionsCamPos = _optionsLookTarget.transform.FindChild("CamPointWallE").gameObject;

            iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _optionsLookTarget.transform.position, "time", _charMoveTime));
    
            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", optionsCamPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform,
                "oncomplete", "OnOptionsCameraComplete", "oncompletetarget", gameObject));
    
            if(_selectedStageChar != null)
            {
                BeginMoveBackAnimation(_selectedStageChar);
            }
        }
    }

    void OnOptionsCameraComplete()
    {
        SetCamAtRest();
        OnOptionsView();
    }

    void ChangeViewToMain()
    {
        if(_camAtRest)
        {
            _camAtRest = false;

            iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _camPointDefault.transform.position, "time", _charMoveTime));

            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", _camStartPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform,
                "oncomplete", "OnMainViewCameraComplete", "oncompletetarget", gameObject));
        }
    }

    void OnMainViewCameraComplete()
    {
        SetCamAtRest();
        OnMainView();

        GestureManager.OnTap += SelectStage;
        GestureManager.OnTap += SelectLevel;
    }

    void SetCamAtRest()
    {
        _camAtRest = true;
    }
}
