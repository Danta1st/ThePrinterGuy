using UnityEngine;
using System.Collections;

public class LoadNextSceneAfterAnimation : MonoBehaviour
{
    #region Setup of Delegates
    void OnEnable()
    {
        GestureManager.OnTap += LoadNextScene;
    }
 
    void OnDisable()
    {
        GestureManager.OnTap -= LoadNextScene;
    }
    #endregion

    void Start()
    {
        SoundManager.Music_CutScene_Main();
        iTween.MoveFrom(Camera.main.gameObject, iTween.Hash("position", new Vector3(-3.737667f, 3.327735f, 7.618786f), "easeType", iTween.EaseType.linear, "time", 3f));
        iTween.RotateFrom(Camera.main.gameObject, iTween.Hash("rotation", new Vector3(23.81647f, 141.8127f, 359.106f), "easeType", iTween.EaseType.linear, "time", 3f));
    }

    // Update is called once per frame
    void Update()
    {
        LoadNextSceneAfterAnimationIfTrue();
    }

    public void LoadNextSceneAfterAnimationIfTrue()
    {
        if(!gameObject.animation.isPlaying)
        {
            LoadingScreen.Load("Tutorial1");
        }
    }

    public void LoadNextScene(GameObject go, Vector2 screenPosition)
    {
        LoadingScreen.Load("Tutorial1");
    }
}
