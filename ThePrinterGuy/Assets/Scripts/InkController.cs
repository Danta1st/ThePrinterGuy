using UnityEngine;
using System.Collections;

public class InkController : MonoBehaviour
{
	#region Variables Editable in Editor
	[SerializeField]
	private iTween.EaseType _lidSmoothing = iTween.EaseType.easeOutBack;
	[SerializeField]
	private float _animationSpeed = 0.5f;
	[SerializeField]
	private Color[] _cartridgeColor;
	#endregion
	
	#region Private Variables
	private System.Collections.Generic.List<InkCartridge> _inkList = new System.Collections.Generic.List<InkCartridge> ();
	#endregion
	
	#region Setup of Delegates
	void OnEnable ()
	{
		//ZoomHandler.OnInk += EnableInkTask;
	}
	
	void OnDisable ()
	{
		//ZoomHandler.OnInk -= EnableInkTask;
	}
	#endregion
	
	#region Initializatio of InkCartridges
	void Start ()
	{
		int index = 0;
		foreach (Transform t in transform) {
			if (t.gameObject.tag == "Ink" && index < _cartridgeColor.Length) {
				_inkList.Add (t.gameObject.GetComponent<InkCartridge> ());
				_inkList [index].InitializeInkCartridge (_cartridgeColor [index]);
				index++;
			}
		}
	}
	#endregion
	
	#region Delegate methods
	private void EnableInkTask ()
	{
		foreach (InkCartridge ic in _inkList) {
			ic.collider.enabled = true;	
		}
//		ZoomHandler.OnGoingFreeroam += DisableInkTask;
		GestureManager.OnSwipeRight += RotateRight;
	}
	
	private void DisableInkTask ()
	{
		foreach (InkCartridge ic in _inkList) {
			ic.collider.enabled = false;	
		}
//		ZoomHandler.OnGoingFreeroam -= DisableInkTask;
		GestureManager.OnSwipeRight -= RotateRight;
	}
	
	private void RotateLeft ()
	{
	}
	
	private void RotateRight (GameObject go)
	{
	}
	
	private void InsertInk (GameObject go, Vector2 screenPos)
	{
		if (go != null) {
			foreach (InkCartridge ink in _inkList) {
				if (go.name == ink.name)
					ink.RefillInk ();
			}
		}
	}
	#endregion
}
