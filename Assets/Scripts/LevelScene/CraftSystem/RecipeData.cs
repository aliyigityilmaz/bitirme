using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class RecipeData : ScriptableObject
{
    public string resultItemName;
    public Sprite resultIcon;
    public InventoryItemData resultItem;

    [System.Serializable]
    public class Ingredient
    {
        public InventoryItemData item;
        public int quantity;
    }

    public Ingredient[] ingredients;
}
