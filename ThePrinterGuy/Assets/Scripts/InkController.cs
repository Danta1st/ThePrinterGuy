using UnityEngine;
using System.Collections;

public class InkController : MonoBehaviour
{
	#region Variables Editable in Editor
	[SerializeField]
	private iTween.EaseType _easeType;
	[SerializeField]
	private float _animationSpeed = 0.5f;
	[SerializeField]
	private float _rotationAmount = 75f;
	[SerializeField]
	private float _sirenRotationSpeed = 50f;
	#endregion
	
	#region Private Variables

	private InkCartridge _redInk;
    private InkCartridge _greenInk;
    private InkCartridge _blueInk;
	private GameObject _errorSiren;
	private bool _tasksEnabled = false;
	private bool _inkSelected = false;
	private Color _inkColorSelected;
	private bool _sirenEnabled = false;
	#endregion
	
	#region Setup of Delegates
	void OnEnable()
	{
		ZoomHandler.OnInk += EnableInkTask;
		ZoomHandler.OnGoingFreeroam += DisableInkTask;
		GestureManager.OnSwipeLeft += RotateLeft;
		GestureManager.OnSwipeRight += RotateRight;
		GestureManager.OnTap += InsertInk;
		InkCartridge.OnInkCartridgeError += StartSiren;
		InkCartridge.OnInkCartridgeRefilled += StopSiren;
		InventoryController.OnInkSelect += GetInkFromInv;
	}
	
	void OnDisable()
	{
		ZoomHandler.OnInk -= EnableInkTask;
		ZoomHandler.OnGoingFreeroam -= DisableInkTask;
		GestureManager.OnSwipeLeft -= RotateLeft;
		GestureManager.OnSwipeRight -= RotateRight;
		GestureManager.OnTap -= InsertInk;
		InkCartridge.OnInkCartridgeError -= StartSiren;
		InkCartridge.OnInkCartridgeRefilled -= StopSiren;
		InventoryController.OnInkSelect -= GetInkFromInv;
	}
	#endregion
	
	#region Initializatio of InkCartridges
    void Start()
    {
        _redInk = GameObject.FindGameObjectWithTag("InkRed").GetComponent<InkCartridge>();
		_greenInk = GameObject.FindGameObjectWithTag("InkGreen").GetComponent<InkCartridge>();
		_blueInk = GameObject.FindGameObjectWithTag("InkBlue").GetComponent<InkCartridge>();
		
		_redInk.InitializeInkCartridge(Color.red);
        _greenInk.InitializeInkCartridge(Color.green);
        _blueInk.InitializeInkCartridge(Color.blue);
		_errorSiren = GameObject.FindGameObjectWithTag("InkSiren");
    }
	
	void Update()
	{
		if(_sirenEnabled)
		{
			_errorSiren.transform.Rotate(Vector3.up * _sirenRotationSpeed * Time.deltaTime);
		}
	}
	#endregion
	
	#region Delegate methods
	private void EnableInkTask()
	{
		_tasksEnabled = true;		
	}
	
	private void DisableInkTask()
	{
		_tasksEnabled = false;
	}
	
	private void RotateLeft()
	{
		if (_tasksEnabled)
		{
			iTween.RotateAdd(this.gameObject, iTween.Hash("amount", new Vector3(0, _rotationAmount, 0),
					"time", _animationSpeed, "easetype", _easeType));
		}
	}
	
	private void RotateRight()
	{
		if (_tasksEnabled)
		{
			iTween.RotateAdd(this.gameObject, iTween.Hash("amount", new Vector3(0, -_rotationAmount, 0),
					"time", _animationSpeed, "easetype", _easeType));
		}
	}
	
	private void InsertInk(GameObject go, Vector2 screenPos)
	{
		if(_tasksEnabled)
		{
			if (_inkSelected)
			{
				switch(go.tag)
				{
				case "InkRed":
					if(_inkColorSelected == Color.red)
						_redInk.RefillInk();
					
					_inkSelected = false;
					break;
				case "InkGreen":
					if(_inkColorSelected == Color.green)
						_greenInk.RefillInk();
					_inkSelected = false;
					break;
				case "InkBlue":
					if(_inkColorSelected == Color.blue)
						_blueInk.RefillInk();
					
					_inkSelected = false;
					break;
				default:
					_inkSelected = false;
					break;
				}
			}
		}
	}
	
	
	private void StartSiren(GameObject go)
	{
		_sirenEnabled = true;
		_errorSiren.renderer.material.SetFloat("_Progress", 0.5f);
	}
	
	private void StopSiren(GameObject go)
	{
		_sirenEnabled = false;
		_errorSiren.renderer.material.SetFloat("_Progress", 1.0f);
	}
	
//	private void GetInkFromInv(Color col)
//	{
//		_inkSelected = true;
//		_inkColorSelected = col;
//	}
	#endregion
}
