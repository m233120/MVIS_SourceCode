using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;

public class Mapbox : MonoBehaviour
{
    public string accessToken = "pk.eyJ1Ijoiam9leWthY2U1NSIsImEiOiJjbGQyNnpqbmgwMm5rM3VueTNjYjZ5NWw0In0.JsKGV5iOJ2DN_oiYmbo-eA";
    public float centerLatitude = 38.981f;
    public float centerLongitude = -76.4826f;
    public float zoom = 14.94f;
    public int bearing = 0;
    public int pitch = 0;
    public enum style { Light, Dark, Streets, Outdoors, Satellite, SatelliteStreets };
    public style mapStyle = style.Satellite;
    public enum resolution { low = 1, high = 2};
    public resolution mapResolution = resolution.low;

    private int mapWidth = 250;
    private int mapHeight = 250;
    private string[] styleStr = new string[] { "light-v10", "dark-v10", "streets-v11", "satellite-v9", "satellite-streets-v11" };
    private string url = "";
    private bool mapIsLoading = false;
    private Rect rect;
    private bool updateMap = true;

    private string accessTokenLast;
    private float centerLatitudeLast = 38.981f;
    private float centerLongitudeLast = -76.4826f;
    private float zoomLast = 14.94f;
    private int bearingLast = 0;
    private int pitchLast = 0;
    private style mapStyleLast = style.Satellite;
    private resolution mapResolutionLast = resolution.low;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetMapbox());
        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (updateMap && (accessTokenLast != accessToken || !Mathf.Approximately(centerLatitudeLast, centerLatitude) || !Mathf.Approximately(centerLongitudeLast, centerLongitude) || zoomLast != zoom ||
            bearingLast != bearing || pitchLast != pitch || mapStyleLast != mapStyle || mapResolutionLast != mapResolution))
        {
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Math.Round(rect.width);
            mapHeight = (int)Math.Round(rect.height);
            StartCoroutine(GetMapbox());
            updateMap = false;
        }
    }

    IEnumerator GetMapbox()
    {
        url = "https://api.mapbox.com/styles/v1/mapbox/" + styleStr[(int)mapStyle] + "/static/" + centerLongitude + "," + centerLatitude + "," + zoom + "," + bearing + "," + pitch + "/" + mapWidth + "x" +
          mapHeight + "?" + "access_token=" + accessToken;
        //url = "https://api.mapbox.com/styles/v1/mapbox/streets-v11/static/-76.4861,38.9815,15.07,0/600x600?access_token=pk.eyJ1Ijoiam9leWthY2U1NSIsImEiOiJjbGQyNnN2NjkwMzQ1M3Jxa3NoMXJtY3hqIn0.O1-ITgLIPaCeT1XIuwowDw";
        mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log("WWW ERROR:" + www.error);
        else
        {
            mapIsLoading = false;
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            accessTokenLast = accessToken;
            centerLatitudeLast = centerLatitude;
            centerLongitudeLast = centerLongitude;
            zoomLast = zoom;
            bearingLast = bearing;
            pitchLast = pitch;
            mapStyleLast = mapStyle;
            mapResolutionLast = mapResolution;
            updateMap = true;

        }
    }
}
