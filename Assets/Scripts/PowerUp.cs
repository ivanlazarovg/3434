using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : ScriptableObject
{
    public Sprite name;
    public string description;
    public Sprite imageDisplay;
    public Color textDisplayColor;
    public Material displayMaterial;

    public Mesh mesh;

    public bool isActive;
    public abstract void Activate();
    public abstract void Deactivate();
}
