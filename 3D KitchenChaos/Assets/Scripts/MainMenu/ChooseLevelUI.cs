using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelUI : MonoBehaviour
{
    [SerializeField] private Transform levelButtonPrefab;
    [SerializeField] private Transform allLevelsGrid;
    [SerializeField] private Button returnToMainMenuButton;

    [SerializeField] private LevelsSO[] allLevels;

    private List<ChooseLevelSingleCardUI> allLevelsButtonArray = new List<ChooseLevelSingleCardUI>();

    private void Awake()
    {
        for(int i = 0; i < allLevels.Length; i++)
        {
            var levelButton = Instantiate(levelButtonPrefab,allLevelsGrid);

            ChooseLevelSingleCardUI levelButtonUI =  levelButton.GetComponent<ChooseLevelSingleCardUI>();
            levelButtonUI.SetLevelSO(allLevels[i]);
            levelButtonUI.SetLevelInt(i);

            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();

            allLevelsButtonArray.Add(levelButtonUI);
        }

        levelButtonPrefab.gameObject.SetActive(false);

        returnToMainMenuButton.onClick.AddListener(() =>
        {
            MainMenuUIManager.Instance.ChangeMenuState(MainMenuUIManager.MenuStates.MainMenu);
        });

        TryUnlockAllLevels();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void TryUnlockAllLevels()
    {
        for(int i = 0; i < allLevelsButtonArray.Count; i++)
        {
            if(LevelsManager.IsLevelUnlocked(i))
            {
                allLevelsButtonArray[i].UnlockLevel();
            }
            else
            {
                allLevelsButtonArray[i].LockLevel();
            }
        }
    }
}
