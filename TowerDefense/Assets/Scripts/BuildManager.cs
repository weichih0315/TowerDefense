using System;
using UnityEngine;

public class BuildManager : MonoBehaviour {

	public static BuildManager Instance;

    public GameObject buildEffect;
    public GameObject sellEffect;

    Camera viewCamera;

    bool preBuild = false;
    Product currentProduct;
    Turret currentTurret;

    private void Awake ()
	{
		if (Instance != null)
		{
			Debug.LogError("More than one BuildManager in scene!");
			return;
		}
		Instance = this;

        viewCamera = Camera.main;
    }

    private void Update()
    {
        if (!preBuild)
            return;

        //
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * 0.5f);
        float rayDistance;
        Vector3 mousePoint = Vector3.zero;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            mousePoint = ray.GetPoint(rayDistance);
            mousePoint = GetModifyBuildPoint(currentTurret.buildNodes.Length, mousePoint);
            Debug.DrawLine(ray.origin, mousePoint, Color.red);

            currentTurret.transform.position = mousePoint;
        }

        if (Input.GetMouseButtonDown(0))
        {
            bool isCanBuild = true;
            foreach (BuildNode buildNode in currentTurret.buildNodes)
            {
                isCanBuild &= buildNode.IsCanBuild();
            }
            if (isCanBuild)
            {
                if (currentProduct.price <= GameManager.instance.money)
                {
                    GameManager.instance.money -= currentProduct.price;
                    GameUI.instance.UpdateMoney();
                    Build();
                    ReadyToBuild(currentProduct);
                    AudioManager.instance.PlaySound2D("BuildComplete");
                }
                else
                {
                    AudioManager.instance.PlaySound2D("BuildError");
                    Debug.Log("Not enough money ");
                }
            }
            else
            {
                AudioManager.instance.PlaySound2D("BuildError");
                Debug.Log("this place can't build");
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CancelToBuild();
        }
    }

    public void Build()
    {
        GameObject effect = (GameObject)Instantiate(buildEffect, currentTurret.transform.position, currentTurret.transform.rotation);
        ParticleSystem effectParticle = effect.GetComponent<ParticleSystem>();

        //依大小修正
        effectParticle.Stop();
        ParticleSystem.ShapeModule shapeModule = effect.GetComponent<ParticleSystem>().shape;
        shapeModule.radius *= Mathf.Pow(currentTurret.buildNodes.Length, 0.5f);
        ParticleSystem.EmissionModule emissionModule = effect.GetComponent<ParticleSystem>().emission;
        emissionModule.burstCount *= currentTurret.buildNodes.Length;
        effectParticle.Play();

        Destroy(effect, effectParticle.main.startLifetime.constant);

        foreach (BuildNode buildNode in currentTurret.buildNodes)
        {
            buildNode.FinishBuild(currentTurret);
            buildNode.gameObject.SetActive(false);
        }
        preBuild = false;
        currentTurret.enabled = true;
        currentTurret.ShowRangeCircle(false);
        currentTurret = null;
    }

    public void ReadyToBuild(Product product)
    {
        if (preBuild)
            CancelToBuild();
        preBuild = true;
        currentProduct = product;
        currentTurret = Instantiate(currentProduct.turret,Vector3.zero,Quaternion.identity);
        currentTurret.enabled = false;
    }

    public void UpgradeBuild(Turret turret)
    {
        currentTurret = Instantiate(turret.upgradeTurret, turret.transform.position, Quaternion.identity);
        Build();
        AudioManager.instance.PlaySound2D("BuildUpgrade");
        Destroy(turret.gameObject);
    }

    public void CancelToBuild()
    {
        preBuild = false;
        currentProduct = null;
        Destroy(currentTurret.gameObject);
    }

    public void Sell(Turret turret)
    {
        GameObject effect = (GameObject)Instantiate(sellEffect, turret.transform.position, turret.transform.rotation);
        ParticleSystem effectParticle = effect.GetComponent<ParticleSystem>();

        //依大小修正
        effectParticle.Stop();
        ParticleSystem.ShapeModule shapeModule = effect.GetComponent<ParticleSystem>().shape;
        shapeModule.radius *= Mathf.Pow(turret.buildNodes.Length, 0.5f);
        ParticleSystem.EmissionModule emissionModule = effect.GetComponent<ParticleSystem>().emission;
        emissionModule.burstCount *= turret.buildNodes.Length;
        effectParticle.Play();

        Destroy(effect, effectParticle.main.startLifetime.constant);

        foreach (BuildNode buildNode in turret.buildNodes)
        {
            buildNode.gameObject.SetActive(true);
            buildNode.ReleaseNode();
            buildNode.gameObject.SetActive(false);
        }

        AudioManager.instance.PlaySound2D("SellBuild");
        Destroy(turret.gameObject);
    }

    Vector3 GetModifyBuildPoint(int buildNodeNum, Vector3 mousePoint)
    {
        if (buildNodeNum == 1 || buildNodeNum % 9 == 0)
        {
            mousePoint.x = (float)Math.Round(mousePoint.x, MidpointRounding.AwayFromZero);
            mousePoint.z = (float)Math.Round(mousePoint.z, MidpointRounding.AwayFromZero);
        }
        else if (buildNodeNum % 4 == 0)
        {
            mousePoint.x = (float)Math.Round(mousePoint.x, MidpointRounding.AwayFromZero) + 0.5f;
            mousePoint.z = (float)Math.Round(mousePoint.z, MidpointRounding.AwayFromZero) + 0.5f;
        }

        return mousePoint;
    }
}
