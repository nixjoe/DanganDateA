using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public string zoneName;
    public Color debugColor = Color.red;
    public BoxCollider boxCollider;

    private void OnDrawGizmos()
    {
        BoxCollider collider = GetComponent<BoxCollider>();

        var c = debugColor;
        c.a = c.a / 2;

        Gizmos.color = c;
        Vector3 scale = transform.localScale;
        Gizmos.DrawCube(transform.position + collider.center, new Vector3(collider.size.x * scale.x, collider.size.y * scale.y, collider.size.z * scale.z));
    }

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Manager.Menu.location.text = zoneName;
        }
    }
}
