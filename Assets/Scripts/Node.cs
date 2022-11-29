using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using WebSocketSharp;//웹 소켓 라이브러리
using Assets.SimpleAndroidNotifications;
using System;


public class Node : MonoBehaviour
{
    //Coyote Localization value
    private WebSocket ws;//소켓 선언
    
    void Start()
    {
        ws = new WebSocket("http://192.168.2.222:8081/api/coyotes/getInitialCoyotes");// IP : 192.168.2.187, PORT : 3333
        ws.OnOpen += ws_OnOpen;//서버가 연결된 경우 실행할 함수를 등록한다
        ws.OnMessage += ws_OnMessage; //서버에서 유니티 쪽으로 메세지가 올 경우 실행할 함수를 등록한다.
        ws.OnClose += ws_OnClose;//서버가 닫힌 경우 실행할 함수를 등록한다.
        ws.Connect();//서버에 연결한다.
        ws.Send("goHome");
    }
    void ws_OnMessage(object sender, MessageEventArgs e)
    {
        /*
         * {
    "msg": [
        {
            "x": "40.4206008430482",
            "y": "-86.90254211425781",
            "time": "1669654166201",
            "_id": "6384e99e0ba998757f00c7ce"
        },
        {
            "x": "40.4206008410482",
            "y": "-86.90254211425781",
            "time": "1669654927984",
            "_id": "6384e99e0ba998757f00c7cf"
        },
        {
            "x": "40.4206008410482",
            "y": "-86.90254211425781",
            "time": "1669654939320",
            "_id": "6384e99e0ba998757f00c7d0"
        }
    ]
}
         */
        Debug.Log(e.Data);//받은 메세지를 디버그 콘솔에 출력

        //파싱
        string data = "40.4203008430482, -86.90254211425781"; //e.Data;
        string[] codes  = data.Split(',');

        //string to double convert
        NumberFormatInfo provider = new NumberFormatInfo();
        provider.NumberDecimalSeparator = ".";
        double latitude = System.Convert.ToDouble(codes[0], provider);
        double longitude = System.Convert.ToDouble(codes[1], provider);

        //save at singleton
        SingletonLatLng.instance.Lat = latitude;
        SingletonLatLng.instance.Lng = longitude;

        notifyManager();

        Debug.Log(SingletonLatLng.instance.Lat);
        Debug.Log(SingletonLatLng.instance.Lng);
    }

    public void notifyManager()
    {
        var notificationParams = new NotificationParams
        {
            Id = UnityEngine.Random.Range(0, int.MaxValue),
            Delay = TimeSpan.FromSeconds(0.1),
            Title = "Coyote detected.",
            Message = "Coyote was detected on your farm.",
            Ticker = "Ticker",
            Sound = true,
            Vibrate = true,
            Light = true,
            SmallIcon = NotificationIcon.Bell,
            SmallIconColor = Color.red,
            LargeIcon = "app_icon"
        };

        NotificationManager.SendCustom(notificationParams);
    }

    void ws_OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("open"); 
    }
    void ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("close"); 
    }
}