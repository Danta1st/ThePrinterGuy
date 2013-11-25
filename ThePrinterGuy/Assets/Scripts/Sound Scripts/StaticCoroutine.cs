using UnityEngine;
using System.Collections;

public class StaticCoroutine  : MonoBehaviour {

    private static StaticCoroutine instance;

	// Use this for initialization
	void Awake () {
	    instance = this;
	}

    IEnumerator freezeUnFreezeAudio(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);

        AudioListener.pause = !AudioListener.pause;
    }

    public static void DoCoroutine(float fadeTime)
    {
        instance.freezeUnFreezeAudio(fadeTime);
    }
}
