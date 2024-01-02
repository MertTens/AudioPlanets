using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    Noise noise = new Noise();

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(KalmanFilter kalmanFilter, Vector3 pointOnUnitSphere, float[] spectrumSamples, float time)
    {
        // float strength = 0.1f;
        float[] strengths = CalculateStrengths(kalmanFilter);
        float frequency = settings.baseFrequency;
        float elevation = 0;
        float decreaseMult = 1;
        for(int i = 0; i < kalmanFilter.numFilters; i++)
        {
            float logStrengh = (float)System.Math.Log(1 + strengths[i]);
            elevation += EvaluateNoiseOnSinusoid(pointOnUnitSphere, time, logStrengh * decreaseMult * settings.strengthMult, frequency);
            frequency *= settings.frequencyMult;
            decreaseMult *= settings.strengthDecreaseFactor;
        }
        //float elevation = EvaluateNoiseOnSinusoid(pointOnUnitSphere, time, strength, frequency);
        return pointOnUnitSphere * settings.planetRadius * (1 + elevation);
    }

    private float EvaluateNoiseOnSinusoid(Vector3 point, float time, float strength, float frequency)
    {
        float noiseValue = noise.Evaluate(point * frequency) * (float)(2*System.Math.PI);
        float sinusoidPoint = (float)System.Math.Sin(noiseValue + time);
        return sinusoidPoint * strength;
    }

    private float[] CalculateStrengths(KalmanFilter kalmanFilter)
    {
        return kalmanFilter.correctedGuess;
    }
}
