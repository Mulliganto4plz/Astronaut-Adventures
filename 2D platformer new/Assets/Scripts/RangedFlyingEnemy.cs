using UnityEngine;
using System.Collections;

public class RangedFlyingEnemy : MonoBehaviour {

    private float shootingDelay;
    float shootingTimer = 0;
    public float minimumFiringDistance = 10;
    [HideInInspector]
    public bool nowDescending;
    private bool imDead;

    //public float fireRate = 0;      //0 = single burst, the fire rate of the weapon
    public int damageOfMyBullet = 6;
    public LayerMask whatToHit;

    public Transform hitParticleEffect;
    private Transform Alienfirepoint;
    private Transform target;
    public Transform alienBullet;
    

    //float timeToFire = 0;
    float timeToSpawnEffect = 0;

    void Awake()
    {
        
        Alienfirepoint = transform.FindChild("AlienFirepoint");
        if (Alienfirepoint == null)
            Debug.Log("ALIENT WEAPON: cant find the alienfirepoint, check this");
    }

    void Start () {
        target = EnemyAI.target;
        shootingDelay = Random.Range(1.5f, 1.8f);
    }
	
	void Update ()
    {
        if (target == null)
        {
            StartCoroutine(WaitTillPlayerFound());
            return;
            
        }
        else
        {
            shootingTimer -= Time.deltaTime;

            float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
            imDead = gameObject.GetComponent<Enemy>().imDead;
            if (distance <= minimumFiringDistance && shootingTimer <= 0 && !nowDescending && !imDead)
            {
                shootingTimer = shootingDelay;
                Shoot();
            }
        }

        
	}

    IEnumerator WaitTillPlayerFound()
    {
        if (EnemyAI.target == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(WaitTillPlayerFound());
        }
        else
        {
            target = EnemyAI.target;
            yield break;
        }
    }

    void Shoot()
    {
        Vector2 firePointPosition = new Vector2(Alienfirepoint.position.x, Alienfirepoint.position.y);
        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, targetPosition - firePointPosition, 100, whatToHit);

        //Debug.DrawLine(firePointPosition, (targetPosition - firePointPosition) * 100, Color.cyan);
        if (hit.collider != null)
        {
            //Debug.DrawLine(firePointPosition, hit.point, Color.red);
        }

        GameObject bulletClone = ObjectPoolingScript.poolingScript.GetPooledObject("alienBulletList", alienBullet.gameObject);
        //Transform bulletClone = (Transform) Instantiate(alienBullet, Alienfirepoint.position, Alienfirepoint.rotation);
        bulletClone.transform.position = Alienfirepoint.position;
        bulletClone.transform.rotation = Alienfirepoint.rotation;
        bulletClone.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
        BulletScript bs = bulletClone.GetComponent<BulletScript>();
        bs.DamageToDo(damageOfMyBullet);
    }
}
