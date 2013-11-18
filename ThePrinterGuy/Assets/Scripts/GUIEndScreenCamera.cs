using UnityEngine;
using System.Collections;

public class GUIEndScreenCamera : MonoBehaviour {
	
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
	private GameObject _guiCam;
	[SerializeField]
	private GameObject _mainCam;
	[SerializeField]
	private GameObject[] _stars;
	[SerializeField]
	private GameObject _progressBar;
	[SerializeField]
	private TargetScores[] _targetScore;
    #endregion
	
	[System.Serializable]
	public class TargetScores {
		public int starScoreOne;
		public int starScoreTwo;
		public int starScoreThree;
		
	}
	
    #region Private Variables
    private Camera _guiCamera;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
    private RaycastHit _hit;
    private bool _isGUI = true;
    private bool _canTouch = true;
	private TextMesh _scoreText;
	private TextMesh _highScoreText;

    private Vector3 _guiCameraMoveAmount;
    private float _guiCameraDuration = 1.0f;
	private int _levelScore = 0;
	private int _currentLevel = 0;
    #endregion
	
    #region Enable and Disable
    void OnEnable()
    {
		GUIGameCamera.OnGameEnded += DisplayEndScreen;
    }

    void OnDisable()
    {
		GUIGameCamera.OnGameEnded -= DisplayEndScreen;
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
	
	// Use this for initialization
	void Start () {
        //GUI Camera and rescale of GUI elements.
        //--------------------------------------------------//
        _guiCamera = GameObject.Find("GUIEndSceneCamera").camera;
        transform.position = _guiCamera.transform.position;

        _scaleMultiplierX = Screen.width / 1920f;
        _scaleMultiplierY = Screen.height / 1200f;
        AdjustCameraSize();
        //--------------------------------------------------//

        //Find specific gui objects in the text list.
        //--------------------------------------------------//
        foreach(GameObject _textObject in _textList)
        {
            if(_textObject.name == "ScoreText")
				_scoreText = _textObject.GetComponent<TextMesh>();
			else if(_textObject.name == "HighScoreText")
				_highScoreText = _textObject.GetComponent<TextMesh>();
        }
        //--------------------------------------------------//
		if(GUIMainMenuCamera.languageSetting == "EN")
		{
			LocalizationText.SetLanguage(GUIMainMenuCamera.languageSetting);
//			UpdateText();
		}
		else if(GUIMainMenuCamera.languageSetting == "DK")
		{
			LocalizationText.SetLanguage(GUIMainMenuCamera.languageSetting);
//			UpdateText();
		}
		
		PlaceTargetStars();
		
		DisableGUICamera();
		DisableGUIElementAll();
		
		DisplayEndScreen(30000);

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

            if(_guiObject.name == "BG")
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
                    if(_hit.collider.gameObject.name == "RestartButton")
                    {
						Application.LoadLevel(0);
                    }
                    else if(_hit.collider.gameObject.name == "MainMenuButton")
                    {
						Application.LoadLevel(1);
                    }
					else if(_hit.collider.gameObject.name == "NextLevelButton")
					{
						
					}
                }
                //-----------------------------------------------------------------------//
            }
            else
            {

            }
        }
    }
	
//	private void UpdateText()
//	{
//		foreach(GameObject _text in _textList)
//        {
//            _text.GetComponent<LocalizationKeywordText>().LocalizeText();
//        }
//	}
	
	private void DisplayEndScreen(int score)
	{
		_mainCam.camera.enabled = false;
		_guiCam.camera.enabled = false;
		EnableGUICamera();
		EnableGUIElementAll();
		GestureManager.OnTap += CheckCollision;
		_levelScore = score;

		StartCoroutine("MoveEstimateBar");
	}
	
	private void PlaceTargetStars()
	{
		Vector3 pos = _stars[0].transform.localPosition;
		float startX = -0.5f;
		float posOffset = 1;
		
		pos.x = startX + (float)_targetScore[_currentLevel].starScoreOne / (float)_targetScore[_currentLevel].starScoreThree;
		_stars[0].transform.localPosition = pos;
		pos.x = startX + (float)_targetScore[_currentLevel].starScoreTwo / (float)_targetScore[_currentLevel].starScoreThree;
		_stars[1].transform.localPosition = pos;
		pos.x = startX + (float)_targetScore[_currentLevel].starScoreThree / (float)_targetScore[_currentLevel].starScoreThree;
		_stars[2].transform.localPosition = pos;
	}
	
	private void ShowScore(int score)
	{
		_scoreText.text = score.ToString();
	}
	
	private void FindHighScore()
	{
		//TODO	
	}
	
	private void RestartGame()
	{
		GestureManager.OnTap -= CheckCollision;
		Application.LoadLevel(1);
	}
	
	IEnumerator MoveEstimateBar()
	{
		
		Vector3 scoreBarPos = _progressBar.transform.localPosition;
		Vector3 scoreBarScale = _progressBar.transform.localScale;
		
		float startPos = scoreBarPos.x;
		float deltaScale = 0f;
		
		scoreBarPos.x = -0.5f;
		scoreBarScale.x = 0f;
		
		_progressBar.transform.localScale = scoreBarScale;
		_progressBar.transform.localPosition = scoreBarPos;
		
		for(int i = 0; i < _levelScore;)
		{
			if(((_levelScore - i) / 1000) > 1)
			{
				i += 1000;
			}
			else if(((_levelScore - i) / 100) > 1)
			{
				i += 100;
			}
			else if(((_levelScore - i) / 50) > 1)
			{
				i += 50;
			}
			else if(((_levelScore - i) / 10) > 1)
			{
				i += 10;
			}
			else
			{
				i++;
			}
			ShowScore(i);
			
			scoreBarScale.x = (float)i / (float)_targetScore[_currentLevel].starScoreThree;
			deltaScale = scoreBarScale.x - _progressBar.transform.localScale.x;
			_progressBar.transform.localScale = scoreBarScale;
			scoreBarPos.x = scoreBarPos.x + deltaScale / 2f;
			_progressBar.transform.localPosition = scoreBarPos;
	
			yield return new WaitForSeconds(0.1f);
		}
		
	}
			
	private void getCurrentLevel()
	{
	}

}
