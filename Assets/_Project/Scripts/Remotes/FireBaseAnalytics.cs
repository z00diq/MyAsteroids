using Firebase;
using Firebase.Extensions;
using Firebase.Analytics;
using System;
using System.Threading.Tasks;
using Assets.Models;

namespace Assets._Project.Scripts.Remotes
{
    public class FireBaseAnalytics : RemoteAnalytics
    {
        public async override Task InitializeAsync()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                else
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));  
            });
        }

        public override void EndGameEvent()
        {
            Parameter[] parametrs = new Parameter[]
            {
                new Parameter(nameof(FireCount),FireCount),
                new Parameter(nameof(LaserFireCount),LaserFireCount),
                new Parameter(nameof(AsteroidsDestroyedCount),AsteroidsDestroyedCount),
                new Parameter(nameof(UfoDestroyedCount),UfoDestroyedCount)
            };

            FirebaseAnalytics.LogEvent(END_GAME_EVENT_NAME, parametrs);
            base.EndGameEvent();
        }


        public override void LaserUseEvent()
        {
            FirebaseAnalytics.LogEvent(LASER_USE_EVENT_NAME);
        }

        public override void StartGameEvent()
        {
            FirebaseAnalytics.LogEvent(START_GAME_EVENT_NAME);
        }
    }
}
