using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scriptstart : MonoBehaviour
{
    [SerializeField] private CanvasGroup UIGroup;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;
    private bool doneError = false;

    public void ShowUI()
    {
        fadeIn = true;
    }

    public void HideUI()
    {
        fadeOut = true;
    }

    void Update()
    {
        if (doneError)
        {
            if (UIGroup.alpha >= 0)
            {
                UIGroup.alpha -= Time.deltaTime;
                if (UIGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    // Start is called before the first frame update

    private int counter = 10;
    public GameObject Error0;
    public GameObject Error1;
    public GameObject Error2;
    public GameObject Error3;
    public GameObject Error4;
    public GameObject Error5;
    public GameObject Error6;
    public GameObject Error7;

    ///public int Money = 10;

    //public Text ValueText;

    private void Start()
    {
        StartCoroutine(testCoroutine());
        ///ValueText.text = Money.ToString();
    }


    private IEnumerator testCoroutine()
    {
        Debug.Log($"Test counter: {counter}");


        counter++;

        yield return new WaitForSeconds(2f);
        Error0.SetActive(true);



        counter++;


        yield return new WaitForSeconds(2f);
        Error1.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Error2.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        Error3.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Error4.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        Error5.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Error6.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Error7.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        doneError = true;
    }
}
