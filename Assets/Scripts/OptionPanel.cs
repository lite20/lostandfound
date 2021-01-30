using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public DialogueController controller;

    public GameObject optionPrefab;

    // reference to this option panels UI
    public GameObject panel;

    public void Set(DialogueGraph.Option[] options) {
        // Destroy all children
        foreach (Transform child in panel.transform)
            GameObject.Destroy(child.gameObject);

        // Create all new options
        foreach (DialogueGraph.Option optionNode in options) {
            GameObject go = Instantiate(optionPrefab, panel.transform);

            // set the text
            go.GetComponent<UIOption>().buttonText.text = optionNode.optionText;
            
            // set the reference to the panel
            go.GetComponent<UIOption>().optionPanel = this;

            // set the response
            go.GetComponent<UIOption>().response = optionNode.responseText;

            // set the next node
            go.GetComponent<UIOption>().nextNode = optionNode.nextNode;
        }
    }

    public void Advance(int node, string response) {
        controller.RespondThenAdvance(node, response);
    }

    public void Show(bool shown) {
        panel.SetActive(shown);
    }
}
