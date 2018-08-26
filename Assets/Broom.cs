using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable
{
    void Equip(Transform parent);
    void UnEquip(Vector3 pos);
    bool IsWide();
}

public class Broom : MonoBehaviour, IEquippable {

    public bool swingWide;
    bool equipped;

    public void Equip(Transform parent)
    {
        transform.SetParent(parent, true);
        transform.gameObject.layer = parent.gameObject.layer;

        GetComponent<Collider>().enabled = false;
        Destroy(GetComponent<Rigidbody>());

        equipped = true;
    }

    public bool IsWide()
    {
        return swingWide;
    }

    public void UnEquip(Vector3 pos)
    {
        equipped = false;

        transform.parent = null;
        transform.position = pos + Vector3.up;

        transform.gameObject.layer = LayerMask.NameToLayer("Default");

        GetComponent<Collider>().enabled = true;

        if (!GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    void Update()
    {
        if (equipped)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 7);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 7);
        }
    }
}
