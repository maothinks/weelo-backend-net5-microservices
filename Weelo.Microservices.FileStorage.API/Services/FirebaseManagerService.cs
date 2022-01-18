using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Weelo.Microservices.FileStorage.API.DTOS;

namespace Weelo.Microservices.FileStorage.API.Services
{
    public class FirebaseManagerService : IFirebaseManagerServiceBase
    {
        /// <summary>
        /// ConvertBase64ToStream convert an image to Stream content
        /// </summary>
        /// <param name="imageFromRequest"></param>
        /// <returns>StreamContent with image</returns>
        public StreamContent ConvertBase64ToStream(string imageFromRequest)
        {
            byte[] imageStringToBase64 = Convert.FromBase64String(imageFromRequest);
            StreamContent streamContent = new(new MemoryStream(imageStringToBase64));
            return streamContent;
        }

        /// <summary>
        /// Upload image to firebase storage
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="imageDTO"></param>
        /// <returns>Url Image uploaded</returns>
        public async Task<string> UploadImage(Stream stream, ImageDTO imageDTO)
        {
            // Get details from the configuration file
            IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfiguration configuration = configBuilder.Build();

            var firebaseStorage = configuration.GetSection("FirebaseStorage");

            // Firebase authentication
            string imageFromFirebaseStorage = "";
            FirebaseAuthProvider firebaseConfiguration = new(new FirebaseConfig(firebaseStorage["firebaseStorage_ApiKey"]));

            FirebaseAuthLink authConfiguration = await firebaseConfiguration
                .SignInWithEmailAndPasswordAsync(firebaseStorage["firebaseStorage_AuthEmail"], firebaseStorage["firebaseStorage_AuthPassword"]);

            CancellationTokenSource cancellationToken = new();

            // Firebase auth and upload
            FirebaseStorageTask storageManager = new FirebaseStorage(firebaseStorage["firebaseStorage_Bucket"],
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
