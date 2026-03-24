using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PowerUp currentPowerUp;

    public Rigidbody playerRb;

    private static PowerUpManager instance;

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
    void Start()
    {
        
    }

    public void DeactivatePowerUp()
    {
        if (!currentPowerUp.isActive) return;
        currentPowerUp.Deactivate();
        currentPowerUp  = null;
        //RemoveDisplay


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(currentPowerUp != null)
                if (!currentPowerUp.isActive)
                    currentPowerUp.Activate();
            
           
        }
    }

    public void SetActivePowerUp(PowerUp powerUp)
    {
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
