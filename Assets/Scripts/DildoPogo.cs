using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/DildoPogo")]
public class DildoPogo : PowerUp
{
    public override void Activate()
    {
        isActive = true;

        Debug.Log("activated");
        Vector3 force = new Vector3(PowerUpManager.Instance.playerRb.velocity.x, -15f, PowerUpManager.Instance.playerRb.velocity.z);
        PowerUpManager.Instance.playerRb.AddForce(force, ForceMode.Impulse);
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
