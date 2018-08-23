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
            Destroy(gameObject);
        }
    }
}
