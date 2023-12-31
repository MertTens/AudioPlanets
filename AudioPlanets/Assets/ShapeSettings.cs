using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float planetRadius = 1;
    [Range(1,5)]
    public float frequencyMult = 2;
    [Range(0,2)]
    public float baseFrequency = 0.2f;
    [Range(0.5f,1)]
    public float strengthDecreaseFactor = 0.7f;
}
