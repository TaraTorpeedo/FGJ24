using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioDetection : MonoBehaviour
{
    [SerializeField] private int sampleWindow = 64;
    private AudioClip microphoneClip;
    [SerializeField] TMP_Dropdown dropdown;

    private int micValue = 0;

    public void Start()
    {
        MicrophoneToAudioClip(micValue);

        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(dropdown);
        });
    }

    private void DropdownValueChanged(TMP_Dropdown change)
    {
        MicrophoneToAudioClip(change.value);
        micValue = change.value;
    }

    public void MicrophoneToAudioClip(int micId)
    {
        string microphoneName = Microphone.devices[micId];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone() => GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[micValue]), microphoneClip);

    private float GetLoudnessFromAudioClip(int clipPos, AudioClip audioClip)
    {
        int startPos = clipPos - sampleWindow;

        if (startPos < 0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        audioClip.GetData(waveData, startPos);

        float totalLoudness = 0f;

        for (int i = 0; i < sampleWindow; ++i)
        {
            totalLoudness += Mathf.Abs(waveData[i]);

        }

        return totalLoudness / sampleWindow;
    }

    //Goblin king will have this

    /*
     [SerializeField] private AudioDetection audioDetector;
    [SerializeField] private float loudnessSensibility = 100;
    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private GameObject player;

    public void Update()
    {
        float loudness = audioDetector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness >= threshold)
        {
        transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(2, 2, 2), loudness);
        }
        else
        {
            loudness = 0;
        }
    }

     */
}
