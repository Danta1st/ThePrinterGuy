using UnityEngine;
using System.Collections;

public class DialogueText : MonoBehaviour {

    private GameObject _bgText;
    private GameObject _text;
    private float _bgTextMultiplyer;
    private Vector3 _bgTextStartSize;
    private bool _subtitleOn;

    void Start()
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
            _bgText = gameObject.transform.FindChild("TextBG").gameObject;
            _bgText.SetActive(true);
            _text = gameObject.transform.FindChild("Text").gameObject;
            _text.SetActive(true);
            _bgTextStartSize = _bgText.transform.localScale;
            _bgText.renderer.enabled = false;
            _text.renderer.enabled = false;
        }
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
            float _textLength = _text.GetComponent<TextMesh>().text.Length;
            _bgTextMultiplyer = _textLength / 20f;
            Vector3 _size = new Vector3(_bgTextStartSize.x*_bgTextMultiplyer, _bgTextStartSize.y, _bgTextStartSize.z);
            _bgText.transform.localScale = _size;
            _bgText.renderer.enabled = true;
            _text.renderer.enabled = true;
        }
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
