using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : MonoBehaviour
{
    public Text buttonText;

    public int nextNode;

    public string response;

    public OptionPanel optionPanel;

    public void Advance() {
        optionPanel.Advance(nextNode, response);
    }
}
