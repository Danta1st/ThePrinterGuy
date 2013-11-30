using UnityEngine;
using System.Collections;

public class AudioListenerPauseTestScript : MonoBehaviour
{
    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }
 
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            AudioListener.pause = true;
            audio.ignoreListenerPause = true;
        }
        else if(Input.GetKey(KeyCode.Tab))
        {
            AudioListener.pause = false;
        }

        if(Input.anyKey)
        {
            audio.Play();
        }
    }
}
