using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Lube")]
public class SlippyLube : PowerUp
{
    public GameObject lubeBottlePrefab;
    public override void Activate()
    {
        GameObject lubeInstance = Instantiate(lubeBottlePrefab, Camera.main.transform.position + (Camera.main.transform.forward * 0.4f), Quaternion.identity);
        Rigidbody rb = lubeInstance.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * 9, ForceMode.Impulse);
        rb.AddTorque(Camera.main.transform.forward * 3f);
    }

    public override void Deactivate()
    {
        
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
