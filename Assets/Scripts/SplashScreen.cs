using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class SplashScreen : MonoBehaviour
{
    public Image splashScreen;
    
    [Header("Sfx")]
    public AudioSource birdSfx;

    private string startSceneName = "Basic StartScreen";
    private float waitTimeBeforeSplash = 2f;
    private float waitBeforeEnding = 2f;
    private float waitTimeAfterSplash = 1f;
    private float tweenTime = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color c = splashScreen.color;
        c.a = 0;
        splashScreen.color = c; //debug

        StartCoroutine(DoTheSplash());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchScene()
    {
        SceneManager.LoadScene(startSceneName);
    }

    IEnumerator DoTheSplash()
    {
        yield return new WaitForSeconds(waitTimeBeforeSplash);

        splashScreen.DOFade(1f, tweenTime).WaitForCompletion(); //tween in

        birdSfx.Play();
        yield return new WaitForSeconds(waitBeforeEnding);

        splashScreen.DOFade(0f, tweenTime).WaitForCompletion(); //fade out

        yield return new WaitForSeconds(waitTimeAfterSplash);

        SwitchScene(); //switch scene
    }
}
