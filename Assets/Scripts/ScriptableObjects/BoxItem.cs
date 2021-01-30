using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BoxItem", order = 1)]
public class BoxItem : ScriptableObject
{
    public Sprite big_image;
    public Sprite small_image;

    public string description;

    public DialogueGraph[] ownerGraphs;
    public DialogueGraph[] lierGraphs;
}
