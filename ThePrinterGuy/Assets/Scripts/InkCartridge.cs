using UnityEngine;
using System.Collections;

public class InkCartridge : MonoBehaviour
{
	#region Variables Editable in Editor
	[SerializeField]
    private float _lifeTime = 60;
	#endregion
	
	#region Private variables
    private TimerUtilities _inkTimer;
    private bool _isInstantiated = false;
	private bool _isEmpty = false;
	private bool _wasEmpty = false;
    private Vector3 _defaultScale;
    private Vector3 _defaultPosition;
	private Color _inkColor;
    private enum InkStates { Working, ErrorWait };
    private InkStates _inkState = InkStates.Working;
	private GameObject _inkColorObj;
	#endregion
	
	#region delegate declarations
    public delegate void InkCartridgeError(GameObject go);
    public static event InkCartridgeError OnInkCartridgeError;

    public delegate void InkCartridgeRefilled(GameObject go);
    public static event InkCartridgeRefilled OnInkCartridgeRefilled;
	
	public delegate void InkCartridgeRefilledFromEmpty(GameObject go);
    public static event InkCartridgeRefilledFromEmpty OnInkCartridgeRefilledFromEmpty;
	#endregion
	
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
                    OnInkCartridgeError(this.gameObject.transform.root.gameObject);
				
				_isEmpty = true;
                _inkState = InkStates.ErrorWait;
            }
        }
    }
	
	#region OnEnableOnDisable
	public void OnEnable()
	{
		collider.enabled = false;
		PrinterManager.OnPrinterBroken += PauseTimer;
		PrinterManager.OnPrinterFixed += ResumeTimer;
	}
	public void OnDisable()
	{
		PrinterManager.OnPrinterBroken -= PauseTimer;
		PrinterManager.OnPrinterFixed -= ResumeTimer;
	}
	
	#endregion
	
	#region Initialization methods
    public void InitializeInkCartridge(Color color)
    {
		foreach(Transform t in gameObject.transform)
		{
			if(t.gameObject.tag == "InkColor")
			{
				_inkColorObj = t.gameObject;
			}
		}
        
		_defaultScale = _inkColorObj.transform.localScale;
        _defaultPosition = _inkColorObj.transform.localPosition;
		
		_inkColorObj.renderer.material.color = color;
		
        _inkColor = color;
        _inkTimer = gameObject.AddComponent<TimerUtilities>();
        _inkTimer.StartTimer(_lifeTime);
        _isInstantiated = true;
    }

    public void InitializeInkCartridge(Color color, float lifeTime)
    {
		foreach(Transform t in gameObject.transform)
		{
			if(t.gameObject.tag == "InkColor")
			{
				_inkColorObj = t.gameObject;
			}
		}
        
		_defaultScale = _inkColorObj.transform.localScale;
        _defaultPosition = _inkColorObj.transform.localPosition;
		
		_inkColorObj.renderer.material.color = color;
		
        _inkColor = color;
        _lifeTime = lifeTime;
        _inkTimer = gameObject.AddComponent<TimerUtilities>();
        _inkTimer.StartTimer(_lifeTime);
        _isInstantiated = true;
    }
	#endregion
	
	#region public methods
    public void RefillInk()
    {
        if(_inkTimer.GetTimeLeft() == 0)
		{
			_wasEmpty = true;
		}
		
        if(OnInkCartridgeRefilled != null)
            OnInkCartridgeRefilled(this.gameObject.transform.root.gameObject);
		
		if(OnInkCartridgeRefilledFromEmpty != null && _isEmpty)
		{
            OnInkCartridgeRefilledFromEmpty(this.gameObject.transform.root.gameObject);
			_isEmpty = false;
		}
		
		_inkTimer.StartTimer(_lifeTime);
        _inkState = InkStates.Working;
        RescaleCartridge();
    }
	
	public void PauseTimer(GameObject go)
	{
		if(go.Equals(gameObject.transform.root.gameObject))
		{
			_inkTimer.PauseTimer();
		}
	}

	public void ResumeTimer(GameObject go)
	{
		if(go.Equals(gameObject.transform.root.gameObject) && !_isEmpty && !_wasEmpty)
		{
			_inkTimer.ResumeTimer();
		}
		else if(go.Equals(gameObject.transform.root.gameObject) && _wasEmpty)
		{
			_inkTimer.StartTimer(_lifeTime, false);
			RescaleCartridge();
			_wasEmpty = false;
		}
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
	#endregion
	
	#region private methods
    private void UseInk()
    {
        float deltaScale = 0.0f;
        Vector3 scale = _inkColorObj.transform.localScale;
        scale.y = _defaultScale.y * _inkTimer.GetTimeLeftInPctDecimal();
        deltaScale = _inkColorObj.transform.localScale.y - scale.y;
        _inkColorObj.transform.localScale = scale;

        Vector3 pos = _inkColorObj.transform.localPosition;
        pos.y = pos.y - (deltaScale);
        _inkColorObj.transform.localPosition = pos;
    }

    private void RescaleCartridge()
    {
        _inkColorObj.transform.localScale = _defaultScale;
        _inkColorObj.transform.localPosition = _defaultPosition;
    }
	#endregion

}
