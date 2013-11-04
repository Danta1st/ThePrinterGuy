using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour {

	
	public delegate void InventoryPaperSelected();
	public static event InventoryPaperSelected OnPaperSelect;
	
	public delegate void InventoryInkSelected(Color color);
	public static event InventoryInkSelected OnInkSelect;
	
	public delegate void InventoryHammerSelected();
	public static event InventoryHammerSelected OnHammerSelect;
	
	void OnEnable()
	{
		GUIGameCamera.OnInventoryPress += GetItemFromInv;	
	}
	
	void OnDisable()
	{
		GUIGameCamera.OnInventoryPress -= GetItemFromInv;
	}
	
	private void GetItemFromInv(string itemName)
	{
		switch(itemName)
		{
		case "ToolBoxInkBlackButton":
			if(OnInkSelect != null)
				OnInkSelect(Color.black);
			break;
		case "ToolBoxHammerButton":
			if(OnHammerSelect != null)
				OnHammerSelect();
			break;
		case "ToolBoxPaperButton":
			if(OnPaperSelect != null)
				OnPaperSelect();
			break;
		}
	}
}
