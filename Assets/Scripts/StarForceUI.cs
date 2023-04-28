using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;
using Random = UnityEngine.Random;

[System.Serializable]
public class UpgradePercent
{
    public float successPecent;
    public float failPercent;
    public float destroyPercent;
}


public class StarForceUI : MonoBehaviour
{
    public static StarForceUI Inst;
    private Building currentBuilding;
    public BuildingTypeSO BuildingType => currentBuilding.BuildingType;

    private Dictionary<string, int> buildingLevelList = new Dictionary<string, int>();
    [SerializeField]
    private Image buildingImage;
    [SerializeField]
    private TMP_Text upgradeInfoText;
    [SerializeField]
    private TMP_Text needGoldText;

    [SerializeField]
    private TMP_Text upgradeTitleText;

    [SerializeField]
    private StarforceResultPanel starforceResultPanel;

    [SerializeField]
    private ResourceTypeSO goldResourceType;

    [SerializeField]
    private Toggle destroyProctedToggle;

    [SerializeField]
    private Toggle starCatchToggle;



    [SerializeField]
    private List<UpgradePercent> upgradePercentList;

    [SerializeField]
    private Animator effectAnim;

    private bool isUpgrading = false;
    private int needGold = 0;

    private const string SUCCESS_TEXT = "타워 강화에 <color=#BBEE00>성공</color>하였습니다.";
    private const string FAILED_TEXT = "타워 강화에 <color=#D23B27>실패</color>하였습니다.";
    private const string FAILED_DOWN_TEXT = "타워 강화에 <color=#D23B27>실패</color>하여 강화 단계가 <color=#D23B27>하락</color>하였습니다.";
    private const string DESTROY_TEXT = "타워 강화에 <color=#000000>실패</color>하여 타워가 <color=#000000  >파괴</color>하였습니다.";

    private int failedCount = 0;

    private void Awake()
    {
        Inst = this;
        buildingLevelList = new Dictionary<string, int>();
        gameObject.SetActive(false);

    }

    private void Start()
    {
        transform.Find("UpgradeButton").GetComponent<Button>().onClick.AddListener(() => StartCoroutine(ClickUpgradeBtn()));
        transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(ClickCancelBtn);

        destroyProctedToggle.onValueChanged.AddListener((b) => SetUI());
    }


    public void AddBuilding(string name)
    {
        buildingLevelList.Add(name, 1);
    }

