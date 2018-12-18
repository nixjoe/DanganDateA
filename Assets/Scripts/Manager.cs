using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager instance;
    private MenuHandler menu;

    Zone[] zones;
    private float timeUntilNextObjective = 10;
    private float currentTime = 10;

    public GameObject dirtPrefab;
    int moneyCount = 0;

    void Start () {
		
        if (FindObjectsOfType<Manager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            Init();
        }

	}
	
    void Init()
    {
        menu = FindObjectOfType<MenuHandler>();

        zones = FindObjectsOfType<Zone>();

        AddMoney(0);
    }

    private void Update()
    {
        if (currentTime < 0)
        {
            //SpawnObjective();

            currentTime = timeUntilNextObjective;
        } 
        
        currentTime -= Time.deltaTime;
    }

    public static void AddMoney(int count)
    {
        instance.moneyCount += count;
        Menu.money.text = instance.moneyCount.ToString();
    }

    void SpawnObjective()
    {
        Zone zone = zones[Random.Range(0, zones.Length)];

        Vector3 max = zone.boxCollider.bounds.max;


        Vector3 pos = zone.transform.position + new Vector3(Random.value * max.x, 0, Random.value * max.z);

        UnityEngine.AI.NavMeshHit hit;

        UnityEngine.AI.NavMesh.SamplePosition(pos, out hit, 5f, UnityEngine.AI.NavMesh.AllAreas);
        pos = hit.position;

        Instantiate(dirtPrefab, pos, dirtPrefab.transform.rotation);

        Menu.ShowPopup(5, "CLEAN UP SPY IN " + zone.zoneName);
    }


    public static MenuHandler Menu
    {
        get
        {
            return instance.menu;
        }   
    }
}
