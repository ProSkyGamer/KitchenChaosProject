using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSettingSO : ScriptableObject
{
    public float levelGameTime;
    public int minimumNeededRecipesForCompleteLevel;
    public int recipesForLevel;
    public RecipeListSO levelRecipes;
    public CuttingRecipeSO[] levelCuttingRecipes;
    
}
