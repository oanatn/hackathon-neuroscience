using UnityEngine;

public class EEGToConcentrationBridge : MonoBehaviour
{
    public ConcentrationMeter concentrationMeter;

    public void OnEEGDataReceived(float[] eegChannels)
    {
        if (concentrationMeter == null)
            return;

        if (eegChannels == null || eegChannels.Length == 0)
            return;

        int channel = Mathf.Clamp(
            concentrationMeter.eegChannel,
            0,
            eegChannels.Length - 1
        );

        concentrationMeter.AddEEGSample(eegChannels[channel]);
    }
}