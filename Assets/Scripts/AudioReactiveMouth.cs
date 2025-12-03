using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioReactiveMouth : MonoBehaviour
{
    public Material mouthMaterial;
    [Header("Audio Analysis")]
    [Range(1f, 5f)] public float sensitivity = 2.5f;
    [Range(0.1f, 1f)] public float smoothing = 0.3f;
    [Range(0.001f, 0.1f)] public float threshold = 0.01f;
    
    [Header("Ring Settings")]
    [Range(0.1f, 0.9f)] public float minRadius = 0.3f;
    [Range(0.1f, 0.9f)] public float maxRadius = 0.8f;
    [Range(0.01f, 0.2f)] public float ringWidth = 0.1f;
    
    [Header("Response")]
    [Range(5f, 20f)] public float attackSpeed = 15f;
    [Range(1f, 10f)] public float decaySpeed = 3f;
    [Range(0.5f, 3f)] public float volumeMultiplier = 1.8f;
    [Range(0.1f, 1f)] public float wordBoost = 0.5f;
    
    private AudioSource audioSource;
    private float[] samples = new float[2048];  // Increased buffer size
    private float[] spectrum = new float[512];  // Higher resolution FFT
    private float currentVolume = 0f;
    private float targetIntensity = 0f;
    private float currentIntensity = 0f;
    private float[] historyBuffer = new float[8];  // Shorter history for more responsiveness
    private int historyIndex = 0;
    private float peakVolume = 0f;
    private float peakDecay = 0.95f;  // How quickly the peak volume decays
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (mouthMaterial == null)
        {
            Debug.LogError("Please assign a material with the MouthRing shader to the Mouth Material field");
            enabled = false;
            return;
        }
        
        // Initialize shader properties
        mouthMaterial.SetFloat("_RingWidth", ringWidth);
        mouthMaterial.SetFloat("_Smoothness", 0.02f);
        mouthMaterial.SetFloat("_MinRadius", minRadius);
        mouthMaterial.SetFloat("_MaxRadius", maxRadius);
        
        // Initialize history buffer
        for (int i = 0; i < historyBuffer.Length; i++)
        {
            historyBuffer[i] = 0f;
        }
    }
    
    private void Update()
    {
        //if (!audioSource.isPlaying) return;
        if (!audioSource.isPlaying)
        {
            currentIntensity = Mathf.Lerp(currentIntensity, 0f, Time.deltaTime * decaySpeed);
            mouthMaterial.SetFloat("_Intensity", currentIntensity);
            return;
        }

        // Get both time and frequency domain data
        audioSource.GetOutputData(samples, 0);
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        
        // Analyze speech frequencies (85-255Hz for voice)
        float voiceBand = 0f;
        int startFreq = 4;  // Skip very low frequencies
        int endFreq = Mathf.Min(32, spectrum.Length / 4); // Focus on voice range
        
        // Calculate RMS for volume
        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        float rms = Mathf.Sqrt(sum / samples.Length);
        
        // Calculate voice band energy
        for (int i = startFreq; i < endFreq; i++)
        {
            voiceBand += spectrum[i] * (1 + i * 0.1f); // Emphasize higher voice frequencies
        }
        voiceBand /= (endFreq - startFreq);
        
        // Update peak volume (decays over time)
        peakVolume = Mathf.Max(peakVolume * peakDecay, voiceBand * 2f);
        
        // Calculate volume with emphasis on transients (word starts)
        float transientBoost = Mathf.Clamp01((voiceBand - peakVolume * 0.8f) * 5f) * wordBoost;
        float volume = Mathf.Clamp01((voiceBand + transientBoost) * volumeMultiplier);
        
        // Update history buffer
        historyBuffer[historyIndex] = volume;
        historyIndex = (historyIndex + 1) % historyBuffer.Length;
        
        // Calculate smoothed volume
        float smoothedVolume = 0;
        foreach (float v in historyBuffer)
        {
            smoothedVolume += v;
        }
        smoothedVolume /= historyBuffer.Length;
        
        // Apply threshold and sensitivity
        if (smoothedVolume > threshold)
        {
            // Apply a power curve to emphasize the response to words
            float boostedVolume = Mathf.Pow(smoothedVolume, 0.7f) * sensitivity;
            targetIntensity = Mathf.Clamp01(boostedVolume * 1.5f);
        }
        else
        {
            targetIntensity = 0;
        }
        
        // Faster attack than decay for more responsive word detection
        float speed = (currentIntensity < targetIntensity) ? attackSpeed : decaySpeed;
        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * speed);
        
        // Apply to material with slight boost to make it more visible
        float displayIntensity = Mathf.Pow(currentIntensity, 0.8f) * 1.2f;
        mouthMaterial.SetFloat("_Intensity", Mathf.Clamp01(displayIntensity));
    }
    
    private void OnDisable()
    {
        // Reset material properties when disabled
        if (mouthMaterial != null)
        {
            mouthMaterial.SetFloat("_Intensity", 0);
        }
    }
    
    private void OnValidate()
    {
        // Update material properties in editor when changed
        if (mouthMaterial != null)
        {
            mouthMaterial.SetFloat("_RingWidth", ringWidth);
            mouthMaterial.SetFloat("_MinRadius", minRadius);
            mouthMaterial.SetFloat("_MaxRadius", maxRadius);
        }
    }
}
