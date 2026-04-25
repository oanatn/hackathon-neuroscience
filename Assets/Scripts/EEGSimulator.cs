using UnityEngine;

public class FakeEEGSimulator : MonoBehaviour
{
    public ConcentrationMeter concentrationMeter;

    [Range(0f, 1f)]
    public float simulatedConcentration = 0.5f;

    public float noise = 0.05f;
    public float sampleRate = 250f;

    private float timer;

    void Update()
    {
        if (concentrationMeter == null)
            return;

        timer += Time.deltaTime;

        int samplesThisFrame = Mathf.FloorToInt(sampleRate * Time.deltaTime);

        for (int i = 0; i < samplesThisFrame; i++)
        {
            float alphaWave = Mathf.Sin(timer * 2f * Mathf.PI * 10f) * (1f - simulatedConcentration);
            float betaWave = Mathf.Sin(timer * 2f * Mathf.PI * 20f) * simulatedConcentration;

            float randomNoise = Random.Range(-noise, noise);

            float fakeEEG = alphaWave + betaWave + randomNoise;
            Debug.Log(fakeEEG);
            concentrationMeter.AddEEGSample(fakeEEG);
        }
    }
}