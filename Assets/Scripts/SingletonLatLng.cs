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

    //public double LatSensor1 {get; set;}
    //public double LngSensor1 {get; set;}
    //public double LatSensor2 {get; set;}
    //public double LngSensor2 {get; set;}
    //public double LatSensor3 {get; set;}
    //public double LngSensor3 {get; set;}

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

    public void AddLatLng(double lat, double lng)
    {
        for (int i = 0; i < LatSensor.Length; i++)
        {
            if (LatSensor[i] == 0)
                LatSensor[i] = lat;
            if (LngSensor[i] == 0)
                LngSensor[i] = lng;

            Debug.Log("sensor" + i+ ": " +LatSensor[i] + ", " + LngSensor[i]);
        }

    }
}