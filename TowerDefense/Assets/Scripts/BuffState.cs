using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffState : MonoBehaviour {

    public string name = "";
    public Texture texture;
    public float speed = 0;
    public float effectiveTime = 0;         //isPermanent  ex: -1000

    public void SetSetup(string name, float speed, float effectiveTime)
    {
        this.name = name;
        this.speed = speed;
        this.effectiveTime = effectiveTime;
    }
}
