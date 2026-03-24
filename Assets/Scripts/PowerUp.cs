using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : MonoBehaviour
{
    public string name;
    public string description;
    public Image imageDisplay;

    public ActivePowerUpType powerUpType;

    public bool isActive;
    public abstract void Activate();
    public abstract void Deactivate();
}
