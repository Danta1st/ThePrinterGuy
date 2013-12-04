using UnityEngine;
using System.Collections;

public class EasterCat : MonoBehaviour {

	[SerializeField] iTween.EaseType easeIn = iTween.EaseType.linear;
	[SerializeField] iTween.EaseType easeOut = iTween.EaseType.linear;
	[SerializeField] Transform moveToTransform;
	[SerializeField] float waitTime = 25.0f;
	
	private Vector3 startPosition;
	
	private void Awake()
	{
		startPosition = gameObject.transform.position;
	}
	private void Start()
	{
		StartCoroutine(TriggerEasterCat());
	}
	
	IEnumerator TriggerEasterCat()
	{
		yield return new WaitForSeconds(waitTime);
		
		iTween.MoveTo(gameObject,iTween.Hash("position", moveToTransform.position, "time", 2.0f, "easetype", easeIn));
		
		yield return new WaitForSeconds(2.5f);
		
		iTween.MoveTo(gameObject,iTween.Hash("position", startPosition, "time", 1.0f, "easetype", easeOut));
		
		StartCoroutine(TriggerEasterCat());
	}
}
