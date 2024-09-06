import cv2
import mediapipe as mp
import socket
import json

# Mediapipe Pose와 관련된 설정
mp_drawing = mp.solutions.drawing_utils
mp_pose = mp.solutions.pose

# UDP 소켓 설정
UDP_IP = "127.0.0.1"  # 데이터를 수신할 IP 주소
UDP_PORT = 5005       # 데이터를 수신할 포트 번호
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# 웹캠에서 입력을 받아 Mediapipe Pose 모델을 사용
cap = cv2.VideoCapture(0)
with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:
    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break

        # 이미지 색상 변환 (BGR to RGB)
        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        image.flags.writeable = False

        # 포즈 추정 수행
        results = pose.process(image)

        # 이미지 색상 다시 변환 (RGB to BGR)
        image.flags.writeable = True
        image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)

        # 포즈 랜드마크 데이터가 있으면
        if results.pose_landmarks:
            landmarks = []
            for lm in results.pose_landmarks.landmark:
                landmarks.append({
                    'x': lm.x,
                    'y': -lm.y,
                    'z': lm.z,
                    'visibility': lm.visibility
                })

            # 랜드마크 데이터를 JSON으로 직렬화
            data = json.dumps(landmarks)

            # UDP로 데이터 전송
            sock.sendto(data.encode('utf-8'), (UDP_IP, UDP_PORT))

            # 포즈 랜드마크 그리기
            mp_drawing.draw_landmarks(image, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)

        # 결과 이미지 표시
        cv2.imshow('Mediapipe Pose', image)

        if cv2.waitKey(10) & 0xFF == ord('q'):
            break

# 자원 정리
cap.release()
cv2.destroyAllWindows()
sock.close()
