using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarFaceControl_Sol : MonoBehaviour
{
	// External Reference
	[SerializeField] Material _avatarFace;
	[SerializeField] Texture _defaultTexture;

	// private member variables
	bool _crRunning = false;
	IEnumerator _coroutine;

	private void Start() => _avatarFace.SetTexture("_MainTex", _defaultTexture);
    private void OnApplicationQuit() => _avatarFace.SetTexture("_MainTex", _defaultTexture);

	void Update()
	{
		if (Input.GetKey(KeyCode.Alpha1))
			ShowFace(QuickSlotManager_Sol.s_quickSlots[0].fid);
		if (Input.GetKey(KeyCode.Alpha2))
			ShowFace(QuickSlotManager_Sol.s_quickSlots[1].fid);
		if (Input.GetKey(KeyCode.Alpha3))
			ShowFace(QuickSlotManager_Sol.s_quickSlots[2].fid);
		if (Input.GetKey(KeyCode.Alpha4))
			ShowFace(QuickSlotManager_Sol.s_quickSlots[3].fid);
	}


    public void ChangeFace(int faceIndex) => _avatarFace.SetTexture("_MainTex", QuickSlotManager_Sol.s_faceTextures[faceIndex]);

	void ShowFace(int index)
	{
		if (_crRunning && _coroutine != null)
		{
			StopCoroutine(_coroutine);
		}

		_coroutine = ShowFaceCoroutine(index);
		StartCoroutine(_coroutine);
	}

	IEnumerator ShowFaceCoroutine(int index)
	{
		_crRunning = true;

		// ǥ�� ����
		ChangeFace(index);

		// 10�� ���
		yield return new WaitForSeconds(10f);

		// ǥ���� �ٽ� default�� �ٲٱ� (Start���� fid�� �޾ƿ��� ����� ����)
		ChangeFace(11);

		_crRunning = false;
	}
}