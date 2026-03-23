using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedMovementScript : MonoBehaviour
{
    [SerializeField] float movePower;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveBed(Vector3 landingPosition)
    {
        Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 landingPos = new Vector2(landingPosition.x, landingPosition.y);

        Vector2 targetPos = landingPos - pos;

        targetPos *= -movePower;

        Vector3 v3TargetPos = new Vector3(targetPos.x, 0, targetPos.y);

        rb.AddForce(v3TargetPos);

    }
}
