using UnityEngine.SceneManagement;

namespace Assets.Infrastructure
{
    public class SceneSecretary
    {
        private const int MAIN_SCENE_INDEX = 0;
        public void ReloadSceneAsSingle()
        {
            SceneManager.LoadScene(MAIN_SCENE_INDEX, LoadSceneMode.Single);
        }
    }
}
