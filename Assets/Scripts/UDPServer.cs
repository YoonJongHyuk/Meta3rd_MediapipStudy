using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// LandMark 저장 할 구조
[Serializable]
public class LandMarkFormat
{
    public float x;
    public float y;
    public float z;
    public float visibility;
}

[Serializable]
public class LandMarkData
{
    public List<LandMarkFormat> data;
}

public class UDPServer : MonoBehaviour
{
    // Port
    public int serverPort = 5005;

    // UDPServer 컨트롤러
    UdpClient udpServer;
    // EndPoint
    IPEndPoint remoteEndPoint;

    // LandMarkData 
    public LandMarkData landmark;

    void Start()
    {
        StartUDPServer();
    }

    void Update()
    {
        
    }

    // UDP 서버 시작
    void StartUDPServer()
    {
        udpServer = new UdpClient(serverPort);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, serverPort);

        print("서버 시작! 클라이어트에서 들어오는 응답 기다리는 중..");

        // 응답이 들어오면 실행되는 함수 등록
        udpServer.BeginReceive(ReceiveData, null);
    }

    void ReceiveData(IAsyncResult result)
    {
        // 응답온 데이터를 byte 배열로 받자.
        byte[] receiveByte = udpServer.EndReceive(result, ref remoteEndPoint);
        // byte 배열 데이터를 string 변경 (UTF-8)
        string receiveMessage = Encoding.UTF8.GetString(receiveByte);
        print(receiveMessage);

        // receiveMessage 가 배열의 Json 으로 들어와서 Key 값을 만들어줘야 JsonUtility.FromJson 으로
        // 사용이 가능하다. (무조건은 아님, receiveMessage 를 확인 후!)
        receiveMessage = "{ \"data\" : " + receiveMessage + "}";

        // jsonStringData ----> LandMarkData 변환
        landmark = JsonUtility.FromJson<LandMarkData>(receiveMessage);
      
        // 다음 응답이 들어오면 실행되는 함수 등록
        udpServer.BeginReceive(ReceiveData, null);
    }

    private void OnDestroy()
    {
        // 서버 종료
        udpServer.Close();
        print("UDP 서버 종료");
    }
}
