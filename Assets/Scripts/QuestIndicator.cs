using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestIndicator : MonoBehaviour {

    public GameObject QuestPoint;
    Transform target;
    GameObject exclamationPoint;

	// Use this for initialization
	void Start () {
        exclamationPoint = Instantiate(QuestPoint);
        exclamationPoint.transform.SetParent(GameObject.FindGameObjectWithTag("NPC").transform, false);
	}

    private void Awake()
    {
        target = gameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
        exclamationPoint.transform.position = Camera.main.WorldToScreenPoint(target.position);
	}
}
