using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingScript : MonoBehaviour
{

    static public ObjectPoolingScript poolingScript;
    public Dictionary<string, List<GameObject>> objectPool;

    public GameObject staticTurretShot;
    List<GameObject> staticTurretShotList;
    int turretShotAmount = 11;

    public GameObject weaponMuzzleFlash;
    List<GameObject> weaponMouzzleFlashList;
    int muzzleFlashAmount = 5;
    static public int muzzleFlashCloneNumber = 0;

    public GameObject alienBullet;
    List<GameObject> alienBulletList;
    int alienBulletAmount = 3;

    public GameObject impactParticles;
    List<GameObject> impactParticlesList;
    int impactParticlesAmount = 2;

    public GameObject deathParticles;
    List<GameObject> deathParticlesList;
    int deathParticlesAmount = 2;

    void Awake()
    {
        poolingScript = this;
        objectPool = new Dictionary<string, List<GameObject>>();

        staticTurretShotList = new List<GameObject>();
        for (int i = 0; i < turretShotAmount; i++)
        {
            GameObject obj = Instantiate(staticTurretShot);
            obj.SetActive(false);
            staticTurretShotList.Add(obj);
        }

        weaponMouzzleFlashList = new List<GameObject>();
        for (int i = 0; i < muzzleFlashAmount; i++)
        {
            GameObject obj = Instantiate(weaponMuzzleFlash);
            obj.SetActive(false);
            weaponMouzzleFlashList.Add(obj);
        }

        alienBulletList = new List<GameObject>();
        for (int i = 0; i < alienBulletAmount; i++)
        {
            GameObject obj = Instantiate(alienBullet);
            obj.SetActive(false);
            alienBulletList.Add(obj);
        }

        impactParticlesList = new List<GameObject>();
        for (int i = 0; i < impactParticlesAmount; i++)
        {
            GameObject obj = Instantiate(impactParticles);
            obj.SetActive(false);
            impactParticlesList.Add(obj);
        }

        deathParticlesList = new List<GameObject>();
        for (int i = 0; i < deathParticlesAmount; i++)
        {
            GameObject obj = Instantiate(deathParticles);
            obj.SetActive(false);
            deathParticlesList.Add(obj);
        }

        objectPool.Add("staticTurretShotList", staticTurretShotList);
        objectPool.Add("weaponMouzzleFlashList", weaponMouzzleFlashList);
        objectPool.Add("alienBulletList", alienBulletList);
        objectPool.Add("impactParticlesList", impactParticlesList);
        objectPool.Add("deathParticlesList", deathParticlesList);
    }

    public GameObject GetPooledObject(string listName, GameObject objectName)
    {
        int i = 0;
        List<GameObject> list = objectPool[listName];
        if (listName == "weaponMouzzleFlashList")
            i = muzzleFlashCloneNumber;
        for (; i < list.Count; i++)
        {
            if (!list[i].activeInHierarchy)
                return list[i];
        }

        GameObject obj = Instantiate(objectName);
        list.Add(obj);
        return obj;
    }
}
