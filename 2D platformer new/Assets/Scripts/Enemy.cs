using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    //handle camera shaking
    public float camShakeAmt = 0.1f;
    public float camShakeLength = 1.5f;
    CameraShake camShake;

    //is this enemy already dead?
    [HideInInspector]
    public bool imDead = false;

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        public int damageOnCollision = 15;

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

    public EnemyStats stats = new EnemyStats();

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        stats.Init();

        if (statusIndicator != null)
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);

        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null)
            Debug.LogError("no camShake script found on gm object");
        GetComponent<AudioSource>().volume = MainMenuScript.sfxVolume;
    }

     

    public void DamageEnemy(int damage)
    {
        if (!imDead)
        {
            stats.curHealth -= damage;
            if (stats.curHealth <= 0)
            {
                GameMaster.KillEnemy(this);
                camShake.Shake(camShakeAmt, camShakeLength);
            }

            if (statusIndicator != null)
                statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null && !_player.isInvul)
        {
            _player.DamagePlayer(stats.damageOnCollision);
            if (gameObject.tag == ("SuicideAlien"))
                DamageEnemy(stats.maxHealth);
        }
    }
}
