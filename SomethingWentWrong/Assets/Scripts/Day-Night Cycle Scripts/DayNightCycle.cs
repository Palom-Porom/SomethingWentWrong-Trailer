using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using static GameManager;
using UnityEngine.InputSystem;


public enum DayTime { Sunrise = 0, Day, Sunset, Night, Midnight }

public class DayNightCycle : MonoBehaviour
{
    private Light2D globalLight;
    public float currentTime;
    [HideInInspector] public DayTime dayCycle;
    //private SpawnSystem spawnSystem;

    [SerializeField] private float sunriseDuration;
    [SerializeField] private float dayDuration;
    [SerializeField] private float sunsetDuration;
    [SerializeField] private float nightDuration;
    [SerializeField] private float midnightDuration;
    
    
    [SerializeField] private Material landscapeMaterial;

    bool isNightNeeded = false;
    bool buttonReadyToPress = true;

    public bool RetroWaveEffect
    {
        set
        {
            int n = value ? 1 : 0;
            landscapeMaterial.SetInt("_RetroWaveEffect", n);
        }
    }


    public TextMeshProUGUI scoreTitle;
    private int dayCount;
    [SerializeField] private TMP_ColorGradient firstGradient;
    [SerializeField] private TMP_ColorGradient secondGradient;
    private EndScreen endScreenScript;
    
    public int DayCount
    {
        get => dayCount;
        set {
            dayCount = value;
            scoreTitle.text = $"День: {value}";
            if (dayCount > GM.UI.EndScreenScript.MaxScore)
            {
                scoreTitle.colorGradientPreset = secondGradient;
                GM.UI.EndScreenScript.MaxScore = dayCount;
                if (value % 2 == 0)
                    GM.UI.Encyclopedia.LoreTab.transform.GetChild(0).GetChild(value / 2).GetComponent<LoreManager>().UnlockNote();
            }
            else
                scoreTitle.colorGradientPreset = firstGradient;
        }
    }

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

    [SerializeField] private EnemiesSpawnSystem ess;

