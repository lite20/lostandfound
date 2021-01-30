using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public OptionPanel optionPanel;

    public DialoguePanel dialoguePanel;

    public DialogueGraph m_graph;

    public Image characterImage;

    public void RunGraph(DialogueGraph graph) {
        m_graph = graph;
        PresentNode(m_graph.startingNode);
    }

    public void PresentNode(int nodeID) {
        Debug.Log("Next node is: " + nodeID);
        
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
}
