using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public OptionPanel optionPanel;

    public DialoguePanel dialoguePanel;

    public DialogueGraph m_graph;

    public GameObject decisionContainer;

    public GameObject decisionItemPrefab;

    public Image characterImage;

    private int m_currentNode = 0;

    public List<string> disabled = new List<string>();

    public void RunGraph(DialogueGraph graph) {
        m_graph = graph;
        m_currentNode = m_graph.startingNode;
        PresentNode(m_graph.startingNode);
        disabled.Clear();
    }

    public void PresentNode(int nodeID) {
        m_currentNode = nodeID;

        DialogueGraph.Node node = m_graph.nodes[nodeID];

        // show the appropriate panel
        dialoguePanel.Show(node.isDialogue);
        optionPanel.Show(!node.isDialogue);

        // populate the appropriate panel data
        if (node.isDialogue)
            dialoguePanel.Set(node.dialogue, node.nextNode);
        else {
            if (m_currentNode == 0) 
                optionPanel.Set(node.options, disabled);
            else 
                optionPanel.Set(node.options, null);
        }
    }

    public void RespondThenAdvance(int nodeID, string response, int offset) {
        // disable buttons that have been pressed (from root node 0)
        if (m_currentNode == 0)
            disabled.Add(offset + "");

        // present
        dialoguePanel.Set(response, nodeID);

        // set the response node next node
        dialoguePanel.Show(true);
        optionPanel.Show(false);
        Debug.Log("Killed");
    }

    public void SetArt(Sprite character) {
        characterImage.sprite = character;
    }

    public void SetItems(string[] items) {
        for (int i = 0; i < items.Length; i++) {
            string item = items[i];

            // create button
            GameObject btn = Instantiate(
                decisionItemPrefab, decisionContainer.transform);

            // set properties
            btn.transform.GetChild(0).GetComponent<Text>().text = item;

            // apply listener to button
            Button btnObj = btn.GetComponent<Button>();
            btnObj.onClick.AddListener(delegate {
                GameObject go = GameObject.Find("GameController");
                GameController gc = go.GetComponent<GameController>();
                gc.TryGive(item, btnObj);
            });
        }
    }
}
