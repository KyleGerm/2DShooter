using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour , IHUDController
{
    private Image image;
    private RectTransform imageSize;
    private Color controlColor;
     private GameObject HealthBar;
    [SerializeField] Color startColor = Color.white;
    [SerializeField] Color endColor = Color.black;

    [SerializeField] TextMeshProUGUI AmmoCount;
    [SerializeField] TextMeshProUGUI WaveCount;
    

    private void Awake()
    {
        

        GameManager.Instance.EventManager.SubscribeToHUDEvents(this);
    }

    private void Start()
    {
        HealthBar = GameObject.Find("HealthBar");
        image = HealthBar.GetComponent<Image>();
        imageSize = HealthBar.GetComponent<RectTransform>();
    }


    /// <summary>
    /// Sets the colour of the healthbar to one between the start and end colors
    /// </summary>
    /// <param name="current"></param>
    /// <param name="max"></param>
    public void FormatHealthBar(float current, float max)
    {
        float R = MapColor(startColor.r, endColor.r, current, max);
        float G = MapColor(startColor.g, endColor.g, current, max);
        float B = MapColor(startColor.b, endColor.b, current, max);
        float A = 1.0f;
        controlColor = new Color(R, G, B, A);

        float pos = imageSize.rect.xMax - imageSize.rect.xMin;
        imageSize.transform.localScale = new Vector3(current / max,1,1);

        image.color = controlColor;


    }
    /// <summary>
    /// takes a value and inerpolates it based on current health
    /// </summary>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <param name="current"></param>
    /// <param name="maxVal"></param>
    /// <returns></returns>
    private float MapColor(float max, float min, float current, float maxVal)
    {
        return min + ((max - min) * (current / maxVal));
    }
    /// <summary>
    /// Updates the Ammo text to show the current clip in the players weapon
    /// </summary>
    /// <param name="current"></param>
    /// <param name="max"></param>
    public void UpdateAmmoCount(int current, int max)
    {
        AmmoCount.text = new string($"{current}/{max}");
    }

    /// <summary>
    /// Updates the wave text to show the most current wave
    /// </summary>
    /// <param name="wave"></param>
    public void UpdateWave(int wave)
    {
        WaveCount.text = new string($"Wave: {wave}");
    }
}
