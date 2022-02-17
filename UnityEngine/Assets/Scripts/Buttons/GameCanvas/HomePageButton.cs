using UnityEngine;
using UnityEngine.SceneManagement;


public class HomePageButton : MonoBehaviour
{
    public void ClickHomePage()
    {
        Debug.Log("Escape: Click home");
        // TODO Print unsaved status to confirm twice
        SceneManager.LoadScene("Scenes/Menu", LoadSceneMode.Single);
    }
}