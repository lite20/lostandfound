using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueGraph", order = 1)]
public class DialogueGraph : ScriptableObject
{
    [System.Serializable]
    public struct Option {
        public string optionText;
        public string responseText;

        public int nextNode;
    }

    [System.Serializable]
    public struct Node {
        public bool isDialogue;

        public Option[] options;

        public string dialogue;

        public int nextNode;
    }

    public int startingNode = 0;

    public Node[] nodes;

    public string givePhrase;
    public string rejectPhrase;
}
