using UnityEngine;

public class ESCMenuManager : MonoBehaviour
{
    public static ESCMenuManager Instance;

    [Header("Main Panels")]
    public GameObject menuPanel; // ESC paneli

    [Header("UI Screens")]
    public GameObject backpackUI;
    public GameObject craftUI;
    public GameObject settingsUI;
    public GameObject characterUI;

    private bool isESCMenuOpen = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleESCPress();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleUI(backpackUI);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleUI(craftUI);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleUI(characterUI);
        }
    }

    void HandleESCPress()
    {
        if (isESCMenuOpen)
        {
            CloseESCMenu();
        }
        else
        {
            // Diðer UI'lar açýksa onlarý kapat
            CloseAllPanels();
            OpenESCMenu();
        }
    }

    void OpenESCMenu()
    {
        menuPanel.SetActive(true);
        isESCMenuOpen = true;
        UpdateTimeScale();
    }


    void CloseESCMenu()
    {
        menuPanel.SetActive(false);
        isESCMenuOpen = false;
        UpdateTimeScale();
    }


    public void ToggleUI(GameObject targetUI)
    {
        bool alreadyOpen = targetUI.activeSelf;

        // ESC menüsü açýksa kapat
        if (isESCMenuOpen)
        {
            CloseESCMenu();
        }

        // Diðer UI'larý kapat
        CloseAllPanels();

        if (!alreadyOpen)
        {
            targetUI.SetActive(true);
        }
        UpdateTimeScale();
    }

    public void OpenUI(GameObject uiToOpen)
    {
        CloseAllPanels();
        uiToOpen.SetActive(true);
        Time.timeScale = 1f;
    }

    private void CloseAllPanels()
    {
        backpackUI.SetActive(false);
        craftUI.SetActive(false);
        settingsUI.SetActive(false);
        characterUI.SetActive(false);
        UpdateTimeScale();
    }


    void UpdateTimeScale()
    {
        if (menuPanel.activeSelf || backpackUI.activeSelf || craftUI.activeSelf || settingsUI.activeSelf || characterUI.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }


    // Inspector'dan ESC menüsündeki butonlara baðlanabilir
    public void OnBackpackButton() => OpenUI(backpackUI);
    public void OnCraftButton() => OpenUI(craftUI);
    public void OnSettingsButton() => OpenUI(settingsUI);
    public void OnCharacterButton() => OpenUI(characterUI);
}
