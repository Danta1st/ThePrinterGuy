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
    private bool _isSwiping = false;
    private GameObject _touchBeganObject;

    private bool _canTap = true;
    private bool _canDoubletap = true;
    private bool _canPress = true;
    private bool _canSwipe = true;
    private bool _canPinchSpread = true;
    private bool _canDrag = true;
    #endregion

    #region Properties
    public void DisableTap()
    {
        _canTap = false;
    }

    public void EnableTap()
    {
        _canTap = true;
    }

    public void DisableDoubleTap()
    {
        _canDoubletap = false;
    }

    public void EnableDoubleTap()
    {
        _canDoubletap = true;
    }

    public void DisablePress()
    {
        _canPress = false;
    }

    public void EnablePress()
    {
        _canPress = true;
    }

    public void DisableSwipes()
    {
        _canSwipe = false;
    }

    public void EnableSwipes()
    {
        _canSwipe = true;
    }

    public void DisablePinchSpread()
    {
        _canPinchSpread = false;
    }

    public void EnablePinchSpread()
    {
        _canPinchSpread = true;
    }

    public void DisableDrag()
    {
        _canDrag = false;
    }

    public void EnableDrag()
    {
        _canDrag = true;
    }
    #endregion

    #region Delegates & Events
    public delegate void TapAction(GameObject go, Vector2 screenPosition);
    public static event TapAction OnTap;

    public delegate void DoubleTapAction(GameObject go, Vector2 screenPosition);
    public static event DoubleTapAction OnDoubleTap;

    public delegate void PressAction(GameObject go, Vector2 screenPosition);
    public static event PressAction OnPress;

    public delegate void SwipeRightAction();
    public static event SwipeRightAction OnSwipeRight;

    public delegate void SwipeLeftAction();
    public static event SwipeLeftAction OnSwipeLeft;

    public delegate void SwipeUpAction();
    public static event SwipeUpAction OnSwipeUp;

    public delegate void SwipeDownAction();
    public static event SwipeDownAction OnSwipeDown;

    public delegate void DragAction(GameObject go, Vector2 deltaPosition);
    public static event DragAction OnDrag;

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
        UpdateTouchBeginGameObject();

        UpdateTouchBeginTimes();

        UpdateTouchBeginPositions();

        if(Input.touchCount == 1)
        {
            var primaryFinger = Input.GetTouch(0);

            //Single Tap
            if(primaryFinger.phase == TouchPhase.Ended && primaryFinger.tapCount == 1)
            {
                if((Time.time - _touchBeginTimes[primaryFinger.fingerId]) < _tapThreshold)
                {
                    //Single Tap Event
                    if(OnTap != null)
                        OnTap(_touchBeganObject, primaryFinger.position);
                }
            }

            //Double Tap
            else if(primaryFinger.phase == TouchPhase.Ended && primaryFinger.tapCount == 2)
            {
                //DoubleTap Event
                if(OnDoubleTap != null)
                    OnDoubleTap(_touchBeganObject, primaryFinger.position);
            }

            //Press
            else if(primaryFinger.phase == TouchPhase.Stationary && _isSwiping == false)
            {
                if(Time.time - _touchBeginTimes[primaryFinger.fingerId] >= _pressThreshold)
                {
                    if(OnPress != null && _isPressing == false)
                    {
                        //Press Event
                        OnPress(_touchBeganObject, primaryFinger.position);
                        _isPressing = true;
                    }
                }
                else if(Time.time - _touchBeginTimes[primaryFinger.fingerId] <= _pressThreshold)
                    _isPressing = false;
            }

            //Swipes
            else if(primaryFinger.phase == TouchPhase.Moved &&
                    (Time.time - _touchBeginTimes[primaryFinger.fingerId]) > _tapThreshold)
            {
                _isSwiping = true;
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
            else if(primaryFinger.phase == TouchPhase.Ended)
            {
                _isSwiping = false;
                _touchBeganObject = null;
            }

            //Drag
            if(primaryFinger.phase == TouchPhase.Moved)
            {
                //Drag Event
                if(OnDrag != null)
                    OnDrag(_touchBeganObject, primaryFinger.deltaPosition);
            }
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
         if(Input.GetMouseButtonUp(0))
         {
             var mousePosition = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        
             if(OnTap != null)
                 OnTap(_touchBeganObject, mousePosition);
         }

        if(Input.GetMouseButtonUp(1))
        {
            var mousePosition = new Vector2(Input.mousePosition.x,Input.mousePosition.y);

            //DoubleTap Event
            if(OnDoubleTap != null)
                OnDoubleTap(_touchBeganObject, mousePosition);
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
    void UpdateTouchBeginGameObject()
    {
#if UNITY_ANDROID
        foreach(Touch t in Input.touches)
        {
            if(t.fingerId == 0 && t.phase == TouchPhase.Began)
            {
                var touchPosition = new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, 0);
                Ray ray = camera.ScreenPointToRay(touchPosition);
                RaycastHit hit;
    
                if (Physics.Raycast(ray, out hit))
                    {
                        _touchBeganObject = hit.collider.gameObject;
                    }
            }
        }
#endif
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
    
            if (Physics.Raycast(ray, out hit))
                {
                    _touchBeganObject = hit.collider.gameObject;
                }
            }
#endif
    }

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
