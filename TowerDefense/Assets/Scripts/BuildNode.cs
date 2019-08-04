using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNode : MonoBehaviour {
        
    public LayerMask targetMask;

    Material material;

    Node node;

    bool isCanBuild;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        isCanBuild = false;
        material.color = new Color(1, 0, 0, 0.4f);
        Collider[] targetsInRadius = Physics.OverlapBox(transform.position, new Vector3(0.1f,0.5f,0.1f), Quaternion.identity, targetMask);

        if (targetsInRadius.Length < 1)
            return;

        node = targetsInRadius[0].gameObject.GetComponent<Node>();

        if (node.IsCanBuild())
        {
            isCanBuild = true;
            material.color = new Color(0, 1, 0, 0.4f);
        }
    }

    public void FinishBuild(Turret turret)
    {
        Collider[] targetsInRadius = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.5f, 0.1f), Quaternion.identity, targetMask);
        node = targetsInRadius[0].gameObject.GetComponent<Node>();
        node.SetCurrentBuild(turret);
    }

    public bool IsCanBuild()
    {
        return isCanBuild;
    }

    public void ReleaseNode()
    {
        Collider[] targetsInRadius = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.5f, 0.1f), Quaternion.identity, targetMask);
        node = targetsInRadius[0].gameObject.GetComponent<Node>();
        node.Initialize();
    }
}
