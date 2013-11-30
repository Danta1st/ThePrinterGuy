using UnityEngine;
using System.Collections;

public class GUICutsceneCamera : MonoBehaviour 
{
    #region Editor Publics
    //List of all gui elements.
    [SerializeField]
    private GameObject[] _guiList;
    [SerializeField]
    private GameObject[] _textList;
    [SerializeField]
    private SubtitleTimer[] _subtitleTimerList;
    #endregion
	
    #region Private Variables
    private Camera _guiCamera;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
    
    private TimerUtilities _timer;
    private GameObject _bgText;
    private GameObject _textInFocus;
	private Vector3 _bgTextStartSize;
	private float _bgTextMultiplier;
	private float _currentTimeStamp;
    private float _timeStampText;
    private float _durationText;
    private int _textIndex = 0;
    private bool _isReady = true;
    private bool _isLast = false;
    private bool _subtitleOn = true;
    #endregion
	
    #region Enable and Disable
    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    public void EnableGUICamera()
    {
        _guiCamera.gameObject.SetActive(true);
    }

    public void DisableGUICamera()
    {
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
	
	// Use this for initialization
	void Start () 
	{
        if(PlayerPrefs.HasKey("Subtitle"))
        {
            if(PlayerPrefs.GetString("Subtitle") == "On")
            {
                _subtitleOn = true;
            }
            else
            {
                _subtitleOn = false;
            }
        }
        else
        {
            PlayerPrefs.SetString("Subtitle", "On");
            _subtitleOn = true;
        }

        if(_subtitleOn)
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
                if(_guiObject.name == "BGSubtitle")
                {
                	_bgText = _guiObject;
                	_bgTextStartSize = _bgText.transform.localScale;
                    _guiObject.SetActive(false);
                }
            }
            //--------------------------------------------------//

        	UpdateText();
        
        	foreach(GameObject _text in _textList)
        	{
        		_text.SetActive(false);
        	}

        	_timer = gameObject.AddComponent<TimerUtilities>();
        	UpdateSubtitle();
            EnableGUICamera();
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(_subtitleOn)
        {
        	_currentTimeStamp = Time.timeSinceLevelLoad;
        	if(_currentTimeStamp > _timeStampText && _isReady == true && _isLast == false)
        	{
        		ActivateSubtitle();
        		StartSubtitleTimer();
        		_isReady = false;
        	}
        	
        	if(_timer.GetTimeLeft() == 0 && _isReady == false)
        	{
        		DeactivateSubtitle();
        		if(_textIndex < _textList.Length)
        		{
        			UpdateSubtitle();
        		}
        		else
        		{
        			_isLast = true;
        		}
        		
        		_isReady = true;
        	}
        }
	}
	
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

            if(_guiObject.name == "BGBorderTop" || _guiObject.name == "BGBorderBottom")
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
    
    private void StartSubtitleTimer()
    {
    	_timer.StartTimer(_durationText, false);
    }
    
    private void ActivateSubtitle()
    {
    	_bgText.SetActive(true);
    	_textInFocus.SetActive(true);
    }
    
    private void DeactivateSubtitle()
    {
    	_bgText.SetActive(false);
    	_textInFocus.SetActive(false);
    }
    
    private void UpdateSubtitle()
    {
		_textInFocus = _textList[_textIndex];
		_timeStampText = _subtitleTimerList[_textIndex].timeStamp;
		_durationText = _subtitleTimerList[_textIndex].duration;
		
		float _textLength = _textInFocus.GetComponent<TextMesh>().text.Length;
		_bgTextMultiplier = _textLength / 50f;
		Vector3 _size = new Vector3(_bgTextStartSize.x*_bgTextMultiplier, _bgTextStartSize.y, _bgTextStartSize.z);
		_bgText.transform.localScale = _size;
		
		_textIndex++;
    }
    
    private void UpdateText()
    {
        foreach(GameObject _text in _textList)
        {
            _text.GetComponent<LocalizationKeywordText>().LocalizeText();
        }
    }
    #endregion
    
    [System.Serializable]
    public class SubtitleTimer
    {
    	public float timeStamp;
    	public float duration;
    }
}
