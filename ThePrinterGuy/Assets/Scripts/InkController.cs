﻿using UnityEngine;
using System.Collections;

public class InkController : MonoBehaviour
{
	#region Variables Editable in Editor
	[SerializeField]
	private iTween.EaseType _easeType;
	[SerializeField]
	private float _animationSpeed = 0.5f;
	[SerializeField]
	private float _rotationAmount = 90f;
	[SerializeField]
	private float _sirenRotationSpeed = 50f;
	[SerializeField]
	private Color[] _cartridgeColor;
	private bool _lidOneOpen = false;
	private bool _lidTwoOpen = false;
	private bool _lidThreeOpen = false;
	private System.Collections.Generic.List<InkCartridge> _inkList = new System.Collections.Generic.List<InkCartridge>();
	private GameObject _inkHouse;
	#endregion
	
	#region Private Variables

	private InkCartridge _redInk;
    private InkCartridge _greenInk;
    private InkCartridge _blueInk;
	private InkCartridge _blackInk;
	private int _emptyCartridgesAmount = 0;
	private GameObject _errorSiren;
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
		InkCartridge.OnInkCartridgeError += StartSiren;
		InkCartridge.OnInkCartridgeRefilledFromEmpty += StopSiren;
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
		InkCartridge.OnInkCartridgeRefilledFromEmpty -= StopSiren;
		InventoryController.OnInkSelect -= GetInkFromInv;
	}
	#endregion
	
	#region Initializatio of InkCartridges
    void Start()
    {
		int index = 0;
		foreach(Transform t in transform)
		{
			if(t.gameObject.tag == "Ink" && index < _cartridgeColor.Length)
			{
				_inkList.Add(t.gameObject.GetComponent<InkCartridge>());
				_inkList[index].InitializeInkCartridge(_cartridgeColor[index]);
				index++;
			}
		}
		
		_inkHouse = GameObject.FindGameObjectWithTag("InkTask");
//        _redInk = GameObject.FindGameObjectWithTag("InkRed").GetComponent<InkCartridge>();
//		_greenInk = GameObject.FindGameObjectWithTag("InkGreen").GetComponent<InkCartridge>();
//		_blueInk = GameObject.FindGameObjectWithTag("InkBlue").GetComponent<InkCartridge>();
//		_blackInk = GameObject.FindGameObjectWithTag("InkBlack").GetComponent<InkCartridge>();
//		
//		_redInk.InitializeInkCartridge(Color.red);
//        _greenInk.InitializeInkCartridge(Color.green);
//        _blueInk.InitializeInkCartridge(Color.blue);
//		_blackInk.InitializeInkCartridge(Color.black);
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
		GestureManager.OnTap += InsertInk;		
	}
	
	private void DisableInkTask()
	{
		GestureManager.OnTap -= InsertInk;
	}
	
	private void RotateLeft()
	{
		iTween.RotateAdd(_inkHouse, iTween.Hash("amount", new Vector3(0, _rotationAmount, 0),
					"time", _animationSpeed, "easetype", _easeType));
	}
	
	private void RotateRight()
	{
		iTween.RotateAdd(_inkHouse, iTween.Hash("amount", new Vector3(0, -_rotationAmount, 0),
					"time", _animationSpeed, "easetype", _easeType));
	}
	
	private void InsertInk(GameObject go, Vector2 screenPos)
	{
		if(go != null)
		{
			if(go.tag == "Lid")
			{
				float rotateAmount = 90f;
				Vector3 rotateVector;
				
				switch(go.name)
				{
				case "lid1":
					if(_lidOneOpen)
					{
						_lidOneOpen = false;
						rotateVector = new Vector3(rotateAmount, 0, 0);
						iTween.RotateAdd (go, rotateVector, 1f);
					}
					else
					{
						_lidOneOpen = true;
						rotateVector = new Vector3(-rotateAmount, 0, 0);
						iTween.RotateAdd (go, rotateVector, 1f);
					}
					break;
				case "lid2":
					if(_lidTwoOpen)
					{
						_lidTwoOpen = false;
						rotateVector = new Vector3(rotateAmount, 0, 0);
						iTween.RotateAdd (go, rotateVector, 1f);
					}
					else
					{
						_lidTwoOpen = true;
						rotateVector = new Vector3(-rotateAmount, 0, 0);
						iTween.RotateAdd (go, rotateVector, 1f);
					}
					break;
				case "lid3":
					if(_lidThreeOpen)
					{
						_lidThreeOpen = false;
						rotateVector = new Vector3(rotateAmount, 0, 0);
						iTween.RotateAdd (go, rotateVector, 1f);
					}
					else
					{
						_lidThreeOpen = true;
						rotateVector = new Vector3(-rotateAmount, 0, 0);
						iTween.RotateAdd (go, rotateVector, 1f);
					}
					break;
				}
			}
			else if (_inkSelected)
			{
				//Debug.Log("test");
				foreach(InkCartridge ink in _inkList)
				{
					if(go.name == ink.name)
					{
						ink.RefillInk();
					}
				}
//				switch(go.tag)
//				{
//				case "InkRed":
//					// TODO: Indkommenter og fix + 4. color
//					//if(_inkColorSelected == Color.red)
//						_redInk.RefillInk();
//					
//					_inkSelected = false;
//					break;
//				case "InkGreen":
//					//if(_inkColorSelected == Color.green)
//						_greenInk.RefillInk();
//					_inkSelected = false;
//					break;
//				case "InkBlue":
//					//if(_inkColorSelected == Color.blue)
//						_blueInk.RefillInk();
//					
//					_inkSelected = false;
//					break;
//				case "InkBlack":
//					//if( inkColorSelected == Color.black)
//						_blackInk.RefillInk();
//					break;
//				default:
//					_inkSelected = false;
//					break;
//				}
			}
		}
	}
	
	private void StartSiren(GameObject go)
	{
		_emptyCartridgesAmount++;
		_sirenEnabled = true;
		_errorSiren.renderer.material.SetFloat("_Progress", 0.05f);
	}
	
	private void StopSiren(GameObject go)
	{
		_emptyCartridgesAmount--;
		if(_emptyCartridgesAmount == 0)
		{
			_sirenEnabled = false;
			_errorSiren.renderer.material.SetFloat("_Progress", 1.0f);
		}
	}
	
	private void GetInkFromInv(Color col)
	{
		_inkSelected = true;
		_inkColorSelected = col;
	}
	#endregion
}
