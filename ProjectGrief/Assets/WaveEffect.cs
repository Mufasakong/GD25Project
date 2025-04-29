using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    public float waveOffset = 0.0f;     // Set this in the inspector or dynamically in code
    public float amplitude = 0.2f;       // Amplitude of the waves
    public float frequency = 10.0f;      // Frequency of the waves
    public float speed = 5.0f;          // Speed of the wave movement
    public float sharpness = 3.0f;      // Sharpness for triangle waves
    public bool useSharpWaves = true;  // Toggle between sine and triangle wave

    private Material material;

    void Start()
    {
        // Get the material of the waterpool object
        material = GetComponent<SpriteRenderer>().material;
        SetShaderProperties();
    }

    // Set all relevant shader properties
    void SetShaderProperties()
    {
        if (material != null)
        {
            material.SetFloat("_WaveOffset", waveOffset);
            material.SetFloat("_Amplitude", amplitude);
            material.SetFloat("_Frequency", frequency);
            material.SetFloat("_Speed", speed);
            material.SetFloat("_Sharpness", sharpness);
            material.SetFloat("_UseSharpWaves", useSharpWaves ? 1.0f : 0.0f);
        }
    }

    public void UpdateShaderProperties(float newWaveOffset, float newAmplitude, float newFrequency, float newSpeed, float newSharpness, bool newUseSharpWaves)
    {
        waveOffset = newWaveOffset;
        amplitude = newAmplitude;
        frequency = newFrequency;
        speed = newSpeed;
        sharpness = newSharpness;
        useSharpWaves = newUseSharpWaves;

        SetShaderProperties();  // Apply the updated values
    }
}
