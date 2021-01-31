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

    public void Set(DialogueGraph.Option[] options, List<string> disabled) {
        // Destroy all children
        foreach (Transform child in panel.transform)
            GameObject.Destroy(child.gameObject);

        // Create all new options
        for (int i = 0; i < options.Length; i++) {
            DialogueGraph.Option optionNode = options[i];

            GameObject go = Instantiate(optionPrefab, panel.transform);

            // disable it if it should be
            if (disabled != null && disabled.Contains(i + ""))
                go.GetComponent<Button>().interactable = false;

            // set the text
            go.GetComponent<UIOption>().buttonText.text = optionNode.optionText;
            
            // set the reference to the panel
            go.GetComponent<UIOption>().optionPanel = this;

            // set the offset/index of the item in the list
            go.GetComponent<UIOption>().offset = i;

            // set the response
            go.GetComponent<UIOption>().response = optionNode.responseText;

            // set the next node
            go.GetComponent<UIOption>().nextNode = optionNode.nextNode;
        }
    }

    public void Advance(int node, string response, int offset) {
        controller.RespondThenAdvance(node, response, offset);
    }

    public void Show(bool shown) {
        panel.SetActive(shown);
    }
}
