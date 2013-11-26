using UnityEngine;
using System.Collections;

public class GUIEndScreenCamera : MonoBehaviour 
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
	private GameObject[] _stars;
	[SerializeField]
	private GameObject _progressBar;
	[SerializeField]
	private TargetScores _targetScore;
	[SerializeField]
	private ParticleSystem _particle;
    [SerializeField]
    private int _lastLevelIndex;
	//[SerializeField]
	//private int _levelOffset = 0;
    #endregion
	
	[System.Serializable]
	public class TargetScores 
	{
		public int starScoreOne;
		public int starScoreTwo;
		public int starScoreThree;
	}
	
    #region Private Variables
    private Camera _guiCamera;
    private GameObject _realGUIList;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
    private RaycastHit _hit;
    private bool _isGUI = true;
    private bool _canTouch = true;
	private TextMesh _scoreText;
	private TextMesh _highScoreText;
	private TextMesh _speechText;
	private bool _isWin = false;
    private GameObject nextLevelButton;
    private int _levelOffset = 0;

    private Vector3 _guiCameraMoveAmount;
    private float _guiCameraDuration = 1.0f;
	private int _levelScore = 0;
	private int _currentLevel = 0;
	private int _levelHighscore = 0;
    #endregion
	
    #region Enable and Disable
    void OnEnable()
    {
		GUIGameCamera.OnGameEnded += DisplayEndScreenWin;
        StressOMeter.OnGameFailed += DisplayEndScreenLoose;
    }

    void OnDisable()
    {
		GUIGameCamera.OnGameEnded -= DisplayEndScreenWin;
        GestureManager.OnTap -= CheckCollision;
        StressOMeter.OnGameFailed -= DisplayEndScreenLoose;
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
	void Start () 
	{
        //GUI Camera and rescale of GUI elements.
        //--------------------------------------------------//
        _realGUIList = GameObject.Find("GUI List");
        _guiCamera = GameObject.Find("GUIEndSceneCamera").camera;
        transform.position = _guiCamera.transform.position;

     /*   foreach(GameObject _guiObject in _guiList)
        {
            if(_guiObject.name == "GUIButtons")
            {
                nextLevelButton = _guiObject.transform.FindChild("NextLevelButton").gameObject;
                if(Application.loadedLevel == 7)
                {
                    nextLevelButton.SetActive(false);
                }
            }
        };*/

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
			else if(_textObject.name == "SpeechText")
				_speechText = _textObject.GetComponent<TextMesh>();
        }
        //--------------------------------------------------//
		
		PlaceTargetStars();
		
		DisableGUICamera();
		DisableGUIElementAll();
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
                        GestureManager.OnTap -= CheckCollision;
						LoadingScreen.Load(Application.loadedLevel, true);
                    }
                    else if(_hit.collider.gameObject.name == "MainMenuButton")
                    {
                        GestureManager.OnTap -= CheckCollision;
						LoadingScreen.Load(ConstantValues.GetStartScene);
                    }
					else if(_hit.collider.gameObject.name == "NextLevelButton")
					{
                        if(Application.loadedLevel == _lastLevelIndex)
                        {
                            GestureManager.OnTap -= CheckCollision;
                            LoadingScreen.Load(ConstantValues.GetStartScene);
                        }
                        else
                        {
                            GestureManager.OnTap -= CheckCollision;

                            string correspondingLevelName = null;
                            int indexOfNextLevel = Application.loadedLevel+1;
                            switch (indexOfNextLevel) {
                                case 2:
                                    correspondingLevelName = "Stage1Cinematics";
                                    break;
                                case 3:
                                    correspondingLevelName = "Tutorial2";
                                    break;
                                case 4:
                                    correspondingLevelName = "Tutorial3";
                                    break;
                                case 5:
                                    correspondingLevelName = "Tutorial4";
                                    break;
                                case 6:
                                    correspondingLevelName = "Tutorial5";
                                    break;
                                default:
                                    break;
                            }
                            if(correspondingLevelName == null)
                            {
                                LoadingScreen.Load(indexOfNextLevel+1, true);
                            }
                            else
                            {
                                LoadingScreen.Load(correspondingLevelName, true);
                            }
                        }
					}
                }
                //-----------------------------------------------------------------------//
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
	
	private void DisplayEndScreenWin(int score)
	{
            SoundManager.Effect_InGame_Win();
            SoundManager.StopAllSoundEffects();
            SoundManager.FadeAllMusic();


		    GetCurrentLevel();
            //Unlocking the next level!
            if(!(_currentLevel == _lastLevelIndex))
            {
                int[] highScores = SaveGame.GetPlayerHighscores();
                highScores[_currentLevel+1] = 0;
                SaveGame.SavePlayerData(0,0,highScores);
            }
		    EnableGUICamera();
		    EnableGUIElementAll();
		    GestureManager.OnTap += CheckCollision;

		    _levelScore = score;
		    _isWin = true;
		    StartCoroutine("MoveEstimateBar");
	}

    private void DisplayEndScreenLoose()
    {
        SoundManager.Effect_InGame_Lose();
        SoundManager.StopAllSoundEffects();
        SoundManager.FadeAllMusic();

        GetCurrentLevel();
        //Camera.main.enabled = false;
        EnableGUICamera();
        EnableGUIElementAll();
        GestureManager.OnTap += CheckCollision;
        GUIGameCamera guiGameCameraScript = _realGUIList.GetComponent<GUIGameCamera>();

        _levelScore = guiGameCameraScript.GetScore();
        nextLevelButton.SetActive(false);
        _isWin = false;
        StartCoroutine("MoveEstimateBar");
    }
	
	private void PlaceTargetStars()
	{
		foreach(GameObject s in _stars)
		{
			s.SetActive(false);
		}
		
		Vector3 pos = _stars[0].transform.localPosition;
		float startX = -0.5f;
		float posOffset = 1;
		
		pos.x = startX + (float)_targetScore.starScoreOne / (float)_targetScore.starScoreThree;
		_stars[0].transform.localPosition = pos;
		pos.x = startX + (float)_targetScore.starScoreTwo / (float)_targetScore.starScoreThree;
		_stars[1].transform.localPosition = pos;
		pos.x = startX + (float)_targetScore.starScoreThree / (float)_targetScore.starScoreThree;
		_stars[2].transform.localPosition = pos;
	}
	
	private void ShowScore(int score)
	{
		_scoreText.text = score.ToString();
	}
	
	private void ShowHighscore(int score)
	{
		_highScoreText.text = score.ToString();
	}
	
	private void FindHighscore()
	{
		int currency = SaveGame.GetPlayerCurrency();
		int premiumCurrency = SaveGame.GetPlayerPremiumCurrency();
		int[] highScores = SaveGame.GetPlayerHighscores();
		_levelHighscore = highScores[_currentLevel - _levelOffset];
		
		if(_levelScore > _levelHighscore && _isWin)
		{
			highScores[_currentLevel - _levelOffset] = _levelScore;
			_levelHighscore = _levelScore;
			SaveGame.SavePlayerData(currency, premiumCurrency, highScores);
		}
		
		ShowHighscore(_levelHighscore);
	}
	
	IEnumerator MoveEstimateBar()
	{
		bool isScaling = true;
		Vector3 scoreBarPos = _progressBar.transform.localPosition;
		Vector3 scoreBarScale = _progressBar.transform.localScale;
		
		float startPos = scoreBarPos.x;
		float deltaScale = 0f;
		
		scoreBarPos.x = -0.5f;
		scoreBarScale.x = 0f;
		
		_progressBar.transform.localScale = scoreBarScale;
		_progressBar.transform.localPosition = scoreBarPos;
		
		for(int i = 0; i <= _levelScore;)
		{
			
			ShowScore(i);
			if(i >= _targetScore.starScoreThree && !_stars[2].activeSelf)
			{
				_particle.transform.position = _stars[2].transform.position;
				_particle.Play();
				_stars[2].SetActive(true);
			}
			if(i >= _targetScore.starScoreTwo && !_stars[1].activeSelf)
			{
				_particle.transform.position = _stars[1].transform.position;
				_particle.Play();
				_stars[1].SetActive(true);
			}
			if(i >= _targetScore.starScoreOne && !_stars[0].activeSelf)
			{
				_particle.transform.position = _stars[0].transform.position;
				_particle.Play();
				_stars[0].SetActive(true);
			}
			
			if(isScaling) {
				if(i >= _targetScore.starScoreThree)
					isScaling = false;
				scoreBarScale.x = (float)i / (float)_targetScore.starScoreThree;
				deltaScale = scoreBarScale.x - _progressBar.transform.localScale.x;
				_progressBar.transform.localScale = scoreBarScale;
				scoreBarPos.x = scoreBarPos.x + deltaScale / 2f;
				_progressBar.transform.localPosition = scoreBarPos;
			}
			
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
	
			yield return new WaitForSeconds(0.1f);
		}
		if(_isWin)
			InsertSpeechText(LocalizationText.GetText("WinText1"));
		else
			InsertSpeechText(LocalizationText.GetText("LossText1"));
		
		FindHighscore();
	}
			
	private void GetCurrentLevel()
	{
        _currentLevel = Application.loadedLevel - 2;
    }
	
	private void InsertSpeechText(string text)
	{
        _speechText.transform.parent.transform.gameObject.SetActive(true);
		_speechText.text = text;
	}
}
