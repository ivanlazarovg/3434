using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpStrength;
    [SerializeField] float bedBounceStrength;
    [SerializeField] float gravityIncreaseWithTime;

    Rigidbody rb;
    Vector3 wantedMovementDirection;
    bool grounded;
    float timeSinceUngrounded;

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
        rb.AddForce(Vector3.down * timeSinceUngrounded * gravityIncreaseWithTime * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && grounded) Jump(jumpStrength);

    }

    public void Jump(float strength)
    {
        rb.AddForce(Vector3.up * strength);
    }

    public void BedBounce(float strength)
    {
        CumPointsManager.Instance.IncreasePoints((int)(Mathf.Abs(rb.velocity.y) / CumPointsManager.Instance.GetScoreModifier()));
        rb.AddForce(Vector3.up * (strength - rb.velocity.y));
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.material.name == "Bed (Instance)")
        {
            BedBounce(bedBounceStrength);
            collision.gameObject.GetComponent<BedMovementScript>().MoveBed(collision.contacts[0].point);

            if (PowerUpManager.Instance.activePowerUp.powerUpType == ActivePowerUpType.dildo)
            {
                PowerUpManager.Instance.DeactivatePowerUp();
            }
        }
        else 
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        timeSinceUngrounded = 0;
    }
}