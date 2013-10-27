using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestureManager : MonoBehaviour
{

    #region Editor Publics
    [SerializeField]
    private float _tapThreshold = 0.1f;
    [SerializeField]
    private float _pressThreshold = 1.0f;
    [SerializeField]
    private float _swipeThreshold = 10.0f;
    [SerializeField]
    private float _swipeOffset = 2.0f;
    #endregion

    #region Privates
    private Dictionary<int,float> _touchBeginTimes = new Dictionary<int, float>();
    private Dictionary<int,Vector2> _touchBeginPositions = new Dictionary<int, Vector2>();
    private bool _isPressing = false;
    #endregion

    #region Delegates & Events
    public delegate void TapAction(Vector2 screenPosition);
    public static event TapAction OnTap;

    public delegate void DoubleTapAction(Vector2 screenPosition);
    public static event DoubleTapAction OnDoubleTap;

    public delegate void PressAction(Vector2 screenPosition);
    public static event PressAction OnPress;

    public delegate void SwipeRightAction();
    public static event SwipeRightAction OnSwipeRight;

    public delegate void SwipeLeftAction();
    public static event SwipeLeftAction OnSwipeLeft;

    public delegate void SwipeUpAction();
    public static event SwipeUpAction OnSwipeUp;

    public delegate void SwipeDownAction();
    public static event SwipeDownAction OnSwipeDown;

    public delegate void PinchAction(float deltaDistance);
    public static event PinchAction OnPinch;

    public delegate void SpreadAction(float deltaDistance);
    public static event SpreadAction OnSpread;
    #endregion


    void Update()
    {
        //Universal Quit Button
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }


    #if UNITY_ANDROID
        UpdateTouchBeginTimes();

        UpdateTouchBeginPositions();

        if(Input.touchCount == 1)
        {
            var primaryFinger = Input.GetTouch(0);

            //FIXME: Fix single tap solution. This method reacts on fast swipes and it should not.
            if((Time.time - _touchBeginTimes[primaryFinger.fingerId]) <= _tapThreshold)
            {
                if(primaryFinger.phase == TouchPhase.Ended && primaryFinger.tapCount == 1)
                {
                    if(OnTap != null)
                        OnTap(primaryFinger.position);
                }
            }

            //Double Tap
            if(primaryFinger.phase == TouchPhase.Ended && primaryFinger.tapCount == 2)
            {
                //DoubleTap Event
                if(OnDoubleTap != null)
                    OnDoubleTap(primaryFinger.position);
            }

            //FIXME: OnPress is currently counting time wrong. if phase switching to stationary while above treshold OnPress is called.
            if(primaryFinger.phase == TouchPhase.Stationary)
            {
                if(Time.time - _touchBeginTimes[primaryFinger.fingerId] >= _pressThreshold)
                {
                    if(OnPress != null && _isPressing == false)
                    {
                        OnPress(primaryFinger.position);
                        _isPressing = true;
                    }
                }
                else if(Time.time - _touchBeginTimes[primaryFinger.fingerId] <= _pressThreshold)
                    _isPressing = false;
            }

            //Swipes
            if(primaryFinger.phase == TouchPhase.Moved && primaryFinger.deltaTime < 0.02f)
            {
                //Horizontal swipes
                if(primaryFinger.deltaPosition.x >= _swipeThreshold &&
                    Mathf.Abs(primaryFinger.deltaPosition.y) <= _swipeOffset)
                {
                    //SwipeRight Event
                    if(OnSwipeRight != null)
                        OnSwipeRight();
                }
                else if(primaryFinger.deltaPosition.x <= _swipeThreshold * -1.0f &&
                         Mathf.Abs(primaryFinger.deltaPosition.y) <= _swipeOffset)
                {
                    //SwipeLeft Event
                    if(OnSwipeLeft != null)
                        OnSwipeLeft();

                }

                //Vertical swipes
                if(primaryFinger.deltaPosition.y >= _swipeThreshold &&
                    Mathf.Abs(primaryFinger.deltaPosition.x) <= _swipeOffset)
                {
                    //SwipeUp Event
                    if(OnSwipeUp != null)
                        OnSwipeUp();
                }
                else if(primaryFinger.deltaPosition.y <= _swipeThreshold * -1.0f &&
                         Mathf.Abs(primaryFinger.deltaPosition.x) <= _swipeOffset)
                {
                    //SwipeDown Event
                    if(OnSwipeDown != null)
                        OnSwipeDown();
                }
            }

            //TODO: Implement Drag
        }



        if(Input.touchCount == 2)
        {
            var primaryFinger = Input.touches[0];
            var secondaryFinger = Input.touches[1];

            var beginDistance = Vector2.Distance(_touchBeginPositions[primaryFinger.fingerId],
                                                    _touchBeginPositions[secondaryFinger.fingerId]);


            if(primaryFinger.phase == TouchPhase.Moved ||
                secondaryFinger.phase == TouchPhase.Moved)
            {
                var distance = Vector2.Distance(primaryFinger.position,
                                                    secondaryFinger.position);

                var deltaDistance = Vector2.Distance(primaryFinger.deltaPosition,
                                                        secondaryFinger.deltaPosition);

                if(distance < beginDistance)
                {
                    //Pinch Event
                    if(OnPinch != null)
                        OnPinch(deltaDistance);
                }
                else if(distance > beginDistance)
                {
                    //Spead Event
                    if(OnSpread != null)
                        OnSpread(deltaDistance);
                }
            }

            //TODO: Implement twofinger Press & tap
        }
    #endif

    #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //DoubleTap Event
            if(OnDoubleTap != null)
                OnDoubleTap(Vector2.zero);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //SwipeRight Event
            if(OnSwipeRight != null)
                OnSwipeRight();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //SwipeLeft Event
            if(OnSwipeLeft != null)
                OnSwipeLeft();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            //SwipeUp Event
            if(OnSwipeUp != null)
                OnSwipeUp();
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            //SwipeDown Event
            if(OnSwipeDown != null)
                OnSwipeDown();
        }

        if(Input.GetKey(KeyCode.KeypadMinus))
        {
            if(OnPinch != null)
                OnPinch(1.3f);
        }

        if(Input.GetKey(KeyCode.KeypadPlus))
        {
            if(OnSpread != null)
                OnSpread(1.3f);
        }
    #endif

    }

    #region Methods
    void UpdateTouchBeginTimes()
    {
        foreach(Touch t in Input.touches)
        {
            if(t.phase == TouchPhase.Began)
            {
                _touchBeginTimes[t.fingerId] = Time.time;
            }
//            else if(t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
//                _touchBeginTimes.Remove(t.fingerId);
        }
    }

    //Notice this also updates when touch is stationary, for better pinch/spread functionality
    void UpdateTouchBeginPositions()
    {
        foreach(Touch t in Input.touches)
        {
            if(t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary)
            {
                _touchBeginPositions[t.fingerId] = t.position;
            }
            else if(t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                _touchBeginPositions.Remove(t.fingerId);
            }
        }
    }
    #endregion

    //Debug GUI - OnGUI should otherwise be avoided on tablets
    void OnGUI()
    {
        foreach(Touch touch in Input.touches)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("FingerId: " + touch.fingerId.ToString());
            GUILayout.Label("Position: " + touch.position.ToString());
            GUILayout.Label("DeltaTime: " + touch.deltaTime);
            GUILayout.Label("DeltaPosition: " + touch.deltaPosition.ToString());
            GUILayout.Label("TouchPhase: " + touch.phase.ToString());
            GUILayout.Label("TapCount: " + touch.tapCount);
            GUILayout.EndVertical();
        }

        foreach(var pair in _touchBeginTimes)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("fingerId: " + pair.Key + " Began at: " + pair.Value);
            GUILayout.EndVertical();
        }
    }
}
