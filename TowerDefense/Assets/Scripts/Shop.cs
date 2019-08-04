using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    public Product[] products;

    public static Shop instance;

    private void Awake()
    {
        instance = this;   
    }

    public void SelectProduct(int index)
    {
        AudioManager.instance.PlaySound2D("MouseClick");
        BuildManager.Instance.ReadyToBuild(products[index]);
    }
}

[System.Serializable]
public class Product
{
    public Turret turret;
    public int price;

    public Product(Turret turret, int price)
    {
        this.turret = turret;
        this.price = price;
    }
}
