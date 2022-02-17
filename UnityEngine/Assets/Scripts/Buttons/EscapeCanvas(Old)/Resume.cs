using UnityEngine;

namespace SceneControl.ESC
{
    public class Resume : MonoBehaviour
    {
        public GameObject gameObjectEsc;

        private void Start()
        {
            gameObjectEsc = GameObject.Find("UIPublic").transform.Find("UIESC").gameObject;
        }

        public void ClickResume()
        {
            Debug.Log("Escape: Click resume");
            gameObjectEsc.SetActive(false);
            Time.timeScale = 1;
        }
    }
}