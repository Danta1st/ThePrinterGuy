using UnityEngine;
using System.Collections;

public class InkCartridge : MonoBehaviour
{
	private Color _color;
	private Vector3 _defPos;
	//private GameObject _lid;
	
	void Start()
	{
		_defPos = this.gameObject.transform.position;
	}
	
	public Vector3 GetStartPos()
	{
		return _defPos;
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
	
	public bool IsEnabled()
	{
		return gameObject.renderer.enabled;
	}
}
