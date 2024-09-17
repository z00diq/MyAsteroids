using UnityEngine.SceneManagement;

namespace Assets.Infrastructure
{
    public class SceneSecretary
    {
        private const int MAIN_SCENE_INDEX = 1;
        private const int BOOTSTRAP_SCENE_INDEX = 0;

        public void ToGamScene()
        {
            SceneManager.LoadScene(MAIN_SCENE_INDEX, LoadSceneMode.Single);
        }
    }
}
