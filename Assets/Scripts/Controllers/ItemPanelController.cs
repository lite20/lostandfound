using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelController : MonoBehaviour
{
    // container around the list of items
    public GameObject itemContainer;

    // container around expanded item view
    public GameObject expandContainer;

    // the button for continuing
    public GameObject continueButton;

    // prefab of the item list
    public GameObject itemPrefab;

    public Image expandImage;

    public Text expandText;

    public void AddItem(BoxItem item) {
        // instantiate item prefab
        GameObject go = Instantiate(itemPrefab, itemContainer.transform);

        // set the small icon
        go.transform.GetChild(0).GetComponent<Image>().sprite = item.smallImage;

        // set the on press action
        go.GetComponent<Button>().onClick.AddListener(() => Expand(item));
    }

    private void Expand(BoxItem item) {
        // hide the items and show the expansion container
        itemContainer.SetActive(false);
        continueButton.SetActive(false);
        expandContainer.SetActive(true);

        // update the text
        expandText.text = item.description;

        // update the art
        expandImage.sprite = item.bigImage;
    }
}
