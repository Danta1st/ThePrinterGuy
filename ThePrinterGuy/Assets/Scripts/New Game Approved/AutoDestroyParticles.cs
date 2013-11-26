using UnityEngine;
using System.Collections;

public class AutoDestroyParticles : MonoBehaviour {

	void LateUpdate()
	{
		if (!particleSystem.IsAlive())
			Destroy(gameObject);
	}
}