    private void ClickCancelBtn()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private IEnumerator ClickUpgradeBtn()
    {
        if (needGold > ResourceManager.Instance.GetResouceAmount(goldResourceType))
            yield break;

        if (isUpgrading) yield break;
        UpgradePercent percentData = GetUpgradePercent();

        ResourceManager.Instance.AddResource(goldResourceType, -needGold);

        if (starCatchToggle.isOn && percentData.successPecent != 100)
        {
            // 여기서 스타캐치
        }

        SoundManager.Instance.PlaySound(SoundManager.Sound.StarForce);
        isUpgrading = true;
        effectAnim.gameObject.SetActive(true);
        effectAnim.Play("Effect");

        yield return new WaitForSecondsRealtime(2.7f);
        effectAnim.StopPlayback();
        effectAnim.gameObject.SetActive(false);

        int randomNum = Random.Range(0, 100);

        if (randomNum < percentData.successPecent)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Success);
            Success();
        }

        else if (randomNum < percentData.successPecent + percentData.failPercent)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.Failed);
            Fail();
        }

        else
        {
            if (destroyProctedToggle.isOn)
            {
                SoundManager.Instance.PlaySound(SoundManager.Sound.Success);
                Success();
            }

            else
            {
                SoundManager.Instance.PlaySound(SoundManager.Sound.Destroy);
                Destroy();
            }
        }


        isUpgrading = false;
        SetUI();
    }
    private void Success()
    {
        failedCount = 0;
        starforceResultPanel.Open(BuildingType, SUCCESS_TEXT, false);
        buildingLevelList[currentBuilding.name]++;

        switch (BuildingType.nameString)
        {
            case "GoldHarvester":
            case "StoneHarvester":
            case "WoodHarvester":
                {
                    int level = GetLevel(currentBuilding.name);
                    float resourceTime = level == 1 ? 1f : (1f * Mathf.Pow(0.9f, level));
                    BuildingType.ChangeResourceGenerateTime(resourceTime);
                    break;
                }
        }
        int maxHealth = (int)(100f * Mathf.Pow(1.1f, GetLevel(currentBuilding.name)));
        BuildingType.ChangeMaxHealth(maxHealth);
    }

    private void Fail()
    {
        int level = GetLevel(currentBuilding.name);

        if (level > 15 && level != 20)
        {
            failedCount++;
            starforceResultPanel.Open(BuildingType, FAILED_DOWN_TEXT, false);
            buildingLevelList[currentBuilding.name]--;
            switch (BuildingType.nameString)
            {
                case "GoldHarvester":
                case "StoneHarvester":
                case "WoodHarvester":
                    {
                        float resourceTime = level == 1 ? 1f : (1f * Mathf.Pow(0.9f, level));
                        BuildingType.ChangeResourceGenerateTime(resourceTime);
                        break;
                    }
            }
        }
        else
        {
            starforceResultPanel.Open(BuildingType, FAILED_TEXT, false);
        }
    }

    private void Destroy()
    {
        starforceResultPanel.Open(BuildingType, DESTROY_TEXT, true);
        starforceResultPanel.OnClose += () =>
        {
            if (BuildingType.nameString != "HQ")
            {
                BuildingType.ChangeResourceGenerateTime(1f);
                buildingLevelList.Remove(currentBuilding.name);
                Destroy(currentBuilding.gameObject);
            }

            else
            {
                buildingLevelList[currentBuilding.name] = 1;
            }

            Time.timeScale = 1f;
            starforceResultPanel.OnClose = null;
            gameObject.SetActive(false);
        };


    }

    public void Open(Building building)
    {
        Time.timeScale = 0f;
        failedCount = 0;
        currentBuilding = building;
        SetSprite(buildingImage, BuildingType.sprite, new Vector2(165, 165));
        SetUI();

        gameObject.SetActive(true);
    }

    public void SetSprite(Image image, Sprite sprite, Vector2 maxSize)
    {
        image.sprite = sprite;

        Vector2 size = image.sprite.rect.size;

        float scale = 1f;
        if (size.y > maxSize.y)
        {
            scale = maxSize.y / size.y;
        }
        else if (size.x > maxSize.x)
        {
            scale = maxSize.x / size.x;
        }

        image.rectTransform.sizeDelta = size * scale;
    }

    private void SetUI()
    {
        int level = GetLevel(currentBuilding.name);
        destroyProctedToggle.interactable = 15 <= level && level < 17;

        if (level >= 25)
        {
            Debug.Log("만렙");
            return;
        }

        UpgradePercent percent = GetUpgradePercent(level);

        float[] statAmounts = GetStatAmount(currentBuilding.name, BuildingType);
        string[] statNames = GetStatString(BuildingType.nameString);
        if (statNames == null) return;
        if (statNames == null) return;

        upgradeInfoText.text = $"{level}성 > {level + 1}성\n성공확률: {percent.successPecent:0.00}%\n실패({(level > 15 && level != 20 ? "하락" : "유지")})확률: {percent.failPercent:0.00}%\n{(level >= 15 ? $"파괴확률: {percent.destroyPercent:0.00}%\n" : "")}\n{statNames[0]}: {statAmounts[0]:00.00}\n{statNames[1]} : {statAmounts[1]:00.00}";

        needGold = level == 1 ? 10 : (int)(10 * (1.25f * level));
        needGold = (int)(needGold * (destroyProctedToggle.isOn ? 1.5f : 1));
        needGoldText.text = string.Format("{0:#,###}", needGold);

        if (needGold > ResourceManager.Instance.GetResouceAmount(goldResourceType))
        {
            upgradeTitleText.text = "강화에 필요한 골드가 부족합니다.";
        }

        else
        {
            if (percent.successPecent == 100)
            {
                upgradeTitleText.text = "<color=#FFCC00>Chance Time!";
            }

            else if (level < 15)
            {
                upgradeTitleText.text = "<color=#FFCC00>골드</color>를 사용하여 타워를 강화합니다.";
            }

            else if (level == 15)
            {
                upgradeTitleText.text = "실패 시 장비가 <color=#FFCC00>파괴</color>될 수 있습니다.";
            }

            else if (level > 15)
            {
                upgradeTitleText.text = "실패 시 장비가 <color=#FFCC00>파괴</color>되거나 단계가 <color=#FFCC00>하락</color>될 수 있습니다.";
            }
        }
    }


    public UpgradePercent GetUpgradePercent(int idx = -1)
    {
        if (failedCount >= 2)
        {
            return new UpgradePercent { successPecent = 100, destroyPercent = 0, failPercent = 0 };
        }
        if (idx < 0)
        {
            idx = GetLevel(currentBuilding.name) - 1;
        }
        return upgradePercentList[idx];
    }

    public string GetStatText(string buildingName, BuildingTypeSO type)
    {
        int level = GetLevel(buildingName);
        int maxHP = level == 1 ? 100 : (int)(100f * Mathf.Pow(1.1f, level));
        switch (type.nameString)
        {
            case "Tower":
            case "HQ":
                {
                    int damageAmount = level == 1 ? 10 : (int)(10f * Mathf.Pow(1.1f, level));
                    return $"<color=#FF0000>MAXHP:</color> {maxHP}\n<color=#00FF00>ATK:</color> {damageAmount}";
                }

            case "GoldHarvester":
            case "StoneHarvester":
            case "WoodHarvester":
                {
                    float resourceTime = level == 1 ? 1f : (1f * Mathf.Pow(0.9f, level));
                    return $"<color=#FF0000>MAXHP:</color> {maxHP}\n<color=#00FF00>RESOURCE TIME:</color> {resourceTime:0.00}";
                }

        }
        return null;
    }

    public float[] GetStatAmount(string buildingName, BuildingTypeSO type)
    {
        int level = GetLevel(buildingName);
        int maxHealth = level == 1 ? 100 : (int)(100f * Mathf.Pow(1.1f, level));

        switch (type.nameString)
        {
            case "Tower":
            case "HQ":
                {
                    int damageAmount = level == 1 ? 10 : (int)(10f * Mathf.Pow(1.1f, level));
                    return new float[] { maxHealth, damageAmount };
                }

            case "GoldHarvester":
            case "StoneHarvester":
            case "WoodHarvester":
                {
                    float resourceTime = level == 1 ? 1f : (1f * Mathf.Pow(0.9f, level));
                    return new float[] { maxHealth, resourceTime };
                }
        }
        return null;
    }

    public string[] GetStatString(string buildingName)
    {
        switch (buildingName)
        {
            case "Tower":
            case "HQ":
                {
                    return new string[] { "최대 체력", "공격력" };
                }

            case "GoldHarvester":
            case "StoneHarvester":
            case "WoodHarvester":
                {
                    return new string[] { "최대 체력", "리소스 생성 시간" };
                }
        }
        return null;
    }

    public int GetLevel(string buildingName)
    {
        if (buildingLevelList.ContainsKey(buildingName) == false) return 1;
        return buildingLevelList[buildingName];
    }

}
