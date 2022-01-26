using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNoteButton : MonoBehaviour
{
    public GameObject stickyNotePrefab;

    public void CreateStickyNote()
    {
        Instantiate(stickyNotePrefab, Vector3.zero, Quaternion.identity);
    }
}
