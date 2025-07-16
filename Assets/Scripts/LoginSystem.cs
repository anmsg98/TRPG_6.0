using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using TMPro;
using UnityEngine;
public class LoginSystem: MonoBehaviour
{
//private GoogleSignInConfiguration configuration;
//Firebase.DependencyStatus dependencyStatus = Firebase. DependencyStatus. UnavailableOther;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    [SerializeField]
    private TextMeshProUGUI test;
    
    public TextMeshProUGUI Username, UserEmail;
    
    public GameObject LoginScreen, ProfileScreen;
    private void Start()
    {
        InitFirebase();
    }
    void InitFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus. Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Auth initializedsuccessfully.");
            }
            else
            {
                Debug.LogError("Could not resolve Firebasedependencies: " + task.Result);
            }
        });
    }
    public void GoogleSignInClick()
    {
        try
        {
            GoogleSignIn.Configuration = new
                GoogleSignInConfiguration
                {
                    WebClientId = "anmsg98",
                    RequestIdToken = true,
                    UseGameSignIn = false,
                    RequestEmail = true
                };
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith (task =>
            {
                if (task.IsFaulted)
                {
                    test.text += "SignIn Erгог:" + task. Exception;
                }
                else if (task. IsCanceled)
                {
                    test.text += "SignIn Canceled: ";
                }
                else
                {
                    OnGoogleAuthenticatedFinished(task);
                }
            });
        }
        catch (Exception ex)
        {
            test.text += "GoogleSignInClick Exception: "+ ex.Message;
        }
    }
    void OnGoogleAuthenticatedFinished (Task<GoogleSignInUser>
        task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Faulted");
        }
        else if (task. IsCanceled)
        {
            Debug.LogError("Cancelled");
        }
        else
        {
            Firebase.Auth.Credential credential =
                Firebase.Auth.GoogleAuthProvider.GetCredential (task. Result. IdToken,null);
            auth. SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
                if (task. IsCanceled)
                {
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsyncencountered an error: "+ task. Exception);
                    return;
                }
                user = auth.CurrentUser;
                Username.text = user.DisplayName;
                UserEmail.text = user.Email;
                LoginScreen.SetActive (false);
                ProfileScreen.SetActive(true);
                //StartCoroutine (LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));
            });
        }
    }
}