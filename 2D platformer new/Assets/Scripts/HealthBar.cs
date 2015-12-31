using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public RectTransform healthTransform;
    public static bool hasRespawned = false;
    private float cachedY;
    Player player;
    private float minXValue; //no health
    private float maxXValue; //full health
    private float currentHealth;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;
            HandleHealth();
        }
    }

    private float maxHealth;
    public Image visualHealth;
    //public Text healthText;     //if i want to show numeric hp value in the text field
    

    // Use this for initialization
    void Start ()
    {
        cachedY = healthTransform.position.y;
        if (maxXValue == 0)
            maxXValue = healthTransform.position.x;

        Init();
    }

    public void Init()
    {
        if (hasRespawned)
        {
            healthTransform.position = new Vector3(maxXValue, cachedY);
            visualHealth.color = new Color32(0, 255, 12, 255);
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        minXValue = healthTransform.position.x - healthTransform.rect.width;
        maxHealth = player.stats.maxHealth;
        currentHealth = maxHealth;
    }

    private void HandleHealth()
    {
        //healthText.text = "Health: " + currentHealth;     //uncomment if using text to show health

        float currentXValue = MapValues(CurrentHealth, 0, maxHealth, minXValue, maxXValue);
        healthTransform.position = new Vector3(currentXValue, cachedY);

        if(CurrentHealth > maxHealth / 2)//if over 50% hp then the R color is changed
        {
            visualHealth.color = new Color32((byte)MapValues(CurrentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);
        }
        else //changing the G color
        {
            visualHealth.color = new Color32(255, (byte)MapValues(CurrentHealth, 0, maxHealth / 2, 0, 255), 0, 255);
        }
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
