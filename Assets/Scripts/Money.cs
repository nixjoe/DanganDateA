using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().AddExplosionForce(500, transform.position + Vector3.down + Random.insideUnitSphere, 2, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Manager.AddMoney(1);

            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("yes"), transform.position);

            Destroy(gameObject);
        }
    }
}
