using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public Transform recipeListParent;        // Sol sütun
    public GameObject recipeButtonPrefab;     // Prefab: isim + icon
    public GameObject ingredientRowPrefab;    // Saðdaki malzeme listesi (icon + adet)

    public Image resultIcon;
    public TextMeshProUGUI resultName;
    public Button craftButton;
    public Transform ingredientParent;

    public List<RecipeData> recipes;

    private RecipeData selectedRecipe;

    private void Start()
    {
        PopulateRecipeList();
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

            var btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => ShowRecipe(recipe));
        }
    }

    void ShowRecipe(RecipeData recipe)
    {
        selectedRecipe = recipe;

        resultIcon.sprite = recipe.resultIcon;
        resultName.text = recipe.resultItemName;

        foreach (Transform child in ingredientParent)
            Destroy(child.gameObject);

        bool canCraft = true;

        foreach (var ing in recipe.ingredients)
        {
            GameObject row = Instantiate(ingredientRowPrefab, ingredientParent);
            row.transform.Find("Icon").GetComponent<Image>().sprite = ing.item.icon;
            row.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"{ing.item.itemName} x{ing.quantity}";

            if (!BackpackManager.Instance.CheckItem(ing.item.itemName)) // sadece varlýk kontrolü
                canCraft = false;
        }

        craftButton.interactable = canCraft;
        craftButton.onClick.RemoveAllListeners();
        craftButton.onClick.AddListener(() => CraftItem());
    }

    void CraftItem()
    {
        if (selectedRecipe == null) return;

        foreach (var ing in selectedRecipe.ingredients)
        {
            for (int i = 0; i < ing.quantity; i++)
            {
                if (!BackpackManager.Instance.RemoveItem(ing.item.itemName))
                {
                    Debug.LogWarning("Craft iþlemi yarýda kaldý, yeterli malzeme yok!");
                    return;
                }
            }
        }

        BackpackManager.Instance.AddItem(selectedRecipe.resultItem);
        ShowRecipe(selectedRecipe); // UI'yý güncelle
    }
}
