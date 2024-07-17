using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public Sprite sprite;
    public Transform prefab;
    public string objectName;
    public int objectCount;
    [SerializeField] public bool IsEquipable;
    [SerializeField] public bool IsDecoration;
    [SerializeField] public bool canbeToggled;
}
