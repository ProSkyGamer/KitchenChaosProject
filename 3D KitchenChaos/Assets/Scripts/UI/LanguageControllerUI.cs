using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageControllerUI : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Start()
    {
        Loader.OnSceneChange += Loader_OnSceneChange;
    }

    private void Loader_OnSceneChange(object sender, EventArgs e)
    {
        TextTranslationManager.ResetStaticData();
        TextTranslationManager.ChangeLanguage(TextTranslationManager.GetCurrentLanguage());
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;

            TextTranslationManager.ChangeLanguage(TextTranslationManager.GetCurrentLanguage());
        }
    }
}
