using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerScript : MonoBehaviour {

    public Transform alienShip;
    public Transform bigAlienShip;
    public Transform specialEnemy;
    public Transform[] nonEnemyStuff;
    public bool ApplySlowDecsent;
    public Transform[] meleeAlienSpawnPoint;
    public Transform[] rangedAlienSpawnPoint;
    public Transform[] specialEnemySpawnPoint;
    public Transform[] nonEnemyStuffSpawnPoint;
    bool hasSpawned;
    public float descentEnd;

    List<Transform> objectsDescending = new List<Transform>();

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !hasSpawned)
        {
            for(int i = 0; i < meleeAlienSpawnPoint.Length; i++)
            {
                {
                    Transform obj = (Transform) Instantiate(alienShip, meleeAlienSpawnPoint[i].position, meleeAlienSpawnPoint[i].rotation);
                    if(ApplySlowDecsent)
                    {
                        SlowDescent slow = obj.gameObject.AddComponent<SlowDescent>();
                        slow.descentEnd = descentEnd;
                        obj.GetComponent<EnemyAI>().nowDescending = true;
                        objectsDescending.Add(obj);
                    }
                }
            }
            for (int i = 0; i < rangedAlienSpawnPoint.Length; i++)
            {
                Transform obj = (Transform) Instantiate(bigAlienShip, rangedAlienSpawnPoint[i].position, rangedAlienSpawnPoint[i].rotation);
                if (ApplySlowDecsent)
                {
                    SlowDescent slow = obj.gameObject.AddComponent<SlowDescent>();
                    slow.descentEnd = descentEnd;
                    obj.GetComponent<EnemyAI>().nowDescending = true;
                    obj.GetComponent<RangedFlyingEnemy>().nowDescending = true;
                    objectsDescending.Add(obj);
                }
            }
            for(int i = 0; i < specialEnemySpawnPoint.Length; i++)
            {
                Transform obj = (Transform)Instantiate(specialEnemy, specialEnemySpawnPoint[i].position, specialEnemySpawnPoint[i].rotation);
                if(ApplySlowDecsent)
                {
                    SlowDescent slow = obj.gameObject.AddComponent<SlowDescent>();
                    slow.descentEnd = descentEnd;
                    obj.GetComponent<EnemyAI>().nowDescending = true;
                    obj.GetComponent<MinibossScript>().nowDescending = true;
                    objectsDescending.Add(obj);
                }
            }
            if (nonEnemyStuff.Length > 0)
                for (int i = 0; i < nonEnemyStuff.Length; i++)
                {
                    Transform obj = (Transform) Instantiate(nonEnemyStuff[i], nonEnemyStuffSpawnPoint[i].position, nonEnemyStuffSpawnPoint[i].rotation);
                    if (ApplySlowDecsent)
                    {
                        Rigidbody2D rb = obj.gameObject.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            SlowDescent slow = obj.gameObject.AddComponent<SlowDescent>();
                            slow.descentEnd = descentEnd;
                            objectsDescending.Add(obj);
                        }
                    }
                }
            hasSpawned = true;
            if (!ApplySlowDecsent)
                Destroy(gameObject);
            else
                StartCoroutine( DescentManager());
        }
    }

    IEnumerator DescentManager()
    {
        bool continueDescent = true;
        while(continueDescent)
        {
            foreach (Transform trans in objectsDescending)
            {
                if(trans.position.y < descentEnd)
                {
                    if(trans.name == "ForceField(Clone)")
                    {
                        foreach (Transform trans2 in objectsDescending)
                        {
                            SlowDescent sd = trans2.GetComponent<SlowDescent>();
                            Destroy(sd);
                            EnemyAI eai = trans2.GetComponent<EnemyAI>();
                            if(eai != null)
                            {
                                eai.nowDescending = false;
                                RangedFlyingEnemy rfe = trans2.GetComponent<RangedFlyingEnemy>();
                                MinibossScript miniboss = trans2.GetComponent<MinibossScript>();
                                if (rfe != null)
                                    rfe.nowDescending = false;
                                else if (miniboss != null)
                                    miniboss.nowDescending = false;
                            }
                        }
                        Destroy(trans.gameObject);
                    }
                    continueDescent = false;
                    break;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }
}

