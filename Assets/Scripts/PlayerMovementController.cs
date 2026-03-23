using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpStrength;
    [SerializeField] float bedBounceStrength;

    Rigidbody rb;
    Vector3 wantedMovementDirection;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        wantedMovementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) wantedMovementDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D)) wantedMovementDirection += Vector3.right;
        if (Input.GetKey(KeyCode.W)) wantedMovementDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) wantedMovementDirection += Vector3.back;

        wantedMovementDirection = this.transform.rotation * wantedMovementDirection;

        rb.AddForce(wantedMovementDirection * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space)) Jump(jumpStrength);

    }

    public void Jump(float strength)
    {
        rb.AddForce(Vector3.up * jumpStrength);
    }

    public void BedBounce(float strength)
    {
        rb.AddForce(Vector3.up * (bedBounceStrength - rb.velocity.y));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.material.name == "Bed (Instance)")
        {
            BedBounce(bedBounceStrength);
            collision.gameObject.GetComponent<BedMovementScript>().MoveBed(collision.contacts[0].point);
        }
    }
}