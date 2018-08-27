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

    public int healthMax = 100, health;
    private float invTime;

    public float throwForce = 10;
    float throwForceMultiplier;

    float hitCooldown;
    bool equippedWide;

    void Start () {
        camera = Camera.main;
        hand = animator.transform.GetChild(0);

        if (hand.childCount > 0)
        {
            equippedItem = hand.GetChild(0).gameObject;
            equippedItem.GetComponent<IEquippable>().Equip(hand);
        }

        health = healthMax;

        DealDamage(0);
	}

	void Update () {

        RaycastHit hit;
        bool isHit = Physics.Raycast(new Ray(camera.transform.position, camera.transform.forward), out hit, 3f);



        if (Input.GetButtonDown("Fire1"))
        {
            Collider[] hits = { hit.collider };

            if (equippedWide)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("SwingWide") && animator.GetCurrentAnimatorStateInfo(0).length > 0.15f)
                {
                    animator.SetBool("SecondSwing", true);
                }
                else
                {
                    animator.Play("SwingWide");
                    animator.SetBool("SecondSwing", false);
                }

                hits = Physics.OverlapBox(camera.transform.position + camera.transform.forward * 1.5f, new Vector3(2, 1f, 2f), camera.transform.rotation);
            }
            else
            {
                animator.Play("Swing");
            }
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("swing"), transform.position);

            if (isHit)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    var currentCol = hits[i];
                    string tag = currentCol.tag;

                    switch (tag)
                    {
                        case "Guest":
                            if (currentCol.GetComponent<GuestAI>())
                            {
                                AudioSource.PlayClipAtPoint(hitAudioClips[Random.Range(0, hitAudioClips.Length)], transform.position);

                                currentCol.GetComponent<GuestAI>().LaunchGuest(transform.forward * 2 + Vector3.up * 3 + (Vector3)Random.insideUnitCircle);
                            }
                            break;

                        case "Dirt":
                            if (hit.collider.GetComponent<Dirt>())
                            {
                                AudioSource.PlayClipAtPoint(hitAudioClips[Random.Range(0, hitAudioClips.Length)], transform.position);

                                currentCol.GetComponent<Dirt>().GetCleaned(25);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.Play("Charge");
        }

        if (Input.GetButton("Fire2"))
        {

            throwForceMultiplier += Time.deltaTime * 2;
            throwForceMultiplier = Mathf.Clamp01(throwForceMultiplier);
        }

        if (Input.GetButtonUp("Fire2") && equippedItem)
        {
            animator.Play("Swing");

            equippedItem.GetComponent<IEquippable>().UnEquip(camera.transform.position + camera.transform.forward);
            equippedItem.GetComponent<Rigidbody>().AddForce(camera.transform.forward * throwForce * throwForceMultiplier, ForceMode.Impulse);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("swing"), transform.position);
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

                        equippedWide = hit.collider.GetComponent<IEquippable>().IsWide();
                        hit.collider.GetComponent<IEquippable>().Equip(hand);
                        equippedItem = hit.collider.gameObject;

                        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("reload"), transform.position);

                    }
                    break;

                default:
                    break;
            }

        }
	}

    public void CooldownUpdate()
    {
        invTime -= Time.deltaTime;
        hitCooldown -= Time.deltaTime;
    }

    public void DealDamage(int amount)
    {
        if (invTime < 0)
        {
            invTime = 1f;

            health -= amount;
            Manager.Menu.SetHealth(health);
        }
    }
}
