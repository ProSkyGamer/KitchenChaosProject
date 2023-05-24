using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlternativeText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternativeText;
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;

    [SerializeField] private Image tutorialImage;
    [SerializeField] private TextTranslationImagesSO textTranslationImagesSO;

    private void Start()
    {
        GameInput.Instance.OnBindingRebing += GameInput_OnBindingRebing;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Show();

        UpdateVisual();

        TextTranslationManager.OnLanguageChange += TextTranslationManager_OnLanguageChange;
    }

    private void TextTranslationManager_OnLanguageChange(object sender, TextTranslationManager.OnLanguageChangeEventArgs e)
    {
        UpdateVisual();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsCoutdownToStartActive())
        {
            Hide();
            KitchenGameManager.Instance.OnStateChanged -= KitchenGameManager_OnStateChanged;
        }
    }

    private void GameInput_OnBindingRebing(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractAlternativeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternative);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        keyGamepadInteractAlternativeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact_Alternative);
        keyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);

        tutorialImage.sprite = TextTranslationManager.GetCurrentLanguage() == TextTranslationManager.Languages.English
            ? textTranslationImagesSO.enImage :
            TextTranslationManager.GetCurrentLanguage() == TextTranslationManager.Languages.Russian
            ? textTranslationImagesSO.ruImage : null;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
