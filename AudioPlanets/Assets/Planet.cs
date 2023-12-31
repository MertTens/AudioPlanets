using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2,256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public float timeStep = 0.1f;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public KalmanSettings kalmanSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout;

    [HideInInspector]
    public bool colorSettingsFoldout;

    [HideInInspector]
    public bool kalmanSettingsFoldout;

    ShapeGenerator shapeGenerator;
    KalmanFilter kalmanFilter;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private AudioPlayer audioPlayer;
    private float time = 0;
    private int numFilters = 32;

    private void Start()
    {
        // Set up the Kalman filter
        kalmanFilter = new KalmanFilter(kalmanSettings, numFilters);
        audioPlayer = GetComponent<AudioPlayer>();
        GeneratePlanet();
    }

    private void Update()
    {
        Initialize();
        GenerateMesh();
    }



    void Initialize() {


        // Update the Kalman filter
        float[] measurements = new float[kalmanFilter.numFilters];
        for (int i = 0; i < kalmanFilter.numFilters; i++)
        {
            measurements[i] = audioPlayer.spectrumSamples[i];
        }

        kalmanFilter.PredictionGivenMeasurement(measurements);
        
        shapeGenerator = new ShapeGenerator(shapeSettings);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6 ; i++)
        {
            if (meshFilters[i] == null) {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(kalmanFilter, time, audioPlayer.spectrumSamples, shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            
        }

        // Update the time
        // Make it reset after 2pi seconds for now 
        time += timeStep;
        time = time % (float)(2*System.Math.PI);
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }
    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    public void OnKalmanSettingsUpdated()
    {
        // Do nothing for now
    }

    void GenerateMesh()
    {
        foreach(TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    void GenerateColors()
    {
        foreach (MeshFilter m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;
        }
    }
}
