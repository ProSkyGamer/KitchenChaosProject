using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShopCuttingPurchasesSO : ScriptableObject
{
    public int coinsCost;
    public Sprite purchaseImage;
    public CuttingRecipeSO oldCuttingRecipe;
    public CuttingRecipeSO newCuttingRecipe;
}
