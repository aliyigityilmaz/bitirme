using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.SceneManagement;

public class ESCMenuManager : MonoBehaviour
{
    public static ESCMenuManager Instance;

    [Header("Main Panels")]
    public GameObject menuPanel; // ESC paneli

    [Header("UI Screens")]
    public GameObject backpackUI;
    public GameObject craftUI;
    public GameObject settingsUI;
    public GameObject timeUI;
   // public GameObject characterUI;

    private bool isESCMenuOpen = false;
    
    public bool menuOpen = false;



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject); // Ayný objeden birden fazla olmasýn diye
        }
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

        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleUI(craftUI);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //ToggleUI(characterUI);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleUI(timeUI);
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

    public void OpenESCMenu()
    {
        menuPanel.SetActive(true);
        isESCMenuOpen = true;
        menuOpen = true;
        CameraManager.Instance.ZoomIn(); // Zoom out
    }


    public void CloseESCMenu()
    {
        menuPanel.SetActive(false);
        isESCMenuOpen = false;
        menuOpen = false;
        CameraManager.Instance.ZoomOut(); // Zoom out
    }


    public void ToggleUI(GameObject targetUI)
    {
        bool alreadyOpen = targetUI.activeSelf;
        bool anyUIWasOpen = backpackUI.activeSelf || craftUI.activeSelf || settingsUI.activeSelf /*|| characterUI.activeSelf*/;

        // ESC menüsü açýksa onu kapat (çünkü ESC menüsü dýþý UI açýlýyor)
        if (isESCMenuOpen)
        {
            CloseESCMenu(); // Bu sadece ESC panelini kapatýyor
        }

        // Eðer zaten bir UI açýksa kamera zoomu bozmadan diðer panelleri kapat
        if (anyUIWasOpen)
        {
            backpackUI.SetActive(false);
            craftUI.SetActive(false);
            settingsUI.SetActive(false);
            timeUI.SetActive(false);
            //characterUI.SetActive(false);
        }
        else
        {
            CloseAllPanels(); // Bu zoom out da yapar
        }

        if (!alreadyOpen)
        {
            targetUI.SetActive(true);
            if (!anyUIWasOpen)
            {
                CameraManager.Instance.ZoomIn(); // Sadece ilk UI açýlýyorsa zoom yap
            }
            menuOpen = true;
            Debug.Log("UI açýldý: " + targetUI.name);
        }
        else
        {
            CameraManager.Instance.ZoomOut();
            menuOpen = false;
            Debug.Log("UI kapatýldý: " + targetUI.name);
        }
    }



    public void OpenUI(GameObject uiToOpen)
    {
        CloseAllPanels();
        uiToOpen.SetActive(true);
        CameraManager.Instance.ZoomIn();
        menuOpen = true;
        Debug.Log("ESC menu opened/closed");
    }

    public void CloseAllPanels()
    {
        CloseESCMenu();
        backpackUI.SetActive(false);
        craftUI.SetActive(false);
        settingsUI.SetActive(false);
        timeUI.SetActive(false);
        //characterUI.SetActive(false);
        CameraManager.Instance.ZoomOut(); // Zoom out
        menuOpen = false;
        Debug.Log("ESC menu opened/closed");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    // Inspector'dan ESC menüsündeki butonlara baðlanabilir
    public void OnBackpackButton() => OpenUI(backpackUI);
    public void OnCraftButton() => OpenUI(craftUI);
    public void OnSettingsButton() => OpenUI(settingsUI);
    public void OnTimeButton() => OpenUI(timeUI);
    //public void OnCharacterButton() => OpenUI(characterUI);
}
