using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StateDialog : MonoBehaviour {

    public Text nowAttack;
    public Text nowRange;
    public Text nowSpeed;
    public Text upgradeAttack;
    public Text upgradeRange;
    public Text upgradeSpeed;

    public Button upgradeButton;
    public Text upgradeText;
    public Text sellText;

    public Turret currentTurret;

    private static Turret selectTurrent;            //目前選擇物件

    bool hide = false;                              //防止顯示時也判定關閉  第一次點擊的BUG

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject() && currentTurret != selectTurrent && hide)       //UI畫面外  &&  所選物件  && 防BUG用
            {
                currentTurret.ShowRangeCircle(false);
                gameObject.SetActive(false);
            }
            else
                selectTurrent = null;
            hide = true;
        }
    }

    public void UpdateState(Turret turret)
    {
        turret.ShowRangeCircle(true);
        selectTurrent = turret;
        currentTurret = turret;
        hide = false;
        Turret upgradeTurret = currentTurret.upgradeTurret;
        if (turret.turretMode == Turret.TurretMode.Turret)
        {
            nowAttack.text = currentTurret.damage + "";
            nowRange.text = currentTurret.range + "";
            nowSpeed.text = currentTurret.fireRate + "";
            upgradeAttack.text = (upgradeTurret == null) ? "暫無" : upgradeTurret.damage + "";
            upgradeRange.text = (upgradeTurret == null) ? "暫無" : upgradeTurret.range + "";
            upgradeSpeed.text = (upgradeTurret == null) ? "暫無" : upgradeTurret.fireRate + "";
        }
        else
        {
            nowAttack.text = turret.damageOverTime + "";
            nowRange.text = turret.range + "";
            nowSpeed.text = "1";
            upgradeAttack.text = (upgradeTurret == null) ? "暫無" : upgradeTurret.damageOverTime + "";
            upgradeRange.text = (upgradeTurret == null) ? "暫無" : upgradeTurret.range + "";
            upgradeSpeed.text = (upgradeTurret == null) ? "暫無" : "1";
        }

        if (upgradeTurret == null)
        {
            upgradeButton.interactable = false;
            upgradeText.text = "<b>Max</b>";
        }
        else
            upgradeText.text = "<b>Upgrade</b>$" + currentTurret.upgradeMoney;

        sellText.text = "<b>Sell</b>$" + currentTurret.sellMoney;
    }

    public void Upgrade()
    {
        BuildManager.Instance.UpgradeBuild(currentTurret);
    }

    public void Sell()
    {
        GameManager.instance.money += currentTurret.sellMoney;
        GameUI.instance.UpdateMoney();
        BuildManager.Instance.Sell(currentTurret);
    }
}
