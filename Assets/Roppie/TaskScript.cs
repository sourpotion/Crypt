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
                    StartCoroutine(TaskWait(3f, 2));
                    tempItemChecker = false; //temporary!!!
                    taskNumber ++;
                }
            }
        }
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
                    StartCoroutine(TaskWait(3f, 3));
                    tempItemChecker = false; //temporary!!!
                    taskNumber ++;
                }
            }
        }
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
                    StartCoroutine(TaskWait(3f, 4));
                    tempItemChecker = false; //temporary!!!
                    taskNumber ++;
                }
            }
        }
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
                    StartCoroutine(TaskWait(3f, 5));
                    tempItemChecker = false; //temporary!!!
                    taskNumber ++;
                }
            }
        }
    }

public IEnumerator TaskWait(float time, int taskNumber)
{
    yield return new WaitForSeconds(time);


    if (taskNumber == 2)
    {
        Task2();
    } 
    else if (taskNumber == 3)
    {
        Task3();
    }
    else if (taskNumber == 4)
    {     
        Task4();
    } 
    //else if (taskNumber == 5)
    //{
    //  Task5();
    //} 
}
}