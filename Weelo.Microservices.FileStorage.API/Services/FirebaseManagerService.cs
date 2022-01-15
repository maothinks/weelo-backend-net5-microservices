using Firebase.Auth;
using Firebase.Storage;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Weelo.Microservices.FileStorage.API.DTOS;

namespace Weelo.Microservices.FileStorage.API.Services
{
    public class FirebaseManagerService 
    {
        private static readonly string ApiKey = "AIzaSyC7uuXJ8SrTsRKQY59GeYgZiDqRx2x5dfs";
        private static readonly string Bucket = "weeloproject.appspot.com";
        private static readonly string AuthEmail = "maothinks@gmail.com";
        private static readonly string AuthPassword = "weelo123";

        public static StreamContent ConvertBase64ToStream(string imageFromRequest)
        {
            byte[] imageStringToBase64 = Convert.FromBase64String(imageFromRequest);
            StreamContent streamContent = new(new MemoryStream(imageStringToBase64));
            return streamContent;
        }

        public static async Task<string> UploadImage(Stream stream, ImageDTO imageDTO)
        {
            string imageFromFirebaseStorage = "";
            FirebaseAuthProvider firebaseConfiguration = new(new FirebaseConfig(ApiKey));

            FirebaseAuthLink authConfiguration = await firebaseConfiguration
                .SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            CancellationTokenSource cancellationToken = new();

            FirebaseStorageTask storageManager = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authConfiguration.FirebaseToken),
                    ThrowOnCancel = true
                })
                //.Child(imageDTO.FolderName)
                .Child(imageDTO.ImageName)
                .PutAsync(stream, cancellationToken.Token);

            try
            {
                imageFromFirebaseStorage = await storageManager;
            }
            catch
            {
            }
            return imageFromFirebaseStorage;
        }
    }
}
