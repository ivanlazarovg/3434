using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    public PostProcessVolume volume;
    public float lensWarpIntensity;

    private LensDistortion lensDistortion;

    private static PostProcessManager instance;
    public static PostProcessManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<PostProcessManager>();
            }
            return instance;
        }
    }
    void Start()
    {
        volume.profile.TryGetSettings<LensDistortion>(out lensDistortion);
    }

    private void Update()
    {
        lensDistortion.intensity.value = Mathf.Clamp(PowerUpManager.Instance.playerRb.velocity.y * lensWarpIntensity, -80, 80);
        Debug.Log("velocity " + PowerUpManager.Instance.playerRb.velocity.y);
    }

    public void WarpLens()
    {
        //lensDistortion.intensity.value = -100;
    }

    public void UnwarpLens()
    {
        //lensDistortion.intensity.value = 0;
    }
}
