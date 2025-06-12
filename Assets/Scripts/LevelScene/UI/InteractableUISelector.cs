using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUISelector : MonoBehaviour
{
    public static InteractableUISelector Instance;

    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private List<Button> buttons = new List<Button>();
    private int selectedIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateButtonList();

        if (buttons.Count == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            selectedIndex = Mathf.Max(0, selectedIndex - 1);
            UpdateHighlight();
        }
        else if (scroll < 0f)
        {
            selectedIndex = Mathf.Min(buttons.Count - 1, selectedIndex + 1);
            UpdateHighlight();
        }

        // Not: Buradan artýk E basýmý kaldýrýldý ve PlayerController'a taþýndý.
    }

    void UpdateButtonList()
    {
        buttons.Clear();
        foreach (Transform child in InteractableUIManager.Instance.container)
        {
            Button btn = child.GetComponent<Button>();
            if (btn != null)
                buttons.Add(btn);
        }

        if (buttons.Count > 0 && selectedIndex >= buttons.Count)
        {
            selectedIndex = 0;
        }

        UpdateHighlight();
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i == selectedIndex) ? highlightColor : normalColor;
            buttons[i].colors = colors;
        }
    }

    public void TriggerSelected()
    {
        if (buttons.Count > 0)
        {
            buttons[selectedIndex].onClick.Invoke();
        }
    }
}
