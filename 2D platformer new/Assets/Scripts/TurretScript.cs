using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {

    public Transform turretBullet;
    Transform firingPoint;
    public float firingTiming = 3;
    public float x, y, speed;
    bool continueFiring = true;

	// Use this for initialization
	void Start () {
        firingPoint = transform.Find("FiringPoint").GetComponent<Transform>();
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        while (continueFiring)
        {
            GameObject clone = ObjectPoolingScript.poolingScript.GetPooledObject("staticTurretShotList", turretBullet.gameObject);
            //Transform bullet = (Transform) Instantiate(turretBullet, firingPoint.position, firingPoint.rotation);
            clone.transform.position = firingPoint.position;
            clone.transform.rotation = firingPoint.rotation;
            clone.GetComponent<TurretBulletScript>().SetDirectionAndSpeed(x, y, speed);
            clone.SetActive(true);
            yield return new WaitForSeconds(firingTiming);
        }
    }
}
