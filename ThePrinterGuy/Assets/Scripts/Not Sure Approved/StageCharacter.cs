using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageCharacter : MonoBehaviour
{

    #region Editor Publics
    [SerializeField]
    private bool _isUnlocked = false;
    [SerializeField]
    private List<Material> materialList = new List<Material>();
    [SerializeField]
    private List<Texture2D> textureList = new List<Texture2D>();
    [SerializeField]
    private Texture2D blackTexture;
    #endregion

    private int[] highscores;

    void Awake()
    {
        Unlock();
    }

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

    public void Unlock()
    {
        highscores = SaveGame.GetPlayerHighscores();
        if(highscores[4] > 0)
            _isUnlocked = true;

        if(_isUnlocked)
        {
            int i = 0;
            foreach(Material material in materialList)
            {
                material.mainTexture = textureList[i];
                i++;
            }
            gameObject.transform.FindChild("Lock").renderer.enabled = false;
            if((gameObject.name == "stage2Char" && highscores[4] >= 0))
                gameObject.transform.FindChild("lobbyArrow").renderer.enabled = true;
            if((gameObject.name == "stage1Char" && highscores[4] <= 0))
                gameObject.transform.FindChild("lobbyArrow").renderer.enabled = false;
        }
        else
        {
            foreach(Material material in materialList)
            {
                material.mainTexture = blackTexture;
            }
            gameObject.transform.FindChild("Lock").renderer.enabled = true;
            gameObject.transform.FindChild("lobbyArrow").renderer.enabled = false;
        }
    }

}
