using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;
    public static bool playMusic = true;

    private HealthBar healthBar;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public Transform enemyDeathParticles;

    AudioSource backgroundMusic;
    public AudioClip[] musicArray;

    int lives = 3;
    public RectTransform[] LifePics;

    [SerializeField]
    public GameObject gameOverScreen;

    void Awake()
    {
        if (gm == null)
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        if (healthBar == null)
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
    }

    void Start()
    {
        backgroundMusic = transform.Find("BackgroundMusic").GetComponent<AudioSource>();
        //StartCoroutine( BackroundMusicPlayer());
        CheckpointScript.latestCheckpoint = spawnPoint;
    }

    public IEnumerator BackroundMusicPlayer()
    {
        while (playMusic)
        {
            backgroundMusic.Play();
            yield return new WaitForSeconds(backgroundMusic.clip.length);
        }
    }

    public IEnumerator _RespawnPlayer()
    {
        if (lives > 0)
        {
            Destroy(LifePics[lives].gameObject);
            lives--;
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            yield return new WaitForSeconds(spawnDelay);
            spawnPoint = CheckpointScript.latestCheckpoint;
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            HealthBar.hasRespawned = true;
            healthBar.Init();
            Transform spawnParticlesClone = (Transform)Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);   //creates the re-spawn effect than destroyes the clone
            Destroy(spawnParticlesClone.gameObject, 3f);
            ObjectPoolingScript.muzzleFlashCloneNumber++;
        }
        else
            gameOverScreen.SetActive(true);
    }

	public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm._RespawnPlayer());
    }

    public static void KillEnemy (Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        GameObject enemy = _enemy.gameObject;
        enemy.GetComponent<AudioSource>().clip = musicArray[0];
        enemy.GetComponent<AudioSource>().Play();
        //Transform _clone = (Transform) Instantiate(enemyDeathParticles, _enemy.transform.position, Quaternion.identity);
        GameObject clone = ObjectPoolingScript.poolingScript.GetPooledObject("deathParticlesList", enemyDeathParticles.gameObject);
        clone.transform.position = _enemy.transform.position;
        clone.transform.rotation = Quaternion.identity;
        StartCoroutine(EnemyDeathParticles(clone));
        //Destroy(_clone.gameObject, 4f);
        enemy.GetComponent<Rigidbody2D>().gravityScale = 1;
        enemy.GetComponent<EnemyAI>().enabled = false;
        enemy.GetComponent<Enemy>().imDead = true;
        int rnd = Random.Range(0, 2);
        enemy.transform.rotation = Quaternion.Euler(0, 0, (rnd == 0 ? 45 : -45));
        enemy.layer = 14;

        //Destroy(_enemy.gameObject);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator EnemyDeathParticles(GameObject clone)
    {
        clone.SetActive(true);
        yield return new WaitForSeconds(4);
        clone.SetActive(false);
    }
}
