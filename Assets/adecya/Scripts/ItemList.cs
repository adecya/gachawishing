using UnityEngine;

public class ItemList : MonoBehaviour
{
    public CharacterSO[] characterList;
    public CharacterSO[] epicOrMoreCharList;
    public WeaponSO[] weaponList;
    public WeaponSO[] epicOrMoreWeaponList;

    public static ItemList Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
