using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BoxItem", order = 1)]
public class BoxItem : ScriptableObject
{
    public Sprite bigImage;
    public Sprite smallImage;

    public Sprite[] ownerCharacters;
    public Sprite[] liarCharacters;

    public string description;
    public string iName;

    public DialogueGraph[] ownerGraphs;
    public DialogueGraph[] liarGraphs;
}
