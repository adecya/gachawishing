using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public enum InventCategory
    {
        Character,
        Weapon
    }
    private InventCategory currentCategory = InventCategory.Character;

    private Dictionary<object, int> inventoryItems = new Dictionary<object, int>();
    private Dictionary<object, List<GameObject>> itemPools = new Dictionary<object, List<GameObject>>();
    private List<GameObject> itemPool = new List<GameObject>();

    [Header("Inventory UI")]
    [SerializeField] private Transform characterContentTransform;
    [SerializeField] private Transform weaponContentTransform;
    [SerializeField] private GameObject characterCategoryUI;
    [SerializeField] private GameObject weaponCategoryUI;
    [SerializeField] private Image charCategoryBtn;
    [SerializeField] private Image weaponCategoryBtn;

    [Header("Content Prefab")]
    [SerializeField] private GameObject inventoryContentPrefab;

    public static InventoryManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitializeInventory();
        ShowInventory();
    }

    private void InitializeInventory()
    {
        for(int i = 0; i < ItemList.Instance.characterList.Length; i++)
        {
            AddItem(ItemList.Instance.characterList[i]);
        }
        for (int i = 0; i < ItemList.Instance.weaponList.Length; i++)
        {
            AddItem(ItemList.Instance.weaponList[i]);
        }

        PopulatePool();
    }

    public void AddItem<T>(T item)
    {
        if(inventoryItems.ContainsKey(item))
        {
            inventoryItems[item]++;

            if (item is CharacterSO)
            {
                GameObject foundItem = itemPool.Find(obj => obj.GetComponent<InventoryContentDisplay>().characterSO == item as CharacterSO);
                foundItem.GetComponent<InventoryContentDisplay>().quantity++;
            }
            else if (item is WeaponSO)
            {
                GameObject foundItem = itemPool.Find(obj => obj.GetComponent<InventoryContentDisplay>().weaponSO == item as WeaponSO);
                foundItem.GetComponent<InventoryContentDisplay>().quantity++;
            }
        }
        else
        {
            inventoryItems.Add(item, 0);
        }        
    }

    private int GetItemStock<T>(T item)
    {
        if(inventoryItems.ContainsKey(item))
        {
            return inventoryItems[item];
        }
        return 0;
    }

    private void PopulatePool()
    {
        foreach (var kvp in inventoryItems)
        {
            object item = kvp.Key;
            int quantity = kvp.Value;

            if (item is CharacterSO)
            {
                GameObject itemPrefab = Instantiate(inventoryContentPrefab, characterContentTransform);
                InventoryContentDisplay inventoryContentDisplay = itemPrefab.GetComponent<InventoryContentDisplay>();
                inventoryContentDisplay.SetContent(item, quantity);
                itemPool.Add(itemPrefab);
                itemPrefab.SetActive(false);
            }
            else if (item is WeaponSO)
            {
                GameObject itemPrefab = Instantiate(inventoryContentPrefab, weaponContentTransform);
                InventoryContentDisplay inventoryContentDisplay = itemPrefab.GetComponent<InventoryContentDisplay>();
                inventoryContentDisplay.SetContent(item, quantity);
                itemPool.Add(itemPrefab);
                itemPrefab.SetActive(false);
            }
            else
            {
                Debug.LogError("Inventory Manager ERROR on Populate!");
            }
        }
    }

    private void ShowInventory()
    {
        foreach(var kvp in inventoryItems)
        {
            object item = kvp.Key;
            int quantity = kvp.Value;

            if(currentCategory == InventCategory.Character)
            {
                GameObject foundObject = itemPool.Find(obj => obj.GetComponent<InventoryContentDisplay>().characterSO == item as CharacterSO);
                InventoryContentDisplay contentDisplay = foundObject.GetComponent<InventoryContentDisplay>();
                if (contentDisplay.quantity > 0)
                {
                    foundObject.SetActive(true);
                    contentDisplay.UpdateStockTxt();
                }
                else
                {
                    foundObject.SetActive(false);
                }
            }
            else if(currentCategory == InventCategory.Weapon)
            {
                GameObject foundObject = itemPool.Find(obj => obj.GetComponent<InventoryContentDisplay>().weaponSO == item as WeaponSO);
                InventoryContentDisplay contentDisplay = foundObject.GetComponent<InventoryContentDisplay>();
                if (contentDisplay.quantity > 0)
                {
                    foundObject.SetActive(true);
                    contentDisplay.UpdateStockTxt();
                }
                else
                {
                    foundObject.SetActive(false);
                }
            }
        }
    }

    /*index
     * 0 = character
     * 1 = weapon
     */
    public void ChangeCategory(int index)
    {
        characterCategoryUI.SetActive(false);
        weaponCategoryUI.SetActive(false);
        charCategoryBtn.color = Color.black;
        weaponCategoryBtn.color = Color.black;

        switch (index)
        {
            case 0:
                currentCategory = InventCategory.Character;
                characterCategoryUI.SetActive(true);
                charCategoryBtn.color = Color.cyan;
                ShowInventory();
                break;

            case 1:
                currentCategory = InventCategory.Weapon;
                weaponCategoryUI.SetActive(true);
                weaponCategoryBtn.color = Color.magenta;
                ShowInventory();
                break;
        }
    }

}
