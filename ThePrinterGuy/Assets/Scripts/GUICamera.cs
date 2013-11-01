using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUICamera : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private LayerMask _layerMaskGUI;
    //List of all gui elements.
    [SerializeField]
    private GameObject[] _guiList;
    //List of tool box pages.
    [SerializeField]
    private GameObject[] _toolBoxPages;
    #endregion

    #region Private Variables
    private GameObject _guiAnchorPointObject;
    private Camera _guiCamera;
    private RaycastHit _hit;
    private bool _isGUI = false;
    private float _timeScale = 0.0f;

    private List<GameObject> _guiSaveList = new List<GameObject>();

    //Tool box variables.
    private GameObject _toolBoxSelectionObject;
    private GameObject _toolBoxObject;
    private Vector3 _toolBoxStartPos;
    private Vector3 _toolBoxTargetPos;
    private int _toolBoxCurrentPageCount = 1;
    private int _toolBoxMaxPageCount;
    private float _toolBoxMoveDuration = 1.0f;
    private bool _isToolBoxOpen = false;
    #endregion

    #region Delegates and Events
    public delegate void InventoryPress(string buttonName);
    public static event InventoryPress OnInventoryPress;
    #endregion

    #region Enable and Disable
    void OnEnable()
    {
        GestureManager.OnTap += CheckCollision;
    }

    void OnDisable()
    {
        GestureManager.OnTap -= CheckCollision;
    }

    public void EnableGUICamera()
    {
        _isGUI = true;
        _guiCamera.gameObject.SetActive(true);
    }

    public void DisableGUICamera()
    {
        _isGUI = false;
        _guiCamera.gameObject.SetActive(false);
    }

    public void EnableGUIElement(string _name)
    {
        foreach(GameObject _gui in _guiList)
        {
            if(_gui.name == _name)
            {
                _gui.SetActive(true);
            }
        }
    }

    public void DisableGUIElement(string _name)
    {
        foreach(GameObject _gui in _guiList)
        {
            if(_gui.name == _name)
            {
                _gui.SetActive(false);
            }
        }
    }
    #endregion

    #region Start and Update
    // Use this for initialization
    void Start()
    {
        _guiCamera = GameObject.FindGameObjectWithTag("GUICamera").camera;
        _guiAnchorPointObject = GameObject.Find("GUIElements");
        _guiAnchorPointObject.transform.position = _guiCamera.transform.position;

        //Find specific gui objects in the gui list.
        //--------------------------------------------------//
        foreach(GameObject _guiObject in _guiList)
        {
            if(_guiObject.name == "ToolBox")
            {
                _toolBoxObject = _guiObject;
            }

            if(_guiObject.name == "Selection")
            {
                _toolBoxSelectionObject = _guiObject;
            }
        }
        //--------------------------------------------------//

        //Initialization of various tool box variable.
        //--------------------------------------------------//
        if(_toolBoxObject != null)
        {
            Vector3 _tempToolBoxPos = new Vector3(_toolBoxObject.transform.position.x, _toolBoxObject.transform.position.y, 5);
            _toolBoxObject.transform.position = _tempToolBoxPos;
    
            _toolBoxStartPos = _toolBoxObject.transform.position;
            _toolBoxTargetPos = _toolBoxObject.transform.FindChild("DestinationPosition").position;
            _toolBoxTargetPos.y = _toolBoxStartPos.y;
            _toolBoxTargetPos.z = _toolBoxStartPos.z;
    
            _toolBoxObject.transform.FindChild("DestinationPosition").gameObject.SetActive(false);
            _toolBoxMaxPageCount = _toolBoxPages.Length;
        }

        if(_toolBoxSelectionObject != null)
        {
            _toolBoxSelectionObject.SetActive(false);
        }
        //--------------------------------------------------//

        EnableGUICamera();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Class Methods
    private void CheckCollision(GameObject _go, Vector2 _screenPosition)
    {
        if(_isGUI)
        {
            Ray _ray = _guiCamera.ScreenPointToRay(_screenPosition);

            if(Physics.Raycast(_ray, out _hit, 100, _layerMaskGUI.value))
            {
                //General GUI layer mask.
                //-----------------------------------------------------------------------//
                if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUI"))
                {
                    if(_hit.collider.gameObject.name == "RestartButton")
                    {
                        RestartLevel();
                    }
                    else if(_hit.collider.gameObject.name == "PauseButton")
                    {
                        //Do something.
                    }
                }
                //ToolBox layer mask.
                //-----------------------------------------------------------------------//
                if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("GUIToolBox"))
                {
                    if(_hit.collider.gameObject.name == "ToolBoxButton")
                    {
                        if(_isToolBoxOpen)
                        {
                            _isToolBoxOpen = false;
                            MoveGUIElement(_toolBoxObject, _toolBoxStartPos, _toolBoxMoveDuration);
                            _toolBoxSelectionObject.SetActive(false);

                        }
                        else if(!_isToolBoxOpen)
                        {
                            _isToolBoxOpen = true;
                            MoveGUIElement(_toolBoxObject, _toolBoxTargetPos, _toolBoxMoveDuration);
                            _toolBoxSelectionObject.SetActive(false);
                        }
                    }
                    else if(_hit.collider.gameObject.name == "ToolBoxPageUp")
                    {
                        _toolBoxCurrentPageCount -= 1;

                        if(_toolBoxCurrentPageCount < 1)
                        {
                            _toolBoxCurrentPageCount = 1;
                        }
                        UpdateToolBoxPage();
                        _toolBoxSelectionObject.SetActive(false);
                    }
                    else if(_hit.collider.gameObject.name == "ToolBoxPageDown")
                    {
                         _toolBoxCurrentPageCount += 1;

                        if(_toolBoxCurrentPageCount > _toolBoxMaxPageCount)
                        {
                            _toolBoxCurrentPageCount = _toolBoxMaxPageCount;
                        }
                        UpdateToolBoxPage();
                        _toolBoxSelectionObject.SetActive(false);
                    }
                    else
                    {
                        if(OnInventoryPress != null)
                        {
                            OnInventoryPress(_hit.collider.gameObject.name);
                        }

                        _toolBoxSelectionObject.SetActive(true);
                        Vector3 _tempPos = _hit.collider.transform.position;
                        _tempPos.z = 6;
                        _toolBoxSelectionObject.transform.position = _tempPos;
                    }
                }
                //-----------------------------------------------------------------------//
            }
            else
            {
                _toolBoxSelectionObject.SetActive(false);
            }
        }
    }

    private void MoveGUIElement(GameObject _guiElement, Vector3 _position, float _duration)
    {
        iTween.MoveTo(_guiElement, iTween.Hash("position", _position, "duration", _duration));
    }

    #region GUI ToolBox
    private void UpdateToolBoxPage()
    {
        int _index = 0;
        foreach(GameObject _toolBoxPage in _toolBoxPages)
        {
            _index++;

            if(_index == _toolBoxCurrentPageCount)
            {
                _toolBoxPage.SetActive(true);
            }
            else
            {
                _toolBoxPage.SetActive(false);
            }
        }
    }
    #endregion

    #region GUI Restart
    private void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    #endregion

    #region GUI Save and Load
    private void SaveGUIState()
    {
        _timeScale = Time.timeScale;

        foreach(GameObject _gui in _guiList)
        {
            if(_gui.activeInHierarchy)
            {
                _guiSaveList.Add(_gui);
                _gui.SetActive(false);
            }
        }
    }

    private void LoadGUIState()
    {
        Time.timeScale = _timeScale;

        foreach(GameObject _gui in _guiSaveList)
        {
            _gui.SetActive(true);
        }
        _guiSaveList.Clear();
    }
    #endregion

    #endregion
}
