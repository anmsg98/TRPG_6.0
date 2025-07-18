using System;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
public class LoginSystem : MonoBehaviour
{
    public FirebaseAuth Auth { get; private set; }
    public FirebaseUser User { get; private set; }
    public bool IsLoggedIn { get; private set; }

    public void start()
    {
    }// public void Initialize()
    // {
    //     var config = new PlayGamesClientConfiguration.Builder().RequestIdToken().Build();
    //
    //     PlayGamesPlatform.InitializeInstance(config);
    //     PlayGamesPlatform.DebugLogEnabled = true;
    //     PlayGamesPlatform.Activate();
    //
    //     FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
    //     {
    //         if (task.Result == DependencyStatus.Available)
    //         {
    //             Auth = FirebaseAuth.DefaultInstance;
    //             Auth.StateChanged += OnAuthStateChanged;
    //         }
    //         else
    //         {
    //             Debug.LogError(string.Format("[FIREBASE] ERROR: {0}", task.Result));
    //         }
    //     });
    // }

    void OnAuthStateChanged(object sender, EventArgs e)
    {
        IsLoggedIn = Auth.CurrentUser != null;
        User = Auth.CurrentUser;
    }

    public void Login(Action<AuthResult> onSuccess, Action<string> onFail = null)
    {
        Auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                onFail?.Invoke("CANCELED");
            }
            else if (task.IsFaulted)
            {
                onFail?.Invoke(task.Exception.Message);
            }
            else
            {
                onSuccess?.Invoke(task.Result);
            }
        });
    }

    // public void LoginWithGoogle(Action<AuthResult> onSuccess, Action<string> onFail)
    // {
    //     Social.localUser.Authenticate(success =>
    //     {
    //         if (success)
    //         {
    //             var idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
    //             var credential = GoogleAuthProvider.GetCredential(idToken, null);
    //
    //             Auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
    //             {
    //                 if (task.IsCanceled)
    //                 {
    //                     onFail?.Invoke("CANCELED");
    //                 }
    //                 else if (task.IsFaulted)
    //                 {
    //                     onFail?.Invoke(task.Exception.Message);
    //                 }
    //                 else
    //                 {
    //                     onSuccess?.Invoke(task.Result);
    //                 }
    //             });
    //         }
    //         else
    //         {
    //             onFail?.Invoke("Failed to retrieve Google play games authorization code");
    //         }
    //     });
    //  }
}