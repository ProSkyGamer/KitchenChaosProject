using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShopBurningPurchasesSO : ScriptableObject
{
    public int coinsCost;
    public Sprite purchaseImage;
    public BurningRecipeSO oldBurningRecipe;
    public BurningRecipeSO newBurningRecipe;
}
