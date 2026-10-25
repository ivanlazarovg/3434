using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedMovementScript : MonoBehaviour
{
    [SerializeField] float movePower;

    public Rigidbody rb;

    private AudioSource source;

    public GameObject bedPrefab;

    public Transform[] spawnPosArray;

    public LayerMask killMe;

    void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        SetColorsRandom();
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

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        source.pitch = Random.Range(0.6f, 1.4f);
        source.PlayOneShot(source.clip);

        if (collision.collider.gameObject.tag == "Bed")
        {
            AllBeds.Instance.MergeBeds(transform, collision.collider.gameObject.transform, bedPrefab);
        }
        else if(collision.collider.gameObject.tag == "Wall")
        {
            InstantiateBed();
        }
    }


    public void InstantiateBed()
    {
        var cum = Instantiate(bedPrefab, GetFreeSpawnPos().position, Quaternion.identity);

        cum.GetComponent<BedMovementScript>().SetColorsRandom();

        AllBeds.Instance.ReduceSize(cum.GetComponent<BedMovementScript>(), this);
    }

    private Transform GetFreeSpawnPos()
    {
        foreach(Transform spawnPos in spawnPosArray)
        {
            if (Physics.OverlapSphere(spawnPos.transform.position, 0.9f, killMe).Length > 0)
            {
                Debug.Log(Physics.OverlapSphere(spawnPos.transform.position, 0.5f, 7).Length);

            }
            else
            {
                return spawnPos;
            }
        }

        return spawnPosArray[0];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 1);
    }

    public void SetColorsRandom()
    {
        var renderers = GetComponentsInChildren<Renderer>();

        foreach(var renderer in renderers)
        {
            renderer.material.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        }
    }
}
