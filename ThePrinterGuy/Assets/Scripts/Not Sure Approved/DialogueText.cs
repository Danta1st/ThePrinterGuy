using UnityEngine;
using System.Collections;

public class DialogueText : MonoBehaviour {

    private GameObject _bgText;
    private GameObject _text;
    private float _bgTextMultiplyer;
    private Vector3 _bgTextStartSize;

    void Start()
    {
        _bgText = gameObject.transform.FindChild("TextBG").gameObject;;
        _text = gameObject.transform.FindChild("Text").gameObject;
        _bgTextStartSize = _bgText.transform.localScale;
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
        _text.GetComponent<TextMesh>().text = LocalizationText.GetText(localizationKey);
        float _textLength = _text.GetComponent<TextMesh>().text.Length;
        _bgTextMultiplyer = _textLength / 20f;
        Vector3 _size = new Vector3(_bgTextStartSize.x*_bgTextMultiplyer, _bgTextStartSize.y, _bgTextStartSize.z);
        _bgText.transform.localScale = _size;
        _bgText.renderer.enabled = true;
        _text.renderer.enabled = true;
    }

    private void HideSubtitle()
    {
        _bgText.renderer.enabled = false;
        _text.renderer.enabled = false;
    }

}
