using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlarmAlertPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _alertText;
    public TMP_Text AlarmText { get { return _alertText; } set { _alertText = value; } }

    [SerializeField]
    private AudioSource _audioSource;

    // 알람 시작 시
    private void OnEnable()
    {
        StartCoroutine(WaitForAutoClose());
        _audioSource.Play();
    }

    public void OnClick_Close()
    {
        Close();
    }

    private void Close()
    {
        _audioSource.Stop();
        Destroy(gameObject);
    }

    // 알람 시작 후 일정시간이 지나면 자동으로 종료시키는 코루틴
    IEnumerator WaitForAutoClose()
    {
        yield return new WaitForSeconds(20);
        Close();
    }
}
