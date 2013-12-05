using UnityEngine;
using System.Collections;

public class HighscoreSceneScript : MonoBehaviour 
{
	[SerializeField] private LayerMask _layerMaskGUI;
	[SerializeField] private GameObject[] _guiList;
	[SerializeField] private GameObject[] _stars;
	[SerializeField] private GameObject _progressBar;
	[SerializeField] private GameObject _firedTextGameObject;
	[SerializeField] private GameObject _ClueNoteGameObject;
	
	[SerializeField] private ParticleSystem _particle;
	[SerializeField] private GameObject[] _textList;
	//Pressed and Non-pressed icon textures
	[SerializeField] private ButtonTextures _buttonTextures;

	public static class _targetScore 
	{
		public static float starScoreOne;
		public static float starScoreTwo;
		public static float starScoreThree;
		public static float perfectInk;
		public static float perfectPaper;
		public static float perfectUran;
		public static float failedInk;
		public static float failedPaper;
		public static float failedUran;
		public static float _totalPaperNodes;
		public static float _totalInkNodes;
		public static float _totalUranNodes;
		public static float _totalNodesHit;
		public static float _totalNodes;
	}
	
	private Camera _guiCamera;
	private float _scaleMultiplierX;
    private float _scaleMultiplierY;
	private RaycastHit _hit;
	private GameObject nextLevelButton;
	private bool _isWin = false;
	private int _levelScore = 0;
	private int _levelHighscore = 0;
	private int _countingScore = 0;
	
	private TextMesh _scoreText;
	private TextMesh _highScoreText;
	private TextMesh _speechText;
	
