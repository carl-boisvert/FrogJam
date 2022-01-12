using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tooltip1Text;
    [SerializeField] private Image _tooltip1keyImage;
    [SerializeField] private TextMeshProUGUI _tooltip2Text;
    [SerializeField] private Image _tooltip2keyImage;

    public void hideToolTips()
    {
        _tooltip1Text.enabled = false;
        _tooltip1keyImage.enabled = false;
        _tooltip2Text.enabled = false;
        _tooltip2keyImage.enabled = false;
    }

    public void SetInfoTooltip1(TooltipInfo info)
    {
        _tooltip1Text.enabled = true;
        _tooltip1keyImage.enabled = true;
        _tooltip1Text.text = info.text;
        _tooltip1keyImage.sprite = info.icon;
    }
    
    public void SetInfoTooltip2(TooltipInfo info)
    {
        _tooltip2Text.enabled = true;
        _tooltip2keyImage.enabled = true;
        _tooltip2Text.text = info.text;
        _tooltip2keyImage.sprite = info.icon;
    }
}
