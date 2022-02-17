using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneControl.ESC
{
    public class HomePage : MonoBehaviour
    {
        public GameObject gameObjectEsc;

        private void Start()
        {
            gameObjectEsc = GameObject.Find("UIPublic").transform.Find("UIESC").gameObject;
        }

        public void ClickHomePage()
        {
            Debug.Log("Escape: Click home");
            // TODO Print unsaved status to confirm twice
            SceneManager.LoadScene("Scenes/Menu", LoadSceneMode.Single);
        }
    }
}