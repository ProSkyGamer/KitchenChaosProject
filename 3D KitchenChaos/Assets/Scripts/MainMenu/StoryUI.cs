using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class StoryUI : MonoBehaviour
{
    [SerializeField] private Image displayedImage;
    [SerializeField] private Sprite[] allImages;
    private int currentDisplayedImage;

    private int inputButton = 1;
    // 0 - left, 1 - right, 2 - middle

    public void Show()
    {
        gameObject.SetActive(true);

        ChangeDisplayedImage(0);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangeDisplayedImage(int toDisplayFromArray)
    {
        currentDisplayedImage = toDisplayFromArray;
        displayedImage.sprite = allImages[currentDisplayedImage];
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(inputButton))
        {
            if(currentDisplayedImage < allImages.Length - 1)
            {
                ChangeDisplayedImage(currentDisplayedImage + 1);
            }
            else
            {
                MainMenuUIManager.Instance.ChangeMenuState(MainMenuUIManager.MenuStates.MainMenu);
            }
        }
    }
}
