using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{

    private int numAudioSapmles = 1024;
    private int numSpectrumSapmles = 128;
    private AudioSource audioSource;
    private AudioController audioController;


    public float[] audioSamples;
    public float[] spectrumSamples;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSamples = new float[numAudioSapmles];
        spectrumSamples = new float[numSpectrumSapmles];
        audioController = new AudioController();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
        {
            //audioSource.GetOutputData(audioSamples, 0); // Get audio samples
            audioSource.GetSpectrumData(spectrumSamples, 0, FFTWindow.Rectangular);
        }
    }
}
