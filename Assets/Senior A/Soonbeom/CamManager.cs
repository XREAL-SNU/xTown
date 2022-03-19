/*
카메라 과제:
본 과제에서는 카메라의 1인칭과 3인칭 변환을 구현해보도록 하겠습니다.
3인칭시점에서는 마우스 우클릭 후 드래그 시 시점을 조절할 수 있도록 합니다. 그리고 마우스 스크롤을 통해 줌을 할 수 있게 합니다.
단, 줌을 할 경우 너무 가깝거나 너무 먼 거리에서 찍지 못하게 적절한 수준 사이에서만 허용합니다.
그리고 카메라를 내렸을 때 아래에서 바라봐 하늘을 볼 수 있는 정도까지 시점변환을 허용합니다.
카메라가 벽을 뚫고 들어가서 플레이어를 찍을 수 없다거나, 중간에 장애물이 가로막아서 플레이어를 찍지 못 하는 불편한 점이 없도록 합니다.
단 땅과 카메라가 충돌했을 때에는 하늘을 볼 수 있어야 하므로 땅을 무시하고 아래에서 찍을 수 있도록 합니다.
1인칭시점에서는 플레이어의 시선에서 씬을 바라보도록 합니다. 플레이어가 회전 시 카메라가 보는 화면도 회전해야하며 플레이어 시선과 같은 높이에서 씬을 바라보게 합니다.
3인칭과 1인칭 시점 전환시 1초 동안 부드럽게 화면을 전환합니다.

과제에 대한 평가기준은 다음과 같습니다.
1. 1인칭 시점과 3인칭 시점에 적절한 가상카메라를 사용할 것
2. CamManager오브젝트와 CamManager 스크립트를 통해 카메라 관리를 할 것
3. 3인칭 시점일 시 플레이어를 바라보는 데 불편함이 없을 것

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
    public CinemachineFreeLook thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;
    public GameObject player;
    public float xRotateSpeed = 500.0f;
    public float yRotateSpeed = 10.0f;
    public float zoomSpeed = 0.5f;
    private bool _useMouseToRotateTp;
    private bool _isCurrentFp;

    private void Start() {
        // 카메라가 쫓아다닐 플레이어를 설정합니다.
        player = GameObject.FindWithTag("Player");

        // 1인칭 카메라와 3인칭카메라를 초기화시키고 카메라의 타겟을 설정합니다.
        firstPersonCamObj = player.transform.Find("FirstPersonCam").gameObject;
        firstPersonCam = firstPersonCamObj.GetComponent<CinemachineVirtualCamera>();
        thirdPersonCamObj = GameObject.Find("ThirdPersonCam");
        thirdPersonCam = thirdPersonCamObj.GetComponent<CinemachineFreeLook>();
        thirdPersonCam.Follow = player.transform;
        thirdPersonCam.LookAt = player.transform;

        // 처음에는 3인칭카메라로 시작할 것이기 때문에 1인칭카메라를 비활성화시켜줍니다.
        firstPersonCam.gameObject.SetActive(false);
        thirdPersonCam.gameObject.SetActive(true);
        _isCurrentFp = false;
    }
    private void Update(){
        // 키보드 G 키를 누르면 1인칭과 3인칭을 전환합니다.
        if(Input.GetKeyDown(KeyCode.G)){
            if(_isCurrentFp){
                _isCurrentFp = !_isCurrentFp;
            }
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
        if(Input.mouseScrollDelta.y != 0 && !_isCurrentFp){
            Zoom();
        }
	}
    // CamChange에서 1인칭과 3인칭카메라를 변환합니다.
    private void CamChange(){
        if(thirdPersonCam.gameObject.activeSelf){
            firstPersonCam.gameObject.SetActive(true);
            thirdPersonCam.gameObject.SetActive(false);
        }
        else{
            firstPersonCam.gameObject.SetActive(false);
            thirdPersonCam.gameObject.SetActive(true);
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
