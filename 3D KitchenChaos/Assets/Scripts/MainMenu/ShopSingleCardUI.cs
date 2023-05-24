using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSingleCardUI : MonoBehaviour
{
    private int cost;

    private ShopFryingPurchasesSO shopFryingPurchaseSO;
    private ShopBurningPurchasesSO shopBurningPurchaseSO;
    private ShopCuttingPurchasesSO shopCuttingPurchaseSO;

    private Image lockedImage;
    private Button cardButton;
    private bool isBought;

    public bool IsBought { get => isBought; set => isBought = value; }

    private void Awake()
    {
        lockedImage = gameObject.GetComponentsInChildren<Image>()[2];
        cardButton = gameObject.GetComponent<Button>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void SetCardCost(int upgradeCost)
    {
        cost = upgradeCost;
    }

    public void SetFryingRecipeSO(ShopFryingPurchasesSO fryingPurchasesSO)
    {
        shopBurningPurchaseSO = null;
        shopCuttingPurchaseSO = null;

        shopFryingPurchaseSO = fryingPurchasesSO;

        SetCardCost(shopFryingPurchaseSO.coinsCost);

        SetCardVisual(cost, shopFryingPurchaseSO.purchaseImage,
            (int)shopFryingPurchaseSO.oldFryingRecipe.fryingTimerMax, (int)shopFryingPurchaseSO.newFryingRecipe.fryingTimerMax);

        SetCardLocalInfo(shopFryingPurchaseSO.name + "PlayerPrefs");
    }
    
    public void SetBurningRecipeSO(ShopBurningPurchasesSO burningPurchasesSO)
    {
        shopFryingPurchaseSO = null;
        shopCuttingPurchaseSO = null;

        shopBurningPurchaseSO = burningPurchasesSO;

        SetCardCost(shopBurningPurchaseSO.coinsCost);

        SetCardVisual(cost, shopBurningPurchaseSO.purchaseImage,
            (int)shopBurningPurchaseSO.oldBurningRecipe.burningTimerMax, (int)shopBurningPurchaseSO.newBurningRecipe.burningTimerMax);

        SetCardLocalInfo(shopBurningPurchaseSO.name + "PlayerPrefs");
    }
    
    public void SetCuttingRecipeSO(ShopCuttingPurchasesSO cuttingPurchasesSO)
    {
        shopBurningPurchaseSO = null;
        shopFryingPurchaseSO = null;

        shopCuttingPurchaseSO = cuttingPurchasesSO;

        SetCardCost(shopCuttingPurchaseSO.coinsCost);

        SetCardVisual(cost, cuttingPurchasesSO.purchaseImage,
            cuttingPurchasesSO.oldCuttingRecipe.cuttingProgressMax, cuttingPurchasesSO.newCuttingRecipe.cuttingProgressMax);

        SetCardLocalInfo(shopCuttingPurchaseSO.name + "PlayerPrefs");
    }

    private void SetCardVisual(int cost, Sprite cardImage, int oldCount, int newCount)
    {
        gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = cost.ToString();

        gameObject.GetComponentsInChildren<Image>()[1].sprite = cardImage;

        gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = oldCount.ToString() + " -> " + newCount.ToString();
    }

    private void SetCardLocalInfo(string isUnlockedPlayerPrefs)
    {
        isBought = PlayerPrefs.GetInt(isUnlockedPlayerPrefs, 0) == 1;
    }

    public int GetCardCost()
    {
        return shopBurningPurchaseSO != null ? shopBurningPurchaseSO.coinsCost :
            shopCuttingPurchaseSO != null ? shopCuttingPurchaseSO.coinsCost :
            shopFryingPurchaseSO != null ? shopFryingPurchaseSO.coinsCost : 0;
    }

    public void UnlockBought()
    {
        cardButton.interactable = true;
        lockedImage.gameObject.SetActive(false);
    }

    public void LockBought()
    {
        cardButton.interactable = false;
        lockedImage.gameObject.SetActive(true);
    }
}
