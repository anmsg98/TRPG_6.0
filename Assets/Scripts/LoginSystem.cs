using Google;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class LoginSystem : MonoBehaviour
{
    private string web_client_id = "420891439018-6njr6rivluqb0405gdumhjq7lqgqddhs.apps.googleusercontent.com";
    public TMP_Text loginMessage;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            WebClientId = web_client_id,
            UseGameSignIn = false,
            RequestEmail = true,
            RequestIdToken = true
        };
    }

    public void SignIn()
    {
        Debug.Log("Calling SignIn");
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
          OnAuthenticationFinished);
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        Debug.Log("Authentication finished, processing on main thread");
        MainThreadDispatcher.RunOnMainThread(() => ProcessAuthResult(task));
    }

    private void ProcessAuthResult(Task<GoogleSignInUser> task)
    {
        Debug.Log("Auth Result");
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");
            Debug.Log(task.Result.Email + "!");
            Debug.Log(task.Result.IdToken + "!");
        }
    }
}