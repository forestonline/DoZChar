using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class TimeController : MonoBehaviourPunCallbacks
{

    	PhotonView PV;

    static DateTime MasterClock;

    [SerializeField]
    GameObject SunAlpha;

    [SerializeField]
    private float timeMultiplier;

    [SerializeField]
    private float startHour;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private float sunriseHour;

    [SerializeField]
    private float sunsetHour;

    [SerializeField]
    private Color dayAmbientLight;

    [SerializeField]
    private Color nightAmbientLight;

    [SerializeField]
    private AnimationCurve lightChangeCurve;

    [SerializeField]
    private float maxSunLightIntensity;

    [SerializeField]
    private Light moonLight;

    [SerializeField]
    private float maxMoonLightIntensity;

    private DateTime currentTime;

    private TimeSpan sunriseTime;

    private TimeSpan sunsetTime;


    	void Awake()
	{
	
		PV = GetComponent<PhotonView>();

	}


    // Start is called before the first frame update
    void Start()
    {
        if(Launcher.ClientUser)
        {
            DestroySun();
         
           
        }
            else
            {
                             currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
                             sunriseTime = TimeSpan.FromHours(sunriseHour);
                            sunsetTime = TimeSpan.FromHours(sunsetHour);
            }

  
    }

    // Update is called once per frame
    void Update()
    {

        if(!Launcher.ClientUser)
        {     
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        //RPC_SyncTime(MasterClock);    
         //    PV.RPC("RPC_SyncTime", RpcTarget.All, MasterClock);
        }  
         

    }

    private void UpdateTimeOfDay()
    {
                currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
                MasterClock = currentTime;  
     

        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }

       public void DestroySun()
    {
      if(!PV.IsMine)
        return;
  
            PhotonNetwork.Destroy(SunAlpha);
       
         
    }

    	[PunRPC]
	void RPC_SyncTime(DateTime Clock)
	{
	         // print("RUNNING RPC");   
            currentTime = Clock;
                  timeText.text = currentTime.ToString("HH:mm");

	}



}