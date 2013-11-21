﻿using UnityEngine;
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
    #endregion

    #region Private Variables
    private Camera _guiCamera;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
    private RaycastHit _hit;
    private bool _isGUI = true;
    private bool _canTouch = true;
	private bool _isOnStartScreen = true;

    private Vector3 _guiCameraMoveAmount;
    private float _guiCameraDuration = 1.0f;
	LevelManager lvlManager;

    public static string languageSetting = "EN";
    #endregion


    #region Delegates & Events
    public delegate void MainScreen();
    public static event MainScreen OnMainScreen;

    public delegate void CreditScreen();
    public static event CreditScreen OnCreditScreen;

    public delegate void OptionsScreen();
    public static event OptionsScreen OnOptionsScreen;
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
            _gui.SetActive(true);
        }
    }

    public void DisableGUIElementAll()
    {
        foreach(GameObject _gui in _guiList)
        {
            _gui.SetActive(false);
        }
    }
    #endregion

    #region Start and Update
    // Use this for initialization
    void Start()
    {
        //GUI Camera and rescale of GUI elements.
        //--------------------------------------------------//
		lvlManager = gameObject.GetComponent<LevelManager>();
        _guiCamera = GameObject.FindGameObjectWithTag("GUICamera").camera;
        transform.position = _guiCamera.transform.position;

        _scaleMultiplierX = Screen.width / 1920f;
        _scaleMultiplierY = Screen.height / 1200f;
        AdjustCameraSize();
        //--------------------------------------------------//

        //Find specific gui objects in the gui list.
        //--------------------------------------------------//
        foreach(GameObject _guiObject in _guiList)
        {
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

            if(_guiObject.name == "Credits")
            {
                _guiObject.SetActive(false);
            }
            if(_guiObject.name == "MenuButtonLeft")
            {
                _guiObject.SetActive(false);
            }
            if(_guiObject.name == "MenuButtonRight")
            {
                _guiObject.SetActive(false);
            }
            if(_guiObject.name == "CreditsButton")
            {
                _guiObject.SetActive(false);
            }
            if(_guiObject.name == "OptionsButton")
            {
                _guiObject.SetActive(false);
            }


        }
        //--------------------------------------------------//

        EnableGUICamera();
        SwitchToMainMenu();

    }
 
    // Update is called once per frame
    void Update()
    {
 		
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

                    }
                    else if(_hit.collider.gameObject.name == "MusicButton")
                    {
                        SaveGame.ResetPlayerData();
                    }
                    else if(_hit.collider.gameObject.name == "OptionsButton")
                    {
                        if(OnOptionsScreen != null)
                        {
                            OnOptionsScreen();

                            DisableGUIElementAll();
                        }
                    }
                    else if(_hit.collider.gameObject.name == "CreditsButton")
                    {
                        if(OnCreditScreen != null)
                        {
                            OnCreditScreen();

                            DisableGUIElementAll();
                        }
                    }
                    else if(_hit.collider.gameObject.name == "MenuButtonLeft")
                    {
                        if(OnMainScreen != null)
                        {
                            OnMainScreen();

                            DisableGUIElementAll();
                        }
                    }
                    else if(_hit.collider.gameObject.name == "MenuButtonRight")
                    {
                        if(OnMainScreen != null)
                        {
                            OnMainScreen();

                            DisableGUIElementAll();
                        }
                    }
                    else if(_hit.collider.gameObject.name == "DanishButton")
                    {
//                        if(_guiCameraStage == "LanguageSelectionStage")
//                        {
//                            SwitchToMainMenu();
//                        }

                        languageSetting = "DK";
                        LocalizationText.SetLanguage(languageSetting);
                        UpdateText();
                    }
                    else if(_hit.collider.gameObject.name == "EnglishButton")
                    {
//                        if(_guiCameraStage == "LanguageSelectionStage")
//                        {
//                            SwitchToMainMenu();
//                        }

                        languageSetting = "EN";
                        LocalizationText.SetLanguage(languageSetting);
                        UpdateText();
                    }
					
                }
                //-----------------------------------------------------------------------//
            }
            else
            {
                if(_isOnStartScreen)
				{
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
				}
            }
        }
    }

    private void ChangeToCredits()
    {
        DisableGUIElementAll();
        EnableGUIElement("Credits");
        EnableGUIElement("MenuButtonRight");
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
			_isOnStartScreen = false;
			
        	EnableGUIElement("OptionsButton");
        	EnableGUIElement("CreditsButton");
			
			

	        List<GameObject> stageChars = lvlManager.GetStageCharacters();
	
	        foreach(GameObject go in stageChars)
	        {
	            go.collider.enabled = true;
	        }
		}
    
//        DisableGUIElement("LanguageMenu"); 
    }
//
//    private void CheckLeft(GameObject go)
//    {
//        if(_guiCameraStage == "MainMenuStage")
//        {
//            _guiCameraStage = "CreditsStage";
//            iTween.MoveTo(_guiCamera.gameObject, iTween.Hash("position", _creditsPosition, "time", _guiCameraDuration));
//        }
//        else if(_guiCameraStage == "OptionsMenuStage")
//        {
//            _guiCameraStage = "MainMenuStage";
//            iTween.MoveTo(_guiCamera.gameObject, iTween.Hash("position", _mainMenuPosition, "time", _guiCameraDuration));
//        }
//    }
//
//    private void CheckRight(GameObject go)
//    {
//        if(_guiCameraStage == "MainMenuStage")
//        {
//            _guiCameraStage = "OptionsMenuStage";
//            iTween.MoveTo(_guiCamera.gameObject, iTween.Hash("position", _optionsPosition, "time", _guiCameraDuration));
//        }
//        else if(_guiCameraStage == "CreditsStage")
//        {
//            _guiCameraStage = "MainMenuStage";
//            iTween.MoveTo(_guiCamera.gameObject, iTween.Hash("position", _mainMenuPosition, "time", _guiCameraDuration));
//        }
//    }

    private void UpdateText()
    {
        foreach(GameObject _text in _textList)
        {
            _text.GetComponent<LocalizationKeywordText>().LocalizeText();
        }
    }
    #endregion
}
