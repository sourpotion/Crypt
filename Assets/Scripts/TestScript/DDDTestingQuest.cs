using System.Collections;
using UnityEngine;

public class DDDTestingQuest : MonoBehaviour
{
    QuestMangeren qeustMangeren;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        qeustMangeren = QuestMangeren.Instance;
        qeustMangeren.AddQuests(1);
    }

    IEnumerator Test()
    {
        qeustMangeren.AddQuests(1);
        yield return new WaitForSeconds(2f);
        qeustMangeren.AddQuests(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
