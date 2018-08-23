using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable
{
    void Equip(Transform parent);
    void UnEquip(Vector3 pos);
}

public class Broom : MonoBehaviour, IEquippable {

    public void Equip(Transform parent)
    {
        transform.SetParent(parent);
        transform.gameObject.layer = parent.gameObject.layer;
    }

    public void UnEquip(Vector3 pos)
    {
        transform.parent = null;
        transform.position = pos + Vector3.up;
    }
}
