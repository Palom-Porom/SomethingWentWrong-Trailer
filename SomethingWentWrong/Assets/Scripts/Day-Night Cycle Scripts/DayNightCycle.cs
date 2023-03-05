using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using static GameManager;


public enum DayTime { Sunrise = 0, Day, Sunset, Night, Midnight }

public class DayNightCycle : MonoBehaviour
{
    private Light2D globalLight;
    public float currentTime;
    [HideInInspector] public DayTime dayCycle;
    private SpawnSystem spawnSystem;

    [SerializeField] private float sunriseDuration;
    [SerializeField] private float dayDuration;
    [SerializeField] private float sunsetDuration;
    [SerializeField] private float nightDuration;
    [SerializeField] private float midnightDuration;

    private int dayCount = 0;

    //public RetroMaskScript retroMask;

    [SerializeField] private Color sunriseColor;
    [SerializeField] private Color dayColor;
    [SerializeField] private Color sunsetColor;
    [SerializeField] private Color nightColor;
    [SerializeField] private Color midnightColor;

    [SerializeField] private float sunriseIntensity;
    [SerializeField] private float dayIntensity;
    [SerializeField] private float sunsetIntensity;
    [SerializeField] private float nightIntensity;
    [SerializeField] private float midnightIntensity;

    private float timePassedPercent;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
    }

    void Start() 
    {
        spawnSystem = GM.Rocket.GetComponentInChildren<SpawnSystem>();

        currentTime = 0;
        dayCycle = DayTime.Day;
        globalLight.color = sunriseColor;
        globalLight.intensity = sunriseIntensity;

        StartCoroutine(Cycle());
    }

     private IEnumerator Cycle()
     {
         while (true)
         {
             while (true)
             {
                 // Day
                 currentTime += Time.deltaTime;

                 if (currentTime >= dayDuration)
                 {
                     currentTime = 0;
                     dayCycle = DayTime.Sunset;
                     break;
                 }
             
                 timePassedPercent = currentTime / dayDuration;
                 globalLight.color = Color.Lerp(dayColor, sunsetColor, timePassedPercent);
                 globalLight.intensity = Mathf.Lerp(dayIntensity, sunsetIntensity, timePassedPercent);

                 //yield return new WaitForEndOfFrame();
                 yield return new WaitForNextFrameUnit();
             }
             
             while (true)
             {
                 // Sunset
                 currentTime += Time.deltaTime;

                 if (currentTime >= sunsetDuration)
                 {
                     currentTime = 0;
                     dayCycle = DayTime.Night;
                     spawnSystem.spawnEnabled = true;
                     break;
                 }
             
                 timePassedPercent = currentTime / dayDuration;
                 globalLight.color = Color.Lerp(sunsetColor, nightColor, timePassedPercent);
                 globalLight.intensity = Mathf.Lerp(sunsetIntensity, nightIntensity, timePassedPercent);
                 
                 yield return new WaitForNextFrameUnit();
             }
             
             while (true)
             {
                 // Night
                 currentTime += Time.deltaTime;

                 if (currentTime >= nightDuration)
                 {
                     currentTime = 0;
                     dayCycle = DayTime.Midnight;
                     break;
                 }
             
                 timePassedPercent = currentTime / dayDuration;
                 globalLight.color = Color.Lerp(nightColor, midnightColor, timePassedPercent);
                 globalLight.intensity = Mathf.Lerp(nightIntensity, midnightIntensity, timePassedPercent);
                 
                 yield return new WaitForNextFrameUnit();
             }  
             
             while (true)
             {
                 // Midnight
                 currentTime += Time.deltaTime;

                 if (currentTime >= midnightDuration)
                 {
                     currentTime = 0;
                     dayCycle = DayTime.Sunrise;
                     
                     dayCount++;
                     if (dayCount >= 3)
                         GM.UI.WinScreen.SetActive(true);
                     else
                     {
                         GM.UI.SkillsMenu.GetComponentInParent<SkillsScript>().InitSkills();
                         GM.UI.SkillsMenu.SetActive(true);
                     }
                     
                     spawnSystem.spawnEnabled = false;
                     break;
                 }
             
                 timePassedPercent = currentTime / dayDuration;
                 globalLight.color = Color.Lerp(midnightColor, sunriseColor, timePassedPercent);
                 globalLight.intensity = Mathf.Lerp(midnightIntensity, sunriseIntensity, timePassedPercent);
                 
                 yield return new WaitForNextFrameUnit();
             }
             
             while (true)
             {
                 // Sunrise
                 currentTime += Time.deltaTime;

                 if (currentTime >= sunriseDuration)
                 {
                     currentTime = 0;
                     dayCycle = DayTime.Day;
                     break;
                 }
             
                 timePassedPercent = currentTime / dayDuration;
                 globalLight.color = Color.Lerp(sunriseColor, dayColor, timePassedPercent);
                 globalLight.intensity = Mathf.Lerp(sunriseIntensity, dayIntensity, timePassedPercent);
                 
                 yield return new WaitForNextFrameUnit();
             }
         }
     }
}