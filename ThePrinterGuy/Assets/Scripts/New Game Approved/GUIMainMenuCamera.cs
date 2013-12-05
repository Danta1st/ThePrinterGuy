using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIMainMenuCamera : MonoBehaviour
{

    #region Editor Publics
    [SerializeField] private LayerMask _layerMaskGUI;
    //List of all gui elements.
    [SerializeField] private GameObject[] _guiList;
    [SerializeField] private GameObject[] _textList;
    [SerializeField] private iTween.EaseType _easeTypeCamera;
	[SerializeField] private ButtonTextures _menuTextures;
    [SerializeField] private GameObject _danishCheck;
    [SerializeField] private GameObject _englishCheck;
    [SerializeField] private GameObject _soundCheck;
    [SerializeField] private GameObject _subtitleCheck;
    #endregion

    #region Private Variables
    private Camera _guiCamera;
	private Camera _creditsCamera;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
    private RaycastHit _hit;
    private bool _isGUI = true;
    private bool _canTouch = true;
	private bool _isOnStartScreen = true;
	private Credits credits;
	private GameObject _optionsButton;
	private GameObject _creditsButton;
	private GameObject _menuButtonLeft;
	private GameObject _menuButtonRight;
	private GameObject _janitorText;
	private GameObject _bossText;
	private GameObject _selectionText;
    private LevelManager _levelManager;
    GameObject _tutorialScaler;
    private Vector3 _tutorialTransformScale;
    private bool walkAnimationOver = false;

    private Vector3 _guiCameraMoveAmount;
    private float _guiCameraDuration = 1.0f;
	LevelManager lvlManager;
    #endregion


    #region Delegates & Events
    public delegate void MainScreen();
    public static event MainScreen OnMainScreen;

    public delegate void CreditScreen();
    public static event CreditScreen OnCreditScreen;

    public delegate void OptionsScreen();
    public static event OptionsScreen OnOptionsScreen;
	
	public delegate void LevelManagerEvent(GameObject go, Vector2 screenPos);
    public static event LevelManagerEvent OnLevelManagerEvent;
    #endregion

    #region Enable and Disable
    void OnEnable()
    {
        GestureManager.OnTap += CheckCollision;
        LevelManager.OnCreditsView += ChangeToCredits;
        LevelManager.OnMainView += ChangeToMain;
        LevelManager.OnOptionsView += ChangeToOptions;
    }

    void OnDisable()
    {
        GestureManager.OnTap -= CheckCollision;
        LevelManager.OnCreditsView -= ChangeToCredits;
        LevelManager.OnMainView -= ChangeToMain;
        LevelManager.OnOptionsView -= ChangeToOptions;
    }

    public void EnableGUICamera()
    {
        _isGUI = true;
        _guiCamera.gameObject.SetActive(true);
    }

    public void DisableGUICamera()
    {
        _isGUI = false;
        _guiCamera.gameObject.SetActive(false);
    }

    public void EnableGUIElement(string _name)
    {
        foreach(GameObject _gui in _guiList)
        {
			if(_gui == null)
				continue;
            if(_gui.name == _name)
            {
                _gui.SetActive(true);
            }
        }
    }

    public void DisableGUIElement(string _name)
    {
        foreach(GameObject _gui in _guiList)
        {
			if(_gui == null)
				continue;
            if(_gui.name == _name)
            {
                _gui.SetActive(false);
            }
        }
    }

    public void EnableGUIElementAll()
    {
        foreach(GameObject _gui in _guiList)
        {
			if(_gui == null)
				continue;
            _gui.SetActive(true);
        }
    }

    public void DisableGUIElementAll()
    {
        foreach(GameObject _gui in _guiList)
        {
			if(_gui == null)
				continue;
            _gui.SetActive(false);
        }
		ResetGuiTextures();
    }
    #endregion

    void Awake()
    {
        GameObject thisSoundRelay = GameObject.FindGameObjectWithTag("AudioRelay");

        if(thisSoundRelay == null)
        {
            Instantiate(Resources.Load("Prefabs/SoundRelay"));
        }
    }

    #region Start and Update
    // Use this for initialization
    void Start()
    {
        SoundManager.UnFadeAllMusic();
        SoundManager.TurnOnMenuSounds();
        _levelManager = gameObject.GetComponent<LevelManager>();
        _tutorialScaler = gameObject.transform.Find("TutorialScalar").gameObject;

        _danishCheck.renderer.enabled = false;
        _englishCheck.renderer.enabled = false;
        _soundCheck.renderer.enabled = false;
        _subtitleCheck.renderer.enabled = false;

		if(PlayerPrefs.HasKey("selectedLanguage"))
		{
            string selectedLanguage = PlayerPrefs.GetString("selectedLanguage");
			LocalizationText.SetLanguage(selectedLanguage);
            if(selectedLanguage == "EN")
                _englishCheck.renderer.enabled = true;
            else
                _danishCheck.renderer.enabled = true;
		}
		else
		{
			LocalizationText.SetLanguage("EN");
			PlayerPrefs.SetString("selectedLanguage", "EN");
            _englishCheck.renderer.enabled = true;
		}
        //HandleSoundOptionsButtons
        if(PlayerPrefs.HasKey("Sound"))
        {
            if(PlayerPrefs.GetString("Sound") == "On")
            {
                _soundCheck.renderer.enabled = true;
            }
        }
        else
        {
            PlayerPrefs.SetString("Sound", "On");
            _soundCheck.renderer.enabled = true;
        }

        //HandleSubtitleButtons
        if(PlayerPrefs.HasKey("Subtitle"))
        {
            if(PlayerPrefs.GetString("Subtitle") == "On")
            {
                _subtitleCheck.renderer.enabled = true;
            }
        }
        else
        {
            PlayerPrefs.SetString("Subtitle", "On");
            _subtitleCheck.renderer.enabled = true;
        }

        //GUI Camera and rescale of GUI elements.
        //--------------------------------------------------//
		lvlManager = gameObject.GetComponent<LevelManager>();
        _guiCamera = GameObject.FindGameObjectWithTag("GUICamera").camera;
        transform.position = _guiCamera.transform.position;
		credits = GameObject.Find("Television").GetComponent<Credits>();

        _scaleMultiplierX = Screen.width / 1920f;
        _scaleMultiplierY = Screen.height / 1200f;
        AdjustCameraSize();
        //--------------------------------------------------//

        //Find specific gui objects in the gui list.
        //--------------------------------------------------//
        foreach(GameObject _guiObject in _guiList)
        {
			if(_guiObject == null)
				continue;
            if(_guiObject.name == "LanguageMenu")
            {
                _guiObject.SetActive(true);
            }

            if(_guiObject.name == "MainMenu")
            {
                _guiObject.SetActive(false);
            }

            if(_guiObject.name == "OptionsMenu")
            {
                _guiObject.SetActive(false);
            }

            /*if(_guiObject.name == "Credits")
            {
                _guiObject.SetActive(false);
            }*/
            if(_guiObject.name == "MenuButtonLeft")
            {
				_menuButtonLeft = _guiObject;
                _guiObject.SetActive(false);
            }
            if(_guiObject.name == "MenuButtonRight")
            {
				_menuButtonRight = _guiObject;
                _guiObject.SetActive(false);
            }
            if(_guiObject.name == "CreditsButton")
            {
				_creditsButton = _guiObject;
                _guiObject.SetActive(false);
            }
            if(_guiObject.name == "OptionsButton")
            {
				_optionsButton = _guiObject;
                _guiObject.SetActive(false);
            }
        }
        //--------------------------------------------------//

        EnableGUICamera();
        SwitchToMainMenu();
		UpdateText();
        _tutorialTransformScale = _tutorialScaler.transform.localScale;
        _tutorialScaler.transform.localScale = new Vector3(0,0,0);


        SoundManager.Music_Menu_Main();
    }
    #endregion

    #region Class Methods
    private void AdjustCameraSize()
    {
        float _aspectRatio = 1920f / 1200f;
        float _startCameraSize = 600f;
        float _newCameraSize = _guiCamera.orthographicSize * _scaleMultiplierY;

        foreach(GameObject _guiObject in _guiList)
        {
			if(_guiObject == null)
				continue;
            _guiCamera.aspect = _aspectRatio;
            _guiCamera.orthographicSize = _startCameraSize;

            Vector3 _startPosition = _guiCamera.WorldToViewportPoint(_guiObject.transform.position);

            if(_guiObject.name == "BGMainMenu" || _guiObject.name == "BGOptions" || _guiObject.name == "BGCredits")
            {
                Vector3 _scale = new Vector3(_guiObject.transform.localScale.x * _scaleMultiplierX,
                                            _guiObject.transform.localScale.y * _scaleMultiplierY,
                                            _guiObject.transform.localScale.z);
                _guiObject.transform.localScale = _scale;
            }
            else
            {
                _guiObject.transform.localScale *= _scaleMultiplierY;
            }

            _guiCamera.ResetAspect();
            _guiCamera.orthographicSize = _newCameraSize;
            _guiObject.transform.position = _guiCamera.ViewportToWorldPoint(_startPosition);
        }
        _guiCamera.orthographicSize = _newCameraSize;
    }

    private void CheckCollision(GameObject _go, Vector2 _screenPosition)
    {
		Ray _mainCamRay = Camera.main.ScreenPointToRay(_screenPosition);
		
        if(_isGUI && _canTouch)
        {
            Ray _ray = _guiCamera.ScreenPointToRay(_screenPosition);

            if(Physics.Raycast(_ray, out _hit, 100, _layerMaskGUI.value))
            {
                //General GUI layer mask.
                //-----------------------------------------------------------------------//
                if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUI"))
                {
					var hitObject = _hit.collider.gameObject;
					
                    if(_hit.collider.gameObject.name == "SoundButton")
                    {
                        if(_soundCheck.renderer.enabled == true)
                        {
                            _soundCheck.renderer.enabled = false;
                            PlayerPrefs.SetString("Sound", "Off");
                            SoundManager.CheckAudioToogle();
                            //AudioListener.pause = true - does not seem to work! :(
                        }
                        else
                        {
                            _soundCheck.renderer.enabled = true;
                            PlayerPrefs.SetString("Sound", "On");
                            SoundManager.CheckAudioToogle();
                        }
                    }
                    else if(_hit.collider.gameObject.name == "SubtitleButton")
                    {
                        if(_subtitleCheck.renderer.enabled == true)
                        {
                            _subtitleCheck.renderer.enabled = false;
                            PlayerPrefs.SetString("Subtitle", "Off");
                            //Turn Subtitles OFF
                        }
                        else
                        {
                            _subtitleCheck.renderer.enabled = true;
                            PlayerPrefs.SetString("Subtitle", "On");
                            //Turn Subtitles ON
                        }
                    }

                    else if(_hit.collider.gameObject.name == "ResetButton")
                    {
                        SaveGame.ResetPlayerData();
                    }

                    else if(_hit.collider.gameObject.name == "OptionsButton")
                    {
                        if(OnOptionsScreen != null)
                        {
                            OnOptionsScreen();
							
							//Do fancy 'User Pressed a button' Animation
							SetTexture(hitObject, _menuTextures.optionsPressed);
							PunchButton(hitObject);
							//Reset
                            Invoke("DisableGUIElementAll", _punchTime);
                        }
                    }
                    else if(_hit.collider.gameObject.name == "CreditsButton")
                    {
                        if(OnCreditScreen != null)
                        {
                            OnCreditScreen();
							
							
							//Do fancy 'User Pressed a button' Animation
							SetTexture(hitObject, _menuTextures.creditsPressed);
							PunchButton(hitObject);
							//Reset
                            Invoke("DisableGUIElementAll", _punchTime);
							
							credits.SetCreditsRunning(true);
                        }
                    }
                    else if(_hit.collider.gameObject.name == "MenuButtonLeft")
                    {
                        if(OnMainScreen != null)
                        {
                            OnMainScreen();
							
							//Do fancy 'User Pressed a button' Animation
							SetTexture(hitObject, _menuTextures.leftPressed);
							PunchButton(hitObject);
							//Reset
                            Invoke("DisableGUIElementAll", _punchTime);
                        }
                    }
                    else if(_hit.collider.gameObject.name == "MenuButtonRight")
                    {
                        if(OnMainScreen != null)
                        {
                            OnMainScreen();
							credits.SetCreditsRunning(false);
                            DisableGUIElementAll();
                        }
                    }
                    else if(_hit.collider.gameObject.name == "DanishButton")
                    {
						PlayerPrefs.SetString("selectedLanguage", "DK");
                        _danishCheck.renderer.enabled = true;
                        _englishCheck.renderer.enabled = false;
                        LocalizationText.SetLanguage("DK");
                        UpdateText();
                    }
                    else if(_hit.collider.gameObject.name == "EnglishButton")
                    {
						PlayerPrefs.SetString("selectedLanguage", "EN");
                        _danishCheck.renderer.enabled = false;
                        _englishCheck.renderer.enabled = true;
                        LocalizationText.SetLanguage("EN");
                        UpdateText();
                    }
                    else if (_hit.collider.gameObject.name == "TakeTutorialNo")
                    {
                        GameObject checkMark = _hit.collider.gameObject;
                        PlayerPrefs.SetString("Tutorial", "Answered");
                        int[] highScore = SaveGame.GetPlayerHighscores();

                        for (int i = 0; i < 6; i++) {
                            highScore[i] = 0;
                        }

                        SaveGame.SavePlayerData(0,0,highScore);
                        foreach(GameObject stageChar in _levelManager.GetStageCharacters())
                        {
                            StageCharacter stageCharScript = stageChar.GetComponent<StageCharacter>();
                            stageCharScript.Unlock();
                        }
                        _levelManager.UnlockLevels();

                        ScaleArrowUpAndThenTutorialQuestionDown(checkMark);
                    }
                    else if (_hit.collider.gameObject.name == "TakeTutorialYes")
                    {   //GameObject checkMark = _hit.collider.gameObject.transform.Find("TakeTutorialYes").gameObject;
                        ScaleArrowUpAndThenTutorialQuestionDown(_hit.collider.gameObject);
                        Invoke("SwitchToJanitorFromMainMenu", 1f);
                    }

                    SoundManager.Effect_Menu_Click();
                }
                //-----------------------------------------------------------------------//
            }
            else if(Physics.Raycast(_mainCamRay, out _hit))
			{
				if(_isOnStartScreen && _hit.collider.gameObject.name == "TapToPlay")
            	{
                    SoundManager.Effect_Menu_Intro();               

					//Disable GUI Object
					_hit.collider.gameObject.SetActive(false);
					_isOnStartScreen = false;
					DisableGUIElement("MainMenu");
                    GameObject goLPRP = GameObject.Find ("levelPointerRotationPoint");
					GameObject goLM = GameObject.Find ("elevatorDoor_L_MoveTo");
					GameObject goRM = GameObject.Find ("elevatorDoor_R_MoveTo");
					GameObject goL = GameObject.Find ("elevatorDoor_L");
					GameObject goR = GameObject.Find ("elevatorDoor_R");
                    iTween.RotateTo(goLPRP, iTween.Hash("z", 90f, "time", 2, "easetype", iTween.EaseType.easeOutElastic));
					iTween.MoveTo(goL, iTween.Hash("position", goLM.transform.position,
                                                    "time", 2,
                                                    "easetype", iTween.EaseType.easeOutSine));
					iTween.MoveTo(goR, iTween.Hash("position", goRM.transform.position,
                                                    "time", 2,
                                                    "easetype", iTween.EaseType.easeOutSine));
					iTween.MoveTo(Camera.main.gameObject, iTween.Hash("path", iTweenPath.GetPath("intoLobby"),
                                                    "time", 3,
                                                    "easetype", iTween.EaseType.linear,
                                                    "looktarget", lvlManager.getLookTarget().transform,
                                                    "looktime", 3,
                                                    "oncomplete", "CheckForTutorial",
                                                    "oncompletetarget", gameObject));
				}
                else if(PlayerPrefs.GetString("Tutorial") == "Answered" && walkAnimationOver)
                {
					if(OnLevelManagerEvent != null)
						OnLevelManagerEvent(_go, _screenPosition);
				}
				
            }
        }
    }

    private void CheckForTutorial()
    {
        if(!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetString("Tutorial", "NotAnswered");
        }
        else if(PlayerPrefs.GetString("Tutorial") == "NotAnswered")
        {
           iTween.ScaleTo(_tutorialScaler, iTween.Hash("scale", _tutorialTransformScale, "time", 0.5f, "easeType", iTween.EaseType.easeOutBack));
        }
        else
        {
            SwitchToMainMenu();
            walkAnimationOver = true;
        }
    }

    private void ChangeToCredits()
    {
        DisableGUIElementAll();
        EnableGUIElement("Credits");
        EnableGUIElement("MenuButtonLeft");
    }

    private void ChangeToOptions()
    {
        DisableGUIElementAll();
        EnableGUIElement("OptionsMenu");
        EnableGUIElement("MenuButtonLeft");
    }

    private void ChangeToMain()
    {
        DisableGUIElementAll();
        EnableGUIElement("OptionsButton");
        //EnableGUIElement("CreditsButton");
    }

    private void SwitchToMainMenu()
    {
		if(LoadingScreen.isStart)
		{
			EnableGUIElement("MainMenu");
        	LoadingScreen.isStart = false;
			_isOnStartScreen = true;
		}
		else
		{
			iTweenPath path = Camera.main.GetComponent<iTweenPath>();
		
	        Camera.main.transform.position = path.nodes[path.nodes.Count - 1];
            walkAnimationOver = true;
			_isOnStartScreen = false;
			
        	EnableGUIElement("OptionsButton");
        	//EnableGUIElement("CreditsButton");
			

	        List<GameObject> stageChars = lvlManager.GetStageCharacters();
	
	        foreach(GameObject go in stageChars)
	        {
	            go.collider.enabled = true;
	        }
		}
    }

    private void SwitchToJanitorFromMainMenu()
    {
        OnLevelManagerEvent(_levelManager.GetStageCharacters()[0], new Vector3(0,0,0));
    }

    private void ScaleArrowUpAndThenTutorialQuestionDown(GameObject checkMark)
    {
        checkMark.transform.localScale = new Vector3(0,0,1);
        checkMark.renderer.enabled = true;
        iTween.ScaleTo(checkMark, iTween.Hash("scale", new Vector3(175,175,1), "time", 0.5f, "easeType", iTween.EaseType.easeOutBack));
        iTween.ScaleTo(_tutorialScaler, iTween.Hash("scale", new Vector3(0,0,0), "time", 0.5f, "easeType", iTween.EaseType.easeInBack, "delay", 0.5f));
        SwitchToMainMenu();
        PlayerPrefs.SetString("Tutorial", "Answered");
    }

    private void UpdateText()
    {
        foreach(GameObject _text in _textList)
        {
			if(_text != null)
            	_text.GetComponent<LocalizationKeywordText>().LocalizeText();
			else
				Debug.LogWarning(gameObject.name+" found no text in _textList");
			
			if(_text.name == "BossSelectionText")
			{
				_janitorText = _text;
			}
			if(_text.name == "JanitorSelectedText")
			{
				_bossText = _text;
			}
			if(_text.name == "SelectCharText")
			{
				_selectionText = _text;
			}
        }		
    }
	
	private void ResetGuiTextures()
	{        
		foreach(GameObject _guiObject in _guiList)
        {
			if(_guiObject == null)
				continue;
			
            if(_guiObject.name == "MenuButtonLeft")
            {
				SetTexture(_guiObject, _menuTextures.left);
            }			
            if(_guiObject.name == "OptionsButton")
            {
				SetTexture(_guiObject, _menuTextures.options);
            }
            if(_guiObject.name == "CreditsButton")
            {
				SetTexture(_guiObject, _menuTextures.credits);
            }
        }
		
	}
	
	private float _punchTime = 0.4f;
	private void PunchButton(GameObject button)
	{		
		iTween.PunchScale(button, new Vector3(35f, 35f, 35f), _punchTime);
	}
	private void PunchButtonPrecise(GameObject button, Vector3 scale)
	{		
		iTween.PunchScale(button, scale, _punchTime);
	}
	
	private void SetTexture(GameObject go, Texture2D texture)
	{
		go.renderer.material.mainTexture = texture;
	}
    #endregion
	
	[System.Serializable]
	public class ButtonTextures
	{
		public Texture2D options;
		public Texture2D optionsPressed;
		public Texture2D credits;
		public Texture2D creditsPressed;
		public Texture2D left;
		public Texture2D leftPressed;
	}
}

