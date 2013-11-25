using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	
    public float scrollSpeed = 0.05F;
	
	private bool _isScrolling = false;
	private float _offset = 0;
		
    void Update() {
		if(_isScrolling)
			Scroll();
		else if(_offset != 0)
			Reset();
    }
	
	public void SetCreditsRunning(bool isScrolling)
	{
		if(isScrolling)
			Invoke("Enable",1.0f);
		else
			Invoke("Reset",1.0f);
	}
	
	private void Enable()
	{
		_isScrolling = true;
	}
	
	private void Scroll()
	{		
        _offset = Time.time * Mathf.Abs(scrollSpeed);
        renderer.material.SetTextureOffset("_MainTex", new Vector2(0, _offset * -1.0f));		
	}
	
	private void Reset()
	{
		_isScrolling = false;
		_offset = 0;
	}
}
