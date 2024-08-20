using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public string paramName;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        slider.value = slider.maxValue;
        float savedVol = PlayerPrefs.GetFloat(paramName, slider.maxValue);
        SetVol(savedVol); 
        //Manually set value & volume to ensure it is set
        // even if slider.value happens to start at the same value as is saved
        slider.onValueChanged.AddListener((float _) => SetVol(_));
    } 

    void SetVol(float _value)
    {
        mixer.SetFloat(paramName, ConvertToDecibel(_value / slider.maxValue)); //Dividing by max allows arbitrary positive slider maxValue
        PlayerPrefs.SetFloat(paramName, _value);
    }

    // convert percentage fraction to decibels
    public float ConvertToDecibel(float _value)
    {
        return Mathf.Log10(Mathf.Max(_value, 0.0001f)) * 20f;
    }
}
