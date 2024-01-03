using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character")]
public class CharacterSO : ScriptableObject
{
    public Character.CharacterName characterName;
    public RarityClass.Rarity characterRarity;
    public int characterHp;
    public int characterAtk;
    public int characterDef;
    public Sprite characterSprite;
}
