using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : ScriptableObject
{
    public Image name;
    public string description;
    public Image imageDisplay;

    public bool isActive;
    public abstract void Activate();
    public abstract void Deactivate();
}
