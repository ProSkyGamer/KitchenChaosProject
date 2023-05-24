using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform purchaseCardPrefab;
    [SerializeField] private Transform allPurchaseCradsGrid;
    [SerializeField] private Button returnToMainMenuButton;

    [SerializeField] private ShopFryingPurchasesSO[] allFryingPurchases;
    [SerializeField] private ShopBurningPurchasesSO[] allBurningPurchases;
    [SerializeField] private ShopCuttingPurchasesSO[] allCuttingPurchases;

    [SerializeField] private TextMeshProUGUI coinsText;

    private List<ShopSingleCardUI> allCardButtons = new List<ShopSingleCardUI>();

    private void Awake()
    {
        foreach (var fryingPurchase in allFryingPurchases)
        {
            var purchaseCard = Instantiate(purchaseCardPrefab, allPurchaseCradsGrid);
            ShopSingleCardUI cardUI =  purchaseCard.GetComponent<ShopSingleCardUI>();
            cardUI.SetFryingRecipeSO(fryingPurchase);

            Button purchaseButton = purchaseCard.GetComponent<Button>();
            purchaseButton.onClick.AddListener(() =>
            {
                if (ShopManager.IsEnoughCoins(fryingPurchase.coinsCost))
                {
                    ShopManager.GetUpgrade(fryingPurchase);
                    purchaseButton.GetComponent<ShopSingleCardUI>().IsBought = true;
                    TryUnlockAllCards();
                }
            });
            allCardButtons.Add(cardUI);
        }

        foreach (var burningPurchase in allBurningPurchases)
        {
            var purchaseCard = Instantiate(purchaseCardPrefab, allPurchaseCradsGrid);
            ShopSingleCardUI cardUI = purchaseCard.GetComponent<ShopSingleCardUI>();
            cardUI.SetBurningRecipeSO(burningPurchase);

            Button purchaseButton = purchaseCard.GetComponent<Button>();
            purchaseButton.onClick.AddListener(() =>
            {
                if (ShopManager.IsEnoughCoins(burningPurchase.coinsCost))
                {
                    ShopManager.GetUpgrade(burningPurchase);
                    purchaseButton.GetComponent<ShopSingleCardUI>().IsBought = true;
                    TryUnlockAllCards();
                }
            });
            allCardButtons.Add(cardUI);
        }

        foreach (var cuttingPurchase in allCuttingPurchases)
        {
            var purchaseCard = Instantiate(purchaseCardPrefab, allPurchaseCradsGrid);
            ShopSingleCardUI cardUI = purchaseCard.GetComponent<ShopSingleCardUI>();
            cardUI.SetCuttingRecipeSO(cuttingPurchase);

            Button purchaseButton = purchaseCard.GetComponent<Button>();
            purchaseButton.onClick.AddListener(() =>
            {
                if (ShopManager.IsEnoughCoins(cuttingPurchase.coinsCost))
                {
                    ShopManager.GetUpgrade(cuttingPurchase);
                    purchaseButton.GetComponent<ShopSingleCardUI>().IsBought = true;
                    TryUnlockAllCards();
                }
            });
            allCardButtons.Add(cardUI);
        }

        purchaseCardPrefab.gameObject.SetActive(false);

        returnToMainMenuButton.onClick.AddListener(() =>
        {
            MainMenuUIManager.Instance.ChangeMenuState(MainMenuUIManager.MenuStates.MainMenu);
        });

        TryUnlockAllCards();

        //ShopManager.AddCoins(100);
    }

    private void Start()
    {
        ShopManager.OnCoinsValueChanged += ShopManager_OnCoinsValueChanged;
        ShopManager.StartInitializeCoinsVisual();
    }

    private void ShopManager_OnCoinsValueChanged(object sender, ShopManager.OnCoinsValueChangedEventArgs e)
    {
        coinsText.text = e.coins.ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void TryUnlockAllCards()
    {
        foreach (ShopSingleCardUI cardUI in allCardButtons)
        {
            if (ShopManager.IsEnoughCoins(cardUI.GetCardCost()) && !cardUI.IsBought)
            {
                cardUI.UnlockBought();
            }
            else
            {
                cardUI.LockBought();
            }
        }
    }
}
