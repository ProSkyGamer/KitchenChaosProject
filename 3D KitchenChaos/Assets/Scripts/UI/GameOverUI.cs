using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private TextMeshProUGUI coinsEarnedText;

    [SerializeField] private TextMeshProUGUI levelFailedText;
    [SerializeField] private TextMeshProUGUI levelCompleteGoodText;
    [SerializeField] private TextMeshProUGUI levelCompleteExcellentText;

    [SerializeField] private TextMeshProUGUI nextLevelUnlockedText;

    private bool isFirstUpdate = true;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
    }

    private void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;

            Hide();
        }
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();

            UpdateRecipesDeliveredText();
        }
        else
            Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);

        if(DeliveryManager.Instance.SuccessfulRecipesAmount >= DeliveryManager.Instance.MinRecipesToCompleteLevelAmmount)
        {
            if(DeliveryManager.Instance.SuccessfulRecipesAmount > DeliveryManager.Instance.NormalRecipesAmmount)
            {
                levelCompleteExcellentText.gameObject.SetActive(true);

                coinsEarnedText.text = ((int)(DeliveryManager.Instance.GetSuccessfulRecipesAmount() * 1.5)).ToString();
                ShopManager.AddCoins((int)(DeliveryManager.Instance.GetSuccessfulRecipesAmount() * 1.5));
            }
            else
            {
                levelCompleteGoodText.gameObject.SetActive(true);

                coinsEarnedText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
                ShopManager.AddCoins(DeliveryManager.Instance.GetSuccessfulRecipesAmount());
            }

            if (LevelsManager.GetCurrentLevel() >= LevelsManager.GetReachedLevel())
            {
                LevelsManager.UnlockLevel(LevelsManager.GetCurrentLevel() + 1);
                nextLevelUnlockedText.gameObject.SetActive(true);
            }
            else
            {
                nextLevelUnlockedText.gameObject.SetActive(false);
            }
        }
        else
        {
            levelFailedText.gameObject.SetActive(true);
            coinsEarnedText.text = "1";
            ShopManager.AddCoins(1);
        }
    }

    private void Hide()
    {
        levelFailedText.gameObject.SetActive(false);
        levelCompleteGoodText.gameObject.SetActive(false);
        levelCompleteExcellentText.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    private void UpdateRecipesDeliveredText()
    {
        recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
    }
}
