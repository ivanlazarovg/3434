using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    public PowerUp powerUpToPickUp;

    float rotationTime = 0;
    float sinTime = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PowerUpManager.Instance.SetActivePowerUp(powerUpToPickUp);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 20 * Time.deltaTime);

        sinTime += Time.deltaTime * 2;

        transform.position = new Vector3(transform.position.x, Mathf.Sin(sinTime) * 0.5f, transform.position.z);
    }
}
