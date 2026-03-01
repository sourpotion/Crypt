using System.Collections;
using UnityEngine;

public class DDDTestingAudioSenderKK : MonoBehaviour
{
    public Vector3 soundLocation = Vector3.zero;
    public float amplutide = 60f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1f);

        GameMangeren.Instance.onMakingNoise?.Invoke(soundLocation, amplutide);
    }
}
