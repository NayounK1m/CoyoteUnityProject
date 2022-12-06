    using UnityEngine;
    using System.Globalization;
    using WebSocketSharp;//웹 소켓 라이브러리
    using Assets.SimpleAndroidNotifications;
    using System;

    public class Node : MonoBehaviour
    {
        //Coyote Localization value
        private WebSocket ws;//Socket Declaration

    void Start()
        {
            ws = new WebSocket("ws://192.168.2.222:3333");//IP : 192.168.2.222, PORT : 3333
            ws.OnOpen += ws_OnOpen;//Register a function to run if the server is connected
            ws.OnMessage += ws_OnMessage; //Register a function to run when a message comes from the server toward Unity.
            ws.OnClose += ws_OnClose;//Register a function to run when the server is closed.
            ws.Connect();//Connect to the server.
            ws.Send("Client : Connected"); //Send a message to the server
    }

        //실시간
        void ws_OnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log(e.Data);//받은 메세지를 디버그 콘솔에 출력
            //파싱
            string data = e.Data;
            string[] codes = data.Split(',');

            //string to double convert
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            double latitude = System.Convert.ToDouble(codes[0], provider);
            double longitude = System.Convert.ToDouble(codes[1], provider);

            //save at singleton
            SingletonLatLng.instance.CoyoteLat.Add(latitude);
            SingletonLatLng.instance.CoyoteLng.Add(longitude);
            //SingletonLatLng.instance.CoyoteLat.ForEach(delegate (double lat) {  Debug.Log("Node: " + lat); });
            //SingletonLatLng.instance.CoyoteLng.ForEach(delegate (double lng){ Debug.Log("Node: " + lng); });

            notifyManager();
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