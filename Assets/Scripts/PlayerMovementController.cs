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

    [Header("Enema")]

    Vector3 enemaVolatileOffset = Vector3.zero;

    public float enemaStrength;
    public float enemaMaxDistance;

    public float volatilityRange;

    float gravityIncreaseTemp;


    float hitDistance = 0;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        gravityIncreaseTemp = gravityIncreaseWithTime;
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


        if (PowerUpManager.Instance.currentPowerUp != null && PowerUpManager.Instance.currentPowerUp is EnemaJetpack && PowerUpManager.Instance.currentPowerUp.isActive)
        {
            EnemaCounterForce();
        }
        else
        {
            gravityIncreaseWithTime = gravityIncreaseTemp;
            enemaVolatileOffset = Vector3.zero;
        }

    }

    void EnemaCounterForce()
    {
        gravityIncreaseWithTime = 0;

        enemaVolatileOffset = new Vector3(Mathf.Sin(Time.time * 5) * volatilityRange, Mathf.Cos(Time.time * 3) * volatilityRange , 0);

        Debug.Log("volatile offset " + enemaVolatileOffset);

        RaycastHit hit;



        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, enemaMaxDistance))
        {
            hitDistance = hit.distance;
            rb.AddForce((-Camera.main.transform.forward + enemaVolatileOffset) * enemaStrength);
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward + enemaVolatileOffset);
        }
        PowerUpManager.Instance.lineRenderer.SetPosition(1, new Vector3(enemaVolatileOffset.x, enemaVolatileOffset.y, hitDistance));
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

            if(PowerUpManager.Instance.currentPowerUp is DildoPogo && PowerUpManager.Instance.currentPowerUp.isActive)
            {
                //blah blah score stuff
            }
            
        }
        else 
        {
            grounded = true;
        }

        if (PowerUpManager.Instance.currentPowerUp is DildoPogo && PowerUpManager.Instance.currentPowerUp.isActive)
        {
            PowerUpManager.Instance.DeactivatePowerUp();
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
        timeSinceUngrounded = 0;
    }
}