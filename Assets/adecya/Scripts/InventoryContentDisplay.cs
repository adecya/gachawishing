using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Category = InventoryManager.InventCategory;

public class InventoryContentDisplay : MonoBehaviour
{
    public CharacterSO characterSO;
    public WeaponSO weaponSO;
    public int quantity;
    public Category category;

    [SerializeField] private Image iconImg;
    [SerializeField] private TextMeshProUGUI stockTxt;

    public void SetContent<T>(T item, int stock)
    {
        if(item is CharacterSO)
        {
            characterSO = (item as CharacterSO);
            iconImg.sprite = (item as CharacterSO).characterSprite;
            category = Category.Character;
        }
        else if(item is WeaponSO)
        {
            weaponSO = (item as WeaponSO);
            iconImg.sprite = (item as WeaponSO).weaponSprite;
            category = Category.Weapon;
        }
        else
        {
            Debug.LogError("Inventory Content ERROR!");
        }

        quantity = stock;
    }

    public void UpdateStockTxt()
    {
        stockTxt.text = quantity.ToString();
    }
}
