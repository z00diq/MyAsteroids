using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SceneSecretary
    {
        private const int MainSceneIndex = 0;
        public void ReloadSceneAsSingle()
        {
            SceneManager.LoadScene(MainSceneIndex, LoadSceneMode.Single);
        }
    }
}
