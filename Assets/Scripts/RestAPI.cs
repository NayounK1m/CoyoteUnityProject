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
        GetAllCoyoteHistory("http://192.168.2.222:8081/api/coyotes/getInitialCoyotes");

        if (failed == false)
            SceneManager.LoadScene("Coyote");
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

            string[] codes = result.Split('[');
            string[] code = codes[1].Split(']');
            Debug.Log(code[0]);
            string[] splitCodes = code[0].Split('"');

            int dataLength = 0;
            if (splitCodes[3] == null)
                dataLength = 1;
            else if (splitCodes[5] == null)
                dataLength = 2;
            else if (splitCodes[7] == null)
                dataLength = 3;
            else if (splitCodes[9] == null)
                dataLength = 4;
            else if (splitCodes[9] != null)
                dataLength = 5;

            SaveToSingleton(splitCodes, dataLength);


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

    public void SaveToSingleton(string[] splitcodes, int count)
    {
        NumberFormatInfo provider = new NumberFormatInfo();
        provider.NumberDecimalSeparator = ".";

        for (int i = 0; i < count; i++)
        {
            int num = 2 * i + 1;
            string[] coyoteDataSplited = splitcodes[num].Split('/');
            SingletonLatLng.instance.AddCoyoteLatLng(System.Convert.ToDouble(coyoteDataSplited[0], provider),
            System.Convert.ToDouble(coyoteDataSplited[1], provider), i);

            Debug.Log(i+1);
        }
    }

    public void GetAllSensorLatLng(string sendurl, int sensorNumber)
    {
        //Debug.Log(sensorNumber);
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

            //Debug.Log(result);
            string[] splitResult = result.Split('"');
            string lat = splitResult[7];
            string lng = splitResult[11];

            //Debug.Log(lat + ", " + lng);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            double latitude = System.Convert.ToDouble(lat, provider);
            double logitude = System.Convert.ToDouble(lng, provider);

            //Debug.Log("double: "+latitude + ", " + logitude);

            SingletonLatLng.instance.AddLatLng(latitude, logitude, sensorNumber);

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
