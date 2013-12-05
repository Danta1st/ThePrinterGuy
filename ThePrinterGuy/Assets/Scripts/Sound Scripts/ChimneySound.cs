using UnityEngine;
using System.Collections;

public class ChimneySound : MonoBehaviour
{
    private AudioSource thisSource;

    // Use this for initialization
    void Awake()
    {
        thisSource = gameObject.audio;
    }

    void OnEnable()
    {
        BeatController.OnAll4Beats += PlayThisSound;
    }

    void OnDisable()
    {
        BeatController.OnAll4Beats -= PlayThisSound;
    }

    void PlayThisSound()
    {
        thisSource.Play();
    }
}
