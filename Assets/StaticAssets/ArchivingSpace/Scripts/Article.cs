using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Article", menuName = "Article")]
public class Article : ScriptableObject
{
    public string title;
    public string publisher;
    public Sprite thumbnail;
    public Sprite content;
}
