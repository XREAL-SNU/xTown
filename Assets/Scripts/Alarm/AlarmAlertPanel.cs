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

    IEnumerator WaitForAutoClose()
    {
        yield return new WaitForSeconds(20);
        Close();
    }
}
