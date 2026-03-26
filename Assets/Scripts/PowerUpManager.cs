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
    [Header("Enema")]

    public LineRenderer lineRenderer;
    public AudioSource enemaSource;
    public AudioSource sexedSource;
    public bool enemaSpray;

    float enemaTimer = 0;

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

        powerUpDisplay.ClearPowerUpDisplay();
        powerUpDisplay.displayMain.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLubed)
        {
            lubeTimer += Time.deltaTime;

            if (lubeTimer > lubeDuration)
            {
                isLubed = false;
            }
        }


        if (currentPowerUp == null) return;

        if (currentPowerUp.isActive && currentPowerUp is EnemaJetpack)
        {
            EnemaJetpack enema = (EnemaJetpack)currentPowerUp;
            if (enemaTimer < enema.enemaDuration)
            {
                enemaTimer += Time.deltaTime;

            }
            else
            {
                DeactivatePowerUp();
                enemaTimer = 0;
            }

        }

        if (Input.GetMouseButtonDown(0))
        {

                if (!currentPowerUp.isActive)
                {
                    source.clip = glugClip;
                    source.Play();
                    currentPowerUp.Activate();

                    if(currentPowerUp is SlippyLube)
                    {
                        DeactivatePowerUp();
                    }
                }
                    
            
                
        }

       
    }

    public void SetActivePowerUp(PowerUp powerUp)
    {
        powerUpDisplay.displayMain.SetActive(true);
        powerUpDisplay.SetPowerUpDisplay(powerUp);

        if(currentPowerUp is EnemaJetpack)
        {
            currentPowerUp.Deactivate();
        }

        source.clip = schlumpClip;
        source.Play();

        currentPowerUp = powerUp;
    }

    public void SetLubeActive()
    {
        lubeTimer = 0;
        isLubed = true;
    }

    private void OnDisable()
    {
        if(currentPowerUp != null)
        {
            currentPowerUp.isActive = false;
        }
    }
}

public enum ActivePowerUpType
{
    dildo,
    enema,
    lube,
    none
}
