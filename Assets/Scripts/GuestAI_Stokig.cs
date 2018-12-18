using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestAI_Stokig : GuestAI, IEquippable
{
    private float[] spectrum;

    bool isPickedUp;

    public Transform mouth;
    float originalMouthScale;

    public AudioClip[] angerClips;

    float randOffset;

    public override void Start()
    {
        base.Start();
        spectrum = new float[64];

        randOffset = Random.value * 10;

        audio.clip = angerClips[Random.Range(0, angerClips.Length)];
        audio.Play((ulong)Random.value);

        originalMouthScale = mouth.localScale.y;
    }

    void Update()
    {
        var v = mouth.localScale;

        float sum = 0;

        audio.GetSpectrumData(spectrum, 0, FFTWindow.Triangle);

        for (int i = 0; i < spectrum.Length; i++)
        {
            sum += spectrum[i];
        }

        v.y = Mathf.Lerp(0, originalMouthScale, sum / 0.5f / audio.volume);

        audio.pitch = 1 + Mathf.Sin(Time.time + Random.value + randOffset) * 0.2f;

        mouth.localScale = v;

        if (isPickedUp)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 7);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 7);
        }

    }

    public override void LaunchGuest(Vector3 force)
    {
        tag = "Equipable";

        base.LaunchGuest(force);
    }

    protected override void RestoreGuest()
    {
        if (!isPickedUp)
        {
            tag = "Guest";

            base.RestoreGuest();
        }
    }

    public void Equip(Transform parent)
    {
        isPickedUp = true;

        transform.SetParent(parent, true);
        transform.gameObject.layer = parent.gameObject.layer;

        GetComponent<Collider>().enabled = false;

        Destroy(GetComponent<Rigidbody>());
        //GetComponent<Rigidbody>().isKinematic = true;
    }

    public void UnEquip(Vector3 pos)
    {
        isPickedUp = false;

        transform.parent = null;
        transform.position = pos + Vector3.up;

        transform.gameObject.layer = LayerMask.NameToLayer("Default");

        GetComponent<Collider>().enabled = true;

        if (!GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    public bool IsWide()
    {
        return false; 
    }
}
