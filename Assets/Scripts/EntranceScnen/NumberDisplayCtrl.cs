using UnityEngine;
using UnityEngine.UI;

public class NumberDisplayCtrl : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] Sprite[] sprites;

    public void ShowNumber(int index,int number)
    {
        images[index].sprite = sprites[number];
    }


}
