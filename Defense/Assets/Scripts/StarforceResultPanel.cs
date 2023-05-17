using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StarforceResultPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text resultText;

    [SerializeField]
    private Image beforeImage;

    [SerializeField]
    private Image afterImage;

    public Action OnClose;

    private void Start()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(Close);
        Close();
    }

    public void Open(BuildingTypeSO building, string text, bool isDestroy)
    {
        SetSprite(beforeImage, building.sprite, new Vector2(58, 58));
        SetSprite(afterImage, isDestroy ? building.destroySprite : building.sprite, new Vector2(58, 58));
        resultText.text = text;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        OnClose?.Invoke();
        gameObject.SetActive(false);
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

}
