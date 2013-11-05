using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIGameCamera : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private LayerMask _layerMaskGUI;
    //List of all gui elements.
    [SerializeField]
    private GameObject[] _guiList;
    //List of tool box pages.
    [SerializeField]
    private GameObject[] _toolBoxPages;
	[SerializeField]
    private GameObject[] _textList;
	[SerializeField]
	private iTween.EaseType _easeTypeToolBox;
	[SerializeField]
	private iTween.EaseType _easeTypeIngameMenu;
    #endregion

    #region Private Variables
	private int _score = 0;
	private string _scoreString;
	private Camera _guiCamera;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
    private RaycastHit _hit;
    private bool _isGUI = false;
    private bool _canTouch = true;
    private float _timeScale = 0.0f;
	

    private List<GameObject> _guiSaveList = new List<GameObject>();

    //Ingame menu variables.
    private GameObject _statsOverviewObject;
    private Vector3 _statsOverviewMoveAmount;
    private float _statsOverviewDuration = 1.0f;

    //Tool box variables.
    private GameObject _toolBoxSelectionObject;
    private GameObject _toolBoxObject;
    private Vector3 _toolBoxMoveAmount;
	private float _toolBoxMoveDuration = 1.0f;
	private int _toolBoxCurrentPageCount = 1;
    private int _toolBoxMaxPageCount;
    private bool _isToolBoxOpen = false;
    #endregion

    #region Delegates and Events
    public delegate void InventoryPress(string buttonName);
    public static event InventoryPress OnInventoryPress;
    #endregion

    #region Enable and Disable
    void OnEnable()
    {
        GestureManager.OnTap += CheckCollision;
    }

    void OnDisable()
    {
        GestureManager.OnTap -= CheckCollision;
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

            if(_guiObject.name == "ToolBox")
            {
                _toolBoxObject = _guiObject;

                Vector3 _tempToolBoxPos = new Vector3(_toolBoxObject.transform.position.x, _toolBoxObject.transform.position.y, 5);
                _toolBoxObject.transform.position = _tempToolBoxPos;
    
                _toolBoxMoveAmount = new Vector3(200*_scaleMultiplierY, 0, 0);
                _toolBoxMaxPageCount = _toolBoxPages.Length;
            }

            if(_guiObject.name == "ToolBoxSelection")
            {
                _toolBoxSelectionObject = _guiObject;
                _toolBoxSelectionObject.SetActive(false);
            }
        }
        //--------------------------------------------------//

        EnableGUICamera();
		
		

    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.N))
		{
			IncreaseScore(100);
			PopupText(_scoreString);
		}
    }
    #endregion

    #region Class Methods
	public void IncreaseScore(int _ammount)
	{
		_score += _ammount;	
		_scoreString = _score.ToString();
	}
	
	private void ShowScore()
	{
		
	}
	
	public void PopupText(string _str)
	{
		StartCoroutine("InstantiatePopup", _str);
	}
	
	private IEnumerator InstantiatePopup(string _str)
	{
		
		float _xPopupPos = Random.Range(0.35f,0.65f);
		float _yPopupPos = Random.Range(0.3f,0.4f);
		
		Vector3 _popupTextPos = _guiCamera.ViewportToWorldPoint(new Vector3(_xPopupPos,_yPopupPos, _guiCamera.nearClipPlane));
		_popupTextPos.z = 1f;
		GameObject _popupTextPrefab = Resources.Load("GUIPopupText") as GameObject;
		GameObject _popupTextObject = (GameObject)Instantiate(_popupTextPrefab, _popupTextPos , Quaternion.identity);
		
		float _fontSize = 200f;
		
		_popupTextObject.GetComponent<TextMesh>().fontSize = Mathf.CeilToInt(_fontSize * _scaleMultiplierY);
		
		_popupTextObject.GetComponent<TextMesh>().text = _str;
		
		float _fadeInDuration = 0.5f;
		float _fadeOutDuration = 1.2f;
		float _punchAmmount = -10f;
		float _moveLength = 600f * _scaleMultiplierY;
		
		iTween.MoveTo(_popupTextObject, _popupTextPos + new Vector3(0f,_moveLength,0f), 
			_fadeInDuration + _fadeOutDuration);
		iTween.PunchScale(_popupTextObject, new Vector3(_punchAmmount,_punchAmmount,0f), 
			_fadeOutDuration);
		iTween.FadeFrom(_popupTextObject, 0f, _fadeInDuration);
		yield return new WaitForSeconds(_fadeInDuration);
		iTween.FadeTo(_popupTextObject, 0f, _fadeOutDuration);
		yield return new WaitForSeconds(_fadeOutDuration);
		Destroy(_popupTextObject);
		
	}
	
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
                        OpenIngameMenu();
                    }
                    else if(_hit.collider.gameObject.name == "ResumeButton")
                    {
                        CloseIngameMenu();
                    }
                    else if(_hit.collider.gameObject.name == "RestartButton")
                    {
                        RestartLevel();
                    }
                    else if(_hit.collider.gameObject.name == "QuitButton")
                    {
                        QuitLevel();
                    }
                    else if(_hit.collider.gameObject.name == "SettingsButton")
                    {
						Settings();
                    }
                }
                //ToolBox layer mask.
                //-----------------------------------------------------------------------//
                if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUIToolBox"))
                {
                    if(_hit.collider.gameObject.name == "ToolBoxButton")
                    {
						OpenToolBox();
                    }
                    else if(_hit.collider.gameObject.name == "ToolBoxUpButton")
                    {
                        UpdateToolBoxPage(true);
                    }
                    else if(_hit.collider.gameObject.name == "ToolBoxDownButton")
                    {
                        UpdateToolBoxPage(false);
                    }
                    else
                    {
						UpdateToolBoxSelection(_hit.collider.gameObject);
                    }
                }
                //-----------------------------------------------------------------------//
            }
            else
            {
                _toolBoxSelectionObject.SetActive(false);
            }
        }
    }
	
    #region GUI ToolBox
	private void OpenToolBox()
	{
        if(_isToolBoxOpen)
        {
            _isToolBoxOpen = false;
			iTween.MoveAdd(_toolBoxObject, iTween.Hash("amount", -_toolBoxMoveAmount,
							"duration", _toolBoxMoveDuration, "easetype", _easeTypeToolBox));
        }
        else
        {
            _isToolBoxOpen = true;
			iTween.MoveAdd(_toolBoxObject, iTween.Hash("amount", _toolBoxMoveAmount,
							"duration", _toolBoxMoveDuration, "easetype", _easeTypeToolBox));
        }
		_toolBoxSelectionObject.SetActive(false);
	}
	
    private void UpdateToolBoxPage(bool _isUp)
    {
		if(_isUp)
		{
            _toolBoxCurrentPageCount -= 1;
            if(_toolBoxCurrentPageCount < 1)
            {
                _toolBoxCurrentPageCount = 1;
            }			
		}
		else
		{
             _toolBoxCurrentPageCount += 1;
            if(_toolBoxCurrentPageCount > _toolBoxMaxPageCount)
            {
                _toolBoxCurrentPageCount = _toolBoxMaxPageCount;
            }			
		}
		
        int _index = 0;
        foreach(GameObject _toolBoxPage in _toolBoxPages)
        {
            _index++;

            if(_index == _toolBoxCurrentPageCount)
            {
                _toolBoxPage.SetActive(true);
            }
            else
            {
                _toolBoxPage.SetActive(false);
            }
        }
		
		_toolBoxSelectionObject.SetActive(false);
    }
	
	private void UpdateToolBoxSelection(GameObject _buttonObject)
	{
        if(OnInventoryPress != null)
        {
            OnInventoryPress(_buttonObject.name);
        }

        _toolBoxSelectionObject.SetActive(true);
        Vector3 _tempPos = _buttonObject.transform.position;
        _tempPos.z = 6;
        _toolBoxSelectionObject.transform.position = _tempPos;		
	}
    #endregion

    #region GUI Ingame Menu
    private void OpenIngameMenu()
    {
        SaveGUIState();
        DisableGUIElement("Pause");
        EnableGUIElement("IngameMenu");
		EnableGUIElement("StatsOverview");
		EnableGUIElement("BGIngameMenu");
		
		iTween.MoveAdd(_statsOverviewObject, iTween.Hash("amount", _statsOverviewMoveAmount,
						"duration", _statsOverviewDuration, "easetype", _easeTypeIngameMenu));
    }

    private void CloseIngameMenu()
    {
        float _start = 0.0f;
        float _end = 3.0f;
        _canTouch = false;
        StartCoroutine(UnpauseTimer(_start, _end));
    }

    IEnumerator UnpauseTimer(float _start, float _end)
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
								"duration", _statsOverviewDuration, "easetype", _easeTypeIngameMenu));
                yield return new WaitForSeconds(_statsOverviewDuration+0.1f);
                LoadGUIState();
				DisableGUIElement("IngameMenu");
				DisableGUIElement("StatsOverview");
				DisableGUIElement("BGIngameMenu");
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }
    #endregion

    #region GUI Restart, Quit and Settings
    private void RestartLevel()
    {
		//Need confirmation before restart.
        Application.LoadLevel(Application.loadedLevel);
    }
	
	//TODO: Quit button functionality.
    private void QuitLevel()
    {
        //Application.LoadLevel("SOMETHING SCENE");
    }
	
	//TODO: Settings button functionality.
	private void Settings()
	{
		//Settings for level.	
	}
    #endregion

    #region GUI Save and Load
    private void SaveGUIState()
    {
        _timeScale = Time.timeScale;

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
        Time.timeScale = _timeScale;

        foreach(GameObject _gui in _guiSaveList)
        {
            _gui.SetActive(true);
        }
        _guiSaveList.Clear();
    }
    #endregion

    #endregion
}
