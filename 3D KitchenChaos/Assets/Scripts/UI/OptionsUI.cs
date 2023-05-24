using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [Header("Buttons")]
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button intearctAlternativeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadIntearctAlternativeButton;
    [SerializeField] private Button gamepadPauseButton;

    [Header("Button Labels")]
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternativeText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlternativeText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;

    [Header("Additional Button Texts")]
    [SerializeField] private TextTranslationsSO soundEffectsTextTranslationSO;
    [SerializeField] private TextTranslationsSO musicTextTranslationSO;

    [SerializeField] private Transform pressToRebingKeyTransform;

    private Action onCloseButtonAction;

    private bool IsFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Interact); });
        intearctAlternativeButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Interact_Alternative); });
        pauseButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Pause); });
        gamepadInteractButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Gamepad_Interact); });
        gamepadIntearctAlternativeButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Gamepad_Interact_Alternative); });
        gamepadPauseButton.onClick.AddListener(() => { RebingBinding(GameInput.Binding.Gamepad_Pause); });

    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        TextTranslationManager.OnLanguageChange += TextTranslationManager_OnLanguageChange;
    }

    private void TextTranslationManager_OnLanguageChange(object sender, TextTranslationManager.OnLanguageChangeEventArgs e)
    {
        UpdateVisual();
    }

    private void Update()
    {
        if(IsFirstUpdate)
        {
            IsFirstUpdate = false;

            UpdateVisual();
            Hide();
            HidePressToRebingKey();
        }
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = TextTranslationManager.GetTextFromTextTranslationSOByLanguage(TextTranslationManager.GetCurrentLanguage(),
            soundEffectsTextTranslationSO) + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = TextTranslationManager.GetTextFromTextTranslationSOByLanguage(TextTranslationManager.GetCurrentLanguage(),
            musicTextTranslationSO) + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternativeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternative);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAlternativeText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact_Alternative);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);

        soundEffectsButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebingKey()
    {
        pressToRebingKeyTransform.gameObject.SetActive(true);
    }
    
    private void HidePressToRebingKey()
    {
        pressToRebingKeyTransform.gameObject.SetActive(false);
    }

    private void RebingBinding(GameInput.Binding binding)
    {
        ShowPressToRebingKey();

        GameInput.Instance.RebingBinding(binding, () => 
        { 
            HidePressToRebingKey();
            UpdateVisual();
        });

    }
}
