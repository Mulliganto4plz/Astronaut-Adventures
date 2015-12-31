using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            curHealth = maxHealth;

        }
    }

    public PlayerStats stats = new PlayerStats();
    //period of time the player is invulnerable after taking damage
    private float invulTime = 1f;
    //how long has the player been imvulnerable
    private float invulStartTime;
    //indicator for the enemys if the player is invulnerable or not
    [HideInInspector]
    public bool isInvul = false;
    //refs to the sprites of the player and his arm for flashing when the player in invul
    SpriteRenderer player, arm, pistol;

    bool gravityIsReversed;

    public int fallBoundary = -20;

    [SerializeField]
    private StatusIndicator statusIndicator;
    [SerializeField]
    private HealthBar healthBar;

    void Start()
    {
        stats.Init();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        player = transform.Find("Graphics").GetComponent<SpriteRenderer>();
        arm = transform.Find("Arm").GetComponent<SpriteRenderer>();
        pistol = transform.Find("Arm/Pistol").GetComponent<SpriteRenderer>();
        if (pistol == null)
            Debug.Log("Cant find the pistol friend, try again");
        if (statusIndicator == null)
        {
            Debug.LogError("No status indicator referenced on Player");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    void Update()
    {
        if (transform.position.y < fallBoundary)
            DamagePlayer(stats.maxHealth);
        if(gameObject.transform.position.y >= 3 && !gravityIsReversed)
        {
            gravityIsReversed = true;
            Physics.gravity = new Vector3(-1, -50F, 0);
        }
    }

    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
        healthBar.CurrentHealth = stats.curHealth;
        if (stats.curHealth <= 0)
            GameMaster.KillPlayer(this);
        StartCoroutine( Invulnerability());
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    IEnumerator Invulnerability()
    {
        isInvul = true;
        Color32 normal = new Color32(255, 255, 255, 255);
        Color32 faded = new Color32(255, 255, 255, 50);
        invulStartTime = Time.time;
        while ((Time.time - invulStartTime) < invulTime)
        {
            player.color = faded;
            arm.color = faded;
            pistol.color = faded;
            yield return new WaitForSeconds(0.2f);
            player.color = normal;
            arm.color = normal;
            pistol.color = faded;
            yield return new WaitForSeconds(0.2f);
        }
        isInvul = false;
    }
}
