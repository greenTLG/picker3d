using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] GameObject failPanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject winPanel_BG;
    [SerializeField] GameObject inGamePanel;
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject moneyCounter;
    [SerializeField] Transform moneyCounterImage;
    [SerializeField] TextMeshProUGUI moneyCounterText;
    [SerializeField] TextMeshProUGUI currentLevelText, nextLevelText;
    [SerializeField] List<GameObject> checkpointCompletedImages = new List<GameObject>();
    [SerializeField] GameObject nextLevelButton;
    [SerializeField] Transform moneyImage_winPanel;
    [SerializeField] TextMeshProUGUI moneyText_winPanel;
    int currentCheckpointIndex = 0;
    int currentRewardMoney;
    [SerializeField] Sprite[] failEmojies = new Sprite[0];
    [SerializeField] Sprite[] successEmojies = new Sprite[0];
    [SerializeField] Image emojiImage_failPanel;
    [SerializeField] Image emojiImage_winPanel;

    public enum MoneySendingTypes
    {
        Sequential,
        Parallel
    }

    void Awake()
    {
        Instance = this;
    }

    public void ShowFailPanel()
    {
        if (failPanel == null)
            return;
        inGamePanel.SetActive(false);
        moneyCounter.SetActive(false);
        emojiImage_failPanel.sprite = failEmojies[Random.Range(0, failEmojies.Length)];
        failPanel.SetActive(true);
    }

    public void HideFailPanel()
    {
        if (failPanel == null)
            return;
        inGamePanel.SetActive(true);
        moneyCounter.SetActive(true);
        failPanel.SetActive(false);
    }

    public void ShowWinPanel(int rewardMoney)
    {
        if (winPanel == null)
            return;
        currentRewardMoney = rewardMoney;
        moneyText_winPanel.text = rewardMoney.ToString();
        inGamePanel.SetActive(false);
        nextLevelButton.SetActive(true);
        emojiImage_winPanel.sprite = successEmojies[Random.Range(0, successEmojies.Length)];
        winPanel.SetActive(true);
    }

    public void HideWinPanel()
    {
        if (winPanel == null)
            return;
        inGamePanel.SetActive(true);
        winPanel.SetActive(false);
    }

    public void WhenCheckpointCompleted()
    {
        if (currentCheckpointIndex >= checkpointCompletedImages.Count)
            return;
        checkpointCompletedImages[currentCheckpointIndex++].SetActive(true);
    }

    public void ResetUIManager()
    {
        currentCheckpointIndex = 0;
        checkpointCompletedImages.ForEach(x => x.SetActive(false));
    }

    public void SetLevelNum(int levelNum)
    {
        if (currentLevelText != null)
            currentLevelText.text = levelNum.ToString();
        if (nextLevelText != null)
            nextLevelText.text = (levelNum + 1).ToString();
    }

    public void SetMoneyCount(int moneyCount, float duration = 0, float delay = 0)
    {
        if (duration == 0)
        {
            DOTween.To(x => x = 0, 0, 0, delay).OnComplete(() =>
            {
                moneyCounterText.text = moneyCount.ToString();
            });
        }
        else
        {
            moneyCounterText.DOText(moneyCount.ToString(), duration, true, ScrambleMode.Numerals).SetDelay(delay);
        }
    }

    public void SendMoneyImagesToMoneyCounterImage(Vector3 worldPos, bool isPosUI, float duration, int moneyImageCount, MoneySendingTypes moneySendingType)
    {
        Vector3 imagePos = isPosUI ? worldPos : Camera.main.WorldToScreenPoint(worldPos);
        switch (moneySendingType)
        {
            case MoneySendingTypes.Sequential:
                float durationPerMoneyImage = 2 * duration / (moneyImageCount + 1);
                for (int i = 0; i < moneyImageCount; i++)
                {
                    Transform tempMoneyTransform = ObjectPooler.Instance.SpawnFromPool("MoneyImage", worldPos, Quaternion.identity).transform;
                    tempMoneyTransform.parent = moneyCounterImage;
                    tempMoneyTransform.position = imagePos;
                    tempMoneyTransform.DOLocalMove(Vector3.zero, durationPerMoneyImage).SetDelay(i * durationPerMoneyImage / 2).OnComplete(() =>
                    {
                        tempMoneyTransform.gameObject.SetActive(false);
                        Taptic.Medium();
                        SoundManager.Instance.Play("Money");
                    });
                    tempMoneyTransform.DOLocalRotate(new Vector3(0, 0, 360), durationPerMoneyImage, RotateMode.FastBeyond360).SetDelay(i * durationPerMoneyImage / 2);
                }
                break;
            case MoneySendingTypes.Parallel:
                for (int i = 0; i < moneyImageCount; i++)
                {
                    Transform tempMoneyTransform = ObjectPooler.Instance.SpawnFromPool("MoneyImage", worldPos, Quaternion.identity).transform;
                    tempMoneyTransform.parent = moneyCounterImage;
                    tempMoneyTransform.position = imagePos + (Random.insideUnitCircle * 150).ToVector3();
                    tempMoneyTransform.DOLocalMove(Vector3.zero, duration * 2 / 3).SetDelay(duration / 3).OnComplete(() =>
                    {
                        tempMoneyTransform.gameObject.SetActive(false);
                        Taptic.Medium();
                        SoundManager.Instance.Play("Money");
                    });
                    tempMoneyTransform.DOLocalRotate(new Vector3(0, 0, 360), duration * 2 / 3, RotateMode.FastBeyond360).SetDelay(duration / 3);
                }
                break;
            default:
                break;
        }


    }

    public void OnClickButton(string tag)
    {
        switch (tag)
        {
            case "NextLevel":
                GameManager.OnClickedNextLevelButton();
                nextLevelButton.SetActive(false);
                moneyText_winPanel.DOText("0", 1f, true, ScrambleMode.Numerals);
                MoneyManager.Instance.EarnMoney(currentRewardMoney, moneyImage_winPanel.position, true, MoneySendingTypes.Sequential);
                Invoke("HideWinPanel", 1.5f);
                Invoke("ShowStartPanel", 1.5f);


                break;
            case "RestartLevel":
                GameManager.OnClickedRestartLevelButton();
                HideFailPanel();
                break;
            default:
                break;
        }
        Taptic.Medium();
        SoundManager.Instance.Play("UI");

    }

    public void ShowStartPanel()
    {
        if (startPanel == null)
            return;
        startPanel.SetActive(true);
    }

    public void HideStartPanel()
    {
        if (startPanel == null)
            return;
        startPanel.SetActive(false);
    }
}
