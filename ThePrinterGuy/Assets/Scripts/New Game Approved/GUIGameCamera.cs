using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIGameCamera : MonoBehaviour
{
    #region Editor Publics
    [SerializeField] private LayerMask _layerMaskGUI;
    //List of all gui elements.
    [SerializeField] private GameObject[] _guiList;
	[SerializeField] private GameObject[] _textList;
	[SerializeField] private iTween.EaseType _easeTypeIngameMenu = iTween.EaseType.easeOutBack;
	//Feedback prefabs for rewards (points)
	[SerializeField] private GameObject _popupPrefab;
	[SerializeField] private GameObject _popupTextPrefab;
	//Task Prefabs for the sequencer
    [SerializeField] private GameObject _inkPrefab;
    [SerializeField] private GameObject _paperPrefab;
    [SerializeField] private GameObject _uraniumRodPrefab;
    [SerializeField] private GameObject _barometerPrefab;
	[SerializeField] private float _secondsUntilEndScreen = 1.5f;
	[SerializeField] private iTween.EaseType _easeTypeTextMove;
	[SerializeField] private iTween.EaseType _easeTypeTextPunch;
	[SerializeField] private TextPositionalOffset _offsetValues;
    //Pressed and Non-pressed icon textures
    [SerializeField] private Texture2D LevelPlayTexture;
    [SerializeField] private Texture2D LevelPlayPressedTexture;
    [SerializeField] private Texture2D LevelReplayPressedTexture;
    [SerializeField] private Texture2D LevelQuitPressedTexture;
    #endregion

    #region Private Variables
    private GreenZone _greenZoneScript;
	private Camera _guiCamera;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
    private RaycastHit _hit;
    private bool _isGUI = false;
    private bool _canTouch = true;
    private float _timeScale = 0.0f;
	private ParticleSystem[] _particleSystems;
    private bool _waitForMe = false;
    private string _currentTaskType;
    private GameObject resumeButtom;

    private List<GameObject> _guiSaveList = new List<GameObject>();

    //Ingame menu variables.
    private GameObject _statsOverviewObject;
    private Vector3 _statsOverviewMoveAmount;
    private float _statsOverviewDuration = 1.0f;
	
	//PauseScreen Objects
	private GameObject _pauseCurrScore;
	private GameObject _pauseHighScore;
	
	//Highscore Variables.
	private int _score = 0;
	private string _strScore = "0";
	private GameObject _scoreValueObject;
	private GameObject _star1Object;
	private GameObject _star2Object;
	private GameObject _star3Object;
	private bool _isStar1Spawned;
	private bool _isStar2Spawned;
	private bool _isStar3Spawned;

    //Action Sequencer
    private Vector3 _spawnPoint;
    private GameObject _sequencerObject;
    private GameObject _queuedObject;
    private Queue<GameObject> _sequencerObjectQueue = new Queue<GameObject>();
    private int _zone = 0;
    private bool _isLastNode = false;
	
	//Dialogue variables - Speech bubble
	private GameObject _speechTextObject;
	private GUISpeechText _guiSpeechTextScript;
    #endregion
	
	#region Delegates & Events
	public delegate void OnUpdateActionAction();
	public static event OnUpdateActionAction OnUpdateAction;

    public delegate void GameEndedAction(int score);
    public static event GameEndedAction OnGameEnded;

    public delegate void PauseAction();
    public static event PauseAction OnPause;

    public delegate void RestartAction();
    public static event RestartAction OnRestart;

    public delegate void ToMainMenuFromLevelAction();
    public static event ToMainMenuFromLevelAction OnToMainMenuFromLevel;


    public delegate void TaskEndAction(string type, int zone);
    public static event TaskEndAction OnTaskEnd;
	#endregion

    #region Enable and Disable
    void OnEnable()
    {
        GestureManager.OnTap += CheckCollision;
//        ActionSequencerManager.OnCreateNewNode += InstantiateNodeAction;
//        ActionSequencerManager.OnLastNode += LastNode;
        BpmSequencer.OnPaperNode += SetCurrentTaskTypeToPaper;
        BpmSequencer.OnInkNode += SetCurrentTaskTypeToInk;
        BpmSequencer.OnUraniumRodNode += SetCurrentTaskTypeToRod;
        BpmSequencer.OnBarometerNode += SetCurrentTaskTypeToBarometer;
		
		BpmSequencer.OnCreateNewNode += InstantiateNodeAction;
		BpmSequencer.OnLastNode += LastNode;
		
        ScoreManager.OnTaskCompleted += CheckZone;

    }

    void OnDisable()
    {
        GestureManager.OnTap -= CheckCollision;
//        ActionSequencerManager.OnCreateNewNode -= InstantiateNodeAction;
//        ActionSequencerManager.OnLastNode -= LastNode;

        BpmSequencer.OnPaperNode -= SetCurrentTaskTypeToPaper;
        BpmSequencer.OnInkNode -= SetCurrentTaskTypeToInk;
        BpmSequencer.OnUraniumRodNode -= SetCurrentTaskTypeToRod;
        BpmSequencer.OnBarometerNode -= SetCurrentTaskTypeToBarometer;
		
		BpmSequencer.OnCreateNewNode -= InstantiateNodeAction;
		BpmSequencer.OnLastNode -= LastNode;
		
        ScoreManager.OnTaskCompleted -= CheckZone;
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
        _guiCamera = GameObject.FindGameObjectWithTag("GUICamera").camera;

		transform.position = _guiCamera.transform.position;

        _scaleMultiplierX = Screen.width / 1920f;
		_scaleMultiplierY = Screen.height / 1200f;
		AdjustCameraSize();
        //--------------------------------------------------//

        //Find specific gui objects in the gui list.
        //--------------------------------------------------//
		_pauseCurrScore = GameObject.Find("CurrentScoreValue").gameObject;
		_pauseHighScore = GameObject.Find("HighscoreValue").gameObject;
		
        foreach(GameObject _guiObject in _guiList)
        {
			if(_guiObject.name == "IngameMenu" || _guiObject.name == "BGIngameMenu")
			{
				_guiObject.SetActive(false);
			}
			
            if(_guiObject.name == "StatsOverview")
            {
				
                _statsOverviewObject = _guiObject;

                Vector3 _tempStatsOverviewPos = new Vector3(_statsOverviewObject.transform.position.x, _statsOverviewObject.transform.position.y, 1);
                _statsOverviewObject.transform.position = _tempStatsOverviewPos;

                _statsOverviewMoveAmount = new Vector3(0, 1100*_scaleMultiplierY, 0);
                _guiObject.SetActive(false);
            }
			
			if(_guiObject.name == "Highscore")
            {
                _scoreValueObject = _guiObject.transform.FindChild("ScoreValue").gameObject;
				_star1Object = _guiObject.transform.FindChild("Star1").gameObject;
				_star2Object = _guiObject.transform.FindChild("Star2").gameObject;
				_star3Object = _guiObject.transform.FindChild("Star3").gameObject;
            }

            if(_guiObject.name == "ActionSequencer")
            {
                _spawnPoint = _guiObject.transform.FindChild("SpawnZone").gameObject.transform.position;
            }
			
			if(_guiObject.name == "SpeechBubble")
			{
				_guiObject.SetActive(false);
				_speechTextObject = _guiObject.transform.FindChild("SpeechText").gameObject;
				_guiSpeechTextScript = _speechTextObject.GetComponent<GUISpeechText>();
			}
        }
        //--------------------------------------------------//
		
		UpdateText();
		
        EnableGUICamera();
		
		_star1Object.SetActive(false);
		_star2Object.SetActive(false);
		_star3Object.SetActive(false);
		
		_greenZoneScript = GameObject.Find("GreenZone").GetComponent<GreenZone>();
    }

    // Update is called once per frame
    void Update()
    {
	#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.N))
		{
			IncreaseScorePopup(1000);
		}
		if(Input.GetKeyDown(KeyCode.B))
		{
			IncreaseScorePopup(2000);
		}
		if(Input.GetKeyDown(KeyCode.V))
		{
			IncreaseScorePopup(10000);
		}
		
        if(Input.GetKeyDown(KeyCode.L))
        {
            CheckZone();
        }
		if(Input.GetKeyDown(KeyCode.M))
		{
			// Writes "SpeechBubbleExample" from the LocalizationText.xml
			EnableGUIElement("SpeechBubble");
			_guiSpeechTextScript.WriteText("SpeechBubbleExample");
		} 
		if(Input.GetKeyDown(KeyCode.Y))
		{
			PopupTextBig("PERFECT!");
		}
		if(Input.GetKeyDown(KeyCode.U))
		{
			PopupTextMedium("GREAT!");
		}
		if(Input.GetKeyDown(KeyCode.I))
		{
			PopupTextSmall("GOOD!");
		}
		if(Input.GetKeyDown(KeyCode.O))
		{
			PopupTextSmall("NOT BAD!");
		}
	#endif
    }
    #endregion
	
	#region Public methods
	public int GetQueueCount()
	{
		return _sequencerObjectQueue.Count;	
	}
	#endregion
	
    #region Class Methods

    #region Highscore
	public void IncreaseScore(float _amount)
	{
		
		_score += (int)_amount;
		_strScore = _score.ToString();
		ShowScore();
		ShowStars();
	}
	
	public void IncreaseScorePopup(float _amount)
	{
		_score += (int)_amount;
		_strScore = _score.ToString();
		string _strAmount = _amount.ToString();
		if(_amount <= 1000)
		{
			PopupTextSmall(_strAmount);
		}
		else if(_amount > 1000 && _amount < 10000)
		{
			PopupTextMedium(_strAmount);
		}
		else
		{
			PopupTextBig(_strAmount);
		}
		
		ShowScore();
		ShowStars();
	}
	
	private void ShowScore()
	{
		 _scoreValueObject.GetComponent<TextMesh>().text = _strScore;
		iTween.PunchScale(_scoreValueObject, new Vector3(3f,3f,0f),0.4f);	
	}
	
	private void ShowStars()
	{
		if(_score >= 10000 && _score < 25000)
		{
			_star1Object.SetActive(true);
			_star2Object.SetActive(false);
			_star3Object.SetActive(false);
			
			if(!_isStar1Spawned)
			{
				iTween.PunchScale(_star1Object, new Vector3(20f,20f,0f),1f);
				_isStar1Spawned = true;
			}
			
		}
		else if(_score >= 25000 && _score < 40000)
		{
			_star1Object.SetActive(true);
			_star2Object.SetActive(true);
			_star3Object.SetActive(false);
			
			if(!_isStar2Spawned)
			{
				iTween.PunchScale(_star2Object, new Vector3(20f,20f,0f),1f);
				_isStar2Spawned = true;
			}
			
		}
		else if(_score >= 40000)
		{
			_star1Object.SetActive(true);
			_star2Object.SetActive(true);
			_star3Object.SetActive(true);
			
			
			if(!_isStar3Spawned)
			{
				iTween.PunchScale(_star3Object, new Vector3(20f,20f,0f),1f);
				_isStar3Spawned = true;
			}
		}
		else
		{
			_star1Object.SetActive(false);
			_star2Object.SetActive(false);
			_star3Object.SetActive(false);
			
			_isStar1Spawned = false;
			_isStar2Spawned = false;
			_isStar3Spawned = false;
			
			
		}
	}
	
	public void PopupTextSmall(string _str)
	{
		PopupText(_str, 4f, 10f, new Color(0.82f,0.55f,0.3f, 1f), new Color(0.82f,0.55f,0.3f, 0.5f));
	}
	
	public void PopupTextMedium(string _str)
	{
		PopupText(_str, 6f, 50f, new Color(0.7f,0.8f,0.84f, 1f), new Color(0.7f,0.8f,0.84f, 0.5f));
	}
	
	public void PopupTextBig(string _str)
	{
		PopupText(_str, 10f, 100f, new Color(1f ,0.7f ,0f, 1f), new Color(1f ,0.7f ,0f, 0.7f));
	}
	
	public void PopupText(string _str, float _circles, float _starTrail, Color _trailColor, Color _circleColor)
	{
		StartCoroutine(InstantiatePopup(_str, _circles, _starTrail, _trailColor, _circleColor));
	}
	
	private IEnumerator InstantiatePopup(string _str, float _circles, float _starTrail, Color _trailColor, Color _circleColor)
	{	
		
		float _xPopupPos = UnityEngine.Random.Range(_offsetValues.startX,_offsetValues.endX);
		float _yPopupPos = UnityEngine.Random.Range(_offsetValues.startY,_offsetValues.endY);
		float _fontSize = 150f;
		float _fadeInDuration = 0.5f;
		float _fadeOutDuration = 1.2f;
		float _punchAmmount = -10f;
		float _moveLength = 600f * _scaleMultiplierY;
		
		Vector3 _popupTextPos = _guiCamera.ViewportToWorldPoint(new Vector3(_xPopupPos,_yPopupPos, _guiCamera.nearClipPlane));
		_popupTextPos.z = 1f;
		
		GameObject _popupObject = (GameObject)Instantiate(_popupPrefab, _popupTextPos , Quaternion.identity);
		GameObject _popupTextObject = (GameObject)Instantiate(_popupTextPrefab, _popupTextPos , Quaternion.identity);
		
		_popupTextObject.GetComponent<TextMesh>().fontSize = Mathf.CeilToInt(_fontSize * _scaleMultiplierY);
		_popupTextObject.GetComponent<TextMesh>().text = _str;
		
		_particleSystems = _popupObject.GetComponentsInChildren<ParticleSystem>();
		
		_particleSystems[0].particleSystem.emissionRate = _starTrail;
		_particleSystems[1].particleSystem.emissionRate = _circles;
		_particleSystems[0].particleSystem.startColor = _trailColor;
		_particleSystems[1].particleSystem.startColor = _circleColor;
		
		iTween.MoveTo(_popupTextObject, iTween.Hash("position", _popupTextPos + new Vector3(0f,_moveLength,0f), 
			"time", (_fadeInDuration + _fadeOutDuration), "easetype", _easeTypeTextMove));
		
		iTween.MoveTo(_popupObject, iTween.Hash("position", _popupTextPos + new Vector3(0f,_moveLength,0f), 
			"time", (_fadeInDuration + _fadeOutDuration), "easetype", _easeTypeTextMove));
		
		iTween.PunchScale(_popupTextObject, iTween.Hash("amount", new Vector3(_punchAmmount,_punchAmmount,0f), 
			"time", _fadeOutDuration, "easetype", _easeTypeTextPunch));
		
		iTween.FadeFrom(_popupTextObject, 0f, _fadeInDuration);
		yield return new WaitForSeconds(_fadeInDuration);
		iTween.FadeTo(_popupTextObject, 0f, _fadeOutDuration);
		yield return new WaitForSeconds(_fadeOutDuration);
		Destroy(_popupTextObject);
		Destroy(_popupObject);
		
	}

    public int GetScore()
    {
        return _score;
    }
    #endregion

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
            
			if(_guiObject.name == "BGIngameMenu")
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

                }
                //GUI Menu layer mask.
                //-----------------------------------------------------------------------//
                if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUIMenu"))
                {
                    if(_hit.collider.gameObject.name == "PauseButton")
                    {
                        if(OnPause != null)
                            OnPause();
                        OpenIngameMenu();
                    }
                    else if(_hit.collider.gameObject.name == "ResumeButton")
                    {
                        _hit.collider.gameObject.renderer.material.mainTexture = LevelPlayPressedTexture;
                        CloseIngameMenu();
                    }
                    else if(_hit.collider.gameObject.name == "RestartButton")
                    {
                        _hit.collider.gameObject.renderer.material.mainTexture = LevelReplayPressedTexture;
                        if(OnRestart != null)
                            OnRestart();
                        RestartLevel();
                    }
                    else if(_hit.collider.gameObject.name == "QuitButton")
                    {
                        _hit.collider.gameObject.renderer.material.mainTexture = LevelQuitPressedTexture;
                        if(OnToMainMenuFromLevel != null)
                            OnToMainMenuFromLevel();
                        QuitLevel();
                    }
                    else if(_hit.collider.gameObject.name == "SettingsButton")
                    {
						Settings();
                    }

                    SoundManager.Effect_Menu_Click();
                }
                //-----------------------------------------------------------------------//
            }
            else
            {

            }
        }
    }

    #region GUI Ingame Menu
    private void OpenIngameMenu()
    {
        SaveGUIState();
        DisableGUIElement("Pause");
        EnableGUIElement("IngameMenu");
		EnableGUIElement("StatsOverview");
		EnableGUIElement("BGIngameMenu");
		
		try
		{
			_pauseCurrScore.GetComponent<TextMesh>().text = GetScore().ToString();
			int[] highScores = SaveGame.GetPlayerHighscores();
			_pauseHighScore.GetComponent<TextMesh>().text = highScores[Application.loadedLevel].ToString();
		}
		catch(Exception e)
		{
			Debug.LogError("HighScore error! " + e.Message);
		}
		
		iTween.MoveAdd(_statsOverviewObject, iTween.Hash("amount", _statsOverviewMoveAmount,
						"duration", _statsOverviewDuration, "easetype", _easeTypeIngameMenu, "ignoretimescale", true));
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        SoundManager.StoreVolumes();
    }

    private void CloseIngameMenu()
    {
        float _start = 0.0f;
        float _end = 3.0f;
        _canTouch = false;
        UnpauseTimer(_start, _end);
    }

    private void UnpauseTimer(float _start, float _end)
    {
        while(true)
        {
            if(_start < _end)
            {
                _start++;
            }
            else
            {
                _canTouch = true;
				iTween.MoveAdd(_statsOverviewObject, iTween.Hash("amount", -_statsOverviewMoveAmount,
								"duration", _statsOverviewDuration, "easetype", _easeTypeIngameMenu, "ignoretimescale", true,
                    "oncomplete", "UnPauseTimeScale", "oncompletetarget", gameObject));
                break;

                //ToDo: Insert flashy numbers counting down from 3 to 1 --> Request from Nicolai.
                //This should happen after the menu has disappeared and the ingame gui appeared, but before the game resumes.
            }
        }
    }

    private void UnPauseTimeScale()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SoundManager.FadeAllSourcesUp();
        //Resets the play icon back to the non-pressed.
        _hit.collider.gameObject.renderer.material.mainTexture = LevelPlayTexture;

        LoadGUIState();

        while(!_waitForMe)
        {
            //NAILED IT!!!!
        }

        DisableGUIElement("IngameMenu");
        DisableGUIElement("StatsOverview");
        DisableGUIElement("BGIngameMenu");

    }

    private void DelayToStart()
    {
        LoadGUIState();
        DisableGUIElement("IngameMenu");
        DisableGUIElement("StatsOverview");
        DisableGUIElement("BGIngameMenu");
    }

    #endregion

    #region GUI Restart, Quit and Settings
    private void RestartLevel()
    {
		//TODO: Need confirmation before restart.
		Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SoundManager.FadeAllSourcesUp();
        LoadingScreen.Load(Application.loadedLevel, true);
    }
	
	//TODO: Quit button functionality.
    private void QuitLevel()
    {
		Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SoundManager.FadeAllSourcesUp();
        LoadingScreen.Load(ConstantValues.GetStartScene);
    }
	
	//TODO: Settings button functionality.
	private void Settings()
	{
		//Settings for level.	
	}
    #endregion

    #region Action Sequencer
    private void LastNode()
    {
        _isLastNode = true;
    }

    private void InstantiateNodeAction(string _itemName)
    {
        if(_itemName == "Paper")
        {
            _sequencerObject = _paperPrefab;
        }
        else if(_itemName == "Ink")
        {
            _sequencerObject = _inkPrefab;
        }
        else if(_itemName == "UraniumRod")
        {
            _sequencerObject = _uraniumRodPrefab;
        }
        else if(_itemName == "Barometer")
        {
            _sequencerObject = _barometerPrefab;
        }

        _spawnPoint = new Vector3(_spawnPoint.x, _spawnPoint.y, 4);
        GameObject _nodeItem = (GameObject)Instantiate(_sequencerObject, _spawnPoint, _sequencerObject.transform.localRotation);
		//TODO: Set _nodeItem parent to DynamicObjects
        _nodeItem.transform.localScale *= _scaleMultiplierY;
        _sequencerObjectQueue.Enqueue(_nodeItem);
		if(_sequencerObjectQueue.Count == 1)
		{
			if(OnUpdateAction != null)
	        {
	            OnUpdateAction();
	        }
		}
    }

    private void CheckZone()
    {
        if(_sequencerObjectQueue.Count > 0)
        {
            _queuedObject = _sequencerObjectQueue.Peek();

            Debug.Log(gameObject.name+" Setting _queuedObject to: "+_queuedObject.name);
			BpmSequencerItem _bpmSequencerItem = _queuedObject.GetComponent<BpmSequencerItem>();
            //ActionSequencerItem _actionSequencerItemScript = _queuedObject.GetComponent<ActionSequencerItem>();
            _zone = _bpmSequencerItem.GetZoneStatus();
			
            if(OnTaskEnd != null)
                OnTaskEnd(_currentTaskType, _zone);
			
            EndZone(_queuedObject, true);
        }
    }

    public void EndZone(GameObject _go, bool shouldDestroy)
    {
		_greenZoneScript.GreenOff();
		
		_sequencerObjectQueue.Dequeue();
        _sequencerObjectQueue.TrimExcess();
		
		if(shouldDestroy)
			Destroy(_go);
		
        _queuedObject = null;
		
        if(OnUpdateAction != null)
        {
            OnUpdateAction();
        }

        if(_isLastNode && _sequencerObjectQueue.Count == 0)
        {
            StartCoroutine("ShowEndScreen");
        }
    }
	
	public void DestroyTask(GameObject go)
	{
		Destroy(go);
	}

    public int GetZone()
    {
        return _zone;
    }
	
	private IEnumerator ShowEndScreen()
	{
		yield return new WaitForSeconds(_secondsUntilEndScreen);
		if(OnGameEnded != null)
        {
            OnGameEnded(_score);
        }
	}
    #endregion

    #region GUI Save and Load
    private void SaveGUIState()
    {
        _timeScale = Time.timeScale;
		
		if(_sequencerObjectQueue.Count > 0)
		{
			foreach(GameObject obj in _sequencerObjectQueue)
			{
				obj.SetActive(false);
			}
		}

        foreach(GameObject _gui in _guiList)
        {
            if(_gui.activeInHierarchy)
            {
                _guiSaveList.Add(_gui);
                _gui.SetActive(false);
            }
        }
    }

    private void LoadGUIState()
    {
        _waitForMe = false;
		
		if(_sequencerObjectQueue.Count > 0)
		{
			foreach(GameObject obj in _sequencerObjectQueue)
			{
				obj.SetActive(true);
			}
		}
		
        foreach(GameObject _gui in _guiSaveList)
        {
            _gui.SetActive(true);
        }

        _guiSaveList.Clear();

        Time.timeScale = _timeScale;
        _waitForMe = true;
    }
    #endregion
	
	private void UpdateText()
	{
		foreach(GameObject _text in _textList)
        {
            _text.GetComponent<LocalizationKeywordText>().LocalizeText();
        }
	}
	
    #endregion

    #region GA
    private void SetCurrentTaskTypeToPaper(int stateNumber)
    {
        _currentTaskType = "Paper";
    }
    private void SetCurrentTaskTypeToInk(int stateNumber)
    {
        _currentTaskType = "Ink";
    }
    private void SetCurrentTaskTypeToRod(int stateNumber)
    {
        _currentTaskType = "Rods";
    }
    private void SetCurrentTaskTypeToBarometer(int stateNumber)
    {
        _currentTaskType = "Barometers";
    }
    #endregion
}

[System.Serializable]
public class TextPositionalOffset
{
	public float startX = 0.35f;
	public float endX = 0.65f;
	public float startY = 0.35f;
	public float endY = 0.45f;
}