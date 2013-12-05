using UnityEngine;
using System.Collections;

public class LightSwitch : MonoBehaviour
{
 
    [SerializeField]
    private BeatType _beatType = BeatType.All8Beats;
    [SerializeField]
    private Texture _textureOn;
    [SerializeField]
    private Texture _textureOff;
    [SerializeField]
    private bool _allowedToPlay = false;
    private string _type;
    private bool _on = false;
 
    void OnEnable()
    {
        OnEnableBeatType();
    }
 
    void OnDisable()
    {
        OnDisableBeatType();
    }
 
    // Use this for initialization
    void Start()
    {
     
    }
 
    // Update is called once per frame
    void Update()
    {
 
    }
 
    private void OnEnableBeatType()
    {
        _type = _beatType.ToString();
     
        if(_type == "All3Beats")
        {
            BeatController.OnAll3Beats += UpdateTexture;
        }
        else if(_type == "All4Beats")
        {
            BeatController.OnAll4Beats += UpdateTexture;
        }
        else if(_type == "All6Beats")
        {
            BeatController.OnAll6Beats += UpdateTexture;
        }
        else if(_type == "All8Beats")
        {
            BeatController.OnAll8Beats += UpdateTexture;
        }
    }
 
    private void OnDisableBeatType()
    {
        _type = _beatType.ToString();
        if(_type == "All3Beats")
        {
            BeatController.OnAll3Beats -= UpdateTexture;
        }
        else if(_type == "All4Beats")
        {
            BeatController.OnAll4Beats -= UpdateTexture;
        }
        else if(_type == "All6Beats")
        {
            BeatController.OnAll6Beats -= UpdateTexture;
        }
        else if(_type == "All8Beats")
        {
            BeatController.OnAll8Beats -= UpdateTexture;
        }
    }
 
    private void UpdateTexture()
    {
        if(_on == false)
        {
            if(_allowedToPlay)
            {
                SoundManager.Effect_UraniumRods_Light1();
            }

            renderer.material.mainTexture = _textureOn;
            _on = true;
        }
        else if(_on == true)
        {
            if(_allowedToPlay)
            {
                SoundManager.Effect_UraniumRods_Light2();
            }

            renderer.material.mainTexture = _textureOff;
            _on = false;
        }
    }

    private enum BeatType
    {
        All3Beats,
        All4Beats,
        All6Beats,
        All8Beats
    };
}
