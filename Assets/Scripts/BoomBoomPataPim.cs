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

    [SerializeField] AudioSource boomboompatapim;

    private void Start()
    {
        currentTimeUntilActive = Random.Range(timeBetweenActivationsMin, timeBetweenActivationsMax);
    }

    private void Update()
    {
        currentTimeUntilActive -= Time.deltaTime;
        if (currentTimeUntilActive <= 0 && !active)
        {
            active = true;
            boomboompatapim.Play();
        }

        if (active)
        {
            CumPointsManager.Instance.IncreasePointsHidden(-pointsLostPerSecond * Time.deltaTime);
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
        }
    }
}
