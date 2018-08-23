using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuestAI : MonoBehaviour {

    NavMeshAgent agent;
    private Rigidbody rb;
    public float minDelay, maxDelay;

    public float radius;

    bool inAir = false;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
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

    public void LaunchGuest()
    {
        if (!inAir)
        {
            CancelInvoke();

            inAir = true;

            agent.enabled = false;

            StartCoroutine(DelayedEnableGuest(4f));
        }

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

        inAir = false;
        agent.enabled = true;

        FindNewPath();
    }
}
