using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class WeaponSO : ScriptableObject
{
    public Weapon.WeaponName weaponName;
    public RarityClass.Rarity weaponRarity;
    public int baseAttack;
    public Weapon.SpecialEffect specialEffect;
    public Sprite weaponSprite;
}
