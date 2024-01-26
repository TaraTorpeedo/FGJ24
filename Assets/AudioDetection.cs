using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDetection : MonoBehaviour
{
    public int sampleWindow = 64;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetLoudnessFromAudioClip(int clipPos, AudioClip audioClip)
    {  
        int startPos = clipPos - sampleWindow;

        if(startPos < 0)
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
}
