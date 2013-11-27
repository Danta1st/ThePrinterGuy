using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIMainMenuCamera : MonoBehaviour
{

    #region Editor Publics
    [SerializeField]
    private LayerMask _layerMaskGUI;
    //List of all gui elements.
    [SerializeField]
    private GameObject[] _guiList;
    [SerializeField]
    private GameObject[] _textList;
    [SerializeField]
    private iTween.EaseType _easeTypeCamera;
	[SerializeField]
	private ButtonTextures _menuTextures;
    [SerializeField]
    private GameObject _danishCheck;
    [SerializeField]
    private GameObject _englishCheck;
    [SerializeField]
    private GameObject _soundCheck;
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
    }
    #endregion

    #region Start and Update
    // Use this for initialization
    void Start()
    {
        SoundManager.Music_Menu_Main();
        _danishCheck.renderer.enabled = false;
        _englishCheck.renderer.enabled = false;
        _soundCheck.renderer.enabled = false;

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
                Camera.main.audio.mute = false;
            }
            else
                 Camera.main.audio.mute = true;
        }
        else
        {
            PlayerPrefs.SetString("Sound", "On");
            _soundCheck.renderer.enabled = true;
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
                    if(_hit.collider.gameObject.name == "SoundButton")
                    {
                        if(_soundCheck.renderer.enabled == true)
                        {
                            _soundCheck.renderer.enabled = false;
                            PlayerPrefs.SetString("Sound", "Off");
                            //AudioListener.pause = true - does not seem to work! :(
                        }
                        else
                        {
                            _soundCheck.renderer.enabled = true;
                            PlayerPrefs.SetString("Sound", "On");
                        }
                    }
                    else if(_hit.collider.gameObject.name == "MusicButton")
                    {
                        SaveGame.ResetPlayerData();
                    }
                    else if(_hit.collider.gameObject.name == "OptionsButton")
                    {
                        if(OnOptionsScreen != null)
                        {
							//_optionsButton.renderer.material.mainTexture = _menuTextures.OptionsButtonPressed;
							
                            OnOptionsScreen();
                            DisableGUIElementAll();
                        }
                    }
                    else if(_hit.collider.gameObject.name == "CreditsButton")
                    {
                        if(OnCreditScreen != null)
                        {
                            OnCreditScreen();
							credits.SetCreditsRunning(true);
                            DisableGUIElementAll();
                        }
                    }
                    else if(_hit.collider.gameObject.name == "MenuButtonLeft")
                    {
                        if(OnMainScreen != null)
                        {
							_optionsButton.renderer.material.mainTexture = _menuTextures.OptionsButton;
                            OnMainScreen();

                            DisableGUIElementAll();
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

                    SoundManager.Effect_Menu_Click();
                }
                //-----------------------------------------------------------------------//
            }
            else if(Physics.Raycast(_mainCamRay,out _hit))
			{
				if(_isOnStartScreen && _hit.collider.gameObject.name == "TapToPlay")
            	{
                    SoundManager.Effect_Menu_Intro();               

					//Disable GUI Object
					_hit.collider.gameObject.SetActive(false);
					_isOnStartScreen = false;
					DisableGUIElement("MainMenu");
					GameObject goLM = GameObject.Find ("elevatorDoor_L_MoveTo");
					GameObject goRM = GameObject.Find ("elevatorDoor_R_MoveTo");
					GameObject goL = GameObject.Find ("elevatorDoor_L");
					GameObject goR = GameObject.Find ("elevatorDoor_R");
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
                                                    "oncomplete", "SwitchToMainMenu",
                                                    "oncompletetarget", gameObject));
				} else {
					if(OnLevelManagerEvent != null)
						OnLevelManagerEvent(_go, _screenPosition);
				}
				
            }
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
        EnableGUIElement("MenuButtonRight");
    }

    private void ChangeToMain()
    {
        DisableGUIElementAll();
        EnableGUIElement("OptionsButton");
        EnableGUIElement("CreditsButton");
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
			_isOnStartScreen = false;
			
        	EnableGUIElement("OptionsButton");
        	EnableGUIElement("CreditsButton");
			

	        List<GameObject> stageChars = lvlManager.GetStageCharacters();
	
	        foreach(GameObject go in stageChars)
	        {
	            go.collider.enabled = true;
	        }
		}
    }

    private void UpdateText()
    {
        foreach(GameObject _text in _textList)
        {
			if(_text != null)
            	_text.GetComponent<LocalizationKeywordText>().LocalizeText();
			else
				Debug.LogWarning(gameObject.name+" found no text in _textList");
        }		
    }
    #endregion
}

[System.Serializable]
public class ButtonTextures
{
	public Texture OptionsButton;
	public Texture OptionsButtonPressed;
	public Texture CreditsButton;
	public Texture CreditsButtonPressed;
	public Texture BackToLevelSelecButton;
	public Texture BackToLevelSelecButtonPressed;
}
