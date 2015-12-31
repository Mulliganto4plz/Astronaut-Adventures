using UnityEngine;
using System.Collections;

public class MinibossScript : MonoBehaviour
{

    private float shootingDelay;
    float shootingTimer = 0;
    public float minimumFiringDistance = 10;
    [HideInInspector]
    public bool nowDescending;
    [HideInInspector]
    public bool imDead;

    //public float fireRate = 0;      //0 = single burst, the fire rate of the weapon
    public int Damage = 8;
    int shotCount;
    public LayerMask whatToHit;

    public Transform hitParticleEffect;
    Transform Alienfirepoint;
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
    // Use this for initialization
    void Start()
    {
        target = EnemyAI.target;
        shootingDelay = 0.9f;
        GetComponent<AudioSource>().volume = MainMenuScript.sfxVolume;
    }

    void Update()
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
                shotCount++;
                if (shotCount == 3)
                    StartCoroutine( RapidFire());
            }
        }
    }

    IEnumerator RapidFire()
    {
        shootingTimer = 2.5f;
        shotCount = 0;
        yield return new WaitForSeconds(1);
        //how many shots will fire in the rapid fire attack
        for(int i =0; i < 5; i++)
        {
            Shoot();
            //the space between the shots
            yield return new WaitForSeconds(0.15f);
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
        bs.DamageToDo(Damage);

    }
}