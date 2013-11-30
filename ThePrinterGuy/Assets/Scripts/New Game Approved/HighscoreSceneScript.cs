using UnityEngine;
using System.Collections;

public class HighscoreSceneScript : MonoBehaviour 
{
	[SerializeField] private LayerMask _layerMaskGUI;
	[SerializeField] private GameObject[] _guiList;
	[SerializeField] private GameObject[] _stars;
	[SerializeField] private GameObject _progressBar;
	
	[SerializeField] private ParticleSystem _particle;
	[SerializeField] private GameObject[] _textList;
	
	public static class _targetScore 
	{
		public static int starScoreOne;
		public static int starScoreTwo;
		public static int starScoreThree;
	}
	
	private Camera _guiCamera;
	private float _scaleMultiplierX;
    private float _scaleMultiplierY;
	private RaycastHit _hit;
	private GameObject nextLevelButton;
	private bool _isWin = false;
	private int _levelScore = 0;
	private int _levelHighscore = 0;
	private TextMesh _scoreText;
	private TextMesh _highScoreText;
	private TextMesh _speechText;
	
	private static int _levelCompleted;
	private static int _lastScore;
	private static bool _win;
	private static bool _isPrepared = false;
	
	public delegate void FailedLevelAction(float score);
    public static event FailedLevelAction OnFailedLevel;
	
	public delegate void CompletedLevelAction(float score);
    public static event CompletedLevelAction OnCompletedLevel;
	
	// Use this for initialization
	void OnDisable()
	{
		GestureManager.OnTap -= CheckCollision;
	}
	
	void Start () 
	{
        //CRAZY HACK!!!
		if(!_isPrepared)
			return; //HACKY HACKY HACK
		//GoToHighScoreScreen(2, 1000, true, 200, 300, 1000); // TESTCODE - REMOVE LOAD FROM THE METHOD FIRST
		_guiCamera = GameObject.Find ("GUI Camera").camera;
		
		_scaleMultiplierX = Screen.width / 1920f;
		_scaleMultiplierY = Screen.height / 1200f;
		AdjustCameraSize();
		foreach(GameObject _guiObject in _guiList)
        {
            if(_guiObject.name == "IngameMenu")
            {
                nextLevelButton = _guiObject.transform.FindChild("NextButton").gameObject;
                if(_levelCompleted == ConstantValues.GetLastLevel || !_isPrepared)
                {
                    nextLevelButton.SetActive(false);
                }
				
            }
        }
		foreach(GameObject _textObject in _textList)
        {
            if(_textObject.name == "ScoreText")
				_scoreText = _textObject.GetComponent<TextMesh>();
			else if(_textObject.name == "HighScoreText")
				_highScoreText = _textObject.GetComponent<TextMesh>();
			else if(_textObject.name == "SpeechText")
				_speechText = _textObject.GetComponent<TextMesh>();
        }
		
		if(!_isPrepared)
		{
			Debug.LogError("HighscoreScene not properly Prepared! Use 'HighscoreSceneScript.PrepareHighScoreScreen(int, int, bool, int, int, int)' before switching to Highscore screen.");
			GestureManager.OnTap += CheckCollision;
			return;
		}
		_isPrepared = false;
		
		PlaceTargetStars();
		if(_win)
		{
			if(OnCompletedLevel != null)
				OnCompletedLevel((float)_lastScore);
			_isWin = true;
			LaunchEndScreen();
		}
		else
		{
			if(OnFailedLevel != null)
				OnFailedLevel((float)_lastScore);
			_isWin = false;
			nextLevelButton.SetActive(false);
			LaunchEndScreen();
		}
	}
	
	public void GoToHighScoreScreen(int level, int score, bool win, int starScoreOneInput, int starScoreTwoInput, int starScoreThreeInput)
	{
		_targetScore.starScoreOne = starScoreOneInput;
		_targetScore.starScoreTwo = starScoreTwoInput;
		_targetScore.starScoreThree = starScoreThreeInput;
		_isPrepared = true;
		_levelCompleted = level;
		_lastScore = score;
		_win = win;
		Application.LoadLevelAsync(ConstantValues.GetHighScoreScreenLevel);
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
        Ray _ray = _guiCamera.ScreenPointToRay(_screenPosition);

        if(Physics.Raycast(_ray, out _hit, 100, _layerMaskGUI.value))
        {
            if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUI"))
            {
                if(_hit.collider.gameObject.name == "RestartButton")
                {
                    GestureManager.OnTap -= CheckCollision;
					LoadingScreen.Load(_levelCompleted, true);
                }
                else if(_hit.collider.gameObject.name == "QuitButton")
                {
                    GestureManager.OnTap -= CheckCollision;
					LoadingScreen.Load(ConstantValues.GetStartScene, false);
                }
				else if(_hit.collider.gameObject.name == "NextButton")
				{
                    if(_levelCompleted == ConstantValues.GetLastLevel)
                    {
                        GestureManager.OnTap -= CheckCollision;
                        LoadingScreen.Load(ConstantValues.GetStartScene);
                    }
                    else
                    {
                        GestureManager.OnTap -= CheckCollision;
                        LoadingScreen.Load(_levelCompleted+1);
                    }
				}
            }
        }
    }
	
	private void LaunchEndScreen()
	{
		GestureManager.OnTap += CheckCollision;
		_levelScore = _lastScore;
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
		_levelHighscore = highScores[_levelCompleted];
		
		if(_levelScore > _levelHighscore && _isWin)
		{
			highScores[_levelCompleted] = _levelScore;
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
				i += 100;
			}
			else if(((_levelScore - i) / 100) > 1)
			{
				i += 10;
			}
			else if(((_levelScore - i) / 10) > 1)
			{
				i += 5;
			}
			else
			{
				i++;
			}
	
			yield return new WaitForSeconds(0.01f);
		}
		if(_isWin)
			InsertSpeechText(LocalizationText.GetText("WinText1"));
		else
			InsertSpeechText(LocalizationText.GetText("LossText1"));
		
		FindHighscore();
	}
	
	private void InsertSpeechText(string text)
	{
        _speechText.transform.parent.transform.gameObject.SetActive(true);
		_speechText.text = text;
	}
	
}