    private float timePassedPercent;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
    }

    void Start() 
    {
        //spawnSystem = GM.Rocket.GetComponentInChildren<SpawnSystem>();
        DayCount = 1;
        
        currentTime = 0;
        dayCycle = DayTime.Night;
        globalLight.color = sunriseColor;
        globalLight.intensity = sunriseIntensity;

        StartCoroutine(Cycle());
    }

    private void Update()
    {
        //Debug.Log(2);
        if (Keyboard.current.nKey.isPressed)
        {
            
            if (buttonReadyToPress)
            {
                isNightNeeded = !isNightNeeded;
                //Debug.Log(isNightNeeded);
                if (isNightNeeded)
                {
                    dayCycle = DayTime.Sunset;
                    currentTime = 0;
                    StartCoroutine(WaitFor1Sec());
                }
                else
                {
                    dayCycle = DayTime.Sunrise;
                    currentTime = 0;
                    StartCoroutine(WaitFor1Sec());
                }
            }
        }
    }

    IEnumerator WaitFor1Sec()
    {
        buttonReadyToPress = false;
        yield return new WaitForSecondsRealtime(1f);
        buttonReadyToPress = true;
    }

    private IEnumerator Cycle()
     {
         while (true)
         {
            //sunriseDuration += 3f;
            Debug.Log("sunrise now");
            //while (true)
            //{
            //    // Sunrise
            //    if (dayCycle == DayTime.Sunset)
            //    {
            //        currentTime = 0;
            //        break;
            //    }

            //    currentTime += Time.deltaTime;

            //    //if (currentTime >= sunriseDuration)
            //    //{
            //    //    currentTime = 0;
            //    //    dayCycle = DayTime.Day;

            //    //    break;
            //    //}

            //    landscapeMaterial.SetFloat("_Fade", Mathf.Lerp(1f, 0f, 2 * timePassedPercent));
            //    timePassedPercent = currentTime / sunriseDuration;
            //    globalLight.color = Color.Lerp(sunriseColor, dayColor, timePassedPercent);
            //    globalLight.intensity = Mathf.Lerp(sunriseIntensity, dayIntensity, timePassedPercent);

            //    yield return new WaitForNextFrameUnit();
            //}

            ////while (true)
            //// {
            ////    // Day
            ////    if (dayCycle == DayTime.Sunset)
            ////    {
            ////        currentTime = 0;
            ////        break;
            ////    }

            ////    currentTime += Time.deltaTime;

            ////     //if (currentTime >= dayDuration)
            ////     //{
            ////     //    currentTime = 0;
            ////     //    dayCycle = DayTime.Sunrise;
            ////     //    break;
            ////     //}
             
            ////     timePassedPercent = currentTime / dayDuration;
            ////     globalLight.color = Color.Lerp(dayColor, sunsetColor, timePassedPercent);
            ////     globalLight.intensity = Mathf.Lerp(dayIntensity, sunsetIntensity, timePassedPercent);

            ////     //yield return new WaitForEndOfFrame();
            ////     yield return new WaitForNextFrameUnit();
            //// }
             
            // Debug.Log("it's sunset now");
             
            // while (true)
            // {
            //    // Sunset
            //    if (dayCycle == DayTime.Sunrise)
            //    {
            //        currentTime = 0;
            //        break;
            //    }

            //    currentTime += Time.deltaTime;

            //     if (currentTime >= sunsetDuration)
            //     {
            //         currentTime = 0;
            //         dayCycle = DayTime.Night;
            //         StartCoroutine(ess.SpawnEnemies());
            //         //spawnSystem.spawnEnabled = true;
            //         break;
            //     }
             
            //     timePassedPercent = currentTime / sunsetDuration;
            //     landscapeMaterial.SetFloat("_Fade", Mathf.Lerp(0, 0.2f, timePassedPercent));
            //     globalLight.color = Color.Lerp(sunsetColor, nightColor, timePassedPercent);
            //     globalLight.intensity = Mathf.Lerp(sunsetIntensity, nightIntensity, timePassedPercent);
                 
            //     yield return new WaitForNextFrameUnit();
            // }

             Debug.Log("it's night now");
             
             while (true)
             {
                // Night
                if (dayCycle == DayTime.Sunrise)
                {
                    currentTime = 0;
                    break;
                }

                currentTime += Time.deltaTime;
                 currentTime = Mathf.Min(currentTime, nightDuration);

                 //if (GM.Spawner.Enemies.ExistingEnemies == 0 && currentTime >= nightDuration)
                 //{
                 //    currentTime = 0;
                 //    dayCycle = DayTime.Midnight;
                 //    break;
                 //}
             
                 timePassedPercent = currentTime / nightDuration;
                 landscapeMaterial.SetFloat("_Fade", Mathf.Lerp(0.2f, 1f, 5 * timePassedPercent));
                 globalLight.color = Color.Lerp(nightColor, midnightColor, timePassedPercent);
                 globalLight.intensity = Mathf.Lerp(nightIntensity, midnightIntensity, timePassedPercent);
                     
                 yield return new WaitForNextFrameUnit();
             }  
             
             //while (true)
             //{
             //   // Midnight
             //   if (dayCycle == DayTime.Day)
             //   {
             //       currentTime = 0;
             //       break;
             //   }

             //   currentTime += Time.deltaTime;

             //    if (currentTime >= midnightDuration)
             //    {
             //        currentTime = 0;
             //        dayCycle = DayTime.Night;
                     
             //        // DayCount++;
             //        // if (dayCount >= 3)
             //        //     GM.GameOver("Вы победили");
             //        // else
             //        // {
             //        //     вызов окна скилов
             //        //     GM.UI.SkillsMenu.GetComponentInParent<SkillsScript>().InitSkills();
             //        //     if (!GM.UI.EndScreen.GetComponentInParent<EndScreen>().isOpened)
             //        //         GM.UI.SkillsMenu.SetActive(true);
             //        // }
                     
             //        //spawnSystem.spawnEnabled = false;
             //        break;
             //    }
             
             //    timePassedPercent = currentTime / midnightDuration;
             //    landscapeMaterial.SetFloat("_Fade", Mathf.Lerp(1f, 0f, 2 * timePassedPercent));
             //    globalLight.color = Color.Lerp(midnightColor, sunriseColor, timePassedPercent);
             //    globalLight.intensity = Mathf.Lerp(midnightIntensity, sunriseIntensity, timePassedPercent);
                 
             //    yield return new WaitForNextFrameUnit();
             //}
         }
     }


     private void OnDestroy()
     {
         landscapeMaterial.SetFloat("_Fade", 0f);
     }
}