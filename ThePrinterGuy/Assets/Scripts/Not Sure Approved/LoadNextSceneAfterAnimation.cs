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
