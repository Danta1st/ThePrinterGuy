using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageCharacter : MonoBehaviour
{

    #region Editor Publics
    [SerializeField]
    private bool _isUnlocked = false;
    [SerializeField]
    private List<bool> _unlockedLevels = new List<bool>();
    #endregion

    #region StageUnlocked Get/Set
    public void SetUnlocked()
    {
        _isUnlocked = true;
    }

    public bool GetUnlocked()
    {
        return _isUnlocked;
    }
    #endregion

    #region LevelUnlocked Get/Set
    public void SetUnlockedLevel(int index)
    {
        _unlockedLevels[index] = true;
    }

    public bool GetUnlockedLevel(int index)
    {
        return _unlockedLevels[index];
    }
    #endregion

    #region AllLevelsUnlocked Get/Set
    public void SetUnlockedLevelsAll()
    {
        for (int i = 0; i < _unlockedLevels.Count; i++)
        {
            _unlockedLevels[i] = true;
        }
    }

    public List<bool> GetUnlockedLevelsAll()
    {
        return _unlockedLevels;
    }
    #endregion
}
