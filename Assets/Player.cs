using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    new Camera camera;

    public AudioClip[] hitAudioClips;
    public Animator animator;
    Transform hand;

    public float hitForce;

    public GameObject equippedItem;

	void Start () {
        camera = Camera.main;


        hand = animator.transform.GetChild(0);

        if (hand.childCount > 0)
        {
            equippedItem = hand.GetChild(0).gameObject;
        }
	}

	void Update () {

        RaycastHit hit;
        bool isHit = Physics.Raycast(new Ray(camera.transform.position, camera.transform.forward), out hit, 3f);

        Debug.DrawRay(camera.transform.position, camera.transform.forward);

        if (Input.GetButtonDown("Fire1"))
        {
            animator.Play("Swing");

            if (isHit)
            {
                string tag = hit.collider.tag;

                switch (tag)
                {
                    case "Guest":
                        if (hit.collider.GetComponent<GuestAI>())
                        {
                            AudioSource.PlayClipAtPoint(hitAudioClips[Random.Range(0, hitAudioClips.Length)], transform.position);

                            hit.collider.GetComponent<GuestAI>().LaunchGuest(transform.forward * 2 + Vector3.up * 3 + (Vector3)Random.insideUnitCircle);
                        }

                        break;

                    case "Dirt":
                        if (hit.collider.GetComponent<Dirt>())
                        {
                            AudioSource.PlayClipAtPoint(hitAudioClips[Random.Range(0, hitAudioClips.Length)], transform.position);

                            hit.collider.GetComponent<Dirt>().GetCleaned(25);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        if (isHit)
        {
            switch (hit.collider.tag)
            {
                case "Equipable":
                    Manager.Menu.ShowTooltip(Time.deltaTime, "Press E to pick up " + hit.collider.name);

                    if (Input.GetButtonDown("Interact"))
                    {
                        if (equippedItem)
                        {
                            equippedItem.GetComponent<IEquippable>().UnEquip(hit.point);
                        }
                        hit.collider.GetComponent<IEquippable>().Equip(hand);

                        equippedItem = hit.collider.gameObject;
                    }
                    break;

                default:
                    break;
            }

        }

	}
}
