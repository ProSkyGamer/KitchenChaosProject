using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button selectLevelsButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button readStoryButton;

    private void Awake()
    {
        selectLevelsButton.onClick.AddListener(() =>
        {
            /*Loader.Load(Loader.Scene.GameScene);*/
            MainMenuUIManager.Instance.ChangeMenuState(MainMenuUIManager.MenuStates.LevelSelectMenu);
        });

        shopButton.onClick.AddListener(() =>
        {
            MainMenuUIManager.Instance.ChangeMenuState(MainMenuUIManager.MenuStates.ShopMenu);
        });

        readStoryButton.onClick.AddListener(() =>
        {
            MainMenuUIManager.Instance.ChangeMenuState(MainMenuUIManager.MenuStates.StoryMenu);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale = 1f;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
