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
    bool canDash;
    float timeSinceUngrounded;

    [Header("Enema")]

    [HideInInspector] public Vector3 enemaVolatileOffset = Vector3.zero;

    public float enemaStrength;
    public float enemaMaxDistance;

    public float volatilityRange;

    float gravityIncreaseTemp;

    public float dashStrength;

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
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) Dash();


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
        EnemaJetpack enema = (EnemaJetpack)PowerUpManager.Instance.currentPowerUp;

        gravityIncreaseWithTime = 0;

        enemaVolatileOffset = new Vector3(Mathf.Sin(Time.time * 5) * volatilityRange, Mathf.Cos(Time.time * 3) * volatilityRange , 0);

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, enemaMaxDistance))
        {
            hitDistance = hit.distance;
            rb.AddForce((-Camera.main.transform.forward + enemaVolatileOffset) * enemaStrength);

            if(hit.collider.tag == "Butt")
            {
                enema.timer += Time.deltaTime;

                if(enema.timer > enema.pointsDisplayDelay)
                {
                    CumPointsManager.Instance.IncreasePoints(enema.pointsToDisplay + Random.Range(-25,25));
                    PowerUpManager.Instance.sexedSource.pitch = Random.Range(0.7f, 1.3f);
                    PowerUpManager.Instance.sexedSource.PlayOneShot(PowerUpManager.Instance.sexedSource.clip);
                    enema.timer = 0;
                }
            }
        }
        //PowerUpManager.Instance.lineRenderer.SetPosition(1, new Vector3(enemaVolatileOffset.x, enemaVolatileOffset.y, hitDistance));
    }

    public void Jump(float strength)
    {
        rb.AddForce(Vector3.up * strength);
    }

    public void Dash()
    {
        canDash = false;
        rb.AddForce(transform.forward * dashStrength);
    }

    public void BedBounce(float strength)
    {
        CumPointsManager.Instance.IncreasePoints(Mathf.Abs(rb.velocity.y) * 0.5f/ CumPointsManager.Instance.GetScoreModifier());
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

        canDash = true;
        grounded = true;
        

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

    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }
}