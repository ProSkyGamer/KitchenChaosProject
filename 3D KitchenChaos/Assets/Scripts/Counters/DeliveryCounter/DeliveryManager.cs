using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }
    public int SuccessfulRecipesAmount { get => successfulRecipesAmount; set => successfulRecipesAmount = value; }
    public int MinRecipesToCompleteLevelAmmount { get => minRecipesToCompleteLevelAmmount; set => minRecipesToCompleteLevelAmmount = value; }
    public int NormalRecipesAmmount { get => normalRecipesAmmount; set => normalRecipesAmmount = value; }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private bool isFirstUpdate = true;


    private int successfulRecipesAmount;

    private int maxRecipesAmmount;
    private int minRecipesToCompleteLevelAmmount;
    private int normalRecipesAmmount;

    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>();

        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

        maxRecipesAmmount = LevelsManager.GetCurrentLevelSO().levelSettings.recipesForLevel;
        minRecipesToCompleteLevelAmmount = LevelsManager.GetCurrentLevelSO().levelSettings.minimumNeededRecipesForCompleteLevel;
        normalRecipesAmmount = LevelsManager.GetCurrentLevelSO().levelSettings.recipesForLevel - 2;
    }

    private void Start()
    {
        OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
    }

    private void Update()
    {
        if(isFirstUpdate && KitchenGameManager.Instance.IsGamePlaying())
        {
            isFirstUpdate = false;
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
            waitingRecipeSOList.Add(waitingRecipeSO);

            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        if (successfulRecipesAmount >= maxRecipesAmmount)
        {
            KitchenGameManager.Instance.EndGame();
        }
        else
        {
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
            waitingRecipeSOList.Add(waitingRecipeSO);

            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    successfulRecipesAmount++;
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
