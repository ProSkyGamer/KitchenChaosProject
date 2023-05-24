using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    public enum State
    {
        Iddle,
        Frying,
        Fried,
        Burned
    }

    private State state;

    private float fryingTimer;
    private float burningTimer;

    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Awake()
    {
        state = State.Iddle;
        foreach (ShopFryingPurchasesSO shopFryingPurchasesSO in ShopManager.BoughtShopFryingPurchasesSOArray)
        {
            if (fryingRecipeSO == shopFryingPurchasesSO.oldFryingRecipe)
            {
                fryingRecipeSO = shopFryingPurchasesSO.newFryingRecipe;
            }
        }
        foreach (ShopBurningPurchasesSO shopBurningPurchasesSO in ShopManager.BoughtShopBurningPurchasesSOArray)
        {
            if (burningRecipeSO == shopBurningPurchasesSO.oldBurningRecipe)
            {
                burningRecipeSO = shopBurningPurchasesSO.newBurningRecipe;
            }
        }
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Iddle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    { progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Fried:

                    OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    { progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax });
                    burningTimer += Time.deltaTime;
                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;

                        OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        { progressNormalized = 0f });

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
                if (HasRecepieWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    { progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax });

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Iddle;

                OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                { progressNormalized = 0f });

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Iddle;

                        OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        { progressNormalized = 0f });

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                }
            }
        }
    }

    private bool HasRecepieWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null ? fryingRecipeSO.output : null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
                return fryingRecipeSO;
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
                return burningRecipeSO;
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
