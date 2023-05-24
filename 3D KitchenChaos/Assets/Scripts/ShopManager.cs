using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ShopManager
{
    public static event EventHandler<OnCoinsValueChangedEventArgs> OnCoinsValueChanged;
    public class OnCoinsValueChangedEventArgs : EventArgs
    {
        public int coins;
    }

    private static List<ShopBurningPurchasesSO> shopBurningPurchasesSOArray; 
    private static List<ShopFryingPurchasesSO> shopFryingPurchasesSOArray; 
    private static List<ShopCuttingPurchasesSO> shopCuttingPurchasesSOArray;
    private static List<ShopBurningPurchasesSO> boughtShopBurningPurchasesSOArray = new List<ShopBurningPurchasesSO>(); 
    private static List<ShopFryingPurchasesSO> boughtShopFryingPurchasesSOArray = new List<ShopFryingPurchasesSO>(); 
    private static List<ShopCuttingPurchasesSO> boughtShopCuttingPurchasesSOArray = new List<ShopCuttingPurchasesSO>(); 
    

    private const string COINS_PLAYER_PREFS = "PlayerCoins";

    public static List<ShopBurningPurchasesSO> BoughtShopBurningPurchasesSOArray { get => boughtShopBurningPurchasesSOArray; private set => boughtShopBurningPurchasesSOArray = value; }
    public static List<ShopFryingPurchasesSO> BoughtShopFryingPurchasesSOArray { get => boughtShopFryingPurchasesSOArray; private set => boughtShopFryingPurchasesSOArray = value; }
    public static List<ShopCuttingPurchasesSO> BoughtShopCuttingPurchasesSOArray { get => boughtShopCuttingPurchasesSOArray; private set => boughtShopCuttingPurchasesSOArray = value; }

    public static bool IsEnoughCoins(int coins)
    {
        return PlayerPrefs.GetInt(COINS_PLAYER_PREFS, 0) >= coins;
    }

    public static void AddCoins(int toAdd)
    {
        PlayerPrefs.SetInt(COINS_PLAYER_PREFS, PlayerPrefs.GetInt(COINS_PLAYER_PREFS) + toAdd);

        OnCoinsValueChanged?.Invoke(null, new OnCoinsValueChangedEventArgs
        {
            coins = PlayerPrefs.GetInt(COINS_PLAYER_PREFS)
        });
    }

    private static void SpendCoins(int toSpend)
    {
        PlayerPrefs.SetInt(COINS_PLAYER_PREFS, PlayerPrefs.GetInt(COINS_PLAYER_PREFS) - toSpend);

        OnCoinsValueChanged?.Invoke(null, new OnCoinsValueChangedEventArgs
        {
            coins = PlayerPrefs.GetInt(COINS_PLAYER_PREFS)
        });
    }

    public static void SetAllPurchases(ShopBurningPurchasesSO[] shopBurningPurchasesSOArray,
        ShopFryingPurchasesSO[] shopFryingPurchasesSOArray, ShopCuttingPurchasesSO[] shopCuttingPurchasesSOArray)
    {
        ShopManager.shopBurningPurchasesSOArray.AddRange(shopBurningPurchasesSOArray);
        ShopManager.shopFryingPurchasesSOArray.AddRange(shopFryingPurchasesSOArray);
        ShopManager.shopCuttingPurchasesSOArray.AddRange(shopCuttingPurchasesSOArray);

        foreach(ShopBurningPurchasesSO shopBurningPurchasesSO in shopBurningPurchasesSOArray)
        {
            if (PlayerPrefs.GetInt(shopBurningPurchasesSO.name + "PlayerPrefs", 0) == 1)
                boughtShopBurningPurchasesSOArray.Add(shopBurningPurchasesSO);
        }
        foreach(ShopFryingPurchasesSO shopFryingPurchasesSO in shopFryingPurchasesSOArray)
        {
            if (PlayerPrefs.GetInt(shopFryingPurchasesSO.name + "PlayerPrefs", 0) == 1)
                boughtShopFryingPurchasesSOArray.Add(shopFryingPurchasesSO);
        }
        foreach(ShopCuttingPurchasesSO shopCuttingPurchasesSO in shopCuttingPurchasesSOArray)
        {
            if (PlayerPrefs.GetInt(shopCuttingPurchasesSO.name + "PlayerPrefs", 0) == 1)
                boughtShopCuttingPurchasesSOArray.Add(shopCuttingPurchasesSO);
        }
    }

    public static void GetUpgrade(ShopBurningPurchasesSO shopBurningPurchasesSO)
    {
        boughtShopBurningPurchasesSOArray.Add(shopBurningPurchasesSO);
        PlayerPrefs.SetInt(shopBurningPurchasesSO.name + "PlayerPrefs", 1);
        SpendCoins(shopBurningPurchasesSO.coinsCost);
    }
    
    public static void GetUpgrade(ShopCuttingPurchasesSO shopCuttingPurchasesSO)
    {
        boughtShopCuttingPurchasesSOArray.Add(shopCuttingPurchasesSO);
        PlayerPrefs.SetInt(shopCuttingPurchasesSO.name + "PlayerPrefs", 1);
        SpendCoins(shopCuttingPurchasesSO.coinsCost);
    }
    
    public static void GetUpgrade(ShopFryingPurchasesSO shopFryingPurchasesSO)
    {
        boughtShopFryingPurchasesSOArray.Add(shopFryingPurchasesSO);
        PlayerPrefs.SetInt(shopFryingPurchasesSO.name + "PlayerPrefs", 1);
        SpendCoins(shopFryingPurchasesSO.coinsCost);
    }

    public static void StartInitializeCoinsVisual()
    {
        OnCoinsValueChanged?.Invoke(null, new OnCoinsValueChangedEventArgs
        {
            coins = PlayerPrefs.GetInt(COINS_PLAYER_PREFS)
        });
    }
}
