using Assets._Project.Scripts.Remotes;
using UnityEngine.SceneManagement;

namespace Assets.Infrastructure
{
    public class SceneSecretary
    {
        private const int MAIN_SCENE_INDEX = 1;
        private GameState _gameState;
        private RemoteAnalytics _remoteAnalytics;

        public SceneSecretary(RemoteAnalytics remoteAnalytics)
        {
            _remoteAnalytics = remoteAnalytics;
        }

        public void ToGamScene()
        {
            if (_gameState == GameState.NonStarted)
            {
                _gameState = GameState.Running;
                _remoteAnalytics.StartGameEvent();
            }
            else
            {
                _remoteAnalytics.EndGameEvent();
            }

            SceneManager.LoadScene(MAIN_SCENE_INDEX, LoadSceneMode.Single);
        }
    }

    public enum GameState
    {
        NonStarted,
        Running
    }
}
