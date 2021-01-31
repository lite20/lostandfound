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

    public void RunGraph(DialogueGraph graph) {
        m_graph = graph;
        PresentNode(m_graph.startingNode);
    }

    public void PresentNode(int nodeID) {
        DialogueGraph.Node node = m_graph.nodes[nodeID];

        // show the appropriate panel
        dialoguePanel.Show(node.isDialogue);
        optionPanel.Show(!node.isDialogue);

        // populate the appropriate panel data
        if (node.isDialogue)
            dialoguePanel.Set(node.dialogue, node.nextNode);
        else
            optionPanel.Set(node.options);
    }

    public void RespondThenAdvance(int nodeID, string response) {
        // present
        dialoguePanel.Set(response, nodeID);

        // set the response node next node
        dialoguePanel.Show(true);
        optionPanel.Show(false);
    }

    public void SetArt(Sprite character) {
        characterImage.sprite = character;
    }

    public void SetItems(string[] items) {
        foreach (string item in items) {
            GameObject btn = Instantiate(
                decisionItemPrefab, decisionContainer.transform);

            btn.transform.GetChild(0).GetComponent<Text>().text = item;
            Button btnObj = btn.GetComponent<Button>();
            btnObj.onClick.AddListener(delegate {
                GameObject go = GameObject.Find("GameController");
                GameController gc = go.GetComponent<GameController>();
                gc.TryGive(item, btnObj);
            });
        }
    }
}
