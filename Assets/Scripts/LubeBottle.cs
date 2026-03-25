using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LubeBottle : MonoBehaviour
{

    private void Start()
    {
        Invoke("DestroyThis", 3f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.material.name == "Bed (Instance)")
        {
            collision.gameObject.GetComponent<BedMovementScript>().MoveBed(collision.contacts[0].point);
            PowerUpManager.Instance.SetLubeActive();
            Destroy(gameObject);
        }

        
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
