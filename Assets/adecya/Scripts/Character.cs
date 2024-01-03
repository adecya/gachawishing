using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using rarity = RarityClass.Rarity;

public class Character : MonoBehaviour
{
    #region Character
    public enum CharacterName
    {
        Asuka,
        Boa,
        Mei,
        Leona,
        Shin
    }

    private CharacterSO characterSO;
    private CharacterName characterName;
    private rarity characterRarity;
    private int characterHp;
    private int characterAtk;
    private int characterDef;
    private Sprite characterSprite;
    #endregion

    #region Character UI Display
    [Header("Character UI Display")]
    [SerializeField] private Image iconImg;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI descTxt;
    #endregion

    public void InitializeCharacter()
    {
        characterName = characterSO.characterName;
        characterRarity = characterSO.characterRarity;
        characterHp = characterSO.characterHp;
        characterAtk = characterSO.characterAtk;
        characterDef = characterSO.characterDef;
        characterSprite = characterSO.characterSprite;
    }

    public void InitializeDisplay()
    {
        iconImg.sprite = characterSprite;
        nameTxt.text = characterName.ToString();
        descTxt.text = $"Rarity\t: {characterRarity}\n" +
            $"HP\t\t: {characterHp}\n" +
            $"Attack\t: {characterAtk}\n" +
            $"Deffend\t: {characterDef}";
    }

    public void ChangeCharacterSO(CharacterSO newCharSO)
    {
        characterSO = newCharSO;
    }
}
