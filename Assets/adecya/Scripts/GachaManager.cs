using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using rarity = RarityClass.Rarity;

public class GachaManager : MonoBehaviour
{
    public enum GachaType
    {
        CharacterWish,
        WeaponWish,
        StandartWish
    }

    [SerializeField] private CharacterSO[] characterList;
    [SerializeField] private WeaponSO[] weaponList;
    private int gachaCount;

    [Header("Chance For Each Rarity")]
    [SerializeField] private float commonChance;
    [SerializeField] private float rareChance;
    [SerializeField] private float epicChance;
    [SerializeField] private float legendaryChance;

    [Header("UI")]
    [SerializeField] private GameObject resultDisplayUI;
    [SerializeField] private GameObject gachaResultUI;

    [Header("Script")]
    [SerializeField] private Character character;
    [SerializeField] private Weapon weapon;

    public void PerformGachaCharacter(GachaType gachaType, int gachaCount)
    {
        this.gachaCount = gachaCount;

        switch(gachaType)
        {
            case GachaType.CharacterWish:
                Gacha(characterList);
                break;

            case GachaType.WeaponWish:
                Gacha(weaponList);
                break;

            case GachaType.StandartWish:
                break;
        }        
    }

    public void ShowNextGacha()
    {
        if(gachaCount > 0 )
        {
            Gacha(characterList);
        }
        else
        {
            resultDisplayUI.SetActive(false);
            gachaResultUI.SetActive(false);
        }
    }

    public void Gacha<T>(T[] gachaList)
    {
        float totalChance = 0f;

        // Hitung total peluang dari semua item
        foreach (var item in gachaList)
        {
            switch(GetRarity(item))
            {
                case rarity.common:
                    totalChance += commonChance;
                    break;

                case rarity.rare:
                    totalChance += rareChance;
                    break;

                case rarity.epic:
                    totalChance += epicChance;
                    break;

                case rarity.legendary:
                    totalChance += legendaryChance;
                    break;
            }
        }

        resultDisplayUI.SetActive(false);

        // Mendapatkan nilai acak untuk menentukan item yang didapatkan
        float randomValue = Random.Range(0f, totalChance);

        // Memilih item berdasarkan peluang
        foreach (var item in gachaList)
        {
            switch (GetRarity(item))
            {
                case rarity.common:
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

                case rarity.rare:
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

                case rarity.epic:
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

                case rarity.legendary:
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
            return (item as CharacterSO).characterRarity;
        }
        else
        {
            return rarity.common;
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
        resultDisplayUI.SetActive(true);
        gachaCount--;
    }
}
