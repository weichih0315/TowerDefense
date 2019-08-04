using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour {

    public enum TurretMode { Turret, Laser}
    public TurretMode turretMode;

    [Header("General")]
    public int damage = 5;
	public float range = 15f;
    public LayerMask targetMask;
    public Turret upgradeTurret;
    public int upgradeMoney;
    public int sellMoney;

    [Header("Bullet")]
	public GameObject bulletPrefab;
	public float fireRate = 1f;
	private float fireCountdown = 0f;

	[Header("Laser")]
	public int damageOverTime = 30;

	public LineRenderer lineRenderer;
	public ParticleSystem laserImpactEffect;

	[Header("Unity Setup")]

	public Transform partToRotate;
	public float turnSpeed = 10f;

	public Transform firePoint;

    public BuildNode[] buildNodes;

    public GameObject rangeCircle;

    public StateDialog stateDialog;

    [Header("BuffState")]

    public BuffState[] buffStatesToTarget;
    public List<BuffState> buffStates;

    private Enemy targetEnemy;

    private void Awake()
    {
        rangeCircle.transform.localScale *= range;
    }

    void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}
	
	void UpdateTarget ()
	{

        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, range, targetMask);

        if (targetsInRadius.Length == 0)
        {
            targetEnemy = null;
            return;
        }
            

		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
		foreach (Collider target in targetsInRadius)
		{
            GameObject enemy = target.gameObject;
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}
		targetEnemy = nearestEnemy.GetComponent<Enemy>();
	}

	void Update () {
		if (targetEnemy == null)
		{
			if (turretMode == TurretMode.Laser)
			{
				if (lineRenderer.enabled)
				{
					lineRenderer.enabled = false;
                    laserImpactEffect.Stop();
                }
            }
		}
        else
        {
            if (turretMode == TurretMode.Laser)
            {
                Laser();
            }
            else
            {
                if (fireCountdown <= 0f)
                {
                    Shoot();
                    fireCountdown = 1f / fireRate;
                }

                fireCountdown -= Time.deltaTime;
            }
            LockOnTarget();
        }
	}

	void LockOnTarget ()
	{
		Vector3 dir = targetEnemy.transform.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	void Laser ()
	{
		targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        foreach (BuffState buffState in buffStatesToTarget)
        {
            targetEnemy.AddBuffState(buffState);
        }        

        if (!lineRenderer.enabled)
		{
			lineRenderer.enabled = true;
            laserImpactEffect.Play();
		}

		lineRenderer.SetPosition(0, firePoint.position);
		lineRenderer.SetPosition(1, targetEnemy.transform.position);

		Vector3 direction = (firePoint.position - targetEnemy.transform.position).normalized;
        SphereCollider collider = targetEnemy.GetComponent<SphereCollider>();

        laserImpactEffect.transform.position = targetEnemy.transform.position + direction * collider.radius;
        laserImpactEffect.transform.rotation = Quaternion.LookRotation(direction);
	}

	void Shoot ()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.SetDamage(damage);

		if (bullet != null)
			bullet.Seek(targetEnemy.transform);
	}

    public void ShowRangeCircle(bool show)
    {
        rangeCircle.SetActive(show);
    }

    public void ShowStateDialog()
    {
        stateDialog.gameObject.SetActive(true);
        stateDialog.UpdateState(this);
    }

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
