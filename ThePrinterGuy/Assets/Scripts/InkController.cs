using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InkController : MonoBehaviour
{
	#region editor variables
	[SerializeField]
	private Color[] _inkColor;
	[SerializeField]
	private float _doorDelay = 0.5f;
	#endregion
	
	#region Private Variables
	private List<InkCartridge> _printInks = new List<InkCartridge>();
	private List<InkCartridge> _guiInks = new List<InkCartridge>();
	private List<InkLid> _inkLids = new List<InkLid>();
	#endregion
	
	
	
	#region Setup of Delegates
	void OnEnable ()
	{
		//ZoomHandler.OnInk += EnableInkTask;
		GestureManager.OnSwipeRight += InsertInk;;
	}
	
	void OnDisable ()
	{
		//ZoomHandler.OnInk -= EnableInkTask;
		GestureManager.OnSwipeRight -= InsertInk;
	}
	
	void Start()
	{
		int index = 0;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("GuiInk"))
		{
			if(index < _inkColor.Length) {
				_guiInks.Add(g.GetComponent<InkCartridge>());
				_guiInks[index].SetColor(_inkColor[index]);
				index++;
			}
		}
		
		index = 0;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Ink"))
		{
			if(index < _inkColor.Length) {
				_printInks.Add(g.GetComponent<InkCartridge>());
				_printInks[index].SetColor(_inkColor[index]);
				index++;
			}
		}
		
		index = 0;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("InkLid"))
		{
			if(index < _inkColor.Length)
			{
				_inkLids.Add(g.GetComponent<InkLid>());
				index++;
			}
		}
		
		EnableInkTask();
	}
	#endregion
	
	#region delegate methods
	private void EnableInkTask()
	{
		foreach(InkCartridge i in _guiInks)
		{
			i.EnableCollider();
			i.EnableRenderer();
		}
		
		foreach(InkCartridge i in _printInks)
		{
			i.EnableCollider();
		}
		
		StartCoroutine("SwapLidStatus");
	}
	
	private void DisableInkTask()
	{
		foreach(InkCartridge i in _guiInks)
		{
			i.DisableCollider();
			i.DisableRenderer();
		}
		
		foreach(InkCartridge i in _printInks)
		{
			i.DisableCollider();
		}
		
		StopCoroutine("SwapLidStatus");
	}
	
	private void InsertInk()
	{
	}
	#endregion
	
	#region coroutines
	IEnumerator SwapLidStatus()
	{
		while(true)
		{
			
			
			yield return new WaitForSeconds(_doorDelay);
		}
	}
	#endregion
}
