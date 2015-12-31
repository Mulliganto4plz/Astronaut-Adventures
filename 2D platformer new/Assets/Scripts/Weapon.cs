using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0;      //0 = single burst, the fire rate of the weapon
    public int Damage = 10;
    float knockbackAmount = 500;
    public LayerMask whatToHit;

    public Transform hitParticleEffect;     //the 'sparks' particle effect from hitting obstacle with a bullet
    public Transform BulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    public float effectSpawnRate = 10;      //maximum number of bullets per second, maximum effects shown on screen

    

    float timeToFire = 0;
    float timeToSpawnEffect = 0;
    Transform firePoint;

    // Use this for initialization
    void Awake()
    {
        firePoint = transform.FindChild("FirePoint");
        if (firePoint == null)
            Debug.Log("missing the firepoint! address por favor");
        GetComponent<AudioSource>().volume = Mathf.Clamp(MainMenuScript.sfxVolume, 0, 0.2f); 
    }

    

    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
                transform.root.GetComponent<PlatformerCharacter2D>().Knockback(knockbackAmount);
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();

                transform.root.GetComponent<PlatformerCharacter2D>().Knockback(knockbackAmount);
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.DamageEnemy(Damage);
            }
        }

        //if (Time.time >= timeToSpawnEffect)
        //{
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null || hit.transform.gameObject.layer == 14)
            {
                hitPos = (mousePosition - firePointPosition) * 40;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
                

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        //}
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        

        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            StartCoroutine(ImpactParticles(hitPos, hitNormal));
            //Transform hitParticle = (Transform) Instantiate(hitParticleEffect, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
            //Destroy(hitParticle.gameObject, 1f);
        }

        //Transform clone = (Transform)Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        GameObject clone = ObjectPoolingScript.poolingScript.GetPooledObject("weaponMouzzleFlashList", muzzleFlashPrefab.gameObject);
        clone.transform.position = firePoint.position;
        clone.transform.rotation = firePoint.rotation;
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        clone.transform.parent = firePoint;
        float size = Random.Range(0.4f, 0.6f);
        clone.transform.localScale = new Vector3(size, size, size);
        StartCoroutine(MuzzleFlash(clone));
        //Destroy(clone.gameObject, 0.02f);
    }

    public IEnumerator ImpactParticles(Vector3 hitPos, Vector3 hitNormal)
    {
        GameObject clone = ObjectPoolingScript.poolingScript.GetPooledObject("impactParticlesList", hitParticleEffect.gameObject);
        clone.transform.position = hitPos;
        clone.transform.rotation = Quaternion.FromToRotation(Vector3.right, hitNormal);
        clone.SetActive(true);
        yield return new WaitForSeconds(1);
        clone.SetActive(false);
    }

    public IEnumerator MuzzleFlash(GameObject clone)
    {
        clone.SetActive(true);
        yield return new WaitForSeconds(0.02f);
        clone.SetActive(false);
    }
}
