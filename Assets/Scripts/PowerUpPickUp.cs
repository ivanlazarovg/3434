using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    public PowerUp powerUpToPickUp;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PowerUpManager.Instance.SetActivePowerUp(powerUpToPickUp);
            Destroy(gameObject);
        }
    }
}
