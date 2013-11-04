using UnityEngine;
using System.Collections;

public class InkCartridge : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 60;
	
	private GameObject _smoke;
    private TimerUtilities _inkTimer;
    private bool _isInstantiated = false;
    private Vector3 _defaultScale;
    private Vector3 _defaultPosition;
    private Color _inkColor;
    private enum InkStates { Working, ErrorWait };
    private InkStates _inkState = InkStates.Working;

    public delegate void InkCartridgeError(GameObject go);
    public static event InkCartridgeError OnInkCartridgeError;

    public delegate void InkCartridgeRefilled(GameObject go);
    public static event InkCartridgeRefilled OnInkCartridgeRefilled;
	
    void Update()
    {
        if (_isInstantiated) {
            if (_inkTimer.GetTimeLeft() > 0 && _inkState == InkStates.Working)
            {
                UseInk();
            }
            else if (_inkTimer.GetTimeLeft() <= 0 && _inkState != InkStates.ErrorWait)
            {
                if (OnInkCartridgeError != null)
                    OnInkCartridgeError(this.gameObject);
				
				string path = "Prefabs/SmokeParticle";
				
                _smoke = (GameObject)Instantiate (Resources.Load(path), this.gameObject.transform.position, Quaternion.identity);

                _inkState = InkStates.ErrorWait;
            }
        }
    }

    public void InitializeInkCartridge(Color color)
    {
        _defaultScale = this.gameObject.transform.localScale;
        _defaultPosition = this.gameObject.transform.localPosition;

        _inkColor = color;
        _inkTimer = gameObject.AddComponent<TimerUtilities>();
        _inkTimer.StartTimer(_lifeTime);
        _isInstantiated = true;
    }

    public void InitializeInkCartridge(Color color, float lifeTime)
    {
        _defaultScale = gameObject.transform.localScale;
        _defaultPosition = gameObject.transform.localPosition;

        _inkColor = color;
        _lifeTime = lifeTime;
        _inkTimer = gameObject.AddComponent<TimerUtilities>();
        _inkTimer.StartTimer(_lifeTime);
        _isInstantiated = true;
    }

    public void RefillInk()
    {
        _inkTimer.StartTimer(_lifeTime);

        if(OnInkCartridgeRefilled != null)
            OnInkCartridgeRefilled(this.gameObject);

        _inkState = InkStates.Working;
        RescaleCartridge();
        Destroy (_smoke);
    }

    /*public void RefillInk(float amount)
    {
        if(amount <= _lifeTime && amount > 0)
        {
            _inkTimer.StartTimer(amount);

            if(OnInkCartridgeRefilled != null)
                OnInkCartridgeRefilled(this.gameObject);

            RescaleCartridge(amount);
        }
        else if(amount > _lifeTime)
        {
            _inkTimer.StartTimer(_lifeTime);

            if(OnInkCartridgeRefilled != null)
                OnInkCartridgeRefilled(this.gameObject);

            RescaleCartridge(amount);
        }
    }*/

    private void UseInk()
    {
        float deltaScale = 0.0f;
        Vector3 scale = this.gameObject.transform.localScale;
        scale.y = _defaultScale.y * _inkTimer.GetTimeLeftInPctDecimal();
        deltaScale = this.gameObject.transform.localScale.y - scale.y;
        this.gameObject.transform.localScale = scale;

        Vector3 pos = this.gameObject.transform.localPosition;
        pos.y = pos.y - (deltaScale * 4);
        this.gameObject.transform.localPosition = pos;
    }

    private void RescaleCartridge()
    {
        this.gameObject.transform.localScale = _defaultScale;
        this.gameObject.transform.localPosition = _defaultPosition;
    }


}
