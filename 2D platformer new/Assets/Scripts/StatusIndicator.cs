using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]
    private RectTransform healthBarRect;
    //[SerializeField]
    //private Text healthText;

    void Start()
    {
        if(healthBarRect == null)
            Debug.Log("STATUS INDICATOR: no health bar object preserto");
        //if (healthText == null)
            //Debug.LogError("STATUS INDICATOR: No health text object referenced!");
        
    }

    public void SetHealth(int _cur, int _max)
    {
        float _value = (float)_cur / _max;
        Image healthBarColor = healthBarRect.GetComponent<Image>();

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        //healthText.text = _cur + "/" + _max + " HP";

        if (_value >= 0.5 && _value <= 0.74)
            healthBarColor.color = new Color32(214, 255, 47, 255);
        else if (_value >= 0.25 && _value <= 0.49)
            healthBarColor.color = new Color32(255, 243, 26, 255);
        else if (_value >= 0 && _value <= 0.24)
            healthBarColor.color = new Color32(255, 82, 26, 255);
    }

}
