using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {

    private GameObject character;
    private Color oldColor;

    void Start()
    {
        character = gameObject.transform.FindChild("bossChar").gameObject;
    }

    #region Delegates & Events
    public delegate void DialogueStart();
    public static event DialogueStart OnDialogueStart;

    public delegate void DialogueEnd();
    public static event DialogueEnd OnDialogueEnd;
    #endregion

    #region Enable and Disable
    void OnEnable()
    {
        StressOMeter.OnHappyZoneEntered += HappyCharacter;
        StressOMeter.OnAngryZoneEntered += AngryCharacter;
    }

    void OnDisable()
    {
        StressOMeter.OnHappyZoneEntered -= HappyCharacter;
        StressOMeter.OnAngryZoneEntered -= AngryCharacter;
    }
    #endregion

    private void HappyCharacter()
    {
       // OnDialogueStart();
        character.animation.CrossFade("Selection");
        character.animation.CrossFadeQueued("Idle");
        StartCoroutine("CheckForDialogueEnd");
    }

    private void AngryCharacter()
    {
        //OnDialogueStart();
        character.animation.CrossFade("Selection");
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("amount", new Vector3(0.3f,0.3f,0.3f), "time", 1));
        iTween.ColorFrom(Camera.main.gameObject, Color.red, 2f);
        oldColor = Camera.main.backgroundColor;
        iTween.ValueTo(gameObject, iTween.Hash("from", oldColor, "to", Color.red, "time", 1f, "onupdate", "changeSkyboxValue"));
        character.animation.CrossFadeQueued("Idle");
        StartCoroutine("CheckForDialogueEnd");
    }

    IEnumerator CheckForDialogueEnd()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", Color.red, "to", oldColor, "time", 1f, "onupdate", "changeSkyboxValue", "delay", 1f));
        while(character.animation.IsPlaying("Selection"))
        {
            yield return new WaitForSeconds(1);
        }
        //OnDialogueEnd();
    }

    private void changeSkyboxValue(Color color)
    {
        Camera.main.backgroundColor = color;
    }

}
