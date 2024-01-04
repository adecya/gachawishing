using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] private CharacterSO[] epicOrMoreCharList;
    [SerializeField] private WeaponSO[] weaponList;
    [SerializeField] private WeaponSO[] epicOrMoreWeaponList;
    private List<object> standartList;
    private List<object> gachaTemp;
    private int gachaCount;
    private bool isGacha10X;
    private bool hasEpicOrMore;
    private bool isSkip;
    private static int pityCount = 0;

    [Header("Result 10x Draw Image")]
    [SerializeField] private List<Image> resultObject10XUI;

    [Header("Chance For Wishing")]
    [SerializeField] private WishingChanceSO standartWishingChance;
    [SerializeField] private WishingChanceSO specialWishingChance;

    [Header("UI")]
    [SerializeField] private GameObject resultDisplayUI;
    [SerializeField] private GameObject gachaResultUI;
    [SerializeField] private GameObject resultGacha10XUI;
    [SerializeField] private Button wish1xBtn;
    [SerializeField] private Button wish10xBtn;
    [SerializeField] private GameObject skipBtn;
    [SerializeField] private TextMeshProUGUI pityTxt;
    [Header("Details Gacha UI")]
    [SerializeField] private GameObject detailsStandartWishUI;
    [SerializeField] private GameObject detailsCharacterWishUI;
    [SerializeField] private GameObject detailsWeaponWishUI;

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
        ChangePityText();

        isGacha10X = false;
        hasEpicOrMore = false;
        isSkip = false;

        gachaTemp = new List<object>();
    }

    public void PerformGacha(int gachaCount)
    {
        this.gachaCount = gachaCount;
        if(gachaCount >= 10)
        {
            isGacha10X = true;
            skipBtn.SetActive(true);
        }

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
                if (isGacha10X && !hasEpicOrMore && gachaCount == 1)
                {
                    Gacha(epicOrMoreCharList);
                }
                else
                {
                    if (gachaCount > 0)
                    {
                        Gacha(characterList);
                    }
                    else
                    {
                        if (isGacha10X)
                        {
                            ShowGachaTemp();
                        }
                        else
                        {
                            ExitGacha();
                        }
                    }
                }
                break;

            case GachaType.WeaponWish:
                if (isGacha10X && !hasEpicOrMore && gachaCount == 1)
                {
                    Gacha(epicOrMoreWeaponList);
                }
                else
                {
                    if (gachaCount > 0)
                    {
                        Gacha(weaponList);
                    }
                    else
                    {
                        if (isGacha10X)
                        {
                            ShowGachaTemp();
                        }
                        else
                        {
                            ExitGacha();
                        }
                    }
                }
                break;

            case GachaType.StandartWish:
                if (gachaCount > 0)
                {
                    Gacha(standartList.ToArray());
                }
                else
                {
                    if (isGacha10X)
                    {
                        ShowGachaTemp();
                    }
                    else
                    {
                        ExitGacha();
                    }
                }
                break;
        }
    }

    private void ExitGacha()
    {
        resultDisplayUI.SetActive(false);
        gachaResultUI.SetActive(false);
        resultGacha10XUI.SetActive(false);

        isGacha10X = false;
        hasEpicOrMore = false;
        gachaTemp.Clear();
        skipBtn.SetActive(false);
        isSkip = false;
    }

    private void ShowGachaTemp()
    {
        resultDisplayUI.SetActive(false);
        isSkip = false;
        isGacha10X = false;

        int i = 0;
        var sortedList = gachaTemp.Cast<object>().OrderBy(GetRarityNumber);

        foreach (var item in sortedList)
        {
            if(item is CharacterSO)
            {
                resultObject10XUI[i].sprite = (item as CharacterSO).characterSprite;
            }
            else
            {
                resultObject10XUI[i].sprite = (item as WeaponSO).weaponSprite;
            }
            i++;
        }

        resultGacha10XUI.SetActive(true);
    }

    public void SkipGacha()
    {
        isSkip = true;

        switch (currentGachaType)
        {
            case GachaType.CharacterWish:
                SkipGachaTillEnd(characterList, epicOrMoreCharList);
                break;

            case GachaType.WeaponWish:
                SkipGachaTillEnd(weaponList, epicOrMoreWeaponList);
                break;

            case GachaType.StandartWish:
                SkipGachaTillEnd(standartList.ToArray(), standartList.ToArray());
                break;
        }

        isSkip = false;
        ShowGachaTemp();
    }

    private void SkipGachaTillEnd<T>(T[] gachaList, T[] epicList)
    {
        if(gachaCount > 0)
        {
            for (int i = 0; i < gachaCount;)
            {
                if (hasEpicOrMore)
                {
                    Gacha(gachaList);
                }
                else if(!hasEpicOrMore && gachaCount == 1)
                {
                    Gacha(epicList);
                }
                else
                {
                    Gacha(gachaList);
                }
            }
        }
    }

    private void Gacha<T>(T[] gachaList)
    {
        float totalChance = 0f;
        float commonChance, rareChance, epicChance, legendaryChance;
        pityCount++;
        ChangePityText();
        resultDisplayUI.SetActive(false);

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

        float randomValue = Random.Range(0f, totalChance);

        if (pityCount == 50)
        {
            randomValue = totalChance;
            pityCount = 0;
            ChangePityText();
        }

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
                        hasEpicOrMore = true;
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
                        hasEpicOrMore = true;
                        pityCount = 0;
                        ChangePityText();
                        return;
                    }
                    else
                    {
                        randomValue -= legendaryChance;
                    }
                    break;
            }
        }
    }

    private int GetRarityNumber<T>(T item)
    {
        if (item is CharacterSO)
        {
            return (int)(item as CharacterSO).characterRarity;
        }
        else if (item is WeaponSO)
        {
            return (int)(item as WeaponSO).weaponRarity;
        }
        else
        {
            return 0;
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
            Debug.Log($"Get : {(item as CharacterSO).characterName}");
        }
        else if(item is WeaponSO)
        {
            SetWeaponGacha(item as WeaponSO);
            Debug.Log($"Get : {(item as WeaponSO).weaponName}");
        }
        else
        {
            Debug.Log("Item not found");
        }

        gachaTemp.Add(item);
    }

    private void SetCharacterGacha(CharacterSO characterSO)
    {
        character.ChangeCharacterSO(characterSO);
        character.InitializeCharacter();
        character.InitializeDisplay();
        if (!isSkip)
        {
            resultDisplayUI.SetActive(true);
        }
        gachaCount--;
    }

    private void SetWeaponGacha(WeaponSO weaponSO)
    {
        weapon.ChangeWeaponSO(weaponSO);
        weapon.InitializeWeapon();
        weapon.InitializeDisplay();
        if (!isSkip)
        {
            resultDisplayUI.SetActive(true);
        }
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

    public void OpenDetailsGachaUI()
    {
        switch(currentGachaType)
        {
            case GachaType.CharacterWish:
                detailsCharacterWishUI.SetActive(true);
                break;

            case GachaType.WeaponWish:
                detailsWeaponWishUI.SetActive(true);
                break;

            case GachaType.StandartWish:
                detailsStandartWishUI.SetActive(true);
                break;
        }
    }

    private void ChangePityText()
    {
        pityTxt.text = $"Pity : {pityCount}";
    }
}
