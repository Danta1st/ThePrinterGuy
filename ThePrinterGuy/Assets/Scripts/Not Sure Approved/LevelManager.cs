using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float _charLockedDistance = 12.0f;
    [SerializeField] private float _charUnlockedDistance = 10.0f;
    [SerializeField] private float _charMoveTime = 2.0f;
    [SerializeField] private List<GameObject> _stageCharacters = new List<GameObject>();
    [SerializeField] private List<GameObject> _gameLevels = new List<GameObject>();
    //[SerializeField]
    private List<bool> _gameLevelsUnlocked = new List<bool>();
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private int _levelBoxCount;
    [SerializeField] private iTween.EaseType _easyTypeOfLevelParentObjectIn = iTween.EaseType.easeOutBack;
    [SerializeField] private iTween.EaseType _easyTypeOfLevelParentObjectOut = iTween.EaseType.easeInBack;
    [SerializeField] private Texture2D _levelUnlockedTexture;
    [SerializeField] private Texture2D _levelUnlockedTutorialTexture;

    private GameObject _selectedStageChar;
    private GameObject _lookTarget;
    private GameObject _creditsLookTarget;
    private GameObject _optionsLookTarget;
    private GameObject _camPointDefault;
    private bool _isZoomed = false;
    private GameObject _camStartPos;
    private bool _camAtRest = true;
    private int[] highScores;
	private enum MainMenuStates { MainMenu, LevelSelection, Options, Credits };
	private MainMenuStates state = MainMenuStates.MainMenu;
    private GameObject LevelParentObject;
    int indexChar;
    int minIndex;


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
        if(!PlayerPrefs.HasKey("highscoresAsString")){
            SaveGame.ResetPlayerData();
        }

        UnlockLevels();
    }

    // Use this for initialization
    void Start()
    {
        #region Camera Positioning Objects
        _camPointDefault = new GameObject();
        _camPointDefault.name = "CamLookPosDefault";
        _camPointDefault.transform.position = new Vector3(0, 3, 14);

        _lookTarget = new GameObject();
        _lookTarget.name = "LookTarget";
        _lookTarget.transform.position = _camPointDefault.transform.position;
		
		iTweenPath path = Camera.main.GetComponent<iTweenPath>();
		
        _camStartPos = new GameObject();
        _camStartPos.name = "CamStartPosition";
        _camStartPos.transform.position = path.nodes[path.nodes.Count - 1];
        #endregion
    }

    void OnEnable()
    {
        GUIMainMenuCamera.OnCreditScreen += ChangeViewToCredits;
        GUIMainMenuCamera.OnMainScreen += ChangeViewToMain;
        GUIMainMenuCamera.OnOptionsScreen += ChangeViewToOptions;
		GUIMainMenuCamera.OnLevelManagerEvent += SelectStage;
    }

    void OnDisable()
    {
        GUIMainMenuCamera.OnCreditScreen -= ChangeViewToCredits;
        GUIMainMenuCamera.OnMainScreen -= ChangeViewToMain;
        GUIMainMenuCamera.OnOptionsScreen -= ChangeViewToOptions;
		GUIMainMenuCamera.OnLevelManagerEvent -= SelectStage;
    }

    public void UnlockLevels()
    {
        highScores = SaveGame.GetPlayerHighscores();

        #region Unlock Levels based on Highscore
        _gameLevelsUnlocked.Clear();
        foreach (int highscore in highScores)
        {
            if(highscore > -1)
            {
                _gameLevelsUnlocked.Add(true);
            }
            else
            {
                _gameLevelsUnlocked.Add(false);
            }
        }
        #endregion 
    }

    public List<GameObject> GetStageCharacters()
    {
        return _stageCharacters;
    }

    void SilhouetteCharacter(GameObject go)
    {
        Projector thisProjector = go.transform.GetComponent<Projector>();

        Color thisColor = thisProjector.material.color;

        thisColor.a = 0.3f;

        thisProjector.material.SetColor("_Color", thisColor);
    }

    void SelectStage(GameObject go, Vector2 screenPos)
    {
        //It works, change at own risk
 
		switch(state) {
		case MainMenuStates.MainMenu:
			MainMenuHandler(go);
			break;
		case MainMenuStates.LevelSelection:
			LevelSelectionHandler(go);
			break;
		case MainMenuStates.Options:
			break;
		case MainMenuStates.Credits:
			break;
		}
		 
//		else
//        {
//            CameraZoomOut();
//            BeginMoveBackAnimation(_selectedStageChar);
//        }
    	
	}
		
	void MainMenuHandler(GameObject go)
	{
		if(go == null)
			return;
		
		if(_stageCharacters.Contains(go) && go.GetComponent<StageCharacter>().GetUnlocked())
		{
			state = MainMenuStates.LevelSelection;
			CameraFocusOnStage(go);
			BeginMoveForwardAnimation(go);
			_selectedStageChar = go;
		}

	}
	
	void LevelSelectionHandler(GameObject go)
	{
		if(go != null) {
			if(_stageCharacters.Contains(go) && go.GetComponent<StageCharacter>().GetUnlocked())
			{
				if(_selectedStageChar == go)
				{
					state = MainMenuStates.MainMenu;
					CameraZoomOut();
					BeginMoveBackAnimation(_selectedStageChar);
					_selectedStageChar = null;
				}
				else
				{
					CameraFocusOnStage(go);
					BeginMoveForwardAnimation(go);
	
					BeginMoveBackAnimation(_selectedStageChar);
					_selectedStageChar = go;
				} 
			}
			else if(go != null && _gameLevels.Contains(go))
			{
	            int index = _gameLevels.IndexOf(go);

                SoundManager.Effect_Menu_Click();
	
                if(_gameLevelsUnlocked[index])
                {
                    string correspondingLevelName = null;
                    switch (index) {
                        case 0:
                            correspondingLevelName = "Tutorial1";
                            break;
                        case 1:
                            correspondingLevelName = "Tutorial2";
                            break;
                        case 2:
                            correspondingLevelName = "Tutorial3";
                            break;
                        case 3:
                            correspondingLevelName = "Tutorial4";
                            break;
                        case 4:
                            correspondingLevelName = "Tutorial5";
                            break;
                        case 5:
                            correspondingLevelName = "Stage1Cinematics";
                            break;
                        default:
                            break;
                    }
                    if(correspondingLevelName == null)
                    {
                        LoadingScreen.Load(index + 2, true);
                    }
                    else
                    {
                        LoadingScreen.Load(correspondingLevelName, true);
                    }
                }
        	}
		} else if(go == null && _selectedStageChar != null) {
			state = MainMenuStates.MainMenu;
			CameraZoomOut();
			BeginMoveBackAnimation(_selectedStageChar);
		}

	}

    void CameraFocusOnStage(GameObject go)
    {
		if(go != null) {
	        Transform charCamPoint = go.transform.FindChild("CamPoint");
	
	        iTween.MoveTo(_lookTarget, iTween.Hash("position", go.transform.position, "time", _charMoveTime));
	
	        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", charCamPoint, "time", _charMoveTime, "looktarget", _lookTarget));
		}
    }

    void CameraZoomOut()
    {
        iTween.MoveTo(_lookTarget, iTween.Hash("position", _camPointDefault, "time", _charMoveTime));

        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", _camStartPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget));
    }

    void BeginMoveForwardAnimation(GameObject go)
    {
		if(go != null) {
			GUIMainMenuCamera.OnLevelManagerEvent -= SelectStage;
	        Vector3 tmpPos = go.transform.position;
	        tmpPos.z = _charUnlockedDistance;
	
			LevelBoxesAppear(go);
	        iTween.MoveTo(go, iTween.Hash("position", tmpPos, "time", _charMoveTime, "oncomplete", "OnMoveForwardAnimationEnd", "oncompletetarget", gameObject, "oncompleteparams", go));

	        Animation characterAnimation = go.GetComponentInChildren<Animation>();
	        characterAnimation.CrossFade("Selection");
		}
    }

    void OnMoveForwardAnimationEnd(GameObject go)
    {
		GUIMainMenuCamera.OnLevelManagerEvent += SelectStage;
    }

    void LevelBoxesAppear(GameObject go)
    {
        LevelParentObject = go.transform.FindChild("LevelBoxes").gameObject;
        GameObject Arrow = go.transform.FindChild("lobbyArrow").gameObject;
        iTween.ScaleTo(Arrow, iTween.Hash("scale", new Vector3(0,0,0),"time", 0.5f, "easeType", _easyTypeOfLevelParentObjectOut));
        iTween.ScaleTo(LevelParentObject, iTween.Hash("scale", new Vector3(1,1,1),"time", 1f, "easeType", _easyTypeOfLevelParentObjectIn, "Delay", 0.5f));

        indexChar = _stageCharacters.IndexOf(go);
        minIndex = indexChar * 5;

        for(int i = minIndex; i < (minIndex + 5); i++)
        {
            if(_gameLevelsUnlocked[i])
            {
                if(go.name == "stage1Char")
                    _gameLevels[i].renderer.material.mainTexture = _levelUnlockedTutorialTexture;
                else
                    _gameLevels[i].renderer.material.mainTexture = _levelUnlockedTexture;

                if(highScores[i] > 0)
                {
                    int boxNumber = (i % 5) + 1;
                    GameObject box = LevelParentObject.transform.FindChild("Box" + boxNumber).gameObject;
                    box.transform.FindChild("Completed").gameObject.renderer.enabled = true;
                    int maxHighscore = SaveGame.GetMaxHighscores()[i];

                    if(highScores[i] >= System.Convert.ToInt32(maxHighscore * 0.25))
                        box.transform.FindChild("Star1").gameObject.renderer.enabled = true;
                    if(highScores[i] >= System.Convert.ToInt32(maxHighscore * 0.50))
                        box.transform.FindChild("Star2").gameObject.renderer.enabled = true;
                    if(highScores[i] >= System.Convert.ToInt32(maxHighscore * 0.75))
                        box.transform.FindChild("Star3").gameObject.renderer.enabled = true;
                }

            }
        }
    }

    void BeginMoveBackAnimation(GameObject go)
    {
        if(go != null) {
			Vector3 tmpPos = go.transform.position;
	        tmpPos.z = _charLockedDistance;
	
	        iTween.MoveTo(go, iTween.Hash("position", tmpPos, "time", _charMoveTime));

            Animation characterAnimation = go.GetComponentInChildren<Animation>();
            characterAnimation.CrossFade("SelectionBack");
            characterAnimation.CrossFadeQueued("Idle");

	        LevelBoxesDisappear(go);
		}
    }

    void LevelBoxesDisappear(GameObject go)
    {
        LevelParentObject = go.transform.FindChild("LevelBoxes").gameObject;
        iTween.ScaleTo(LevelParentObject, iTween.Hash("scale", new Vector3(0,0,0),"time", 0.3f, "easeType", _easyTypeOfLevelParentObjectOut));
        indexChar = _stageCharacters.IndexOf(go);
        minIndex = indexChar * _levelBoxCount;
    }

    void SelectLevel(GameObject go, Vector2 screenPos)
    {
        if(go != null && _gameLevels.Contains(go))
        {
            int index = _gameLevels.IndexOf(go);

            if(_gameLevelsUnlocked[index])
            {
                LoadingScreen.Load(index+1, true);
            }
        }
    }

    void ChangeViewToCredits()
    {
	
        if(_camAtRest)
        {
			state = MainMenuStates.Credits;

            _camAtRest = false;
			
			GUIMainMenuCamera.OnLevelManagerEvent -= SelectStage;
//            GestureManager.OnTap -= SelectStage;
//            GestureManager.OnTap -= SelectLevel;
    
            GameObject creditsCamPos = _creditsLookTarget.transform.FindChild("CamPointWallW").gameObject;
    
            iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _creditsLookTarget.transform.position, "time", _charMoveTime));

            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", creditsCamPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform,
                "oncomplete", "OnCreditCameraComplete", "oncompletetarget", gameObject, "easeType", _easeType));

            if(_selectedStageChar != null)
            {
                BeginMoveBackAnimation(_selectedStageChar);
            }
        }
    }

    void OnCreditCameraComplete()
    {
        SetCamAtRest();
		if(OnCreditsView != null)
        	OnCreditsView();
    }

    void ChangeViewToOptions()
    {
        if(_camAtRest)
        {
			state = MainMenuStates.Options;

            _camAtRest = false;
			
			GUIMainMenuCamera.OnLevelManagerEvent -= SelectStage;
//            GestureManager.OnTap -= SelectStage;
//            GestureManager.OnTap -= SelectLevel;
    
            GameObject optionsCamPos = _optionsLookTarget.transform.FindChild("CamPointWallE").gameObject;

            iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _optionsLookTarget.transform.position, "time", _charMoveTime));
    
            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", optionsCamPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform,
                "oncomplete", "OnOptionsCameraComplete", "oncompletetarget", gameObject, "easeType", _easeType));
    
            if(_selectedStageChar != null)
            {
                BeginMoveBackAnimation(_selectedStageChar);
            }
        }
    }

    void OnOptionsCameraComplete()
    {
        SetCamAtRest();
		if(OnOptionsView != null)
        	OnOptionsView();
    }

    void ChangeViewToMain()
    {
        if(_camAtRest)
        {
			state = MainMenuStates.MainMenu;
            _camAtRest = false;

            iTween.MoveTo(_lookTarget, iTween.Hash(("position"), _camPointDefault.transform.position, "time", _charMoveTime));

            iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", _camStartPos.transform.position, "time", _charMoveTime, "looktarget", _lookTarget.transform,
                "oncomplete", "OnMainViewCameraComplete", "oncompletetarget", gameObject, "easeType", _easeType));
        }
    }

    void OnMainViewCameraComplete()
    {
        SetCamAtRest();
		if(OnMainView != null)
        	OnMainView();
		GUIMainMenuCamera.OnLevelManagerEvent += SelectStage;		
//		GestureManager.OnTap += SelectStage;
//        GestureManager.OnTap += SelectLevel;
    }

    void SetCamAtRest()
    {
        _camAtRest = true;
    }
	
	public GameObject getLookTarget()
	{
		return _lookTarget;
	}
}
