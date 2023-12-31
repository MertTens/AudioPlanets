using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


// Eventually, this will become a proper Kalman filter 
// Many scalar filters running in parallel
// Observation matrix is assumed to be identity
public class KalmanFilter
{

    public float[] correctedGuess;
    private float[] correctedCovariance;
    private float[] predictedGuess;
    private float[] predictedCovariance;
    public int numFilters;

    KalmanSettings settings;

    public KalmanFilter(KalmanSettings kalmanSettings, int numFilters){

        this.correctedGuess = new float[numFilters];
        this.correctedCovariance = new float[numFilters];
        this.predictedGuess = new float[numFilters];
        this.predictedCovariance = new float[numFilters];
        this.settings = kalmanSettings;
        this.numFilters = numFilters;
        for (int i = 0; i < numFilters; i++)
        {
            this.predictedGuess[i] = kalmanSettings.initialGuess;
            this.predictedCovariance[i] = kalmanSettings.initialCovariance;
        }
    }

    public void PredictionGivenMeasurement(float[] measurement)
    {

        if (measurement.Length != numFilters)
        {
            Debug.Log("Error: length of measurement array should be " + numFilters.ToString());
        }

        // Predict based on the filter properties and the previous corrected guess
        // Then correct based on the measurement
        for (int i = 0; i < numFilters; i++) {
            // Prediction
            predictedGuess[i] = correctedGuess[i] * settings.stateTransition;
            predictedCovariance[i] = settings.stateTransition * settings.stateTransition * correctedCovariance[i] + settings.processNoiseVar;

            // Correction
            float gain = predictedCovariance[i] / (predictedCovariance[i] + settings.measurementNoiseVar);
            correctedGuess[i] = predictedGuess[i] + gain * (measurement[i] - predictedGuess[i]);
            correctedCovariance[i] = (1 - gain) * (1 - gain) * predictedCovariance[i] + gain * gain * settings.measurementNoiseVar;
        }
    }


}
