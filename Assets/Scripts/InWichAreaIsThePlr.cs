using UnityEngine;

//[DisallowMultipleComponent] //so it can't have 3 script that do the same
public class InWichAreaIsThePlr : MonoBehaviour //can go multiply i hope else ping my and i fix it so one script can do all
{
    [HideInInspector] public int areaId; //id of the area
    [HideInInspector] public Enemie enemieScript; //enemie

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider areaCollider = GetComponent<Collider>();

        if (enemieScript.target != null && areaCollider.bounds.Contains(enemieScript.target.transform.position)) {enemieScript.areaId = areaId;}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == enemieScript.target) {enemieScript.areaId = areaId;}
    }
}
