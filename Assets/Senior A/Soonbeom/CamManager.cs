/*
카메라 과제
----------------------------------------------------
본 과제에서는 카메라의 1인칭과 3인칭 변환을 구현해보도록 하겠습니다.

1. 1인칭과 3인칭 시점을 찍을 적절한 가상카메라를 추가하기
2. 3인칭 카메라 시점
    - 적절한 거리에서 플레이어를 찍을 것
    - 마우스 우클릭 후 드래그로 시점 조절이 가능하게 할 것
    - 카메라 회전 시 하늘을 볼 수 있을 정도로 회전 가능하게 할 것
    - 마우스 스크롤으로 줌을 할 수 있게 할 것.
    - 카메라가 플레이어를 찍으면서 벽과 부딪히거나 장애물에 가리는 일이 없도록 할 것. 단 바닥과 충돌 시에는 무시한다.
    - FXAA모드로 안티앨리어싱을 적용하고, 임의의 포스트 프로세싱 하나 이상 추가하기.
3. 1인칭 카메라 시점
    - 플레이어의 눈높이에서 바라보는 세상을 보여줄 것
    - 플레이어의 얼굴을 보여주는 창을 오른쪽 아래에 띄웁니다.
4. 카메라 제어는 camManager를 통해 합니다.
5. 1인칭과 3인칭 카메라 전환시 1초의 시간동안 부드럽게 전환합니다.

---------------------------------------------------
과제에 대한 구현은 강의자료를 참고하시기 바랍니다.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    public GameObject thirdPersonCamObj;
    public GameObject firstPersonCamObj;
    public GameObject faceCamCanvas;
    public GameObject faceCam;
    public CinemachineFreeLook thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;
    public GameObject player;
    public GameObject playerFace;
    public float xRotateSpeed = 500.0f;
    public float yRotateSpeed = 10.0f;
    public float zoomSpeed = 0.5f;
    private bool _useMouseToRotateTp;
    private bool _isCurrentFp;

    private void Start() {
        // 카메라가 쫓아다닐 플레이어를 설정합니다.
        player = GameObject.FindWithTag("Player").gameObject;
        playerFace = player.transform.Find("FollowTarget").gameObject;

        // 1인칭 카메라와 3인칭카메라를 초기화시키고 카메라의 타겟을 설정합니다.
        firstPersonCamObj = player.transform.Find("FirstPersonCam").gameObject;
        firstPersonCam = firstPersonCamObj.GetComponent<CinemachineVirtualCamera>();
        thirdPersonCamObj = GameObject.Find("ThirdPersonCam");
        thirdPersonCam = thirdPersonCamObj.GetComponent<CinemachineFreeLook>();
        thirdPersonCam.Follow = player.transform;
        thirdPersonCam.LookAt = player.transform;
        faceCamCanvas = GameObject.Find("FaceCamCanvas");
        faceCam = faceCamCanvas.transform.Find("FaceCam").gameObject;

        // 처음에는 3인칭카메라로 시작할 것이기 때문에 1인칭카메라를 비활성화시켜줍니다.
        firstPersonCam.gameObject.SetActive(false);
        thirdPersonCam.gameObject.SetActive(true);
        faceCamCanvas.gameObject.SetActive(false);
        _isCurrentFp = false;
    }
    private void Update(){
        // 키보드 G 키를 누르면 1인칭과 3인칭을 전환합니다.
        if(Input.GetKeyDown(KeyCode.G)){
            CamChange();
        }
        // 마우스를 클릭했을때 3인칭 시점변경을 활성화시킵니다.
		if(Input.GetMouseButtonDown(1) && !_isCurrentFp){
            _useMouseToRotateTp = true;
        }
        if(Input.GetMouseButtonUp(1)){
            _useMouseToRotateTp = false;
        }
    }
    // LateUpdate에서 카메라 시점변환과 줌을 구현합니다.
    private void LateUpdate(){
		if(_useMouseToRotateTp){
            RotateTp();
        } else {
            thirdPersonCam.m_XAxis.m_MaxSpeed = 0;
            thirdPersonCam.m_YAxis.m_MaxSpeed = 0;
        }
        if(_isCurrentFp){
            faceCam.transform.position =  playerFace.transform.position + player.transform.forward * 0.9f;
            faceCam.transform.LookAt(playerFace.transform.position);
        }
        if(Input.mouseScrollDelta.y != 0 && !_isCurrentFp){
            Zoom();
        }
	}
    // CamChange에서 1인칭과 3인칭카메라를 변환합니다.
    private void CamChange(){
        _isCurrentFp = !_isCurrentFp;
        if(thirdPersonCam.gameObject.activeSelf){
            firstPersonCam.gameObject.SetActive(true);
            thirdPersonCam.gameObject.SetActive(false);
            faceCamCanvas.gameObject.SetActive(true);
        }
        else{
            firstPersonCam.gameObject.SetActive(false);
            thirdPersonCam.gameObject.SetActive(true);
            faceCamCanvas.gameObject.SetActive(false);
        }
    }
    // RotateTp에서 3인칭 시점 조절을 합니다.
    private void RotateTp()
	{
      thirdPersonCam.m_XAxis.m_MaxSpeed = xRotateSpeed;
      thirdPersonCam.m_YAxis.m_MaxSpeed = yRotateSpeed;
	}
    // Zoom으로 3인칭 시 마우스 스크롤을 통해 줌 인, 줌 아웃을 구현합니다. 이 때 카메라의 위치를 변경하지 말고 구현합니다.
    private void Zoom(){
		
		if(Input.mouseScrollDelta.y < 0){
            if (thirdPersonCam?.m_Lens.FieldOfView < 80)
            {
                Debug.Log("Zoom out");
                thirdPersonCam.m_Lens.FieldOfView += zoomSpeed;
            }
        }
        if(Input.mouseScrollDelta.y > 0){
            if (thirdPersonCam?.m_Lens.FieldOfView > 5)
            {
                Debug.Log("Zoom in");
                thirdPersonCam.m_Lens.FieldOfView -= zoomSpeed;
            }
        }
	}
}
