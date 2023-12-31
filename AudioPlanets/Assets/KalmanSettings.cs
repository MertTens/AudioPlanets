using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KalmanSettings : ScriptableObject
{
    
    public float stateTransition = 1;
    public float processNoiseVar = 1;
    public float measurementNoiseVar = 1;
    public float initialGuess = 0;
    public float initialCovariance = 0.01f;
 
}
