using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBoomPataPim : MonoBehaviour
{
    [SerializeField] float pointGain;
    [SerializeField] float pointsLostPerSecond;
    [SerializeField] float timeBetweenActivationsMin;
    [SerializeField] float timeBetweenActivationsMax;

    float currentTimeUntilActive;
    bool active;

    MeshRenderer mattVid;

    [SerializeField] AudioSource boomboompatapim;

    public GameObject Matt;
    public GameObject Walsh;

    private void Start()
    {
        currentTimeUntilActive = Random.Range(timeBetweenActivationsMin, timeBetweenActivationsMax);
        mattVid = GetComponentsInChildren<MeshRenderer>()[1];
        mattVid.enabled = false;
    }

    private void Update()
    {
        currentTimeUntilActive -= Time.deltaTime;
        if (currentTimeUntilActive <= 0 && !active)
        {
            active = true;
            boomboompatapim.Play();
            mattVid.enabled = true;
        }

        if (active)
        {
            CumPointsManager.Instance.IncreasePointsHidden(-pointsLostPerSecond * Time.deltaTime);
            Matt.SetActive(true);
            Walsh.SetActive(true);

            Matt.transform.Rotate(Vector3.up, Time.deltaTime * 150f);
            Walsh.transform.Rotate(Vector3.up, -Time.deltaTime * 150f);
        }
        else
        {
            Matt.SetActive(false);
            Walsh.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.material.name == "Bed (Instance)" && active)
        {
            boomboompatapim.Stop();
            active = false;
            currentTimeUntilActive = Random.Range(timeBetweenActivationsMin, timeBetweenActivationsMax);
            CumPointsManager.Instance.IncreasePoints(pointGain);
            mattVid.enabled = false;
        }
    }
}
