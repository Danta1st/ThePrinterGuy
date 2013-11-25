using UnityEngine;
using System.Collections;

public class credits : MonoBehaviour {

	
    public float scrollSpeed = 0.05F;
	
    void Update() {
        float offset = Time.time * Mathf.Abs(scrollSpeed);
        renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset * -1.0f));
    }
}
