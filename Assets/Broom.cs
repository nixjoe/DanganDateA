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
        transform.SetParent(parent, true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.gameObject.layer = parent.gameObject.layer;


        GetComponent<Collider>().enabled = false;
        Destroy(GetComponent<Rigidbody>());
    }

    public void UnEquip(Vector3 pos)
    {
        transform.parent = null;
        transform.position = pos + Vector3.up;

        transform.gameObject.layer = LayerMask.NameToLayer("Default");

        GetComponent<Collider>().enabled = true;
        gameObject.AddComponent<Rigidbody>();
    }
}
