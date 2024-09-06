using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoseName
{
    nose,
    left_eye_inner,
    left_eye,
    left_eye_outer,
    right_eye_inner,
    right_eye,
    right_eye_outer,
    left_ear,
    right_ear,
    mouth_left,
    mouth_right,
    left_shoulder,
    right_shoulder,
    left_elbow,
    right_elbow,
    left_wrist,
    right_wrist,
    left_pinky,
    right_pinky,
    left_index,
    right_index,
    left_thumb,
    right_thumb,
    left_hip,
    right_hip,
    left_knee,
    right_knee,
    left_ankle,
    right_ankle,
    left_heel,
    right_heel,
    left_foot_index,
    right_foot_index,

    pose_max
}


public class MediaPipeData : MonoBehaviour
{
    // point prefab
    public GameObject pointFactory;

    // UDPServer
    public UDPServer udpServer;

    // ��� point ���� ����
    public Transform[] allPoints;

    void Start()
    {
        // allPoints �� � ������ 
        allPoints = new Transform[(int)PoseName.pose_max];
                
        //point �� pose_max �� ������
        for (int i = 0; i < (int)PoseName.pose_max; i++)
        {
            GameObject point = Instantiate(pointFactory);
            //������� point �� ���� �ڽ����� ���� 
            point.transform.parent = transform;
            //������� point �� �̸��� �ش� �Ǵ� PoseName ���� ����
            point.name = ((PoseName)i).ToString();
            //������� point �� transform �� allPoints �� ����
            allPoints[i] = point.transform;
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    for (int i = 0; i < (int)PoseName.pose_max; i++)
        //    {
        //        allPoints[i].parent = null;
        //    }
        //}

        // ��� LandMarkData �� ���� ����
        LandMarkData landMark = udpServer.landmark;

        // �����Ͱ� ����ִٸ�
        if(landMark.data.Count == (int)PoseName.pose_max)
        {
            // landMark �� ��ġ���� point �� ��ġ������ ����
            for(int i = 0; i < allPoints.Length; i++)
            {
                Vector3 pos = new Vector3(landMark.data[i].x, landMark.data[i].y, landMark.data[i].z);
                allPoints[i].position = pos;
            }
        }
    }
}
