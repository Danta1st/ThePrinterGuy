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
		public static int perfectInk;
		public static int perfectPaper;
		public static int perfectUran;
		public static int failedInk;
		public static int failedPaper;
		public static int failedUran;
		public static int _totalNodes;
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
	private Material _material = null;
	private float alphaFloat;
	
	[SerializeField]
	private float _fadeInTime = 0.2f;
	[SerializeField]
	private float _fadeOutTime = 0.2f;
		
	public delegate void FailedLevelAction(float score);
    public static event FailedLevelAction OnFailedLevel;
	
	public delegate void CompletedLevelAction(float score);
    public static event CompletedLevelAction OnCompletedLevel;
	
	// Use this for initialization
	void Awake()
	{
		_material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
	}
	
	void OnDisable()
	{
		GestureManager.OnTap -= CheckCollision;
	}
	
	void Start () 
	{
		if(!_isPrepared || _guiList == null)
			return;
		
		_guiCamera = GameObject.Find ("GUI Camera").camera;
		
		_scaleMultiplierX = Screen.width / 1920f;
		_scaleMultiplierY = Screen.height / 1200f;

		foreach(GameObject _guiObject in _guiList)
        {
            if(_guiObject.name == "IngameMenu")
            {
                nextLevelButton = _guiObject.transform.FindChild("NextButton").gameObject;
                if((_levelCompleted + 1) % 5 == 0 || _levelCompleted == ConstantValues.GetLastLevel || !_isPrepared)
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
		AdjustCameraSize();
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
				OnCompletedLevel(_lastScore);
			_isWin = true;
			LaunchEndScreen();
		}
		else
		{
			if(OnFailedLevel != null)
				OnFailedLevel(_lastScore);
			_isWin = false;
			nextLevelButton.SetActive(false);
			LaunchEndScreen();
		}
	}
	
	public void GoToHighScoreScreen(int level, int score, bool win)
	{
		_isPrepared = true;
		_levelCompleted = level;
		_lastScore = score;
		_win = win;
		
		if(_win)
			StartFadeHS(_fadeOutTime, _fadeInTime, Color.black);
		else
			StartFadeHS(_fadeOutTime, _fadeInTime, Color.red);
	}
	
	private void DrawQuad(Color aColor,float aAlpha)
    {
        aColor.a = aAlpha;
        _material.SetPass(0);
		
        GL.Color(aColor);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Vertex3(0, 0, -1);
        GL.Vertex3(0, 1, -1);
        GL.Vertex3(1, 1, -1);
        GL.Vertex3(1, 0, -1);
        GL.End();
        GL.PopMatrix();
    }
	
    private IEnumerator FadeToHSScreen(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        alphaFloat = 0.0f;
		
        while (alphaFloat<1.0f)
        {
            yield return new WaitForEndOfFrame();
            alphaFloat = Mathf.Clamp01(alphaFloat + Time.deltaTime / aFadeOutTime);
            DrawQuad(aColor, alphaFloat);
        }
		
		Application.LoadLevel(ConstantValues.GetHighScoreScreenLevel);
		
        while (alphaFloat>0.0f)
        {
            yield return new WaitForEndOfFrame();
	        alphaFloat = Mathf.Clamp01(alphaFloat - Time.deltaTime / aFadeInTime);
	        DrawQuad(aColor, alphaFloat);
        }
	}
	
	private void StartFadeHS(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
		StartCoroutine(FadeToHSScreen(aFadeOutTime, aFadeInTime, aColor));
    }
	
	private void AdjustCameraSize()
    {
        float _aspectRatio = 1920f / 1200f;
        float _startCameraSize = 600f;
        float _newCameraSize = _guiCamera.orthographicSize * _scaleMultiplierY;

        foreach(GameObject _guiObject in _guiList)
        {
			if(_guiObject == null)
			{
				Debug.Log(gameObject.name);
				return;
			}
			
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
					LoadingScreen.Load(_levelCompleted + 2, true);
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

                        string correspondingLevelName = null;
                        int indexOfNextLevel = _levelCompleted + 3;
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
                            LoadingScreen.Load(indexOfNextLevel, true);
                        }
                        else
                        {
                            LoadingScreen.Load(correspondingLevelName, true);
                        }
                    }
				}
            }
        }
    }
	
	private void LaunchEndScreen()
	{
		GestureManager.OnTap += CheckCollision;
		if(_isWin)
		{
			SoundManager.Effect_InGame_Win();
		}
		else
		{
			SoundManager.Effect_InGame_Lose();
		}
		_levelScore = _lastScore;
        FindHighscore();
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

            //Unlocking next level!
            if(highScores[_levelCompleted + 1] == -1)
                 highScores[_levelCompleted + 1] = 0;

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
		
		float startPos = scoreBarPos.y;
		float deltaScale = 10f;
		
		scoreBarPos.y = -0.5f;
		scoreBarScale.y = 0f;
		
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
				scoreBarScale.y = (float)i / (float)_targetScore.starScoreThree;
				deltaScale = scoreBarScale.y - _progressBar.transform.localScale.y;
				_progressBar.transform.localScale = scoreBarScale;
				scoreBarPos.y = scoreBarPos.y + deltaScale / 2f;
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
	
			yield return new WaitForSeconds(0.1f);
		}
		if(_isWin)
			InsertSpeechText(LocalizationText.GetText("WinText1"));
		else
			InsertSpeechText(LocalizationText.GetText("LossText1"));
	}
	
	private void InsertSpeechText(string text)
	{
        _speechText.transform.parent.transform.gameObject.SetActive(true);
		_speechText.text = text;
	}
	
}
