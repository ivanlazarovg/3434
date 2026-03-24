using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Lube")]
public class SlippyLube : PowerUp
{
    public GameObject lubeBottlePrefab;
    public override void Activate()
    {
        GameObject lubeInstance = Instantiate(lubeBottlePrefab, PowerUpManager.Instance.transform.position, Quaternion.identity);
        Rigidbody rb = lubeInstance.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * 4, ForceMode.Impulse); 

    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
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
