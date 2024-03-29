using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private GameObject simonAnim,
        red, yellow, blue, green, mainMenu, game, gameOver;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverFX, clickFX;
    private bool mainMenuActive = true, selectColor = false, timeChange;
    private Vector3 initRotationVector;
    private float animTime = 0;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        mainMenu.SetActive(true);
        game.SetActive(false);
        gameOver.SetActive(false);

        initRotationVector = simonAnim.transform.rotation.eulerAngles;
        RotateBackAndForth();
    }

    private void Update()
    {
        if (mainMenuActive)
        {
            if (!selectColor && Time.time > animTime + 1f)
            {
                animTime = Time.time;
                selectColor = true;
                timeChange = true;
                SelectBlock(UnityEngine.Random.Range(0, 4));
            }

            if (timeChange)
            {
                timeChange = false;
                selectColor = false;
            }
        }
    }

    public void RotateBackAndForth()
    {
        simonAnim.transform.DOLocalRotate(new Vector3(0, 20, 0), 2f)
            .SetRelative(true)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void SimulateButtonClick(Button button)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(button.gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
    }

    public void SimulateButtonRelease(Button button)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(button.gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
    }

    public void SelectBlock(int block)
    {
        if (block == 0)
            SelectRed();
        else if (block == 1)
            SelectYellow();
        else if (block == 2)
            SelectBlue();
        else if (block == 3)
            SelectGreen();
    }

    public void SelectRed()
    {
        red.SetActive(true);
        yellow.SetActive(false);
        blue.SetActive(false);
        green.SetActive(false);
    }

    public void SelectYellow()
    {
        red.SetActive(false);
        yellow.SetActive(true);
        blue.SetActive(false);
        green.SetActive(false);
    }

    public void SelectBlue()
    {
        red.SetActive(false);
        yellow.SetActive(false);
        blue.SetActive(true);
        green.SetActive(false);
    }

    public void SelectGreen()
    {
        red.SetActive(false);
        yellow.SetActive(false);
        blue.SetActive(false);
        green.SetActive(true);
    }
    public void TurnOffSimonAnim()
    {
        red.SetActive(false);
        yellow.SetActive(false);
        blue.SetActive(false);
        green.SetActive(false);
    }

    public void StartAnim()
    {
        mainMenuActive = true;
        selectColor = false;
        animTime = Time.time;
    }

    public void StopAnim()
    {
        mainMenuActive = false;
        //simonAnim.transform.rotation = Quaternion.Euler(initRotationVector);
        TurnOffSimonAnim();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowMainMenu()
    {
        StartAnim();
        mainMenu.SetActive(true); 
        game.SetActive(false); 
        gameOver.SetActive(false);
    }

    public void ShowGame()
    {
        StopAnim();
        mainMenu.SetActive(false);
        game.SetActive(true);
        gameOver.SetActive(false);
    }

    public void ShowGameOver()
    {
        StopAnim();
        mainMenu.SetActive(false);
        game.SetActive(false);
        gameOver.SetActive(true);
    }

    public void MouseHovering()
    {
        audioSource.clip = hoverFX;
        audioSource.Play();
    }

    public void MouseClicking()
    {
        audioSource.clip = clickFX;
        audioSource.Play();
    }
}