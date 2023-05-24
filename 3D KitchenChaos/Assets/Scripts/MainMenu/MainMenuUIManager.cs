using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager Instance { get; private set; }

    public enum MenuStates
    {
        MainMenu,
        ShopMenu,
        LevelSelectMenu,
        StoryMenu,
    }

    private MenuStates currentMenuState;

    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private ShopUI shopMenuUI;
    [SerializeField] private ChooseLevelUI chooseLevelMenuUI;
    [SerializeField] private StoryUI storyMenuUI;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate= false;

            ChangeMenuState(MenuStates.MainMenu);
        }
    }

    public void ChangeMenuState(MenuStates state)
    {
        currentMenuState = state;

        switch (currentMenuState)
        {
            case MenuStates.MainMenu:
                HideAllMenu();
                mainMenuUI.Show();
                break;
            case MenuStates.ShopMenu:
                HideAllMenu();
                shopMenuUI.Show();
                shopMenuUI.TryUnlockAllCards();
                break;
            case MenuStates.LevelSelectMenu:
                HideAllMenu();
                chooseLevelMenuUI.Show();
                chooseLevelMenuUI.TryUnlockAllLevels();
                break;
            case MenuStates.StoryMenu:
                HideAllMenu();
                storyMenuUI.Show();
                break;
        }
    }

    private void HideAllMenu()
    {
        mainMenuUI.Hide();
        shopMenuUI.Hide();
        chooseLevelMenuUI.Hide();
        storyMenuUI.Hide();
    }
}
