using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageSelection : MonoBehaviour
{
    [SerializeField]
    private float _charMoveDistance = 2.0f;
    [SerializeField]
    private float _charMoveTime = 2.0f;
    [SerializeField]
    private List<GameObject> _stageCharacters = new List<GameObject>();

    private GameObject _selectedStageChar;

    // Use this for initialization
    void Start()
    {

    }
 
    // Update is called once per frame
    void Update()
    {
 
    }

    void OnEnable()
    {
        GestureManager.OnTap += SelectStage;
        GestureManager.OnTap += SelectLevel;
    }

    void OnDisable()
    {
        GestureManager.OnTap -= SelectStage;
        GestureManager.OnTap -= SelectLevel;
    }

    //TODO:
     /*
      * Start - Language boxes appear, stageCharacters are visible as shadows in the background
      * OnTap - Select a language
      *         After the language is selected the stageCharacters appear
      *         The stages that are unlocked will appear lit, while the rest are silhouettes
      * OnTap - Selected stageCharacter moves forward (only on lit characters)
      *         Camera focus on selected Character
      *         An animation animates the character to move forward
      *         The level boxes appear above the character
      * OnTap - Go to selected levelbox's scene
      */

    void IlluminateCharacters()
    {

    }

    void SelectStage(GameObject go, Vector2 screenPos)
    {
        Debug.Log(go);
        if(go != null)
        {
            if(_stageCharacters.Contains(go))
            {
                BeginMoveAnimation(go);
            }
        }
    }

    void CameraFocus()
    {

    }

    void BeginMoveAnimation(GameObject go)
    {
        Vector3 tmpPos = go.transform.position;
        tmpPos.z -= _charMoveDistance;

        iTween.MoveTo(go, iTween.Hash("position", tmpPos, "time", _charMoveTime, "oncomplete", "OnMoveAnimationEnd", "oncompletetarget", gameObject));
    }

    void OnMoveAnimationEnd()
    {
        LevelBoxesAppear();
    }

    void LevelBoxesAppear()
    {
        _selectedStageChar.transform.FindChild("stageLevelSelection").renderer.enabled = true;
    }

    void SelectLevel(GameObject go, Vector2 screenPos)
    {
        if(go != null)
        {
            if(go.GetComponent<LevelSelection>() != null)
            {
                Debug.Log("GO TO LEVEL");
                LevelSelection lvlSelect = go.GetComponent<LevelSelection>();
                lvlSelect.GoToLevel();
            }
        }
    }
}
