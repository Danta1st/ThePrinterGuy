using UnityEngine;
using System.Collections;

public class PaperJam : MonoBehaviour
{
    [SerializeField]
    private Vector3 _shake = new Vector3(1.0f, 2.0f, 4.0f);
    [SerializeField]
    private float _shakeTime = 1.5f;
    [SerializeField]
    private GameObject tempPrefab;

    private GameObject particalHolder;
    private ParticleSystem smoke;

    void Awake()
    {
        particalHolder = new GameObject();
        particalHolder.transform.parent = gameObject.transform;
        particalHolder.transform.position = new Vector3(0, 0, 0);

        smoke = particalHolder.AddComponent<ParticleSystem>();
        smoke = tempPrefab.GetComponent<ParticleSystem>();

    }

	// Use this for initialization
	void Start ()
    {
        Shake();
        Smoke();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void Shake()
    {
        iTween.PunchRotation(gameObject, iTween.Hash("amount", _shake, "time", _shakeTime, "oncomplete", "Shake"));
    }

    void Smoke()
    {
        smoke.Play();
    }
}
