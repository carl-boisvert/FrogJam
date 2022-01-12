using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
[CreateAssetMenu(fileName = "tooltips", menuName ="Data/tooltips" )]
public class TooltipData : ScriptableObject
{
    [SerializeField] public List<TooltipInfo> tooltips;
}

[Serializable]
public class TooltipInfo
{
    [SerializeField] public String tag;
    [SerializeField] public Sprite icon;
    [SerializeField] public string text;
}
