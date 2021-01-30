using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BoxItem[] items;

    public ItemPanelController itemPanelController;

    public void Start()
    {
        // dummy code that places the first four items into the game
        for (int i = 0; i < 4; i++)
            itemPanelController.AddItem(items[i]);
    }
}
