using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public struct Sequence {
        public BoxItem item;

        public bool isOwner;
    }

    public BoxItem[] items;

    public ItemPanelController itemPanelController;
    public DialogueController dialogueController;

    public GameObject selectionPanel;
    public GameObject interrogationPanel;

    public Sequence[] m_sequence = new Sequence[7];

    private int m_roundIndex = 0;

    private List<int> GetRunItems(int numberOfItems) {
        List<int> runItems = new List<int>();

        // get list of all item indexes
        List<int> itemIndexes = new List<int>();
        for (int i = 0; i < items.Length; i++) itemIndexes.Add(i);

        // generate list of items for this run
        for (int i = 0; i < numberOfItems; i++) {
            int index = Random.Range(0, itemIndexes.Count);
            runItems.Add(itemIndexes[index]);
            itemIndexes.RemoveAt(index);
        }

        return runItems;
    }

    public void Start() {
        BuildRun();
    }

    private void BuildRun() {
        // get the 7 items for these runs
        int[] m_runItems = GetRunItems(7).ToArray();
        
        // the indexes for the items from this run
        List<int> runIndexes = new List<int>(){ 0, 1, 2, 3 };

        // select the item that is duplicated
        int duplicateIndex = Random.Range(0, runIndexes.Count);
        int duplicate = runIndexes[duplicateIndex];
        runIndexes.RemoveAt(duplicateIndex);

        // pick the runIndexes for the duplicate pair
        int dLieIndex = Random.Range(0, 2);
        int dTruthIndex = dLieIndex + Random.Range(1, 3);

        // create the first run
        for (int i = 0; i < 5; i++) {
            Sequence sequence = new Sequence();

            if (i == dLieIndex) {
                sequence.item = items[m_runItems[duplicate]];
                sequence.isOwner = false;
            } else if (i == dTruthIndex) {
                sequence.item = items[m_runItems[duplicate]];
                sequence.isOwner = true;
            } else {
                // pick an item
                int index = Random.Range(0, runIndexes.Count);
                int value = runIndexes[index];
                runIndexes.RemoveAt(index);

                // create the sequence
                sequence.item = items[m_runItems[value]];
                sequence.isOwner = true;
            }

            m_sequence[i] = sequence;
        }

        // flip one truth to be a lie
        int flipIndex = Random.Range(0, 5);
        while (flipIndex == dLieIndex || flipIndex == dTruthIndex)
            flipIndex = Random.Range(0, 5);
        
        m_sequence[flipIndex].isOwner = false;

        // TODO: add in random order
        for (int i = 0; i < 5; i++) {
            // skip the duplicate
            if (i == dLieIndex) continue;

            itemPanelController.AddItem(m_sequence[i].item);
        }
    }

    // proceeds to the next round
    public void Proceed() {
        m_roundIndex++;

        dialogueController.RunGraph(GetSequenceGraph(m_roundIndex));
        dialogueController.SetArt(GetCharacter(m_roundIndex));
    }

    // starts the game
    public void Begin() {
        m_roundIndex = 0;

        dialogueController.RunGraph(GetSequenceGraph(m_roundIndex));
        dialogueController.SetArt(GetCharacter(m_roundIndex));

        selectionPanel.SetActive(false);
        interrogationPanel.SetActive(true);
    }

    // gets a graph for the sequence item requested
    private DialogueGraph GetSequenceGraph(int index) {
        Sequence seq = m_sequence[index];

        // get graph list
        DialogueGraph[] graphs;
        if (seq.isOwner) graphs = seq.item.ownerGraphs;
        else graphs = seq.item.liarGraphs;

        // pick one and return
        return graphs[Random.Range(0, graphs.Length)];
    }

    // gets a person art for the sequence item requested
    private Sprite GetCharacter(int index) {
        Sequence seq = m_sequence[index];

        // pick the character art list
        Sprite[] characters;
        if (seq.isOwner) characters = seq.item.ownerCharacters;
        else characters = seq.item.liarCharacters;

        // pick a character and return
        return characters[Random.Range(0, characters.Length)];
    }
}
