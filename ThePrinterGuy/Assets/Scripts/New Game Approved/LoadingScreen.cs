using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour 
{
	[SerializeField]
	private int _loadingScreenLevelNumber;
	[SerializeField]
	private int _startScreenLevelNumber;
	[SerializeField]
	private int[] _tutorialLevels;
	[SerializeField]
	private int _tutorialTipsAmount;
	[SerializeField]
	private int _funnyTipsAmount;
	[SerializeField]
	private int _practicalTipsAmount;
	[SerializeField]
	private static float _fadeInTime = 0.2f;
	[SerializeField]
	private static float _fadeOutTime = 0.2f;
	[SerializeField]
	private static bool _skipTap = false;
	
    private static LoadingScreen instance;
	private AsyncOperation Async;
	private float alphaFloat;
    private Material _material = null;
    private string _levelName = "";
    private int _levelIndex = 0;
    private bool _fading = false;
	
	public static bool isStart = true;
 
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
		_material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
        DontDestroyOnLoad(this); 
    }
	
	public static void Load(int index, bool skipTap)
    {
        if (NoInstance()) return;
		if (Fading) return;
		_skipTap = skipTap;
        instance._levelName = "";
        instance._levelIndex = index;
        instance.StartFade(_fadeOutTime, _fadeInTime, Color.black);
    }
 
    public static void Load(string name, bool skipTap)
    {
        if (NoInstance()) return;
		if (Fading) return;
		_skipTap = skipTap;
		instance._levelName = name;
        instance.StartFade(_fadeOutTime, _fadeInTime, Color.black);
    }
	
    public static void Load(int index)
    {
        if (NoInstance()) return;
		if (Fading) return;
        instance._levelName = "";
        instance._levelIndex = index;
        instance.StartFade(_fadeOutTime, _fadeInTime, Color.black);
    }
 
    public static void Load(string name)
    {
        if (NoInstance()) return;
		if (Fading) return;
		instance._levelName = name;
        instance.StartFade(_fadeOutTime, _fadeInTime, Color.black);
    }
 
    static bool NoInstance()
    {
        if (!instance)
            Debug.LogError("Loading Screen is not existing in scene.");
        return !instance;
    }
	
    public static bool Fading
    {
        get { return instance._fading; }
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
 
    private IEnumerator FadeToLoadScreen(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        alphaFloat = 0.0f;
		
        while (alphaFloat<1.0f)
        {
            yield return new WaitForEndOfFrame();
            alphaFloat = Mathf.Clamp01(alphaFloat + Time.deltaTime / aFadeOutTime);
            DrawQuad(aColor, alphaFloat);
        }
		
		if(_loadingScreenLevelNumber != null)
			Application.LoadLevel(_loadingScreenLevelNumber);
		
        while (alphaFloat>0.0f)
        {
            yield return new WaitForEndOfFrame();
	        alphaFloat = Mathf.Clamp01(alphaFloat - Time.deltaTime / aFadeInTime);
	        DrawQuad(aColor, alphaFloat);
        }
        StartCoroutine(FadeToLevel(aFadeOutTime, aFadeInTime, aColor));
		
    }
	
	private IEnumerator FadeToLevel(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
		//AdjustCameraSize();
		//LoadingWheel LW = GameObject.Find("LoadingEffect").GetComponent<LoadingWheel>();
		LocalizationKeywordText TapToCont = GameObject.Find("ContinueText").GetComponent<LocalizationKeywordText>();
		LocalizationKeywordText TipText = GameObject.Find("TipText").GetComponent<LocalizationKeywordText>();
		GameObject RotationPointLoadArrow = GameObject.Find("Arrow");
		Quaternion tempRotation = RotationPointLoadArrow.transform.rotation;
		tempRotation.z = 0;
		RotationPointLoadArrow.transform.rotation = tempRotation;
		float difference = 0;
		
		TapToCont.gameObject.SetActive(true);
		TapToCont.SetStringText("Loading");
		TapToCont.LocalizeText();
		//LW.setLoading(true);
        alphaFloat = 0.0f;
		
		string temp = "";
		if(_levelName == "")
		{
			if(_levelIndex == _startScreenLevelNumber)
			{
				temp = "Tips_Funny" + Random.Range(1, _funnyTipsAmount).ToString();
			}
			else
			{
				foreach(int i in _tutorialLevels)
				{
					if(i == _levelIndex)
					{
						temp = "Tips_Tutorial" + Random.Range(1, _tutorialTipsAmount).ToString();
						break;
					}
				}
			}
		}
		if(temp == "")
		{
			temp = "Tips_Tip" + Random.Range(1, _practicalTipsAmount).ToString();
		}
		if(!_skipTap)
		{
			TipText.SetStringText(temp);
			TipText.LocalizeText();
		}
		if(Application.HasProLicense())
		{
			if (_levelName != "")
	            Async = Application.LoadLevelAsync(_levelName);
	        else
	            Async = Application.LoadLevelAsync(_levelIndex);
			
			Async.allowSceneActivation = false;
		
			while(Async.progress < 0.9f) // Load is actually done at 90% when allowSceneActivation = false
			{
				tempRotation.z = (Async.progress + 0.1f) * 180 * -1;
				RotationPointLoadArrow.transform.rotation = tempRotation;
				yield return new WaitForEndOfFrame();
			}
			
			//LW.setLoading(false);
			if(!_skipTap)
			{
				TapToCont.gameObject.SetActive(true);
				TapToCont.SetStringText("TapToCont");
				TapToCont.LocalizeText();
			}
			else
				TapToCont.gameObject.GetComponent<GUIText>().text = "";
			while(true)
			{
				if(_skipTap)
				{
					_skipTap = false;
					break;
				}
				if(Input.anyKey)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
			
	        while (alphaFloat<1.0f)
	        {
	            yield return new WaitForEndOfFrame();
	            alphaFloat = Mathf.Clamp01(alphaFloat + Time.deltaTime / aFadeOutTime);
	            DrawQuad(aColor, alphaFloat);
	        }
			
			TapToCont.setText("");
			
			//TapToCont.gameObject.GetComponent<TextMesh>().text = "";
			
			Async.allowSceneActivation = true;
			
	        while (alphaFloat>0.0f)
	        {
	            yield return new WaitForEndOfFrame();
		        alphaFloat = Mathf.Clamp01(alphaFloat - Time.deltaTime / aFadeInTime);
		        DrawQuad(aColor, alphaFloat);
				
	        }
		}
		else
		{
			if (_levelName != "")
	            Application.LoadLevel(_levelName);
	        else
	            Application.LoadLevel(_levelIndex);
		}
		
		instance._levelName = "";
        _fading = false;
    }
	
    private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        _fading = true;
        StartCoroutine(FadeToLoadScreen(aFadeOutTime, aFadeInTime, aColor));
    }
	
	private void AdjustCameraSize()
    {
		float _scaleMultiplierX = Screen.width / 1920f;
        float _scaleMultiplierY = Screen.height / 1200f;
        float _aspectRatio = 1920f / 1200f;
        float _startCameraSize = 600f;
        float _newCameraSize = Camera.main.orthographicSize * _scaleMultiplierY;
		
		GameObject[] _guiList = new GameObject[1];
		_guiList[0] = GameObject.Find("BGLoad");
		
		foreach(GameObject _guiObject in _guiList)
        {
	        Camera.main.aspect = _aspectRatio;
	        Camera.main.orthographicSize = _startCameraSize;
			
	        Vector3 _startPosition = Camera.main.WorldToViewportPoint(_guiObject.transform.position);
			
	        if(_guiObject.name == "BGLoad")
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
			
	        Camera.main.ResetAspect();
	        Camera.main.orthographicSize = _newCameraSize;
	        _guiObject.transform.position = Camera.main.ViewportToWorldPoint(_startPosition);
		}
        Camera.main.orthographicSize = _newCameraSize;
    }

}