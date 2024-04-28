using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public RawImage imageDisplay; // 拖拽你的RawImage组件到这个字段
    public Texture texture1;      // 在Inspector中设置的第一张图片
    public Texture texture2;      // 在Inspector中设置的第二张图片
    public GameObject colorPanel;
    public GameObject mainPanel;
    public GameObject inputPanel;
    public Button colorButton;
    public Button brushButton;
    public Button canvasButton;


    private bool isTexture1Active = true; // 用于跟踪当前显示的是哪张图片
    // Start is called before the first frame update
    void Start()
    {
        colorButton.onClick.AddListener(() => ToggleVisibility(colorPanel, colorButton, "Color"));
        brushButton.onClick.AddListener(() => ToggleVisibility(inputPanel, brushButton, "Brush"));
        canvasButton.onClick.AddListener(() => ToggleVisibility(mainPanel, canvasButton, "Canvas"));
    }

// Update is called once per frame
void Update()
    {
        if (Input.GetMouseButtonDown(2)) // 检测鼠标中键是否被按下
        {
            ToggleImage();
        }
    }
    void ToggleImage()
    {
        if (isTexture1Active)
        {
            imageDisplay.texture = texture2; // 切换到第二张图片
        }
        else
        {
            imageDisplay.texture = texture1; // 切换回第一张图片
        }
        isTexture1Active = !isTexture1Active; // 更新当前图片的跟踪状态
    }

    public void finishBuild()
    {
        colorPanel.SetActive(true);
        mainPanel.SetActive(true);
        inputPanel.SetActive(true);
        colorButton.gameObject.SetActive(true);
        brushButton.gameObject.SetActive(true);
    }

    public void restartBuild()
    {
        colorPanel.SetActive(false);
        mainPanel.SetActive(true);
        inputPanel.SetActive(false);
        colorButton.gameObject.SetActive(false);
        brushButton.gameObject.SetActive(false);

    }

    void ToggleVisibility(GameObject gameObject, Button button, string panelName)
    {
        bool isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);
        // 获取按钮中的TextMeshPro组件，并更新文本
        TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = isActive ? "--" : panelName;
    }


}
