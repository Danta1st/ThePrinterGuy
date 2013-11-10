using UnityEngine;
using System.Collections;

public class InkCartridge : MonoBehaviour
{
	private Color _color;
	
	public void EnableCartridge()
	{
	}
	
	public void DisableCartridge()
	{
	}

	public void SetColor(Color col)
	{
		_color = col;
		this.gameObject.renderer.material.color = _color;
	}
	
	public Color GetColor()
	{
		return _color;
	}
	
	public void EnableCollider()
	{
		gameObject.collider.enabled = true;
	}
	
	public void DisableCollider()
	{
		gameObject.collider.enabled = false;
	}
	
	public void EnableRenderer()
	{
		gameObject.renderer.enabled = true;
	}
	
	public void DisableRenderer()
	{
		gameObject.renderer.enabled = false;
	}
}
