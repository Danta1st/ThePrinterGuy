using UnityEngine;
using System.Collections;

public class BoostButton : MonoBehaviour {

    #region Editor Publics
    [SerializeField]
    private float leverUnlockScore = 0.0f;
    [SerializeField]
    private float boostDuration = 5.0f;
    [SerializeField]
    private int boostCharges = 1;
    [SerializeField]
    private float leverPullForce = 0.0f;
    [SerializeField]
    private float animTime = 1.0f;
    #endregion

    #region Privates
    private float highscore;
    #endregion

    #region Delegates
    public delegate void BoostOn();
    public static event BoostOn BoostActivated;
    public delegate void BoostOff();
    public static event BoostOff BoostDeactivated;
    private Vector3 leverPullVector = new Vector3();
    private bool canBePulled = true;
    #endregion

	// Use this for initialization
	void Start()
    {
        leverPullVector = new Vector3(leverPullForce, 0.0f, 0.0f);
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
        Debug.Log(thisLever);
        if(gameObject == thisLever && highscore >= leverUnlockScore && boostCharges > 0 && canBePulled)
        {
            canBePulled = false;
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
        iTween.PunchRotation(gameObject, leverPullVector, animTime);

        yield return new WaitForSeconds(boostDuration);

        if(BoostDeactivated != null)
        {
            BoostDeactivated();
        }

        canBePulled = true;

        Debug.Log("Boost Over");
    }
}
