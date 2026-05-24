using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{
    [SerializeField] GameObject eventPanel;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Button choiceAButton;
    [SerializeField] Button choiceBButton;
    [SerializeField] TextMeshProUGUI choiceAText;
    [SerializeField] TextMeshProUGUI choiceBText;
    [SerializeField] Button confirmButton;

    void Start()
    {
        EventManager.OnEventTriggered += ShowEvent;
        eventPanel.SetActive(false);

        choiceAButton.onClick.AddListener(OnChoiceAClicked);
        choiceBButton.onClick.AddListener(OnChoiceBClicked);
        confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    void OnDestroy()
    {
        EventManager.OnEventTriggered -= ShowEvent;
    }

    void ShowEvent(GameEvent gameEvent)
    {
        eventPanel.SetActive(true);
        TimeController.Instance?.Pause();
        titleText.text = gameEvent.Title;
        descriptionText.text = gameEvent.Description;

        if (gameEvent.RequiresChoice)
        {
            choiceAButton.gameObject.SetActive(true);
            choiceBButton.gameObject.SetActive(true);
            choiceAText.text = gameEvent.ChoiceAText;
            choiceBText.text = gameEvent.ChoiceBText;
            confirmButton.gameObject.SetActive(false);
        }
        else
        {
            choiceAButton.gameObject.SetActive(false);
            choiceBButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(true);
        }
    }

    void OnChoiceAClicked()
    {
        TimeController.Instance?.Resume();
        EventManager.Instance.ResolveChoiceA();
        eventPanel.SetActive(false);
    }

    void OnChoiceBClicked()
    {
        TimeController.Instance?.Resume();
        EventManager.Instance.ResolveChoiceB();
        eventPanel.SetActive(false);
    }

    void OnConfirmClicked()
    {
        TimeController.Instance?.Resume();
        EventManager.Instance.ResolveAuto();
        eventPanel.SetActive(false);
    }
}
