using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopoutManager : MonoBehaviour
{

    //TODO: Subscribe in cylinderManager Cylinder.OnCylinderHammeredIn += JOhnJohnFix;
    // and Subscribe in printerManager PaperTray.OnPaperTrayPenalty += PopoutcylinderHAndler;
    // Cylinder.PopoutCylinder(GO);

    #region Privates
    GameObject[] Popouts;
    #endregion

    #region Methods
    void Awake()
    {
        Popouts = GameObject.FindGameObjectsWithTag("Popout");
    }

    void OnEnable()
    {
        PaperTray.OnPaperTrayPenalty += PopoutCylinderHandler;
    }

    void OnDisable()
    {
        PaperTray.OnPaperTrayPenalty -= PopoutCylinderHandler;
    }

    private void PopoutCylinderHandler(GameObject rootPrinter)
    {
        if(rootPrinter != null && gameObject.transform.root.gameObject.Equals(rootPrinter))
        {
            int random = Random.Range(0, Popouts.Length);
			
            for (int i = 0; i < Popouts.Length; i++)
            {
                Popout _popout = Popouts[random].GetComponent<Popout>();
                if(_popout.GetIsOut() == false)
                {
                    _popout.PopoutCylinder();
                    break;
                }
                random++;

                if(random == Popouts.Length)
                {
                    random = 0;
                }
            }
        }
    }
    #endregion
}
