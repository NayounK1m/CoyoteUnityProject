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

        GetAllCoyoteHistory("http://192.168.2.222:8081/api/coyotes/getInitialCoyotes");
    }

    public void GetAllCoyoteHistory(string sendurl)
    {
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
            string[] codes = result.Split('[');
            string[] splitCodes = codes[1].Split('{');
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            for (int i = 0; i < splitCodes.Length; i++)
            {
                switch (splitCodes.Length)
                {
                    case 1:
                        string[] coyoteDataSplited1 = splitCodes[0].Split('"');
                        SingletonLatLng.instance.AddCoyoteLatLng(System.Convert.ToDouble(coyoteDataSplited1[2], provider),
                            System.Convert.ToDouble(coyoteDataSplited1[6], provider));
                        break;
                    case 2:
                        string[] coyoteDataSplited2 = splitCodes[0].Split('"');
                        SingletonLatLng.instance.AddCoyoteLatLng(System.Convert.ToDouble(coyoteDataSplited2[18], provider),
                            System.Convert.ToDouble(coyoteDataSplited2[22], provider));
                        break;
                    case 3:
                        string[] coyoteDataSplited3 = splitCodes[0].Split('"');
                        SingletonLatLng.instance.AddCoyoteLatLng(System.Convert.ToDouble(coyoteDataSplited3[34], provider),
                            System.Convert.ToDouble(coyoteDataSplited3[38], provider));
                        break;
                    case 4:
                        string[] coyoteDataSplited4 = splitCodes[0].Split('"');
                        SingletonLatLng.instance.AddCoyoteLatLng(System.Convert.ToDouble(coyoteDataSplited4[50], provider),
                            System.Convert.ToDouble(coyoteDataSplited4[54], provider));
                        break;
                    case 5:
                        string[] coyoteDataSplited5 = splitCodes[0].Split('"');
                        SingletonLatLng.instance.AddCoyoteLatLng(System.Convert.ToDouble(coyoteDataSplited5[66], provider),
                            System.Convert.ToDouble(coyoteDataSplited5[70], provider));
                        break;
                }

                Debug.Log("Coyote History" + i +" : "+ SingletonLatLng.instance.CoyoteLat[i] + ", " + SingletonLatLng.instance.CoyoteLng[i]);
            }

        }
        catch (WebException e)
        {
            failed = true;
            Debug.Log(e.Message);
            if (FailedLoadPanel.gameObject.activeSelf == false)
            {
                FailedLoadPanel.gameObject.SetActive(true);
                FailedText.text = "Failed to load. \n" + e.Message;
            }

        }
        catch (Exception e)
        {
            failed = true;
            Debug.Log("Failed to load: " + e.Message);
            if (FailedLoadPanel.gameObject.activeSelf == false)
                FailedLoadPanel.gameObject.SetActive(true);
        }
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

            SingletonLatLng.instance.AddLatLng(latitude, logitude);

        }
        catch (WebException e)
        {
            failed = true;
            Debug.Log(e.Message);
            if (FailedLoadPanel.gameObject.activeSelf == false)
            {
                FailedLoadPanel.gameObject.SetActive(true);
                FailedText.text = "Failed to load. \n" + e.Message;
            }

        }
        catch (Exception e)
        {
            failed = true;
            Debug.Log("Failed to load: " + e.Message);
            if (FailedLoadPanel.gameObject.activeSelf == false)
                FailedLoadPanel.gameObject.SetActive(true);
        }
    }
}
