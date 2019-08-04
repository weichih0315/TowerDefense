using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject hpUI;

    public float startSpeed = 10f;
    private float speed = 10f;
    public float startHealth = 10f;
    public float health = 10f;

    public List<BuffState> buffStates;

    private Transform target;
    private int wavepointIndex = 0;

    Material material;

    protected bool dead;                                        //判斷死亡
    public static event System.Action OnDeathStatic;            //死亡觸發
    public static event System.Action OnEndStatic;              //走到底觸發

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Start () {
        target = Waypoints.points[0];
        InvokeRepeating("UpdateBuffStates", 0f, 1f);
        speed = startSpeed;
        health = startHealth;
    }
    
    void Update () {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWaypoint();
        }
	}

    private void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            if (OnEndStatic != null)
                OnEndStatic();
            Destroy(gameObject);
            return;
        }
            
        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    public void SetCharacteristics(float speed,float health, Color color)
    {
        this.startSpeed = speed;
        this.startHealth = health;
        this.material.color = color;
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
        else
            hpUI.transform.localScale = new Vector3(health / startHealth, 1,1);
    }

    public void Die()
    {
        dead = true;
        if (OnDeathStatic != null)
        {
            OnDeathStatic();
        }
        GameObject.Destroy(gameObject);
    }

    public void UpdateBuffStates()
    {
        speed = startSpeed;
        for (int i = 0; i < buffStates.Count; i++)
        {
            if (buffStates[i].effectiveTime < 1)
            {
                buffStates.Remove(buffStates[i]);
                continue;
            }
            buffStates[i].effectiveTime -= 1;
            speed += buffStates[i].speed;
        }
    }

    public void AddBuffState(BuffState buffState)
    {
        for (int i = 0; i < buffStates.Count; i++)
        {
            if (buffState.name == buffStates[i].name)
            {
                buffStates[i].effectiveTime = Mathf.Max(buffStates[i].effectiveTime, buffState.effectiveTime);
                return;
            }
        }
        speed += buffState.speed;
        buffStates.Add(buffState);
    }
}