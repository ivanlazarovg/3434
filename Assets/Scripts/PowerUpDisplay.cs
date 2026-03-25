using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpDisplay : MonoBehaviour
{
    public GameObject displayMain;
    public Image nameDisplay;
    public Image ImageDisplay;
    public Image CumVeil;

    float timerBetween = 0f;
    float timerPulse = 0f;

    bool isPulsing = false;

    RectTransform rectTransform;

    Vector3 initialScale;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialScale = rectTransform.localScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (PowerUpManager.Instance.isLubed)
        {
            CumVeil.gameObject.SetActive(true);
        }
        else
        {
            CumVeil.gameObject.SetActive(false);
        }

        if(timerBetween > 1f)
        {
            isPulsing = true;
        }

        if (isPulsing)
        {
            if (timerPulse < 0.5f)
            {
                timerPulse += Time.deltaTime * 4f;
                rectTransform.localScale = Vector3.Lerp(initialScale, initialScale * 1.5f, Mathf.Sin(Mathf.Deg2Rad * timerPulse * 360));
                Debug.Log(Mathf.Sin(timerPulse * 360));
            }

            else
            {
                timerBetween = 0f;
                timerPulse = 0f;
                isPulsing = false;
            }
                

        }
        else
        {
            timerBetween += Time.deltaTime;
        }
    }

    public void SetPowerUpDisplay(PowerUp power)
    {
        nameDisplay.sprite = power.name;
        ImageDisplay.sprite = power.imageDisplay;
        nameDisplay.color = power.textDisplayColor;
    }

    
}
