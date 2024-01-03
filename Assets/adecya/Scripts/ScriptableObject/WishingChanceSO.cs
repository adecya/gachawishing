using UnityEngine;

[CreateAssetMenu(fileName = "NewWishingChance", menuName = "WishingChance")]
public class WishingChanceSO : ScriptableObject
{
    public float commonChance;
    public float rareChance;
    public float epicChance;
    public float legendaryChance;
}
