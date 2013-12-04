using UnityEngine;
using System.Collections;

public class DialogueText : MonoBehaviour {
	
	[SerializeField]
	private float _bgOffsetSize;
	
    private GameObject _bgText;
    private GameObject _text;
    private Vector3 _bgPosition;
    private float _bgTextMultiplier;
    private bool _subtitleOn;
	
    void Start()
    {
    	_bgTextMultiplier = Screen.width / 1920f;
    	
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

        _bgText = GameObject.Find("TextBG").gameObject;
        _text = GameObject.Find("Text").gameObject;
        _bgPosition = _text.transform.position;
        _bgText.transform.position = _bgPosition;
        _bgText.renderer.enabled = false;
        _text.renderer.enabled = false;
    }

    #region Enable and Disable
    void OnEnable()
    {
        Dialogue.OnDialogueStart += ShowSubtitle;
        Dialogue.OnDialogueEnd += HideSubtitle;
    }

    void OnDisable()
    {
        Dialogue.OnDialogueStart -= ShowSubtitle;
        Dialogue.OnDialogueEnd -= HideSubtitle;
    }
    #endregion

    private void ShowSubtitle(string localizationKey)
    {
        if(_subtitleOn)
        {
            _text.GetComponent<TextMesh>().text = LocalizationText.GetText(localizationKey);
            _bgText.renderer.enabled = true;
            _text.renderer.enabled = true;
            UpdateBG();
        }
    }
	
    private void UpdateBG()
    {
    	Bounds _bounds = _text.GetComponent<TextMesh>().renderer.bounds;
    	float _max = _bounds.max.x;
    	float _min = _bounds.min.x;
    	float _distance = Mathf.Abs(_min - _max);
		
    	Vector3 _bgScale = new Vector3(_distance + (_bgOffsetSize*_bgTextMultiplier),
    									_bgText.transform.localScale.y, _bgText.transform.localScale.z);
    									
    	_bgText.transform.localScale = _bgScale;
    }
	
    private void HideSubtitle()
    {
        if(_subtitleOn)
        {
            _bgText.renderer.enabled = false;
            _text.renderer.enabled = false;
        }
    }

}
