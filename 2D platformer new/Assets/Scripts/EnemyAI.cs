using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{

    // What to chase?
    public static Transform target;

    //how far do ranged attackers stay away
    public float distanceToStayAwayFromPlayer;

    //current distance from the player
    float currentDistanceFromPlayer;

    //distance where enemies notice the player
    public float aggroDistance = 30;

    // How many times each second we will update our path
    public float updateRate = 2f;

    // Caching
    private Seeker seeker;
    private Rigidbody2D rb;

    //The calculated path
    public Path path;

    //The AI's speed per second
    [SerializeField]
    float speed; 
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;
    [HideInInspector]
    public bool nowDescending;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    private bool searchingForPlayer = false;

    void Start()
    {
        
        RangedFlyingEnemy ranged = gameObject.GetComponent<RangedFlyingEnemy>();
        MinibossScript miniboss = gameObject.GetComponent<MinibossScript>();
        if (ranged != null || miniboss != null)
        {
            speed = Random.Range(1060f, 1120f);
        }
        else
        {
            speed = Random.Range(1145f, 1170f);
            distanceToStayAwayFromPlayer = 0f;
        }
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
                return;
            }
        }

        Vector3 playerPos = new Vector3(target.position.x, target.position.y + distanceToStayAwayFromPlayer, target.position.z);
        // Start a new path to the target position, return the result to the OnPathComplete method
        
        seeker.StartPath(transform.position, playerPos, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            searchingForPlayer = false;
            target = sResult.transform;
            StartCoroutine(UpdatePath());
            yield break;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
                yield break;
            }
            yield break;
        }


        Vector3 playerPos = new Vector3(target.position.x + distanceToStayAwayFromPlayer, target.position.y + distanceToStayAwayFromPlayer, target.position.z);
        // Start a new path to the target position, return the result to the OnPathComplete method
        currentDistanceFromPlayer = Vector3.Distance(transform.position, target.transform.position);
       
        seeker.StartPath(transform.position, playerPos, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        

        
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
                return;
            }
        }
        
        //TODO: Always look at player?

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;
        
        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        if (target != null)
            currentDistanceFromPlayer = Vector3.Distance(transform.position, target.transform.position);
        if (currentDistanceFromPlayer < aggroDistance && !nowDescending)
        {
            //Move the enemy
            rb.AddForce(dir, fMode);
        }
        

        //keep the enemy above the foreground
        if (transform.position.y <= -1)
            transform.position = new Vector2(transform.position.x, -1);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }
}
