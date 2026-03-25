using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PowerUp currentPowerUp;

    public Rigidbody playerRb;

    private static PowerUpManager instance;

    public PowerUpDisplay powerUpDisplay;

    [Space(10)]
    [Header("Lube")]

    public float lubeDuration;
    public bool isLubed;

    private float lubeTimer;

    [Space(10)]
    [Header("Audio")]

    private AudioSource source;

    public AudioClip schlumpClip;
    public AudioClip glugClip;

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

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void DeactivatePowerUp()
    {
        //if (!currentPowerUp.isActive) return;
        currentPowerUp.Deactivate();
        currentPowerUp  = null;

        powerUpDisplay.displayMain.SetActive(false);
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
                    source.clip = glugClip;
                    source.Play();
                    currentPowerUp.Activate();

                    if(currentPowerUp is DildoPogo || currentPowerUp is SlippyLube)
                    {
                        DeactivatePowerUp();
                    }
                }
                    
            }
                
        }

        if (isLubed)
        {
            lubeTimer += Time.deltaTime;

            if(lubeTimer > lubeDuration)
            {
                isLubed = false;
            }
        }
    }

    public void SetActivePowerUp(PowerUp powerUp)
    {
        powerUpDisplay.displayMain.SetActive(true);
        powerUpDisplay.SetPowerUpDisplay(powerUp);

        source.clip = schlumpClip;
        source.Play();

        currentPowerUp = powerUp;
    }

    public void SetLubeActive()
    {
        lubeTimer = 0;
        isLubed = true;
    }
}

public enum ActivePowerUpType
{
    dildo,
    enema,
    lube,
    none
}
