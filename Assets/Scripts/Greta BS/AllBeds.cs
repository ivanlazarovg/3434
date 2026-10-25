using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBeds : MonoBehaviour
{
    BedMovementScript[] allCurrentBeds;

    private static AllBeds instance;

    public static AllBeds Instance
    {
        get
        {
            instance = FindAnyObjectByType<AllBeds>();

            return instance;
        }
    }

    public void ReduceSize(BedMovementScript bed1, BedMovementScript bed2)
    {

        bed1.transform.localScale *= 0.5f;
        //bed1.rb.drag *= 0.5f;
        //bed1.rb.mass *= 0.5f;

        bed2.transform.localScale *= 0.5f;
        //bed2.rb.drag *= 0.5f;
        //bed2.rb.mass *= 0.5f;

    }

    public void MergeBeds(Transform bed1, Transform bed2, GameObject bedPrefab)
    {
        var cum = GetBiggerBed(bed1, bed2);

        cum.transform.localScale += GetSmallerBed(bed1, bed2).transform.localScale;


        //cum.transform.localScale = bed1.localScale + bed2.localScale;

        Destroy(GetSmallerBed(bed1, bed2));
    }

    GameObject GetSmallerBed(Transform bed1, Transform bed2)
    {
        return bed1.localScale.magnitude < bed2.localScale.magnitude ? bed1.gameObject : bed2.gameObject;
    }

    GameObject GetBiggerBed(Transform bed1, Transform bed2)
    {
        return bed1.localScale.magnitude > bed2.localScale.magnitude ? bed1.gameObject : bed2.gameObject;
    }


}

