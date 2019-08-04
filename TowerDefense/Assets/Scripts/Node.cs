using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

    bool canBuild = true;

    public Turret currentTurret;

    public void SetCurrentBuild(Turret turret)
    {
        if (turret == null)
        {
            currentTurret = null;
            canBuild = true;
            return;
        }

        currentTurret = turret;
        canBuild = false;
    }

    public bool IsCanBuild()
    {
        return canBuild;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (currentTurret != null)
            currentTurret.ShowStateDialog();
    }

    public void Initialize()
    {
        canBuild = true;
        currentTurret = null;
    }
}