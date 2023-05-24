using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChooseLevelSingleCardUI : MonoBehaviour
{
    private LevelsSO levelSO;

    private Image lockedImage;
    private Button levelButton;
    private int levelNumber;

    public void SetLevelSO(LevelsSO levelSO)
    {
        this.levelSO = levelSO;
    }

    public void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            LevelsManager.SetCurrentLevel(levelNumber);
            LevelsManager.SetLevelSO(levelSO);
            Loader.Load(levelSO.sceneToLoad);
        });

        lockedImage = gameObject.GetComponentsInChildren<Image>()[1];
        levelButton = gameObject.GetComponent<Button>();
    }

    public void UnlockLevel()
    {
        levelButton.interactable = true;
        lockedImage.gameObject.SetActive(false);
    }

    public void LockLevel()
    {
        levelButton.interactable = false;
        lockedImage.gameObject.SetActive(true);
    }

    public void SetLevelInt(int level)
    {
        levelNumber = level;
    }
}
