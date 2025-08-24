using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Button[] buttons;
    [SerializeField] Canvas canvas;
    int currentNumber = 0;
    void Start()
    {
        Init();
    }

    public void OnShowInfo(bool mode)
    {
        canvas.gameObject.SetActive(mode);
        Init();
    }

    void Init()
    {
        currentNumber = 0;
        image.sprite = sprites[currentNumber];
        buttons[0].interactable = true;
        buttons[1].interactable = false;

            }

    public void OnArrowButtonDown(int direction)
    {
        if (direction == 0)
        {
            if (currentNumber < sprites.Length) { 
                currentNumber++;
                buttons[1].interactable = true;
                if (currentNumber== sprites.Length-1)
                    buttons[0].interactable = false;
            }
        }

        else if (direction == 1)
        {
            if (currentNumber > 0) { 
                currentNumber--;
                buttons[0].interactable = true;
                if (currentNumber == 0)
                    buttons[1].interactable = false;
            }

        }
        image.sprite = sprites[currentNumber];

    }
}