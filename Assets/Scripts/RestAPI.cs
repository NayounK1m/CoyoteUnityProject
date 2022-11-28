using System;
using System.IO;
using System.Net;
using UnityEngine;
using System.Globalization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  

public class RestAPI : MonoBehaviour
{
    public Transform FailedLoadPanel;
    public Text FailedText;
    private bool failed = false;

    void Start()
    {
        for (int i= 1; i<4; i++)
        {
            string url = "http://192.168.2.222:8081/api/sensors/getSound" + i + "Coord";
            GetAllSensorLatLng(url, i);
        }
        if (failed == false)
            SceneManager.LoadScene("Coyote");
    }

    public void GetAllSensorLatLng(string sendurl, int sensorNumber)
    {
        Debug.Log(sensorNumber);
        HttpWebRequest httpWebRequest = WebRequest.Create(new Uri(sendurl)) as HttpWebRequest;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/json; charset=utf-8";

        string msg = "";

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(msg);
        httpWebRequest.ContentLength = (long)bytes.Length;
        using (Stream requestStream = httpWebRequest.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);

        string result = null;
        try
        {
            using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                result = new StreamReader(response.GetResponseStream()).ReadToEnd().ToString();

            Debug.Log(result);
            string[] splitResult = result.Split('"');
            string lat = splitResult[7];
            string lng = splitResult[11];

            Debug.Log(lat + ", " + lng);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            double latitude = System.Convert.ToDouble(lat, provider);
            double logitude = System.Convert.ToDouble(lng, provider);

            Debug.Log("double: "+latitude + ", " + logitude);

            SingletonLatLng.instance.LatSensor[0] = latitude;
            SingletonLatLng.instance.LngSensor[0] = logitude;

            Debug.Log(SingletonLatLng.instance.LatSensor[0] + ", " + SingletonLatLng.instance.LngSensor[0]);

        }
        catch (WebException e)
        {
            failed = true;
            Debug.Log(e.Message);
            //if(FailedLoadPanel.gameObject.activeSelf == false)
            //{
            //    FailedLoadPanel.gameObject.SetActive(true);
            //    FailedText.text = "Failed to load. \n" + e.Message;
            //}

        }
        catch
        {
            failed = true;
            Debug.Log("Failed to load");
            //if (FailedLoadPanel.gameObject.activeSelf == false)
            //    FailedLoadPanel.gameObject.SetActive(true);
        }
    }
}
