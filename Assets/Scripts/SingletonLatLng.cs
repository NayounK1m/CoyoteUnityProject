using System;
using System.IO;
using System.Net;
using UnityEngine;
using System.Globalization;

public class SingletonLatLng : MonoBehaviour
{
    public static SingletonLatLng instance = null;
    public double Lat { get; set; }
    public double Lng { get; set; }
    public double[] LatSensor = new double[3];
    public double[] LngSensor = new double[3];
    public double[] CoyoteLat = new double[5];
    public double[] CoyoteLng = new double[5];

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때 
        { 
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지 
        } 
        else 
        { 
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미 
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제 
        }
    }
    public void SwapCoyoteArr(double newCoyoteLat, double newCoyoteLng)
    {
        int dataLength = 0;
        if (CoyoteLat[0] == 0 && CoyoteLng[0]==0)
            dataLength = 0;
        else if (CoyoteLat[1] == 0 && CoyoteLng[1] == 0)
            dataLength = 1;
        else if (CoyoteLat[2] == 0 && CoyoteLng[2] == 0)
            dataLength = 2;
        else if (CoyoteLat[3] == 0 && CoyoteLng[3] == 0)
            dataLength = 3;
        else if (CoyoteLat[4] == 0 && CoyoteLng[4] == 0)
            dataLength = 4;
        else if (CoyoteLat[4] != 0 && CoyoteLng[4] != 0)
            dataLength = 5;

        if (CoyoteLat[0] != 0)
        {
            for (int i = 0; i < dataLength; i++)
            {
                CoyoteLat.SetValue(CoyoteLat[i], i + 1);
                CoyoteLng.SetValue(CoyoteLng[i], i + 1);
            }
        }
        CoyoteLat.SetValue(newCoyoteLat, 0);
        CoyoteLat.SetValue(newCoyoteLng, 0);

        for (int i = 0; i < dataLength; i++)
        {
            Debug.Log(CoyoteLat[i] + ", " + CoyoteLng[i]);
        }

    }

    public void AddLatLng(double lat, double lng, int sensorNum)
    {
        LatSensor.SetValue(lat, sensorNum-1);
        LngSensor.SetValue(lng, sensorNum-1);
        Debug.Log("Sensor" + sensorNum + ": " + lat + ", " + lng);

    }

    public void AddCoyoteLatLng(double lat, double lng, int count)
    {
        if (CoyoteLat[count] != 0)
            return;
        else
        {
            CoyoteLat.SetValue(lat, count);
            CoyoteLng.SetValue(lng, count);
            Debug.Log("Coyote" + count + ": " + CoyoteLat[count] + ", " + CoyoteLng[count]);
        }

    }
}