using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PowerUp currentPowerUp;

    public Rigidbody playerRb;

    private static PowerUpManager instance;

    public PowerUpDisplay powerUpDisplay;

    public static PowerUpManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<PowerUpManager>();
            }
            return instance;
        }
    }

    public void DeactivatePowerUp()
    {
        if (!currentPowerUp.isActive) return;
        currentPowerUp.Deactivate();
        currentPowerUp  = null;

        powerUpDisplay.gameObject.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(currentPowerUp != null)
            {
                if (!currentPowerUp.isActive)
                {
                    currentPowerUp.Activate();

                    if(currentPowerUp is DildoPogo)
                    {
                        DeactivatePowerUp();
                    }
                }
                    
            }
                
                    
            
           
        }
    }

    public void SetActivePowerUp(PowerUp powerUp)
    {
        powerUpDisplay.gameObject.SetActive(true);
        powerUpDisplay.SetPowerUpDisplay(powerUp);

        currentPowerUp = powerUp;
        Debug.Log("picked up power " + powerUp.name);
    }
}

public enum ActivePowerUpType
{
    dildo,
    enema,
    lube,
    none
}
