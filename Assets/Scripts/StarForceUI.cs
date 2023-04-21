using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private List<UpgradePercent> upgradePercentList;

    [SerializeField]
    private Animator effectAnim;

    private bool isUpgrading = false;

    private void Awake()
    {
        Inst = this;
        buildingLevelList = new Dictionary<string, int>();
        gameObject.SetActive(false);

    }

    private void Start()
    {
        foreach (var buildingType in BuildingManager.Instance.BuildingTypeList.list)
        {
            buildingLevelList.Add(buildingType.nameString, 1);
        }

        transform.Find("UpgradeButton").GetComponent<Button>().onClick.AddListener(ClickUpgradeBtn);
        transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(ClickCancelBtn);
    }
    private void ClickCancelBtn()
    {
        gameObject.SetActive(false);
    }

    private async void ClickUpgradeBtn()
    {
        if (isUpgrading) return;
        isUpgrading = true;
        effectAnim.gameObject.SetActive(true);
        effectAnim.Play("Effect");

        await Task.Delay(1600);
        isUpgrading = false;
        effectAnim.StopPlayback();
        effectAnim.gameObject.SetActive(false);

        UpgradePercent percentData = GetUpgradePercent();
        if (percentData == null)
            return;

        int randomNum = Random.Range(0, 100);

        if(randomNum < percentData.successPecent)
        {
            Debug.Log("성공!");
            buildingLevelList[BuildingType.nameString]++;
            Success();
        }

        else if(randomNum < percentData.successPecent + percentData.failPercent)
        {
            Debug.Log("실패!");
            Fail();
        }

        else
        {
            Debug.Log("파괴");
            Destroy();
        }

        SetInfoText();
    }
    private void Success()
    {
        switch (BuildingType.nameString)
        {
            case "GoldHarvester":
            case "StoneHarvester":
            case "WoodHarvester":
                {
                    float resourceTime = (1.5f * Mathf.Pow(0.9f, GetLevel(BuildingType.nameString)));
                    BuildingType.ChangeResourceGenerateTime(resourceTime);
                    break;
                }
        }
        int maxHealth = (int)(100f * Mathf.Pow(1.1f, GetLevel(BuildingType.nameString)));
        BuildingType.ChangeMaxHealth(maxHealth);
    }

    private void Fail()
    {
        int level = GetLevel(BuildingType.nameString);
        if (level > 15 && level != 20)
        {
            buildingLevelList[BuildingType.nameString]--;
        }
    }

    private void Destroy()
    {
        if(BuildingType.nameString == "HQ")
        {
            buildingLevelList[BuildingType.nameString] = 1;
        }

        Destroy(currentBuilding.gameObject);
        gameObject.SetActive(false);
    }

    public void Open(Building building)
    {
        Time.timeScale = 0f;
        currentBuilding = building;
        SetSprite(buildingImage, BuildingType.sprite, new Vector2(165,165));
        SetInfoText();
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

    private void SetInfoText()
    {
        int level = GetLevel(BuildingType.nameString);
        if(level >= 25)
        {
            Debug.Log("만렙");
            return;
        }

        UpgradePercent percent = GetUpgradePercent(level);

        float[] statAmounts = GetStatAmount(BuildingType.nameString);
        string[] statNames = GetStatString(BuildingType.nameString);
        if (statNames == null) return;
        if (statNames == null) return;

        upgradeInfoText.text = $"{level}성 > {level + 1}성\n성공확률: {percent.successPecent:0.00}%\n실패({(level > 15 && level != 20 ? "하락" : "유지")})확률: {percent.failPercent:0.00}%\n{(level >= 15 ? $"파괴확률: {percent.destroyPercent:0.00}%\n" : "")}\n{statNames[0]}: {statAmounts[0]:00.00}\n{statNames[1]} : {statAmounts[1]:00.00}";
    }

    public UpgradePercent GetUpgradePercent(int idx = -1)
    {
        if (idx < 0)
        {
            idx = GetLevel(BuildingType.nameString) - 1;
        }
        return upgradePercentList[idx];
    }

    public float[] GetStatAmount(string buildingName)
    {
        int maxHealth = (int)(100f * Mathf.Pow(1.1f, GetLevel(buildingName)));
        switch (buildingName)
        {
            case "Tower":
            case "HQ":
                {
                    int damageAmount = (int)(10f * Mathf.Pow(1.1f, GetLevel(buildingName)));
                    return new float[] { maxHealth, damageAmount };
                }

            case "GoldHarvester":
            case "StoneHarvester":
            case "WoodHarvester":
                {
                    float resourceTime = (1.5f * Mathf.Pow(0.9f, GetLevel(buildingName)));
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
