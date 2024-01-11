using Firebase;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
public class Game_Analytics : MonoBehaviour
{
    DependencyStatus dependency_status = DependencyStatus.UnavailableOther;
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependency_status = task.Result;
            if (dependency_status == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}",dependency_status));
            }
        });
    }
    void Show_banner_at_start() => Ad_Manager.instance.Show_Banner();
    void OnEnable()
    {
        Invoke("Show_banner_at_start" , 0f);
    } 
    void OnDisable() => Ad_Manager.instance.Destroy_Banner();
}
