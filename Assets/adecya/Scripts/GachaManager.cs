using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using rarity = RarityClass.Rarity;

public class GachaManager : MonoBehaviour
{
    public enum GachaType
    {
        CharacterWish,
        WeaponWish,
        StandartWish
    }

    private GachaType currentGachaType;

    [Header("Gacha List")]
    [SerializeField] private CharacterSO[] characterList;
    [SerializeField] private WeaponSO[] weaponList;
    private List<object> standartList;
    private int gachaCount;

    [Header("Chance For Wishing")]
    [SerializeField] private WishingChanceSO standartWishingChance;
    [SerializeField] private WishingChanceSO specialWishingChance;

    [Header("UI")]
    [SerializeField] private GameObject resultDisplayUI;
    [SerializeField] private GameObject gachaResultUI;
    [SerializeField] private Button wish1xBtn;
    [SerializeField] private Button wish10xBtn;

    [Header("Script")]
    [SerializeField] private Character character;
    [SerializeField] private Weapon weapon;
    [SerializeField] private SimpleScrollSnap simpleSnap;

    private void Start()
    {
        standartList = new List<object>();
        standartList.AddRange(characterList);
        standartList.AddRange(weaponList);

        ChangeGachaType();
    }

    public void PerformGacha(int gachaCount)
    {
        this.gachaCount = gachaCount;

        switch(currentGachaType)
        {
            case GachaType.CharacterWish:
                Gacha(characterList);
                break;

            case GachaType.WeaponWish:
                Gacha(weaponList);
                break;

            case GachaType.StandartWish:
                Gacha(standartList.ToArray());
                break;
        }        
    }

    public void ShowNextGacha()
    {
        switch (currentGachaType)
        {
            case GachaType.CharacterWish:
                if (gachaCount > 0)
                {
                    Gacha(characterList);
                }
                else
                {
                    resultDisplayUI.SetActive(false);
                    gachaResultUI.SetActive(false);
                }
                break;

            case GachaType.WeaponWish:
                if (gachaCount > 0)
                {
                    Gacha(weaponList);
                }
                else
                {
                    resultDisplayUI.SetActive(false);
                    gachaResultUI.SetActive(false);
                }
                break;

            case GachaType.StandartWish:
                if (gachaCount > 0)
                {
                    Gacha(standartList.ToArray());
                }
                else
                {
                    resultDisplayUI.SetActive(false);
                    gachaResultUI.SetActive(false);
                }
                break;
        }
    }

    public void Gacha<T>(T[] gachaList)
    {
        float totalChance = 0f;
        float commonChance, rareChance, epicChance, legendaryChance;
        resultDisplayUI.SetActive(false);

        // Hitung total peluang dari semua item
        if (currentGachaType == GachaType.StandartWish)
        {
            commonChance = standartWishingChance.commonChance;
            rareChance = standartWishingChance.rareChance;
            epicChance = standartWishingChance.epicChance;
            legendaryChance = standartWishingChance.legendaryChance;
        }
        else
        {
            commonChance = specialWishingChance.commonChance;
            rareChance = specialWishingChance.rareChance;
            epicChance = specialWishingChance.epicChance;
            legendaryChance = specialWishingChance.legendaryChance;
        }

        foreach (var item in gachaList)
        {
            switch (GetRarity(item))
            {
                case rarity.Common:
                    totalChance += commonChance;
                    break;

                case rarity.Rare:
                    totalChance += rareChance;
                    break;

                case rarity.Epic:
                    totalChance += epicChance;
                    break;

                case rarity.Legendary:
                    totalChance += legendaryChance;
                    break;
            }
        }

        // Mendapatkan nilai acak untuk menentukan item yang didapatkan
        float randomValue = Random.Range(0f, totalChance);

        // Memilih item berdasarkan peluang
        foreach (var item in gachaList)
        {
            switch (GetRarity(item))
            {
                case rarity.Common:
                    if (randomValue <= commonChance)
                    {
                        SetGacha(item);
                        return;
                    }
                    else
                    {
                        randomValue -= commonChance;
                    }
                    break;

                case rarity.Rare:
                    if (randomValue <= rareChance)
                    {
                        SetGacha(item);
                        return;
                    }
                    else
                    {
                        randomValue -= rareChance;
                    }
                    break;

                case rarity.Epic:
                    if (randomValue <= epicChance)
                    {
                        SetGacha(item);
                        return;
                    }
                    else
                    {
                        randomValue -= epicChance;
                    }
                    break;

                case rarity.Legendary:
                    if (randomValue <= legendaryChance)
                    {
                        SetGacha(item);
                        return;
                    }
                    else
                    {
                        randomValue -= legendaryChance;
                    }
                    break;

                default:
                    Debug.LogError("Not Found Gacha");
                    break;
            }
        }
    }

    private rarity GetRarity<T>(T item)
    {
        if(item is CharacterSO)
        {
            return (item as CharacterSO).characterRarity;
        }
        else if(item is WeaponSO)
        {
            return (item as WeaponSO).weaponRarity;
        }
        else
        {
            return rarity.Common;
        }
    }

    private void SetGacha<T>(T item)
    {
        if(item is CharacterSO)
        {
            SetCharacterGacha(item as CharacterSO);
        }
        else if(item is WeaponSO)
        {
            SetWeaponGacha(item as WeaponSO);
        }
        else
        {
            Debug.Log("Item not found");
        }
    }

    private void SetCharacterGacha(CharacterSO characterSO)
    {
        character.ChangeCharacterSO(characterSO);
        character.InitializeCharacter();
        character.InitializeDisplay();
        resultDisplayUI.SetActive(true);
        gachaCount--;
    }

    private void SetWeaponGacha(WeaponSO weaponSO)
    {
        weapon.ChangeWeaponSO(weaponSO);
        weapon.InitializeWeapon();
        weapon.InitializeDisplay();
        resultDisplayUI.SetActive(true);
        gachaCount--;
    }

    public void ChangeGachaType()
    {
        switch (simpleSnap.SelectedPanel)
        {
            case 0:
                currentGachaType = GachaType.CharacterWish;
                break;

            case 1:
                currentGachaType = GachaType.WeaponWish;
                break;

            case 2:
                currentGachaType = GachaType.StandartWish;
                break;

            default:
                currentGachaType = GachaType.StandartWish;
                Debug.LogError("Selected Panel Not Found");
                break;
        }
    }
}
