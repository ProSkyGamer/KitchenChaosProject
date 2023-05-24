using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShopFryingPurchasesSO : ScriptableObject
{
    public int coinsCost;
    public Sprite purchaseImage;
    public FryingRecipeSO oldFryingRecipe;
    public FryingRecipeSO newFryingRecipe;
}
