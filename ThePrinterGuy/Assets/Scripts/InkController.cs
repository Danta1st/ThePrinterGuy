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
	private int _emptyInk;
	#endregion
	
	
	#region delegates
	private delegate void InkInsertedSuccessfully();
	private static event InkInsertedSuccessfully OnInkInsertedSuccess;

	private delegate void InkInsertedUnsuccessfully();
	private static event InkInsertedUnsuccessfully OnInkInsertedFailed;
	#endregion
	
	#region Setup of Delegates
	void OnEnable ()
	{
		//ZoomHandler.OnInk += EnableInkTask;

	}
	
	void OnDisable ()
	{
		//ZoomHandler.OnInk -= EnableInkTask;
//		
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
				_inkLids[index].InitializeLid(false);
				index++;
			}
		}
		
//		EnableInkTask();
	}
	#endregion
	
	#region delegate methods
	private void EnableInkTask()
	{
		GestureManager.OnSwipeRight += InsertInk;
//		GestureManager.OnTap += InsertInk;
		
		foreach(InkCartridge i in _guiInks)
		{
			i.EnableCollider();
			i.EnableRenderer();
		}
		
		foreach(InkCartridge i in _printInks)
		{
			i.EnableCollider();
		}
		
		_emptyInk = Random.Range(0, 3);
		
		_printInks[_emptyInk].DisableRenderer();
		
		StartCoroutine("SwapLidStatus");
	}
	
	private void DisableInkTask()
	{
		GestureManager.OnSwipeRight -= InsertInk;
//		GestureManager.OnTap -= InsertInk;
		
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
	
	private void InsertInk(GameObject go)//, Vector2 screenPos)
	{
		
		if(go != null && go.tag == "GuiInk")
		{
			Color colorInserted = go.GetComponent<InkCartridge>().GetColor();
			
			foreach(InkCartridge i in _printInks)
			{
				if(colorInserted.Equals(i.GetColor()) && !i.IsEnabled())
				{
					if(_inkLids[_emptyInk].IsOpen())
					{
						i.EnableRenderer();
						if(OnInkInsertedSuccess != null)
							OnInkInsertedSuccess();
						Debug.Log("success");
					}
					else
					{
						if(OnInkInsertedFailed != null)
							OnInkInsertedFailed();
						Debug.Log ("failed");
					}
				}
				else if(colorInserted.Equals(i.GetColor()) && i.IsEnabled())
				{
					if(OnInkInsertedFailed != null)
							OnInkInsertedFailed();
						
					Debug.Log ("failed");
					break;
				}
			}
		}
	}
	#endregion
	
	#region coroutines
	IEnumerator SwapLidStatus()
	{
		while(true)
		{
			foreach(InkLid i in _inkLids)
			{
				i.SwapLidState();
			}
			
			yield return new WaitForSeconds(2);
		}
	}
	#endregion
}
