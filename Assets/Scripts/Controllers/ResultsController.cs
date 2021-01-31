using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsController : MonoBehaviour
{
    public GameObject resultContainer;

    public GameObject resultPrefab;

    public Sprite correctSprite;
    public Sprite incorrectSprite;

    public Text resultsText;

    // items that we've already graded
    private List<string> m_markedResults = new List<string>();

    public void SetScore(string score) {
        resultsText.text = "Today's Results: " + score;
    }

    public void AddResult(string name, bool correct) {
        // dont allow double entries. (they'd be the same, anyway)
        if (m_markedResults.Contains(name)) return;
        m_markedResults.Add(name);

        var go = Instantiate(resultPrefab, resultContainer.transform);
        var text = go.transform.GetChild(0).GetComponent<Text>();
        var image = go.transform.GetChild(1).GetComponent<Image>();

        text.text = name;
        image.sprite = correct ? correctSprite : incorrectSprite;
    }

    public void Reset() {
        m_markedResults.Clear();
    }
}
