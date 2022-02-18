using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePage_End : MonoBehaviour
    {
        public void ClickHomePage()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
