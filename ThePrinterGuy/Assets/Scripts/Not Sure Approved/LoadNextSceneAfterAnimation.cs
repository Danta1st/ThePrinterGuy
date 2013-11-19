using UnityEngine;
using System.Collections;

public class LoadNextSceneAfterAnimation : MonoBehaviour {
    private GameObject Character;
	// Use this for initialization
	void Start () {
        Character = gameObject;
	}

	// Update is called once per frame
	void Update () {
        LoadNextScene();
	}

    public void LoadNextScene()
    {
        if(!gameObject.animation.isPlaying)
            Application.LoadLevel("Stage1Level1");
    }
}
