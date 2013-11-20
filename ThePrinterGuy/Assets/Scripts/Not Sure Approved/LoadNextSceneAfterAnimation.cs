using UnityEngine;
using System.Collections;

public class LoadNextSceneAfterAnimation : MonoBehaviour {
    private GameObject Character;


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

	// Use this for initialization
	void Start () {
        Character = gameObject;
	}

	// Update is called once per frame
	void Update () {
        LoadNextSceneAfterAnimationIfTrue();
	}

    public void LoadNextSceneAfterAnimationIfTrue()
    {
        if(!gameObject.animation.isPlaying)
            Application.LoadLevel("Stage1Level1");
    }

    public void LoadNextScene(GameObject go, Vector2 screenPosition)
    {
        Application.LoadLevel("Stage1Level1");
    }
}
