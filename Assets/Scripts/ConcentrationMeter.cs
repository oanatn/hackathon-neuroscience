using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AttackResult
{
    Fail,
    Weak,
    Strong,
    Special
}

public class ConcentrationMeter : MonoBehaviour
{
    [Header("UI")]
    public Image concentrationBar;

    [Header("EEG")]
    public int sampleRate = 250;
    public int windowSize = 256;
    public int eegChannel = 0;

    [Header("Calibration")]
    public float relaxBaseline = 1f;
    public float focusBaseline = 2f;

    [Header("Smoothing")]
    [Range(0.01f, 1f)]
    public float smoothing = 0.12f;

    [Header("Attack Thresholds")]
    public float failThreshold = 0.25f;
    public float strongThreshold = 0.60f;
    public float specialThreshold = 0.85f;

    private readonly Queue<float> samples = new Queue<float>();
    private float concentration01 = 0f;

    public float Concentration01 => concentration01;

    public void AddEEGSample(float sample)
    {
        samples.Enqueue(sample);

        while (samples.Count > windowSize)
            samples.Dequeue();
    }

    private void Update()
    {
        if (samples.Count < windowSize)
            return;

        float[] buffer = samples.ToArray();

        float alpha = BandPower(buffer, 8f, 12f);
        float beta = BandPower(buffer, 13f, 30f);

        float ratio = beta / (alpha + 0.000001f);

        float rawConcentration = Mathf.InverseLerp(
            relaxBaseline,
            focusBaseline,
            ratio
        );

        rawConcentration = Mathf.Clamp01(rawConcentration);

        concentration01 =
            smoothing * rawConcentration +
            (1f - smoothing) * concentration01;

        if (concentrationBar != null)
            concentrationBar.fillAmount = concentration01;
    }

    public AttackResult GetAttackResult()
    {
        if (concentration01 < failThreshold)
            return AttackResult.Fail;

        if (concentration01 < strongThreshold)
            return AttackResult.Weak;

        if (concentration01 < specialThreshold)
            return AttackResult.Strong;

        return AttackResult.Special;
    }

    private float BandPower(float[] signal, float lowFreq, float highFreq)
    {
        int n = signal.Length;
        float power = 0f;

        for (int k = 0; k < n / 2; k++)
        {
            float freq = k * sampleRate / (float)n;

            if (freq < lowFreq || freq > highFreq)
                continue;

            float real = 0f;
            float imag = 0f;

            for (int i = 0; i < n; i++)
            {
                float angle = 2f * Mathf.PI * k * i / n;
                real += signal[i] * Mathf.Cos(angle);
                imag -= signal[i] * Mathf.Sin(angle);
            }

            power += real * real + imag * imag;
        }

        return power / n;
    }
}