using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using System.IO;
using Crosstales.FB;

[System.Serializable]
public class AddonsSliderEvent : UnityEvent<float, GameObject> { }

[Serializable]
public class AddonsButtons
{
    public Button button;
    public GameObject panelToOpen;
}

[Serializable]
public class AddonsSliders
{
    public Slider slider;
    public float sliderMinValue;
    public float sliderMaxValue;
    public float defaultSliderValue;
    public GameObject objectToSetSize;
    public AddonsSliderEvent sliderFunction;
}


[Serializable]
public class Toggles
{
    public Toggle toggle;
    public bool toggleDefault;
}

public class AddonsManager : MonoBehaviour
{
    public static AddonsManager instance;

public MenuManager menuManager;
    private OpenVirtualKeyboard openVirtualKeyboard;
    private FluidBehaviourScript fluidBehaviourScript;

    public Color clockColor, textColor, defClockColor,defTextColor;
    public Transform logoDefPos, clockDefPos, userTextDefPos, keyboardSpawnPoint;
    public GameObject logo, userClock, userText;
    public Toggle textOnOffTggl, clockOnOffTggl, logoOnOffTggl, _24_formatClockTggl, showSecondsTggl;
    public Slider textSizeSld, clockSizeSld, logoSizeSld;
    public Dropdown textFontsDropdown, clockFontsDropdown;
    public Button dragTextBttn, dragLogoBttn, dragClockBttn,openKeyboardBttn, resetTextPosBttn, resetLogoPosBttn, resetClockPosBttn, browseLogoBttn;
    public Transform[] addonsObjects;
    public TMP_FontAsset[] fontAssets;
    public int clockSelectedFontIndex;
    public int textSelectedFontIndex;
    public int defaultClockFontIndex;
    public int defaultTextSelectedIndex;

    public bool clockDrag, logoDrag, textDrag;

    public List<AddonsButtons> addonsButtons = new List<AddonsButtons>();
    // Start is called before the first frame update
    public List<Toggles> togglesList = new List<Toggles>();
    public List<AddonsSliders> addonsSlidersList = new List<AddonsSliders>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        openVirtualKeyboard = FindObjectOfType<OpenVirtualKeyboard>();
           
            if (!PlayerPrefs.HasKey("StartedFirstTime"))
                LoadAddonsAtStart(true);
            else if (PlayerPrefs.HasKey("StartedFirstTime"))
                LoadAddonsAtStart(false);

        clockDrag = false;
        logoDrag = false;
        textDrag = false;

        #region startFunctions
        //  fluidBehaviourScript = FindObjectOfType<FluidBehaviourScript>();
        clockOnOffTggl.onValueChanged.AddListener(delegate { TurnOffAllDrags(); TurnOnOffClock(clockOnOffTggl.isOn); });
        textOnOffTggl.onValueChanged.AddListener(delegate { TurnOffAllDrags(); TurnOnOffText(textOnOffTggl.isOn); });
        logoOnOffTggl.onValueChanged.AddListener(delegate { TurnOffAllDrags(); TurnOnOffLogo(logoOnOffTggl.isOn); });


        dragClockBttn.onClick.AddListener(() => { clockDrag = !clockDrag; DragItem(dragClockBttn, userClock, clockDrag); });
        dragTextBttn.onClick.AddListener(() => { textDrag = !textDrag; DragItem(dragTextBttn, userText, textDrag); });
        dragLogoBttn.onClick.AddListener(() => { logoDrag = !logoDrag; DragItem(dragLogoBttn, logo, logoDrag); });

        browseLogoBttn.onClick.AddListener(() => { TurnOffAllDrags(); StartCoroutine(LateLogoBrowse()); });

        resetClockPosBttn.onClick.AddListener(() => { TurnOffAllDrags(); userClock.transform.position = clockDefPos.position; SaveAddons(); });
        resetLogoPosBttn.onClick.AddListener(() => { TurnOffAllDrags(); logo.transform.position = logoDefPos.position; SaveAddons(); });
        resetTextPosBttn.onClick.AddListener(() => { TurnOffAllDrags(); userText.transform.position = userTextDefPos.position; SaveAddons(); });

