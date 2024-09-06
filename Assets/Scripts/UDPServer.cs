using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// LandMark ���� �� ����
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

    // UDPServer ��Ʈ�ѷ�
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

    // UDP ���� ����
    void StartUDPServer()
    {
        udpServer = new UdpClient(serverPort);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, serverPort);

        print("���� ����! Ŭ���̾�Ʈ���� ������ ���� ��ٸ��� ��..");

        // ������ ������ ����Ǵ� �Լ� ���
        udpServer.BeginReceive(ReceiveData, null);
    }

    void ReceiveData(IAsyncResult result)
    {
        // ����� �����͸� byte �迭�� ����.
        byte[] receiveByte = udpServer.EndReceive(result, ref remoteEndPoint);
        // byte �迭 �����͸� string ���� (UTF-8)
        string receiveMessage = Encoding.UTF8.GetString(receiveByte);
        print(receiveMessage);

        // receiveMessage �� �迭�� Json ���� ���ͼ� Key ���� �������� JsonUtility.FromJson ����
        // ����� �����ϴ�. (�������� �ƴ�, receiveMessage �� Ȯ�� ��!)
        receiveMessage = "{ \"data\" : " + receiveMessage + "}";

        // jsonStringData ----> LandMarkData ��ȯ
        landmark = JsonUtility.FromJson<LandMarkData>(receiveMessage);
      
        // ���� ������ ������ ����Ǵ� �Լ� ���
        udpServer.BeginReceive(ReceiveData, null);
    }

    private void OnDestroy()
    {
        // ���� ����
        udpServer.Close();
        print("UDP ���� ����");
    }
}
