using UnityEngine;
using System.Collections;

public class InkCartridge : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 60;
    private TimerUtilities _inkTimer;
    private bool _isInstantiated = false;
    private float _defaultScaleY;
    private float _defaultPositionY;
    private Color _inkColor;

    public delegate void InkCartridgeError(GameObject go);
    public static event InkCartridgeError OnInkCartridgeError;

    public delegate void InkCartridgeRefilled(GameObject go);
    public static event InkCartridgeRefilled OnInkCartridgeRefilled;

    public void Update()
    {
        if (_isInstantiated) {
            if (_inkTimer.GetTimeLeft() > 0)
            {
                UseInk();
            }
            else
            {
                if (OnInkCartridgeError != null)
                    OnInkCartridgeError(this.gameObject);
            }
        }
    }

    public void InitializeInkCartridge(Color color)
    {
        _defaultScaleY = gameObject.transform.localScale.y;
        _defaultPositionY = gameObject.transform.localPosition.y;

        _inkColor = color;
        _inkTimer = gameObject.AddComponent<TimerUtilities>();
        _inkTimer.StartTimer(_lifeTime);
        _isInstantiated = true;
    }

    public void InitializeInkCartridge(Color color, float lifeTime)
    {
        _defaultScaleY = gameObject.transform.localScale.y;
        _defaultPositionY = gameObject.transform.localPosition.y;

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
    }

    public void RefillInk(float amount)
    {
        if(amount <= _lifeTime && amount > 0)
        {
            _inkTimer.StartTimer(amount);

            if(OnInkCartridgeRefilled != null)
                OnInkCartridgeRefilled(this.gameObject);
        }
        else if(amount > _lifeTime)
        {
            _inkTimer.StartTimer(_lifeTime);

            if(OnInkCartridgeRefilled != null)
                OnInkCartridgeRefilled(this.gameObject);
        }
    }

    private void UseInk()
    {
        float deltaScale = 0.0f;
        Vector3 scale = gameObject.transform.localScale;
        scale.y = _defaultScaleY * _inkTimer.GetTimeLeftInPctDecimal();
        deltaScale = gameObject.transform.localScale.y - scale.y;
        gameObject.transform.localScale = scale;

        Vector3 pos = gameObject.transform.position;
        pos.y = pos.y - deltaScale/2;
        gameObject.transform.position = pos;
    }
}
