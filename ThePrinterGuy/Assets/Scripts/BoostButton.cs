using UnityEngine;
using System.Collections;

public class BoostButton : MonoBehaviour {

    #region Editor Publics
    [SerializeField]
    private float leverUnlockScore;
    [SerializeField]
    private float boostDuration = 5.0f;
    [SerializeField]
    private int boostCharges = 1;
    #endregion

    #region Privates
    private float highscore;
    #endregion

    #region Delegates
    public delegate void BoostOn();
    public static event BoostOn BoostActivated;
    public delegate void BoostOff();
    public static event BoostOff BoostDeactivated;
    #endregion

	// Use this for initialization
	void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update()
    {
	
	}

    void OnEnable()
    {
        //GestureManager.OnDrag += OnLeverPull;
        GestureManager.OnTap += OnLeverPull;
    }

    void OnDisable()
    {
        //GestureManager.OnDrag -= OnLeverPull;
        GestureManager.OnTap -= OnLeverPull;
    }

    void Highscore(float newHighscore)
    {
        highscore = newHighscore;
    }

    void OnLeverPull(GameObject thisLever, Vector2 screenPos)//, Vector2 deltaPos)
    {
        if(gameObject == thisLever && highscore >= leverUnlockScore && boostCharges > 0)
        {
            boostCharges--;

            if(BoostActivated != null)
            {
                BoostActivated();
            }

            Debug.Log("Boost Begun");

            StartCoroutine(WaitForBoost());
        }
    }

    IEnumerator WaitForBoost()
    {
        yield return new WaitForSeconds(boostDuration);

        if(BoostDeactivated != null)
        {
            BoostDeactivated();
        }

        Debug.Log("Boost Over");
    }
}
