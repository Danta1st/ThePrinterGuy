using UnityEngine;
using System.Collections;

public class LoadNextSceneAfterAnimation : MonoBehaviour {
    #region Setup of Delegates
    void OnEnable ()
    {
        GestureManager.OnTap += LoadNextScene;
    }
 
    void OnDisable ()
    {
        GestureManager.OnTap -= LoadNextScene;
    }
    #endregion

	// Update is called once per frame
	void Update () {
        LoadNextSceneAfterAnimationIfTrue();
	}

    public void LoadNextSceneAfterAnimationIfTrue()
    {
        if(!gameObject.animation.isPlaying)
            LoadingScreen.Load(Application.loadedLevel+1);
    }

    public void LoadNextScene(GameObject go, Vector2 screenPosition)
    {
        LoadingScreen.Load(Application.loadedLevel+1);
    }
}
