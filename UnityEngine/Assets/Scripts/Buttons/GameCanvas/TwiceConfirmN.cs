using UnityEngine;

public class TwiceConfirmN : MonoBehaviour
{
    public GameObject twiceConfirmPanel;

    public void ClickNo()
    {
        twiceConfirmPanel.SetActive(false);
    }
}