using NUnit.Framework;
using UnityEngine;

public class EngineLightScript : MonoBehaviour
{
    Light _engineLight;
    public Color _fullPowerColor;
    void Start()
    {
       _engineLight = GetComponent<Light>(); 
    }

    public void EnginePowered() {
        _engineLight.color = _fullPowerColor;
    }
}
