using UnityEngine;
using UnityEngine.SceneManagement;

public class TwiceConfirmY : MonoBehaviour
{
    public void ClickYes()
    {
        Debug.Log("Twice confirm: Yes.");

        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}