using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameStartCoutdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI coutdownText;
    private Animator animator;
    private int previousCoutdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCoutdownToStartActive())
            Show();
        else
            Hide();
    }

    private void Update()
    {
        int coutdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCoutdownToStartTimer());
        coutdownText.text = coutdownNumber.ToString();

        if(previousCoutdownNumber != coutdownNumber)
        {
            previousCoutdownNumber = coutdownNumber;
            animator.SetTrigger(NUMBER_POPUP);

            SoundManager.Instance.PlayCoutdownSound();
        }    
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
