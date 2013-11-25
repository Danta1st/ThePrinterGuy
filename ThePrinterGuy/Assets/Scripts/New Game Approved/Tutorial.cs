using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {
	
    [SerializeField]
    private GameObject[] _guiList;
	[SerializeField]
	private Texture2D[] _tutorialEnglish;
	[SerializeField]
	private Texture2D[] _tutorialDanish;
	
	#region Private Variables
    private Camera _guiCamera;
    private float _scaleMultiplierX;
    private float _scaleMultiplierY;
	
	private List<Texture2D> _tutorialList = new List<Texture2D>();
	private GameObject _tutorialScreenObject;
	private string _language;
	private int _index = 0;
	
	private bool _once = false;
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
        }
        //--------------------------------------------------//
		UpdateLanguage();
		UpdateTutorial();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			if(_tutorialScreenObject != null && _index < _tutorialList.Count)
			{
				UpdateTutorial();
			}
			else
			{
				LoadingScreen.Load(Application.loadedLevel+1);
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
	
	private void UpdateTutorial()
	{
		_tutorialScreenObject.renderer.material.mainTexture = _tutorialList[_index];
		_index++;
	}
}
