using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour {

    public int health = 100;
    SpriteRenderer renderer;

	void Start () {
        renderer = GetComponent<SpriteRenderer>();
	}
	
    public void GetCleaned(int amount)
    {
        health -= amount;

        Color c = renderer.color;

        c.a = (health / 100f);

        renderer.color = c;

        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("score"), transform.position);

            int val = Random.Range(0, 50);

            for (int i = 0; i < val; i++)
            {
                Instantiate(Resources.Load("Money"), transform.position + Random.insideUnitSphere + Vector3.up, Random.rotation);
            }

            Destroy(gameObject);
        }
    }
}
