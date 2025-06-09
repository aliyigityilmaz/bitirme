using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{

    [Header("Sol Liste")]
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;
    public List<RecipeData> recipes;

    [Header("Sað Panel")]
    public Image resultIcon;
    public TextMeshProUGUI resultName;

    [Header("Ingredient 1")]
    public Image ingredient1Icon;
    public TextMeshProUGUI ingredient1Count;

    [Header("Ingredient 2")]
    public Image ingredient2Icon;
    public TextMeshProUGUI ingredient2Count;

    [Header("Craft")]
    public Button craftButton;

    [Header("Sprites")]
    public Sprite CharacterItemBackground;
    public Sprite QuestItemBackground;
    public Sprite CollectibleItemBackground;
    public Sprite CharacterItemBackgroundSelected;
    public Sprite QuestItemBackgroundSelected;
    public Sprite CollectibleItemBackgroundSelected;

    private RecipeData selectedRecipe;
    private GameObject selectedButton;

    private void Start()
    {
        PopulateRecipeList();
        craftButton.onClick.AddListener(CraftItem); // Tek seferlik baðlanýr
        resultName.text = " ";
    }

    void PopulateRecipeList()
    {
        foreach (Transform child in recipeListParent)
            Destroy(child.gameObject);

        foreach (var recipe in recipes)
        {
            GameObject go = Instantiate(recipeButtonPrefab, recipeListParent);
            go.transform.Find("Icon").GetComponent<Image>().sprite = recipe.resultIcon;
            go.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = recipe.resultItemName;

            Image bgImage = go.GetComponent<Image>();
            if (recipe.resultItem.type == ItemType.QuestItem)
                bgImage.sprite = QuestItemBackground;
            else if (recipe.resultItem.type == ItemType.CharacterItem)
                bgImage.sprite = CharacterItemBackground;
            else if (recipe.resultItem.type == ItemType.Consumable)
                bgImage.sprite = QuestItemBackground;
            else if (recipe.resultItem.type == ItemType.Collectible)
                bgImage.sprite = CollectibleItemBackground;
            else if (recipe.resultItem.type == ItemType.MobDrop)
                bgImage.sprite = CollectibleItemBackground;

            var btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => ShowRecipe(recipe, go)); // Butonu da gönderiyoruz
        }
    }

    void ShowRecipe(RecipeData recipe, GameObject buttonGO)
    {
        
        selectedRecipe = recipe;

        if (selectedButton != null)
        {
            RecipeData prevRecipe = recipes.Find(r => r == selectedRecipe);
            Image prevImage = selectedButton.GetComponent<Image>();

            if (recipe.resultItem.type == ItemType.QuestItem)
                prevImage.sprite = QuestItemBackground;
            else if (recipe.resultItem.type == ItemType.CharacterItem)
                prevImage.sprite = CharacterItemBackground;
            else if (recipe.resultItem.type == ItemType.Consumable)
                prevImage.sprite = QuestItemBackground;
            else if (recipe.resultItem.type == ItemType.Collectible)
                prevImage.sprite = CollectibleItemBackground;
            else if (recipe.resultItem.type == ItemType.MobDrop)
                prevImage.sprite = CollectibleItemBackground;
            selectedButton.transform.Find("Name").GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        Image newImage = buttonGO.GetComponent<Image>();
        if (recipe.resultItem.type == ItemType.QuestItem)
            newImage.sprite = QuestItemBackgroundSelected;
        else if (recipe.resultItem.type == ItemType.CharacterItem)
            newImage.sprite = CharacterItemBackgroundSelected;
        else if (recipe.resultItem.type == ItemType.Collectible)
            newImage.sprite = CollectibleItemBackgroundSelected;
        else if (recipe.resultItem.type == ItemType.Consumable)
            newImage.sprite = QuestItemBackgroundSelected;
        else if (recipe.resultItem.type == ItemType.MobDrop)
            newImage.sprite = CollectibleItemBackgroundSelected;
        selectedButton = buttonGO;
        selectedButton.transform.Find("Name").GetComponent<TextMeshProUGUI>().color = Color.black;

        resultIcon.sprite = recipe.resultIcon;
        resultName.text = recipe.resultItemName;

       

        // Ingredient 1
        if (recipe.ingredients.Length > 0)
        {
            ingredient1Icon.sprite = recipe.ingredients[0].item.icon;
            ingredient1Count.text = "x" + recipe.ingredients[0].quantity;
            ingredient1Count.gameObject.SetActive(true);
            ingredient1Icon.gameObject.SetActive(true);
        }
        else
        {

            ingredient1Count.gameObject.SetActive(false);
            ingredient1Icon.gameObject.SetActive(false);
        }

        // Ingredient 2
        if (recipe.ingredients.Length > 1)
        {
            ingredient2Icon.sprite = recipe.ingredients[1].item.icon;
            ingredient2Count.text = "x" + recipe.ingredients[1].quantity;
            ingredient2Count.gameObject.SetActive(true);
            ingredient2Icon.gameObject.SetActive(true);
        }
        else
        {

            ingredient2Count.gameObject.SetActive(false);
            ingredient2Icon.gameObject.SetActive(false);
        }

        // Craft tuþu aktif mi?
        bool canCraft = true;
        foreach (var ing in recipe.ingredients)
        {
            int playerCount = BackpackManager.Instance.GetItemCount(ing.item);
            if (playerCount < ing.quantity)
                canCraft = false;
        }

        craftButton.interactable = canCraft;
    }

    void CraftItem()
    {
        if (selectedRecipe == null)
        {
            Debug.LogWarning("Hiçbir tarif seçilmedi.");
            return;
        }

        // Malzeme kontrolü
        foreach (var ing in selectedRecipe.ingredients)
        {
            if (BackpackManager.Instance.GetItemCount(ing.item) < ing.quantity)
            {
                Debug.LogWarning("Craft baþarýsýz, yeterli malzeme yok.");
                return;
            }
        }

        // Malzemeleri sil
        foreach (var ing in selectedRecipe.ingredients)
        {
            for (int i = 0; i < ing.quantity; i++)
            {
                if (!BackpackManager.Instance.RemoveItem(ing.item))
                {
                    Debug.LogWarning("Beklenmeyen hata: Malzeme silinemedi.");
                    return;
                }
            }
        }

        // Üretilen itemi envantere ekle
        BackpackManager.Instance.AddItem(selectedRecipe.resultItem);

        // UI’ý güncelle (craft tuþu aktif/pasif vs.)
        ShowRecipe(selectedRecipe, selectedButton);

    }

}