        openKeyboardBttn.onClick.AddListener(() => { TurnOffAllDrags(); openVirtualKeyboard.OpenKeyboard(); });

        AddMenuButtonsListeners();
        #endregion

    }

    IEnumerator LateLogoBrowse()
    {
        yield return new WaitForSeconds(0.7f);
        BrowseLogoFile();
    }
    private void FixedUpdate()
    {
        if (userClock.gameObject.activeInHierarchy)
        {
            if (_24_formatClockTggl.isOn)
            {
                if (showSecondsTggl.isOn)
                    userClock.GetComponent<TextMeshProUGUI>().text = DateTime.Now.ToString("HH:mm:ss");
                else
                    userClock.GetComponent<TextMeshProUGUI>().text = DateTime.Now.ToString("HH:mm");
            }
            else
            {
                if (showSecondsTggl.isOn)
                    userClock.GetComponent<TextMeshProUGUI>().text = DateTime.Now.ToString("hh:mm:ss tt");
                else
                    userClock.GetComponent<TextMeshProUGUI>().text = DateTime.Now.ToString("hh:mm tt");
            }
        }
    }

    private bool lockDrag = false;
    private void DragItem(Button buttonToColor, GameObject itemToDrag, bool drag)
    {
        if (!lockDrag)
        {
            itemToDrag.gameObject.GetComponent<DragDropTexts>().canDrag = drag;
        
            if (drag)
                menuManager.SetButtonColorToPressed(buttonToColor);
            else
                menuManager.SetButtonsColorToNonPressed(buttonToColor);

            StartCoroutine(lockDragIe());
        }      
         
    }

    IEnumerator lockDragIe()
    {
        lockDrag = true;
        yield return new WaitForSeconds(0.1f);
        lockDrag = false;
    }

    private void TurnOnOffClock(bool isOn)
    {
        userClock.SetActive(isOn);

        TurnOffAllDrags();
    }

    private void TurnOnOffText(bool isOn)
    {
        userText.SetActive(isOn);

        TurnOffAllDrags();
    }

    private void TurnOnOffLogo(bool isOn)
    {
        logo.SetActive(isOn);

        TurnOffAllDrags();
    }

    public void LoadTextFontsDropdown(bool def)
    {
        List<string> names = new List<string>();
        for (int i = 0; i < fontAssets.Length; i++)
        {
            names.Add(fontAssets[i].ToString().Replace(" SDF (TMPro.TMP_FontAsset)", ""));
        }

        if(textFontsDropdown.options.Count<=0)
             textFontsDropdown.AddOptions(names);

        if(clockFontsDropdown.options.Count <=0)
             clockFontsDropdown.AddOptions(names);

        if (!def)
        {
            clockFontsDropdown.value = clockSelectedFontIndex;
            textFontsDropdown.value = textSelectedFontIndex;
        }
        else
        {
            clockFontsDropdown.value = defaultClockFontIndex;
            textFontsDropdown.value = defaultTextSelectedIndex;
            clockSelectedFontIndex = defaultClockFontIndex;
            textSelectedFontIndex = defaultTextSelectedIndex;
        }
        
    }


    private void AddMenuButtonsListeners()
    {

        foreach(AddonsButtons button in addonsButtons)
        {
            button.button.onClick.AddListener(()=> 
            {
                for (int i = 0; i < addonsButtons.Count; i++)
                {
                    if(i != addonsButtons.IndexOf(button))
                    {
                        menuManager.SetButtonsColorToNonPressed(addonsButtons[i].button);
                        menuManager.OpenClosePanel(addonsButtons[i].panelToOpen, false);
                    }
                }

                if (!button.panelToOpen.activeInHierarchy)
                {
                    menuManager.SetButtonColorToPressed(button.button);
                    menuManager.OpenClosePanel(button.panelToOpen, true);
                }
                else
                {
                    menuManager.SetButtonsColorToNonPressed(button.button);
                    menuManager.OpenClosePanel(button.panelToOpen, false);
                }
                TurnOffAllDrags();
            });
        }
      
    }

    public void LoadAddonsAtStart(bool isDefault)
    {
        if (isDefault)
        {
            SetClockTextColor(true, defClockColor);
            SetClockTextColor(false, defTextColor);
            userText.transform.position = userTextDefPos.position;
            userClock.transform.position = clockDefPos.position;
            logo.transform.position = logoDefPos.position;
            userText.GetComponent<TextMeshProUGUI>().font = fontAssets[defaultTextSelectedIndex];
            userClock.GetComponent<TextMeshProUGUI>().font = fontAssets[defaultClockFontIndex];
            LoadTextFontsDropdown(true);
            SetSlidersFunctionsOnStart(true);
            LoadToggles(true);
        }
        else
        {
            SetSlidersFunctionsOnStart(false);
            LoadAddons();
            if (PlayerPrefs.HasKey("Logo"))
            {
                LoadFromDatapath();
                logo.SetActive(true);             
            }
            LoadTextFontsDropdown(false);
            
            SetClockTextColor(true, clockColor);
            SetClockTextColor(false, textColor);
            LoadToggles(false);
        }
        SaveAddons();
    }

    public void SetFluid()
    {
        fluidBehaviourScript = CoreManager.instance.FindFluid();
    }

    private void LoadToggles(bool isDefault)
    {
        foreach(Toggles toggle in togglesList)
        {
            if (isDefault)
            {
                toggle.toggle.isOn = toggle.toggleDefault;
                toggle.toggle.onValueChanged.Invoke(toggle.toggle.isOn);
            }               
            else
                toggle.toggle.onValueChanged.Invoke(toggle.toggle.isOn);
        }
    }

    public void SetClockTextColor(bool clock, Color color)
    {
        if (!clock)
            userText.GetComponent<TextMeshProUGUI>().color = color;
        else
            userClock.GetComponent<TextMeshProUGUI>().color = color;

        SaveAddons();
    }

    public void TurnOffAllDrags()
    {//
        if(fluidBehaviourScript != null)
        {
            fluidBehaviourScript.effectOn = true;
        }
        clockDrag = false;
        textDrag = false;
        logoDrag = false;

        menuManager.SetButtonsColorToNonPressed(dragClockBttn);
        menuManager.SetButtonsColorToNonPressed(dragLogoBttn);
        menuManager.SetButtonsColorToNonPressed(dragTextBttn);
        userText.GetComponent<DragDropTexts>().canDrag = false;
        userClock.GetComponent<DragDropTexts>().canDrag = false;
        logo.GetComponent<DragDropTexts>().canDrag = false;
    }

    public void SaveAddons()
    {
        #region savePositions
        for (int i = 0; i < addonsObjects.Length; i++)
        {
            Vector3 pos = addonsObjects[i].position - clockDefPos.position;
            PlayerPrefs.SetFloat(addonsObjects[i].name + ".x", pos.x);
            PlayerPrefs.SetFloat(addonsObjects[i].name + ".y", pos.y);
            PlayerPrefs.SetFloat(addonsObjects[i].name + ".z", pos.z);
        }

        #endregion
        #region saveColors
        PlayerPrefs.SetString("TextColor", ColorUtility.ToHtmlStringRGBA(userText.GetComponent<TextMeshProUGUI>().color));
        PlayerPrefs.SetString("ClockColor", ColorUtility.ToHtmlStringRGBA(userClock.GetComponent<TextMeshProUGUI>().color));
        #endregion

        //Save Toggles
        for (int i = 0; i < togglesList.Count; i++)
        {
            PlayerPrefs.SetInt(togglesList[i].toggle.name, System.Convert.ToInt32(togglesList[i].toggle.isOn));
        }
        SaveLoadSliders(true);

        PlayerPrefs.SetInt("ClockFont", clockSelectedFontIndex);
        PlayerPrefs.SetInt("TextFont", textSelectedFontIndex);

        PlayerPrefs.SetString("UserText.text", userText.GetComponent<TextMeshProUGUI>().text);

        PlayerPrefs.Save();
    }

    private UIColorPicker[] colorPickers;

    public void LoadAddons()
    {
        colorPickers = FindObjectsOfType<UIColorPicker>();
        #region loadPos
        for (int i = 0; i < addonsObjects.Length; i++)
        {
            addonsObjects[i].position = new Vector3(
                PlayerPrefs.GetFloat(addonsObjects[i].name + ".x") + clockDefPos.position.x,
                PlayerPrefs.GetFloat(addonsObjects[i].name + ".y") + clockDefPos.position.y, 
                PlayerPrefs.GetFloat(addonsObjects[i].name + ".z") + clockDefPos.position.z);
        }
        #endregion
        #region loadColors
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("TextColor"), out textColor);
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("ClockColor"), out clockColor);

        for (int i = 0; i < colorPickers.Length; i++)
        {
            if (colorPickers[i].type == UIColorPicker.Type.clock)
            {
                colorPickers[i].buttonImage.color = clockColor;
                colorPickers[i].SetSliders(clockColor);
            }               
            if (colorPickers[i].type == UIColorPicker.Type.text)
            {
                colorPickers[i].buttonImage.color = textColor;
                colorPickers[i].SetSliders(textColor);
            }                
        }


        #endregion

        for (int i = 0; i < togglesList.Count; i++)
        {
            if (PlayerPrefs.HasKey(togglesList[i].toggle.name))
                togglesList[i].toggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt(togglesList[i].toggle.name));
        }
        SaveLoadSliders(false);

        userText.GetComponent<TextMeshProUGUI>().font = fontAssets[PlayerPrefs.GetInt("TextFont")];
        userClock.GetComponent<TextMeshProUGUI>().font = fontAssets[PlayerPrefs.GetInt("ClockFont")];
        userText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("UserText.text");
    }
    public void SaveLoadSliders(bool save)
    {
       foreach(AddonsSliders slider in addonsSlidersList)
        {
            if (save)              
                    PlayerPrefs.SetFloat(slider.slider.name, slider.slider.value);                   
            else 
                    slider.slider.value = PlayerPrefs.GetFloat(slider.slider.name);
        }      
    }

    public void SetSize(float value, GameObject objectToSize)
    {
        objectToSize.transform.localScale = new Vector3(value,value,value);
        Debug.Log("set size " + objectToSize.name + "size to " + value);
    }

    public void ChangeClockFontDropdown(int index)
    {
        userClock.GetComponent<TextMeshProUGUI>().font = fontAssets[index];
        clockSelectedFontIndex = index;
    }

    public void ChangetextFontDropdown(int index)
    {
        userText.GetComponent<TextMeshProUGUI>().font = fontAssets[index];
        textSelectedFontIndex = index;
    }

    private void SetSlidersFunctionsOnStart(bool isDefault)
    {
        
        foreach (AddonsSliders sld in addonsSlidersList)
        {
            sld.slider.minValue = sld.sliderMinValue;
            sld.slider.maxValue = sld.sliderMaxValue;

            sld.slider.onValueChanged.AddListener(delegate { sld.sliderFunction.Invoke(sld.slider.value, sld.objectToSetSize); });

            if (isDefault)
                sld.sliderFunction.Invoke(sld.defaultSliderValue, sld.objectToSetSize);
            else
                sld.sliderFunction.Invoke(sld.slider.value, sld.objectToSetSize);
        }
    }


    #region LOGO_Actions

    private string testPath = "";

    public void BrowseLogoFile()
    {
        try
        {
            FileBrowser.OpenFilesAsync((paths) => { LoadTexture(paths); }, "Open files", testPath, false, new ExtensionFilter("Image Files", "png", "jpg"));
        }
        catch (Exception e)
        { }
    }

    float targetHeight = 0.5f;
    private void LoadTexture(params string[] paths)
    {


        if (paths.Length > 0 && paths[0] != null)
        {
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);

            tex.LoadImage(File.ReadAllBytes(paths[0]));
            logo.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            SaveTextureToFile(tex);
            PlayerPrefs.SetString("Logo", "Loaded");
            logoOnOffTggl.isOn = true;
            logo.SetActive(true);
        }

    }

    private static string iconFilename = "/SavedIcon.png";
    private void SaveTextureToFile(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + iconFilename, bytes);
    }


    private void LoadFromDatapath()
    {
        if (File.Exists(Application.dataPath + iconFilename))
        {
            string[] tempPaths = new string[1];
            tempPaths[0] = Application.dataPath + iconFilename;
            LoadTexture(tempPaths[0]);
        }
    }
    #endregion


}
