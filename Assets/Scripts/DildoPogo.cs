using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DildoPogo : PowerUp
{
    public override void Activate()
    {
        isActive = true;

        PowerUpManager.Instance.playerRb.AddForce(PowerUpManager.Instance.playerRb.velocity * f, ForceMode.Impulse);
    }

    public override void Deactivate()
    {
        isActive = false;
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
