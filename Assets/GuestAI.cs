using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuestAI : MonoBehaviour {

    NavMeshAgent agent;
    private Rigidbody rb;
    public float minDelay, maxDelay;

    new AudioSource audio;

    public float radius;
    public float collideForce = 4;
    bool inAir = false;

    public AudioClip[] collideClips;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

        FindNewPath();
	}
	
    void FindNewPath()
    {
        Vector3 random = (Vector3)Random.insideUnitCircle * radius;
        random.z = random.y;
        random.y = 0;
        agent.SetDestination(transform.position + random);

        Invoke("FindNewPath", Random.Range(minDelay, maxDelay));
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, radius);
    }

    public void LaunchGuest(Vector3 force)
    {
        if (!inAir)
        {
            CancelInvoke();

            inAir = true;
            agent.enabled = false;

            StartCoroutine(DelayedEnableGuest(4f));
        }

        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
    }

    IEnumerator DelayedEnableGuest(float time)
    {
        yield return new WaitForSeconds(time);

        while(Mathf.Abs(rb.velocity.y) > 0.1f)
        {
            yield return new WaitForSeconds(0.5f);
        }

        time = 0;

        while (time < 0.5f)
        {
            time += Time.deltaTime;
            rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.identity, time / 0.5f);
            yield return new WaitForEndOfFrame();
        }

        rb.isKinematic = true;
        inAir = false;
        agent.enabled = true;

        FindNewPath();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.transform.tag)
        {
            case "Player":

                audio.PlayOneShot(collideClips[Random.Range(0, collideClips.Length)]);

                StartCoroutine(PuntPlayer(collision.gameObject));

                break;

            default:
                break;
        }
    }

    IEnumerator PuntPlayer(GameObject player)
    {
        Vector3 dir = (transform.position - player.transform.position).normalized;
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);

        float time = 0;
        while (time < 0.1f)
        {
            player.transform.Translate(dir * Time.deltaTime * 2);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }
}
