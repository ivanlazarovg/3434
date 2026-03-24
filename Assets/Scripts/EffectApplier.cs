using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class EffectApplier : MonoBehaviour
{
    [Range(1,16)]
    [SerializeField] float resolutionChange;
    [SerializeField] Material effect;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture temp = new RenderTexture((int)(source.width / resolutionChange), (int)(source.height / resolutionChange), 0);
        temp.filterMode = FilterMode.Point;

        if (effect != null) Graphics.Blit(source, temp, effect);
        else Graphics.Blit(source, temp);
        Graphics.Blit(temp, destination);

        temp.Release();
    }
}
