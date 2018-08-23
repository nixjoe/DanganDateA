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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        hand = animator.transform.GetChild(0);
	}

	void Update () {

        RaycastHit hit;
        bool isHit = Physics.Raycast(new Ray(camera.transform.position, camera.transform.forward), out hit);

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

                            hit.collider.GetComponent<GuestAI>().LaunchGuest();
                        }
                        if (hit.collider.GetComponent<Rigidbody>())
                        {
                            hit.collider.GetComponent<Rigidbody>().AddForce(Vector3.up * 10 + Random.insideUnitSphere * 4, ForceMode.Impulse);
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
                            equippedItem
                        }
                        hit.collider.GetComponent<IEquippable>().Equip(hand);
                    }
                    break;

                default:
                    break;
            }

        }

	}
}