	private static int _levelCompleted;
	private static int _lastScore;
	private static bool _win;
	private static bool _isPrepared = false;
	private static float _levelMaxscore = 0;
	private Material _material = null;
	private float alphaFloat;
    private bool _waitTime = false;
	
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
//			if(OnFailedLevel != null)
//				OnFailedLevel(_lastScore);
			_isWin = false;
			nextLevelButton.SetActive(false);
			LaunchEndScreen();
		}
	}
	
	public void GoToHighScoreScreen(int level, int score, bool win, int maxScore)
	{
		_isPrepared = true;
		_levelCompleted = level;
		_lastScore = score;
		_win = win;
		_levelMaxscore = maxScore;
		
		if(_win)
			StartFadeHS(_fadeOutTime, _fadeInTime, Color.black);
		else
			StartFadeHS(_fadeOutTime, _fadeInTime, new Color(220, 20, 60));
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
			var hitObject = _hit.collider.gameObject;
            if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUI"))
            {
                if(_hit.collider.gameObject.name == "RestartButton")
                {
					GestureManager.OnTap -= CheckCollision;
					
					SetTexture(hitObject, _buttonTextures.restartPressed);						
					//Punch & restart level
					PunchButtonOnComplete(hitObject, "RestartLevel");                    
                }
                else if(_hit.collider.gameObject.name == "QuitButton")
                {
                    GestureManager.OnTap -= CheckCollision;
					
					SetTexture(hitObject, _buttonTextures.homePressed);	
					//Punch and quit level
					PunchButtonOnComplete(hitObject, "QuitLevel");					
                }
				else if(_hit.collider.gameObject.name == "NextButton")
				{
					GestureManager.OnTap -= CheckCollision;
					SetTexture(hitObject, _buttonTextures.playPressed);	
					//Punch and quit level
					PunchButtonOnComplete(hitObject, "NextLevel");
				}
            }
        }
    }
	
	private void RestartLevel()
	{
		StartCoroutine(PlayNotFired());
	}
	
	private void NextLevel()
	{
		if(_levelCompleted == ConstantValues.GetLastLevel)
        {
            LoadingScreen.Load(ConstantValues.GetStartScene);
        }
        else
        {
            string correspondingLevelName = null;
            int indexOfNextLevel = _levelCompleted + ConstantValues.GetLevel1;
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
	
	private void QuitLevel()
	{
		LoadingScreen.Load(ConstantValues.GetStartScene, false);
	}
	
	private float _punchTime = 0.4f;
	private void PunchButtonOnComplete(GameObject button, string onCompleteMethod)
	{		
		var scale = new Vector3(35f, 35f, 35f);
		iTween.PunchScale(button, iTween.Hash("amount", scale, "time", _punchTime, "ignoretimescale", true,
												"oncomplete", onCompleteMethod, "oncompletetarget", this.gameObject));
	}
	
	private void SetTexture(GameObject go, Texture2D texture)
	{
		go.renderer.material.mainTexture = texture;
	}
	
    IEnumerator PlayNotFired()
    {
        SoundManager.Voice_Boss_EndScene_NotFired1();

        yield return new WaitForSeconds(3.5f);

        _waitTime = true;

        GestureManager.OnTap -= CheckCollision;
        LoadingScreen.Load(_levelCompleted + 2, true);
    }
	
	private void LaunchEndScreen()
	{
		GestureManager.OnTap += CheckCollision;
		if(_isWin)
		{
//			SoundManager.Effect_InGame_Win();
		}
		else
		{
//			SoundManager.Effect_InGame_Lose();
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
		
		/*Vector3 pos = _stars[0].transform.localPosition;
		float startX = -0.5f;
		float posOffset = 1;
		
		pos.x = startX + (float)_targetScore.starScoreOne / (float)_targetScore.starScoreThree;
		_stars[0].transform.localPosition = pos;
		pos.x = startX + (float)_targetScore.starScoreTwo / (float)_targetScore.starScoreThree;
		_stars[1].transform.localPosition = pos;
		pos.x = startX + (float)_targetScore.starScoreThree / (float)_targetScore.starScoreThree;
		_stars[2].transform.localPosition = pos;*/
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
		if(_isWin)
        {
            _firedTextGameObject.SetActive(false);

            //<--------------------***------------------->
             float _percent;
        
             Transform go = GameObject.Find("PerfectFailedTexts").transform;
             GameObject clueNoteStartPos = GameObject.Find ("ClueNoteStartPos");
             _ClueNoteGameObject.transform.position = clueNoteStartPos.transform.position;
             float temp = 0;
             temp = (_targetScore._totalNodesHit +_targetScore.failedInk + _targetScore.failedPaper + _targetScore.failedUran) / _targetScore._totalNodes;
             
             GameObject.Find("TotalPrints").renderer.material.SetFloat("_Progress", temp);
        
             foreach(Transform child in go)
             {
                 switch(child.name)
                 {
                 case "FailedInkText":
                     child.GetComponent<TextMesh>().text = _targetScore.failedInk + "/" + _targetScore._totalInkNodes;
                     break;
                 case "FailedPaperText":
                     child.GetComponent<TextMesh>().text = _targetScore.failedPaper + "/" + _targetScore._totalPaperNodes;
                     break;
                 case "FailedRodsText":
                     child.GetComponent<TextMesh>().text = _targetScore.failedUran + "/" + _targetScore._totalUranNodes;
                     break;
                 case "FailedText":
                     temp = _targetScore.failedInk + _targetScore.failedPaper + _targetScore.failedUran;
                     child.GetComponent<TextMesh>().text = temp + "/" + _targetScore._totalNodes;
                     break;
                 case "FailedPercentsText":
                     temp = System.Convert.ToInt32(((_targetScore.failedInk + _targetScore.failedPaper + _targetScore.failedUran) / _targetScore._totalNodes) * 100);
                     child.GetComponent<TextMesh>().text = temp + "%";
                     break;
                 case "PerfectsInkText":
                     child.GetComponent<TextMesh>().text = _targetScore.perfectInk + "/" + _targetScore._totalInkNodes;
                     break;
                 case "PerfectsPaperText":
                     child.GetComponent<TextMesh>().text = _targetScore.perfectPaper + "/" + _targetScore._totalPaperNodes;
                     break;
                 case "PerfectsRodsText":
                     child.GetComponent<TextMesh>().text = _targetScore.perfectUran + "/" + _targetScore._totalUranNodes;
                     break;
                 case "PerfectsText":
                     temp = _targetScore.perfectInk + _targetScore.perfectPaper + _targetScore.perfectUran;
                     child.GetComponent<TextMesh>().text = temp + "/" + _targetScore._totalNodes;
                     break;
                 case "PerfectsPercentText":
                     temp = System.Convert.ToInt32(((_targetScore.perfectInk + _targetScore.perfectPaper + _targetScore.perfectUran) / _targetScore._totalNodes) * 100);
                     child.GetComponent<TextMesh>().text = temp + "%";
                     break;
                 }
             }
        
             SoundManager.CutScene_Effect_Point();
             iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", _levelScore, "time", 2, "easetype", iTween.EaseType.easeInCubic, "onupdate", "updateCountingScoreValue",
                    "oncomplete", "StopLoopingPointSound", "oncompletetarget", gameObject));
             GameObject scoreTopPoint = GameObject.Find ("ScoreTopPoint");
             GameObject scoreBotPoint = GameObject.Find ("ScoreBotPoint");
             GameObject scoreText = GameObject.Find ("ScoreText");
             GameObject clueNoteMoveTo = GameObject.Find ("ClueNoteMoveTo");

        
             float difference;
             Vector3 newPos;
        
             for(int i = 0; i <= _levelScore;)
             {
                 ShowScore(_countingScore);
                 if(_countingScore >= _targetScore.starScoreThree && !_stars[2].activeSelf)
                 {
                     _particle.transform.position = _stars[2].transform.position;
                     _particle.Play();
                     _stars[2].SetActive(true);
                 }
                 if(_countingScore >= _targetScore.starScoreTwo && !_stars[1].activeSelf)
                 {
                     _particle.transform.position = _stars[1].transform.position;
                     _particle.Play();
                     _stars[1].SetActive(true);
                 }
                 if(_countingScore >= _targetScore.starScoreOne && !_stars[0].activeSelf)
                 {
                     _particle.transform.position = _stars[0].transform.position;
                     _particle.Play();
                     _stars[0].SetActive(true);
                 }

                 _percent = (_countingScore / _levelMaxscore);
                 difference = scoreTopPoint.transform.position.y - scoreBotPoint.transform.position.y;
                 difference = difference * _percent;
                 newPos = scoreBotPoint.transform.position;
                 newPos.y += difference;
                 scoreText.transform.position = newPos;
                 _progressBar.renderer.material.SetFloat("_Progress", _percent);
                 
                 i = _countingScore + 1;
                 yield return new WaitForSeconds(0.01f);
             }

            yield return new WaitForSeconds(0.75f);

            iTween.MoveTo(_ClueNoteGameObject, iTween.Hash("position", clueNoteMoveTo.transform.position, "easetype", iTween.EaseType.linear, "time", 1));
        }
		else if(!_isWin)
        {
			_progressBar.renderer.material.SetFloat("_Progress", 0f);
            yield return new WaitForSeconds(0.25f);


            if(OnFailedLevel != null)
               OnFailedLevel(_lastScore);

            yield return new WaitForSeconds(1.65f);

            //ToDo: Insert correct sound here, currently placeholder...
            SoundManager.CutScene_Effect_Coffee_01();

            _firedTextGameObject.SetActive(true);
        }
	}
	
	private void updateCountingScoreValue(int score)
	{
		_countingScore = score;
	}

    private void StopLoopingPointSound()
    {
        SoundManager.StopPointSound();
    }
	
	[System.Serializable]
	public class ButtonTextures
	{
		public Texture2D play;
		public Texture2D playPressed;
		public Texture2D restart;
		public Texture2D restartPressed;
		public Texture2D home;
		public Texture2D homePressed;
	}
}

	
