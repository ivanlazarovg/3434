using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectApplier : MonoBehaviour
{
    [Range(1,16)]
    [SerializeField] float resolutionChange;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture temp = new RenderTexture((int)(source.width /resolutionChange), (int)(source.height / resolutionChange), 0);
        temp.filterMode = FilterMode.Point;

        Graphics.Blit(source, temp);
        Graphics.Blit(temp, destination);

        temp.Release();
    }
}
