using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public struct Sequence {
        public BoxItem item;

        public bool isOwner;

        public DialogueGraph graph;
    }

    public UnityEvent onWrongItem;
    public UnityEvent onItemGive;

    public BoxItem[] items;

    public ItemPanelController itemPanelController;
    public DialogueController dialogueController;
    public ResultsController resultsController;

    public GameObject selectionPanel;
    public GameObject interrogationPanel;
    public GameObject resultsPanel;
    public GameObject endingPanel;
    public GameObject decisionButton;
    public GameObject closeDecisionButton;

    public Text endingTextUI;

    public Sequence[] m_sequence = new Sequence[7];

    private int m_roundIndex = 0;

    private int[] m_runItems;

    private List<int> GetRunItems(int numberOfItems) {
        List<int> runItems = new List<int>();

        // get list of all item indexes
        List<int> itemIndexes = new List<int>();
        for (int i = 0; i < items.Length; i++) itemIndexes.Add(i);

        // generate list of items for this run
        for (int i = 0; i < numberOfItems; i++) {
            int index = UnityEngine.Random.Range(0, itemIndexes.Count);
            runItems.Add(itemIndexes[index]);
            itemIndexes.RemoveAt(index);
        }

        return runItems;
    }

    public void Start() {
        if (onWrongItem == null) onWrongItem = new UnityEvent();
        if (onItemGive == null) onItemGive = new UnityEvent();
        BuildRun();
    }

    // starts the game
    public void Begin() {
        m_roundIndex = 0;

        // create graphs and set them
        DialogueGraph graph = GetSequenceGraph(m_roundIndex);
        m_sequence[m_roundIndex].graph = graph;
        dialogueController.RunGraph(graph);
        dialogueController.SetArt(GetCharacter(m_roundIndex));

        // set the items
        var itemsThisRun = new List<string>();
        for(int i = 0; i < 5; i++) {
            string name = m_sequence[i].item.name;
            if (itemsThisRun.Contains(name)) continue;
            else itemsThisRun.Add(name);
        }

        dialogueController.SetItems(itemsThisRun.ToArray());

        // toggle the panels
        selectionPanel.SetActive(false);
        interrogationPanel.SetActive(true);
    }
    
    // proceeds to the next round
    public void Proceed() {
        decisionButton.SetActive(true);

        if (m_roundIndex == 4) {
            interrogationPanel.SetActive(false);
            resultsPanel.SetActive(true);

            return;
        }

        m_roundIndex++;

        DialogueGraph graph = GetSequenceGraph(m_roundIndex);
        m_sequence[m_roundIndex].graph = graph;
        dialogueController.RunGraph(graph);
        dialogueController.SetArt(GetCharacter(m_roundIndex));
    }

    private void BuildRun() {
        // get the 7 items for these runs
        m_runItems = GetRunItems(7).ToArray();
        
        // the indexes for the items from this run
        List<int> runIndexes = new List<int>(){ 0, 1, 2, 3 };

        // select the item that is duplicated
        int duplicateIndex = UnityEngine.Random.Range(0, runIndexes.Count);
        int duplicate = runIndexes[duplicateIndex];
        runIndexes.RemoveAt(duplicateIndex);

        // pick the runIndexes for the duplicate pair
        int dLieIndex = UnityEngine.Random.Range(0, 2);
        int dTruthIndex = dLieIndex + UnityEngine.Random.Range(1, 3);

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
                int index = UnityEngine.Random.Range(0, runIndexes.Count);
                int value = runIndexes[index];
                runIndexes.RemoveAt(index);

                // create the sequence
                sequence.item = items[m_runItems[value]];
                sequence.isOwner = true;
            }

            m_sequence[i] = sequence;
        }

        // flip one truth to be a lie
        int flipIndex = UnityEngine.Random.Range(0, 5);
        while (flipIndex == dLieIndex || flipIndex == dTruthIndex)
            flipIndex = UnityEngine.Random.Range(0, 5);
        
        m_sequence[flipIndex].isOwner = false;

        // TODO: add in random order
        for (int i = 0; i < 5; i++) {
            // skip the duplicate
            if (i == dLieIndex) continue;

            itemPanelController.AddItem(m_sequence[i].item);
        }
    }

    public void TryGive(string name, Button btn) {
        closeDecisionButton.SetActive(false);
        if (!m_sequence[m_roundIndex].isOwner) {
            // give the item
            Give(btn);

            // add to results as incorrect
            resultsController.AddResult(name, false);
        } else {
            if (name == m_sequence[m_roundIndex].item.name) {
                // give the item
                Give(btn);

                // add to results as correct
                resultsController.AddResult(name, true);
            } else WrongItem();
        }
    }

    private void Give(Button btn) {
        // disable button
        btn.interactable = false;

        // invoke event
        onItemGive.Invoke();

        // set confirmation text
        endingTextUI.text = m_sequence[m_roundIndex].graph.givePhrase;

        // show confirmation panel
        endingPanel.SetActive(true);
    }

    private void WrongItem() {
        onWrongItem.Invoke();

        // show wrong item text
        dialogueController.RespondThenAdvance(
            0, "No, that's not it!", -1
        );
    }

    public void GiveNothing() {
        // add to results as incorrect
        if (m_sequence[m_roundIndex].isOwner)
            resultsController.AddResult(m_sequence[m_roundIndex].item.name, false);
        
        // add to results as correct
        else resultsController.AddResult(m_sequence[m_roundIndex].item.name, true);
        
        // invoke event
        onItemGive.Invoke();

        // set rejection text
        endingTextUI.text = m_sequence[m_roundIndex].graph.rejectPhrase;
        
        // show rejection panel
        endingPanel.SetActive(true);
    }

    // gets a graph for the sequence item requested
    private DialogueGraph GetSequenceGraph(int index) {
        Sequence seq = m_sequence[index];

        // get graph list
        DialogueGraph[] graphs;
        if (seq.isOwner) graphs = seq.item.ownerGraphs;
        else graphs = seq.item.liarGraphs;

        // pick one and return
        return graphs[UnityEngine.Random.Range(0, graphs.Length)];
    }

    // gets a person art for the sequence item requested
    private Sprite GetCharacter(int index) {
        Sequence seq = m_sequence[index];

        // pick the character art list
        Sprite[] characters;
        if (seq.isOwner) characters = seq.item.ownerCharacters;
        else characters = seq.item.liarCharacters;

        // pick a character and return
        return characters[UnityEngine.Random.Range(0, characters.Length)];
    }
}
