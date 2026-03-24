using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PowerUp activePowerUp;

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
       // activePowerUp.Deactivate();
       // activePowerUp = null;
        //RemoveDisplay


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            activePowerUp.Activate();
            if (!activePowerUp.isActive)
            {
               
            }
           
        }
    }
}

public enum ActivePowerUpType
{
    dildo,
    enema,
    lube
}
