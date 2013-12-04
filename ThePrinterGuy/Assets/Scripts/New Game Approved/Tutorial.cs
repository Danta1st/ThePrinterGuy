using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {
	
	#region SerializeField
	[SerializeField] private LayerMask _layerMaskGUI;
    [SerializeField] private GameObject[] _guiList;
	[SerializeField] private Texture2D[] _tutorialEnglish;
	[SerializeField] private Texture2D[] _tutorialDanish;
	[SerializeField] private ButtonTextures _buttonTextures;
	#endregion
	
	#region Private Variables
	private GameObject _nextObject;
	private GameObject _previousObject;
	
	private RaycastHit _hit;
	private bool _isGUI = true;
    private bool _canTouch = true;
    private Camera _guiCamera;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
	
	private List<Texture2D> _tutorialList = new List<Texture2D>();
	private GameObject _tutorialScreenObject;
	private string _language;
	private int _index = 0;
	
	private bool _once = false;
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
	#endregion
	
	// Use this for initialization
	void Start () {
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
            if(_guiObject.name == "Tutorial")
            {
				_tutorialScreenObject = _guiObject;
            }
            if(_guiObject.name == "NextButton")
            {
            	_nextObject = _guiObject;
            }
            if(_guiObject.name == "BackButton")
            {
            	_previousObject = _guiObject;
            }
        }
        //--------------------------------------------------//
		UpdateLanguage();
		UpdatePage();
		_previousObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	private GameObject backButton;
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
                    if(_hit.collider.gameObject.name == "NextButton")
                    {
						SoundManager.CutScene_Random_Coffee();
						NextPage();
                        if(_index < _tutorialList.Count)
                            UpdatePage();
                    }
                    else if(_hit.collider.gameObject.name == "BackButton")
                    {
						if(backButton == null)
							backButton = _hit.collider.gameObject;

                        SoundManager.TurnOnMenuSounds();
                        SoundManager.Effect_Menu_Click();
						//Do fancy 'User Pressed a button' Animation
						SetTexture(backButton, _buttonTextures.leftPressed);
						PunchButton(backButton);
						//Reset
						Invoke("ResetBackButton", _punchTime);						
						
						PreviousPage();
                        if(_index < _tutorialList.Count)
                            UpdatePage();
                    }
                }
                //-----------------------------------------------------------------------//
            }
        }
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

            if(_guiObject.tag == "Tutorial")
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
	
	private void UpdateLanguage()
	{
		_language = LocalizationText.GetLanguage();

		if(_language == "EN")
		{
			foreach(Texture2D _texture in _tutorialEnglish)
			{
				_tutorialList.Add(_texture);
			}
		}
		else if(_language == "DK")
		{
			foreach(Texture2D _texture in _tutorialDanish)
			{
				_tutorialList.Add(_texture);
			}
		}
	}
	
	private void UpdatePage()
	{
		_tutorialScreenObject.renderer.material.mainTexture = _tutorialList[_index];
	}
	
	private void NextPage()
	{
		_index++;
		
		if(_index > _tutorialList.Count-1)
		{
            string correspondingLevelName = null;
            switch (Application.loadedLevelName) {
            case "Tutorial1":
                correspondingLevelName = "Level1";
                break;
            case "Tutorial2":
                correspondingLevelName = "Level2";
                break;
            case "Tutorial3":
                correspondingLevelName = "Level3";
                break;
            case "Tutorial4":
                correspondingLevelName = "Level4";
                break;
            case "Tutorial5":
                correspondingLevelName = "Level5";
                break;
            default:
                break;
            }
            LoadingScreen.Load(correspondingLevelName);
		}
		else
		{
			_previousObject.SetActive(true);
		}
	}
	
	private void PreviousPage()
	{
		_index--;
		
		if(_index <= 0)
		{
			_index = 0;
			_previousObject.SetActive(false);
		}
        UpdatePage();
	}
		
	private void ResetBackButton()
	{
		if(backButton != null)
			SetTexture(backButton, _buttonTextures.left);
	}
	
	private float _punchTime = 0.4f;
	private void PunchButton(GameObject button)
	{		
		iTween.PunchScale(button, new Vector3(35f, 35f, 35f), _punchTime);
	}
	private void PunchButtonPrecise(GameObject button, Vector3 scale)
	{		
		iTween.PunchScale(button, scale, _punchTime);
	}
	
	private void SetTexture(GameObject go, Texture2D texture)
	{
		go.renderer.material.mainTexture = texture;
	}
	
	[System.Serializable]
	public class ButtonTextures
	{
		public Texture2D left;
		public Texture2D leftPressed;
	}
}
