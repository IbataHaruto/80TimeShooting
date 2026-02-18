
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class TitleUIManager : MonoBehaviour
{
    [Header("ボタンを押した時に表示するPanel")]
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject GuidemenuPanel;
    [SerializeField] GameObject ControlGuidePanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject SoundPanel;
    [SerializeField] GameObject howToPlayPanel;
    [SerializeField] GameObject howToPlayNextPanel;
    [SerializeField] GameObject howToPlay2NextPanel;
    [SerializeField] GameObject howToPlay3NextPanel;
    [SerializeField] GameObject MouthOptionPanel;
    [SerializeField] GameObject ControllerOptionPanel;
    [SerializeField] GameObject ScreenSizeOptionPanel;
    [SerializeField] GameObject ADSOptionPanel;

    [Header("Panelを開いた時に最初選択されているボタン")]
    [SerializeField] Button titleFirstButton;
    [SerializeField] Button optionFirstSelectable;
    [SerializeField] Button GuidemenuFirstButton;
    [SerializeField] Button ControlGuideFirstButton;
    [SerializeField] Button howToPlayFirstButton;
    [SerializeField] Button howToPlayNextFirstButton;
    [SerializeField] Button howToPlay2NextFirstButton;
    [SerializeField] Button howToPlay3NextFirstButton;
    [SerializeField] Button SoundFirstbutton;
    [SerializeField] Button MouthFirstButton;
    [SerializeField] Button ControllerOptionFirstButton;
    [SerializeField] Button ScreenSizeOptionFirstButton;
    [SerializeField] Button ADSOptionFirstButton;

    [SerializeField] InputActionReference cancelAction;

    void OnEnable()
    {
        cancelAction.action.Enable();
    }

    void OnDisable()
    {
        cancelAction.action.Disable();
    }

    void Start()
    {
        OpenTitle();
    }

    void Update()
    {
        // キャンセルボタンでタイトルに戻る
        if ((optionPanel.activeSelf
            || howToPlayPanel.activeSelf
            || howToPlayNextPanel.activeSelf
            || howToPlay2NextPanel.activeSelf
            || howToPlay3NextPanel.activeSelf
            || GuidemenuPanel.activeSelf
            || ControlGuidePanel.activeSelf
            || MouthOptionPanel.activeSelf
            || ControllerOptionPanel.activeSelf
            || SoundPanel.activeSelf
            || ScreenSizeOptionPanel.activeSelf
            || ADSOptionPanel.activeSelf)

            && cancelAction.action.WasPressedThisFrame())
        {
            OpenTitle();
        }
    }

    public void OpenTitle()
    {
        SetPanel(titlePanel);
        StartCoroutine(SelectNextFrame(titleFirstButton));
    }

    public void OpenGuidemenuPanel()
    {
        SetPanel(GuidemenuPanel);
        StartCoroutine(SelectNextFrame(GuidemenuFirstButton));
    }

    public void OpenOption()
    {
        SetPanel(optionPanel);
        StartCoroutine(SelectNextFrame(optionFirstSelectable));
    }

    public void OpenHowToPlay()
    {
        SetPanel(howToPlayPanel);
        StartCoroutine(SelectNextFrame(howToPlayFirstButton));
    }

    public void OpenHowToPlayNextPanel()
    {
        SetPanel(howToPlayNextPanel);
        StartCoroutine(SelectNextFrame(howToPlayNextFirstButton));
    }

    public void OpenHowTo2NextPanel()
    {
        SetPanel(howToPlay2NextPanel);
        StartCoroutine(SelectNextFrame(howToPlay2NextFirstButton));
    }

    public void OpenHowToPlay3NextPanel()
    {
        SetPanel(howToPlay3NextPanel);
        StartCoroutine(SelectNextFrame(howToPlay3NextFirstButton));
    }

    public void OpenControlGuidePanel()
    {
        SetPanel(ControlGuidePanel);
        StartCoroutine(SelectNextFrame(ControlGuideFirstButton));
    }

    public void OpenMouthOptionPanel()
    {
        SetPanel(MouthOptionPanel);
        StartCoroutine(SelectNextFrame(MouthFirstButton));
    }

    public void OpenSoundOptionPanel()
    {
        SetPanel(SoundPanel);
        StartCoroutine(SelectNextFrame(SoundFirstbutton));
    }

    public void OpenControllerPanel()
    {
        SetPanel(ControllerOptionPanel);
        StartCoroutine(SelectNextFrame(ControllerOptionFirstButton));
    }

    public void OpenScreenSizeOptionPanel()
    {
        SetPanel(ScreenSizeOptionPanel);
        StartCoroutine(SelectNextFrame(ScreenSizeOptionFirstButton));
    }

    public void OpenADSOptionPanel()
    {
        SetPanel(ADSOptionPanel);
        StartCoroutine(SelectNextFrame(ADSOptionFirstButton));
    }


    void SetPanel(GameObject activePanel)
    {
        titlePanel.SetActive(false);
        optionPanel.SetActive(false);
        SoundPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        howToPlayNextPanel.SetActive(false);
        howToPlay2NextPanel.SetActive(false);
        howToPlay3NextPanel.SetActive(false);
        MouthOptionPanel.SetActive(false);
        ControllerOptionPanel.SetActive(false);
        ScreenSizeOptionPanel.SetActive(false);
        GuidemenuPanel.SetActive(false);
        ControlGuidePanel.SetActive(false);
        ADSOptionPanel.SetActive(false);

        activePanel.SetActive(true);
    }

    IEnumerator SelectNextFrame(Selectable selectable)
    {
        yield return null; 
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectable.gameObject);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;  // エディタなら再生停止
#else
    Application.Quit();                   // ビルドなら終了
#endif
    }
}
