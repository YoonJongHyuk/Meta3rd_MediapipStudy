using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCharacter : MonoBehaviour
{
    // Point 의 정보 (MediaPipeData)
    public MediaPipeData data;

    // Rig 의 Target (오른손)
    public Transform rigRightHandTarget;
    public Transform rigRIghtHandHint;

    // Animator
    Animator anim;

    // 오른쪽 어깨 to 오른쪽 팔꿈치 의 거리
    float rightShoulderToElbow;
    // 오른쪽 팔꿈치 to 오른쪽 손목 의 거리
    float rightElbowToWrist;

    Vector3 initDir;
    Quaternion initRot;
    void Start()
    {
        // Animator 가져오자
        anim = GetComponent<Animator>();

        // 오른쪽 어깨 to 오른쪽 팔꿈치 의 거리 구하자
        Vector3 rightShoulder = anim.GetBoneTransform(HumanBodyBones.RightUpperArm).position;
        Vector3 rightElbow = anim.GetBoneTransform(HumanBodyBones.RightLowerArm).position;
        rightShoulderToElbow = Vector3.Distance(rightShoulder, rightElbow);
        // 오른쪽 팔꿈치 to 오른쪽 손목의 거리를 구하자
        Vector3 rightWrist = anim.GetBoneTransform(HumanBodyBones.RightHand).position;
        rightElbowToWrist = Vector3.Distance(rightElbow, rightWrist);

        initDir =
            anim.GetBoneTransform(HumanBodyBones.RightLowerArm).position -
            anim.GetBoneTransform(HumanBodyBones.RightUpperArm).position;
        initRot = anim.GetBoneTransform(HumanBodyBones.RightUpperArm).localRotation;
    }

    void Update()
    {
        SetRightHand();
    }

    void SetRightHand()
    {
        // 미디어 파이프 데이터 (오른쪽 어깨 ----> 오른쪽 팔꿈치 방향)
        Vector3 dirShoulderToElbow =
            data.allPoints[(int)PoseName.right_elbow].position -
            data.allPoints[(int)PoseName.right_shoulder].position;

        // 오른쪽팔꿈치를 향하는 정규화된 벡터 * 캐릭터의 팔꿈치까지의 거리
        dirShoulderToElbow = dirShoulderToElbow.normalized * rightShoulderToElbow;

        // 미디어 파이프 데이터 (오른쪽 팔꿈치 ----> 오른쪽 손목 방향)
        Vector3 dirElbowToWrist =
            data.allPoints[(int)PoseName.right_wrist].position -
            data.allPoints[(int)PoseName.right_elbow].position;

        // 오른쪽 손목를 향하는 정규화된 벡터 * 캐릭터의 손목 까지의 거리
        dirElbowToWrist = dirElbowToWrist.normalized * rightElbowToWrist;

        // dirShoulderToElbow + dirShoulderToElbow  방향을 캐릭터의 오른쪽 어깨에서 더하자.
        Vector3 targetPos =
            anim.GetBoneTransform(HumanBodyBones.RightUpperArm).position +
            dirShoulderToElbow;

        // rig의 오른쪽 손 target 의 위치를 targetPos 로 설정
        rigRIghtHandHint.position = targetPos;
        rigRightHandTarget.position = targetPos + dirElbowToWrist;
    }
}
