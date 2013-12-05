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
    [SerializeField] private ButtonTextures _buttonTextures;
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
	private bool _isPaused = false;
    private GameObject _progressBar;
    private int _totalNodes;
    private float _currentSpawnedNodes;

    private List<GameObject> _guiSaveList = new List<GameObject>();

    //Ingame menu variables.
    private List<GameObject> _statsOverviewList = new List<GameObject>();
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

    //Action Sequencer
    private Vector3 _spawnPoint;
    private GameObject _sequencerObject;
    private GameObject _queuedObject;
    private Queue<GameObject> _sequencerObjectQueue = new Queue<GameObject>();
    private int _zone = 0;
    private bool _isLastNode = false;
	private GameObject _pausedGameObject = null;
	
	//Dialogue variables - Speech bubble
	private GameObject _speechTextObject;
	private GUISpeechText _guiSpeechTextScript;
	
	//Module script references
	private UraniumRods _uraniumReference;
	private PaperInsertion _paperReference;
	private Ink _inkReference;
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
        BpmSequencer.OnCreateNewNode += increaseSpawnedNodeCount;
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
        BpmSequencer.OnCreateNewNode -= increaseSpawnedNodeCount;
		BpmSequencer.OnLastNode -= LastNode;
		
        ScoreManager.OnTaskCompleted -= CheckZone;
    }
	
	void OnApplicationPause(bool status)
	{
		if(status == true && !_isPaused)
		{
	        if(OnPause != null)
	            OnPause();
			
			OpenIngameMenu();
		}
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
	
	private void GetModuleReferences()
	{
		if(GameObject.FindGameObjectWithTag("PopoutTask") != null)
			_uraniumReference = GameObject.FindGameObjectWithTag("PopoutTask").GetComponent<UraniumRods>();
		if(GameObject.FindGameObjectWithTag("TrayTask") != null)
			_paperReference = GameObject.FindGameObjectWithTag("TrayTask").GetComponent<PaperInsertion>();
		if(GameObject.FindGameObjectWithTag("InkTask") != null)
			_inkReference = GameObject.FindGameObjectWithTag("InkTask").GetComponent<Ink>();
	}
	
    #region Start and Update
    // Use this for initialization
    void Awake()
    {
        GameObject thisSoundRelay = GameObject.FindGameObjectWithTag("AudioRelay");

        if(thisSoundRelay == null)
        {
            Instantiate(Resources.Load("Prefabs/SoundRelay"));
        }
		
		GetModuleReferences();
    }

    void Start()
    {
        SoundManager.UnFadeAllMusic();
        SoundManager.TurnOnVoice();
        _progressBar = gameObject.transform.FindChild("StatsOverview").transform.FindChild("ProgressBar").transform.FindChild("Bar").gameObject;

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
			if(_guiObject.name == "IngameMenu")
			{
				Transform[] _tempList = _guiObject.GetComponentsInChildren<Transform>();
				foreach(Transform _go in _tempList)
				{
					_statsOverviewList.Add(_go.gameObject);
				}
				_guiObject.SetActive(false);
			}
			
			if(_guiObject.name == "PauseButton")
			{
				_statsOverviewList.Add(_guiObject.gameObject);
			}
			
			if(_guiObject.name == "BGIngameMenu")
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
	}
	
	private void ShowScore()
	{
		 _scoreValueObject.GetComponent<TextMesh>().text = _strScore;
		iTween.PunchScale(_scoreValueObject, new Vector3(3f,3f,0f),0.4f);	
	}
			
	public void PopupTextSmall(string _str)
	{
		//PopupText(_str, 4f, 10f, new Color(0.82f,0.55f,0.3f, 1f), new Color(0.82f,0.55f,0.3f, 0.5f));
		PopupText(_str, new Color(1f ,0.7f ,0f, 1f));
	}
	
	public void PopupTextMedium(string _str)
	{
		//PopupText(_str, 6f, 50f, new Color(0.7f,0.8f,0.84f, 1f), new Color(0.7f,0.8f,0.84f, 0.5f));
		PopupText(_str, new Color(1f ,0.7f ,0f, 1f));
	}
	
	public void PopupTextBig(string _str)
	{
		PopupText(_str, new Color(1f ,0.7f ,0f, 1f));
		//PopupText(_str);
	}
	
	public void PopupText(string _str, Color _particleColor)
	{
		StartCoroutine(InstantiatePopup(_str, _particleColor));
		
	}
	
	private IEnumerator InstantiatePopup(string _str, Color _particleColor)
	{	
		
		//float _xPopupPos = UnityEngine.Random.Range(_offsetValues.startX,_offsetValues.endX);
		float _xPopupPos = 0.125f;
		float _yPopupPos = 0.55f;
		float _fontSize = 150f;
		float _fadeInDuration = 0.5f;
		float _fadeOutDuration = 0.5f;
		float _punchAmmount = -10f;
		float _moveLength = -20.0f * _scaleMultiplierY;
		
		Vector3 _popupTextPos = _guiCamera.ViewportToWorldPoint(new Vector3(_xPopupPos,_yPopupPos, _guiCamera.nearClipPlane));
		_popupTextPos.z = 1f;
		
		GameObject _popupObject = (GameObject)Instantiate(_popupPrefab, _popupTextPos , Quaternion.identity);
		GameObject _popupTextObject = (GameObject)Instantiate(_popupTextPrefab, _popupTextPos , Quaternion.identity);
		
		ParticleSystem _particleSystem = _popupObject.GetComponentInChildren<ParticleSystem>();
		
		_particleSystem.particleSystem.startColor = _particleColor;
		
		_popupTextObject.GetComponent<TextMesh>().fontSize = Mathf.CeilToInt(_fontSize * _scaleMultiplierY);
		_popupTextObject.GetComponent<TextMesh>().text = _str;
		
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
				var hitObject = _hit.collider.gameObject;
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
						
						SetTexture(hitObject, _buttonTextures.pausePressed);
						//Punch & open the menu
						PunchButtonOnComplete(hitObject, "OpenIngameMenu");
                    }
                    else if(_hit.collider.gameObject.name == "ResumeButton")
                    {
						GestureManager.OnTap -= CheckCollision;
						SetTexture(hitObject, _buttonTextures.resumePressed);
						//punch and close menu
						PunchButtonOnComplete(hitObject, "CloseIngameMenu");
                    }
                    else if(_hit.collider.gameObject.name == "RestartButton")
                    {
                        if(OnRestart != null)
                            OnRestart();
						GestureManager.OnTap -= CheckCollision;
						SetTexture(hitObject, _buttonTextures.restartPressed);						
						//Punch & restart level
						PunchButtonOnComplete(hitObject, "RestartLevel");
                    }
                    else if(_hit.collider.gameObject.name == "QuitButton")
                    {
                        if(OnToMainMenuFromLevel != null)
                            OnToMainMenuFromLevel();
						
						GestureManager.OnTap -= CheckCollision;
						SetTexture(hitObject, _buttonTextures.homePressed);	
						//Punch and quit level
						PunchButtonOnComplete(hitObject, "QuitLevel");
                    }
                    else if(_hit.collider.gameObject.name == "SettingsButton")
                    {
						Settings();
                    }
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
		_isPaused = true;
        SaveGUIState();
        DisableGUIElement("Pause");
		ResetGuiTextures();
        EnableGUIElement("IngameMenu");
		EnableGUIElement("StatsOverview");
		EnableGUIElement("BGIngameMenu");
        float progressionInProcent = _currentSpawnedNodes / _totalNodes;
        _progressBar.renderer.material.SetFloat("_Progress", progressionInProcent);
		
		_pauseCurrScore.GetComponent<TextMesh>().text = GetScore().ToString();
		int[] highScores = SaveGame.GetPlayerHighscores();
		try
		{
			_pauseHighScore.GetComponent<TextMesh>().text = highScores[ConstantValues.GetLoadedLevelMinusStartLevels(Application.loadedLevel)].ToString();
		}
		catch(Exception)
		{
			_pauseHighScore.GetComponent<TextMesh>().text = "0";
		}


		
		iTween.MoveAdd(_statsOverviewObject, iTween.Hash("amount", _statsOverviewMoveAmount,
						"time", _statsOverviewDuration, "easetype", _easeTypeIngameMenu, "ignoretimescale", true));
		
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        SoundManager.StoreVolumes();
    }

    public void SetTotalNodes(int totalNodes)
    {
        _totalNodes = totalNodes;
    }

    public void increaseSpawnedNodeCount(string notUsed)
    {
        _currentSpawnedNodes++;
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
		GestureManager.OnTap += CheckCollision;
        Time.timeScale = 1.0f;
		_isPaused = false;
        AudioListener.pause = false;
        SoundManager.TurnOnMenuSounds();
        SoundManager.Effect_Menu_Click();
        SoundManager.FadeAllSourcesUp();
        //Resets the play icon back to the non-pressed.
        //FIXME
		//_hit.collider.gameObject.renderer.material.mainTexture = LevelPlayTexture;

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
		LoadGUIState();
		Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SoundManager.TurnOnMenuSounds();
        SoundManager.Effect_Menu_Click();
        SoundManager.FadeAllSourcesUp();
        LoadingScreen.Load(Application.loadedLevel, true);
    }
	
	//TODO: Quit button functionality.
    private void QuitLevel()
    {
		Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SoundManager.TurnOnMenuSounds();
        SoundManager.Effect_Menu_Click();
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

			BpmSequencerItem _bpmSequencerItem = _queuedObject.GetComponent<BpmSequencerItem>();
            //ActionSequencerItem _actionSequencerItemScript = _queuedObject.GetComponent<ActionSequencerItem>();
            _zone = _bpmSequencerItem.GetZoneStatus();
			
			//Check if player is in completion zone
			if(_zone == 2 || _zone == 3)
			{
	            if(OnTaskEnd != null)
	                OnTaskEnd(_currentTaskType, _zone);
				
	            EndZone(_queuedObject, true);
			}
			else
				ReTriggerTask(_bpmSequencerItem.GetTaskName());
        }
    }
	
	private void ReTriggerTask(string taskName)
	{
		switch(taskName)
		{
		case "Paper":
			_paperReference.ReTriggerLight();
			break;
		case "Ink":
			_inkReference.ReTriggerInkTask();
			break;
		case "UraniumRod":
			_uraniumReference.ReTriggerSpring();
			break;
		case "Barometer":
			break;
		default:
			Debug.LogWarning (gameObject.name+" could not reTrigger a task. Is a reference missing?");
			break;
		}
		
	}
	
	public void SetPauseElement(object obj)
	{
		if(obj != null)
			_pausedGameObject = (GameObject)obj;
		else
			_pausedGameObject = null;
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
		
		if(_pausedGameObject != null)
			_pausedGameObject.SetActive(false);

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
		
		if(_pausedGameObject != null)
			_pausedGameObject.SetActive(true);
		
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
	
	//Gui Helper Methods	
	private void ResetGuiTextures()
	{        
		foreach(GameObject _guiObject in _statsOverviewList)
        {
			if(_guiObject == null)
				continue;
			
			switch(_guiObject.name)
			{
			case "PauseButton":
				SetTexture(_guiObject, _buttonTextures.pause);
				break;
			case "ResumeButton":
				SetTexture(_guiObject, _buttonTextures.resume);
				break;
			case "QuitButton":
				SetTexture(_guiObject, _buttonTextures.home);
				break;
			case "RestartButton":
				SetTexture(_guiObject, _buttonTextures.restart);
				break;
			}
        }		
	}
	
	private float _punchTime = 0.4f;
	private void PunchButton(GameObject button)
	{		
		var scale = new Vector3(35f, 35f, 35f);
		iTween.PunchScale(button, iTween.Hash("amount", scale, "ignoretimescale", true, "time", _punchTime));
	}
	private void PunchButtonOnComplete(GameObject button, string onCompleteMethod)
	{		
		var scale = new Vector3(35f, 35f, 35f);
		iTween.PunchScale(button, iTween.Hash("amount", scale, "time", _punchTime, "ignoretimescale", true,
												"oncomplete", onCompleteMethod, "oncompletetarget", this.gameObject));
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
	
	[System.Serializable]
	public class TextPositionalOffset
	{
		public float startX = 0.125f;
		public float endX = 0.125f;
		public float startY = 0.75f;
		public float endY = 0.75f;
	}
	
	[System.Serializable]
	public class ButtonTextures
	{
		public Texture2D pause;
		public Texture2D pausePressed;
		public Texture2D resume;
		public Texture2D resumePressed;
		public Texture2D restart;
		public Texture2D restartPressed;
		public Texture2D home;
		public Texture2D homePressed;
	}
}

