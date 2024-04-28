using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public RawImage imageDisplay; // ��ק���RawImage���������ֶ�
    public Texture texture1;      // ��Inspector�����õĵ�һ��ͼƬ
    public Texture texture2;      // ��Inspector�����õĵڶ���ͼƬ
    public GameObject colorPanel;
    public GameObject mainPanel;
    public GameObject inputPanel;
    public Button colorButton;
    public Button brushButton;
    public Button canvasButton;


    private bool isTexture1Active = true; // ���ڸ��ٵ�ǰ��ʾ��������ͼƬ
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
        if (Input.GetMouseButtonDown(2)) // �������м��Ƿ񱻰���
        {
            ToggleImage();
        }
    }
    void ToggleImage()
    {
        if (isTexture1Active)
        {
            imageDisplay.texture = texture2; // �л����ڶ���ͼƬ
        }
        else
        {
            imageDisplay.texture = texture1; // �л��ص�һ��ͼƬ
        }
        isTexture1Active = !isTexture1Active; // ���µ�ǰͼƬ�ĸ���״̬
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
        // ��ȡ��ť�е�TextMeshPro������������ı�
        TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = isActive ? "--" : panelName;
    }


}
