using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedMovementScript : MonoBehaviour
{
    [SerializeField] float movePower;

    Rigidbody rb;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    public void MoveBed(Vector3 landingPosition)
    {
        Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 landingPos = new Vector2(landingPosition.x, landingPosition.z);

        Vector2 targetPos = landingPos - pos;

        targetPos *= -movePower;

        Vector3 v3TargetPos = new Vector3(targetPos.x, 0, targetPos.y * 2);

        rb.AddForce(v3TargetPos);


    }

    private void Update()
    {
        if (PowerUpManager.Instance.isLubed)
        {
            rb.drag = 0;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        source.pitch = Random.Range(0.6f, 1.4f);
        source.PlayOneShot(source.clip);
    }
}
