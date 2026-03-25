using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Enema")]
public class EnemaJetpack : PowerUp
{
    public override void Activate()
    {
        isActive = true;
        PowerUpManager.Instance.lineRenderer.enabled = true;
        PowerUpManager.Instance.enemaSource.Play();
    }

    public override void Deactivate()
    {
        isActive = false;
        PowerUpManager.Instance.lineRenderer.enabled = false;
        PowerUpManager.Instance.enemaSource.Stop();
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
