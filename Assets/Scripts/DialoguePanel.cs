using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    public GameObject panel;

    public Text textUI;

    public DialogueController controller;

    private int m_nextNode;

    public void Set(string text, int node)  {
        textUI.text = text;
        m_nextNode = node;
    }

    public void Advance() {
        controller.PresentNode(m_nextNode);
    }

    public void Show(bool shown) {
        panel.SetActive(shown);
    }
}
