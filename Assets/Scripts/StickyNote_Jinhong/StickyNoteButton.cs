using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNoteButton : MonoBehaviour
{
    public void CreateStickyNote()
    {
        Instantiate(Resources.Load("StickyNote/StickyNotePrefab"), Vector3.zero, Quaternion.identity);
    }
}
