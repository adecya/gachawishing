using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Character;
using rarity = RarityClass.Rarity;

public class Weapon : MonoBehaviour
{
    #region Weapon
    public enum WeaponName
    {
        DarkAxe,
        DarkSword,
        FlyingSword,
        ForgottenSword,
        IceQueenSword,
        LightSword,
        SharpEdge,
        ShurikenSword,
        SwingBlade,
        EarthSword
    }

    public enum SpecialEffect
    {
        Normal,
        Dark,
        Earth,
        Ice,
        Swift
    }

    private WeaponSO weaponSO;
    private WeaponName weaponName;
    private rarity weaponRarity;
    private SpecialEffect specialEffect;
    private int baseAttack;
    private Sprite weaponSprite;
    #endregion

    #region Weapon UI Display
    [Header("Weapon UI Display")]
    [SerializeField] private Image iconImg;
    [SerializeField] private TextMeshProUGUI nameTxt;
    #endregion

    public void InitializeWeapon()
    {
        weaponName = weaponSO.weaponName;
        weaponRarity = weaponSO.weaponRarity;
        specialEffect = weaponSO.specialEffect;
        baseAttack = weaponSO.baseAttack;
        weaponSprite = weaponSO.weaponSprite;
    }

    public void InitializeDisplay()
    {
        iconImg.sprite = weaponSprite;
        nameTxt.text = weaponName.ToString();
    }

    public void ChangeWeaponSO(WeaponSO newWeaponSO)
    {
        weaponSO = newWeaponSO;
    }
}
