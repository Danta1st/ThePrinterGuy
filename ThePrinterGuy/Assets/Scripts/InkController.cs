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
	[SerializeField]
	private iTween.EaseType _easetype;
	[SerializeField]
	private float _inkMoveSpeed = 0.5f;
	[SerializeField]
	private float[] _lidStartTime;
	#endregion
	
	#region Private Variables
	private List<InkCartridge> _printInks = new List<InkCartridge>();
	private List<InkCartridge> _guiInks = new List<InkCartridge>();
	private List<InkLid> _inkLids = new List<InkLid>();
	private int _emptyInk;
	#endregion
	
	
	#region delegates
	public delegate void InkInsertedSuccessfully();
	public static event InkInsertedSuccessfully OnInkInsertedSuccess;

	public delegate void InkInsertedUnsuccessfully();
	public static event InkInsertedUnsuccessfully OnInkInsertedFailed;
	#endregion
	

	
	#region Setup of Delegates
	void OnEnable ()
	{
		QATestCamera.OnInk += EnableInkTask;
		QATestCamera.OnInkOff += DisableInkTask;
		//ZoomHandler.OnInk += EnableInkTask;

	}
	
	void OnDisable ()
	{
		QATestCamera.OnInk -= EnableInkTask;
		QATestCamera.OnInkOff -= DisableInkTask;
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
		
		index = _inkColor.Length - 1;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Ink"))
		{
			if(index >= 0) {
				_printInks.Add(g.GetComponent<InkCartridge>());
				_printInks[_printInks.Count - 1].SetColor(_inkColor[index]);
				index--;
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
		
	}
	#endregion
	
	#region delegate methods
	private void EnableInkTask()
	{
		GestureManager.OnSwipeRight += InsertInk;
		
		foreach(InkCartridge i in _guiInks)
		{
			i.EnableCollider();
			i.EnableRenderer();
		}
		
		foreach(InkCartridge i in _printInks)
		{
			i.EnableRenderer();
		}
		
		int index = 0;
		foreach(InkLid l in _inkLids)
		{
			if(index < _inkColor.Length)
			{
				l.InitializeLid(false);
				l.StartTime(_lidStartTime[index]);
				index++;
			}
			else
			{
				l.InitializeLid(false);
				l.StartTime();
			}
		}
		
		_emptyInk = Random.Range(0, _inkColor.Length);
		
		_printInks[_emptyInk].DisableRenderer();
		
		
	}
	
	private void DisableInkTask()
	{
		GestureManager.OnSwipeRight -= InsertInk;
		
		foreach(InkCartridge i in _guiInks)
		{
			i.DisableCollider();
			i.DisableRenderer();
			i.gameObject.transform.position = i.GetStartPos();
		}
		
		foreach(InkLid l in _inkLids)
		{
			l.StopLid();
		}
	}
	
	private void InsertInk(GameObject go)
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
						
						
						iTween.MoveTo(go, iTween.Hash("position", i.gameObject.transform.position, "easetype", _easetype, "time", _inkMoveSpeed, "oncomplete", "MoveBack"));
					}
					else
					{
						if(OnInkInsertedFailed != null)
							OnInkInsertedFailed();
						
						Hashtable values = new Hashtable();
						values.Add("moveObj", go);
						values.Add ("target", go.transform.position);
						
						iTween.MoveTo(go, iTween.Hash("position", i.gameObject.transform.position, "easetype", _easetype, "time", _inkMoveSpeed, "oncomplete", "OnInkSuccess",
							"oncompletetarget", this.gameObject, "oncompleteparams", values));
					}
				}
				else if(colorInserted.Equals(i.GetColor()) && i.IsEnabled())
				{
					if(OnInkInsertedFailed != null)
							OnInkInsertedFailed();
					
					Hashtable values = new Hashtable();
					values.Add("moveObj", go);
					values.Add ("target", go.transform.position);
					
					iTween.MoveTo(go, iTween.Hash("position", i.gameObject.transform.position, "easetype", _easetype, "time", _inkMoveSpeed, "oncomplete", "MoveBack",
						"oncompletetarget", this.gameObject, "oncompleteparams", values));
					
					break;
				}
			}
		}
	}
	#endregion
	
	#region private methods
	private void OnInkSuccess()
	{
		if(OnInkInsertedSuccess != null)
			OnInkInsertedSuccess();
	}
	
	private void MoveBack(object values)
	{
		Hashtable ht = (Hashtable)values;
		iTween.MoveTo((GameObject)ht["moveObj"], iTween.Hash("position", (Vector3)ht["target"], "easetype", _easetype, "time", _inkMoveSpeed));
	}
	#endregion
	
}
