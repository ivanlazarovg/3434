using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Enema")]
public class EnemaJetpack : PowerUp
{
    public float enemaDuration;

    public float pointsToGet;

    public float pointsDisplayDelay;

    [HideInInspector] public float pointsToDisplay;

    [HideInInspector] public float timer;

    public override void Activate()
    {
        isActive = true;
        PowerUpManager.Instance.lineRenderer.gameObject.SetActive(true);
        PowerUpManager.Instance.enemaSource.Play();

        pointsToDisplay = pointsToGet / (enemaDuration / pointsDisplayDelay);
    }

    public override void Deactivate()
    {
        isActive = false;
        PowerUpManager.Instance.lineRenderer.gameObject.SetActive(false);
        PowerUpManager.Instance.enemaSource.Stop();

        timer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
