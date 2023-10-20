using UnityEngine;
using UnityEngine.SceneManagement;

namespace Behaviour
{
    public class GameManager : MonoBehaviour
    {
        void Update()
        {
            UpdateScene();
        }
        
        // For debugging at runtime
        private void UpdateScene()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        
            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
            }
        
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
