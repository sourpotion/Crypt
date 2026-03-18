using UnityEngine;
using TMPro;
using System.Collections;

public class TaskScript : MonoBehaviour
{
    public TextMeshProUGUI taskUI;
    public string currentTask;
    public bool tempItemChecker;
    public int taskNumber;

    void Start()
    {
        tempItemChecker = false;
        taskNumber = 1;
        Task1();
    }

    void Update()
    {
        
    }

    public void Task1()
    {
        currentTask = "Task: Find a key to open the gates to get to the second floor";
        taskUI.text = currentTask;
    }

    public void Task1Checker(GameObject hit)
    {
        // a way to check if player has the key so he can open the gates

        if (currentTask == "Task: Find a key to open the gates to get to the second floor")
        {
            if (hit.CompareTag("Key"))
            {
                taskUI.text = "Good job, now find the gates and open them";
                tempItemChecker = true; //temporary!!!
            }
            if (tempItemChecker == true) //temporary!!!
            {
                if (hit.CompareTag("Gates"))
                {
                    taskUI.text = "The gates are open, now find the stairs to get to the second floor";
                    StartCoroutine(Task1Wait());
                    tempItemChecker = false; //temporary!!!
                    taskNumber ++;
                }
            }
        }
    }

    public IEnumerator Task1Wait()
    {
        yield return new WaitForSeconds(3f);
        Task2();
    }

    public void Task2()
    {
        currentTask = "Task: temp2"; //temporary task
        taskUI.text = currentTask;
    }

    public void Task2Checker(GameObject hit)
    {
        //go make some creative tasks

        if (currentTask == "Task: temp2")
        {
            if (hit.CompareTag("Temp2.1"))
            {
                taskUI.text = "Good job";
                tempItemChecker = true; //temporary!!!
            }
            if (tempItemChecker == true) //temporary!!!
            {
                if (hit.CompareTag("Temp2.2"))
                {
                    taskUI.text = "Well done";
                    StartCoroutine(Task2Wait());
                    tempItemChecker = false; //temporary!!!
                    taskNumber ++;
                }
            }
        }
    }

    public IEnumerator Task2Wait()
    {
        yield return new WaitForSeconds(3f);
        Task3();
    }

    public void Task3()
    {
        currentTask = "Task: temp3"; //temporary task
        taskUI.text = currentTask;
    }

    public void Task3Checker(GameObject hit)
    {
        //go make some creative tasks

        if (currentTask == "Task: temp3")
        {
            if (hit.CompareTag("Temp3.1"))
            {
                taskUI.text = "Good job";
                tempItemChecker = true; //temporary!!!
            }
            if (tempItemChecker == true) //temporary!!!
            {
                if (hit.CompareTag("Temp3.2"))
                {
                    taskUI.text = "Well done";
                    StartCoroutine(Task3Wait());
                    tempItemChecker = false; //temporary!!!
                    taskNumber ++;
                }
            }
        }
    }

    public IEnumerator Task3Wait()
    {
        yield return new WaitForSeconds(3f);
        Task4();
    }

    public void Task4()
    {
        currentTask = "Task: temp4"; //temporary task
        taskUI.text = currentTask;
    }

    public void Task4Checker(GameObject hit)
    {
        //go make some creative tasks

        if (currentTask == "Task: temp4")
        {
            if (hit.CompareTag("Temp4.1"))
            {
                taskUI.text = "Good job";
                tempItemChecker = true; //temporary!!!
            }
            if (tempItemChecker == true) //temporary!!!
            {
                if (hit.CompareTag("Temp4.2"))
                {
                    taskUI.text = "Well done";
                    StartCoroutine(Task4Wait());
                    tempItemChecker = false; //temporary!!!
                    taskNumber ++;
                }
            }
        }
    }

    public IEnumerator Task4Wait()
    {
        yield return new WaitForSeconds(3f);
        //Task5();
    }
}