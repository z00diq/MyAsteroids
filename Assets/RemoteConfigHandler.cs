using System.Threading.Tasks;
using Firebase.RemoteConfig;
using UnityEngine.UI;
using UnityEngine;
using System;
using Firebase.Extensions;
using Assets.Configurations;
using System.Collections.Generic;

public class RemoteConfigHandler : MonoBehaviour
{
    [SerializeField] private Text _text;
    public Action<string> _i;
    private FirebaseRemoteConfig _config;

    private void Awake()
    {
        FetchDataAsync();
    }

    private void Start()
    {
        FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
                _i.Invoke(_config.GetValue("Test").StringValue);
        });
    }

    private Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        _config = FirebaseRemoteConfig.DefaultInstance;
        ConfigInfo info = _config.Info;

        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        Task task = _config.ActivateAsync();
        task.ContinueWithOnMainThread(TakeData);
    }

    private void TakeData(Task task)
    {
        if (!task.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        ConfigInfo info = _config.Info;

        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        AsteroidConfig asteroidConfig = new AsteroidConfig();
        BulletsFireConfig bulletsFireConfig = new BulletsFireConfig();
        UFOConfig ufoConfig = new UFOConfig();
        ShipConfig shipConfig = new ShipConfig();
        ParticleAsteroidConfig particleAsteroidConfig = new ParticleAsteroidConfig();
        LaserFireConfig laserFireConfig = new LaserFireConfig();

        JsonUtility.FromJsonOverwrite(_config.GetValue(nameof(AsteroidConfig)).StringValue,asteroidConfig);
        JsonUtility.FromJsonOverwrite(_config.GetValue(nameof(BulletsFireConfig)).StringValue, bulletsFireConfig);
        JsonUtility.FromJsonOverwrite(_config.GetValue(nameof(UFOConfig)).StringValue, ufoConfig);
        JsonUtility.FromJsonOverwrite(_config.GetValue(nameof(ShipConfig)).StringValue, shipConfig);
        JsonUtility.FromJsonOverwrite(_config.GetValue(nameof(ParticleAsteroidConfig)).StringValue, particleAsteroidConfig);
        JsonUtility.FromJsonOverwrite(_config.GetValue(nameof(LaserFireConfig)).StringValue, laserFireConfig);

        Debug.Log("ara");
    }

    private void aaa(string str)
    {
        _text.text = str;
    }

    public class Chech
    {
        [SerializeField] private int maxCount;
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float frequency;
        [SerializeField] private string _yahca;
    }

    public class AA : Chech
    {

    }
}
