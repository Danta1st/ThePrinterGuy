using UnityEngine;
using System.Collections;

public class PaperIconGlow : MonoBehaviour
{
    void OnEnable()
    {
        BeatController.OnBeat4th1 += LightOnGateOpen;
        BeatController.OnBeat4th4 += LightOffGateClosed;
    }

    void OnDisable()
    {
        BeatController.OnBeat4th1 -= LightOnGateOpen;
        BeatController.OnBeat4th4 -= LightOffGateClosed;
    }

    private void LightOnGateOpen()
    {
        iTween.ValueTo(gameObject, iTween.Hash("From", new Color(0f, 0f, 0f, 0f), "To", new Color(0.4f, 0.4f, 0.4f, 0.4f), "time", 0.1f, "onupdate", "ChangeEmissiveColor"));
    }

    private void LightOffGateClosed()
    {
        iTween.ValueTo(gameObject, iTween.Hash("From", new Color(0.4f, 0.4f, 0.4f, 0.4f), "To", new Color(0f, 0f, 0f, 0f), "time", 0.1f, "onupdate", "ChangeEmissiveColor"));
    }

    private void ChangeEmissiveColor(Color color)
    {
        gameObject.renderer.material.SetColor("_EmisColor", color);
    }
}
