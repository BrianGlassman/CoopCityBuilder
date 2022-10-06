using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSelect : MonoBehaviour
{
    public int HAND_SIZE = 3;
    [SerializeField] GameObject cardPrefab; // Prefab for a card button
    [SerializeField] GameObject handPanel; // The UI panel to create the cards inside of

    /// <summary>
    /// Creates the buttons for the hand
    /// </summary>
    private void Awake()
    {
        for (int i = 0; i < HAND_SIZE; i++)
        {
            GameObject button = GameObject.Instantiate(
                cardPrefab,
                handPanel.transform);
            Button b = button.GetComponent<Button>();
            OnClick(b, i);
        }
    }

    // Note: has to be defined separately for scoping reasons
    private void OnClick(Button b, int i)
    {
        b.onClick.AddListener(() => print("here " + i.ToString()));
    }
}
