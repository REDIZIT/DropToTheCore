using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.GooglePlay
{
    /// <summary>
    /// Class for using Google Play Games cloud saving
    /// </summary>
    public static class GoogleCloud
    {
        private static DateTime startDateTime;

        private static ISavedGameClient gameClient;
        private static ISavedGameMetadata currentMetadata;

        public const string DEFAULT_SAVE_NAME = "Save";


        public static void Initialize()
        {
            startDateTime = DateTime.Now;

            gameClient = PlayGamesPlatform.Instance.SavedGame;
        }




        
        public static void ReadData(string filename, Action<SavedGameRequestStatus, byte[]> onDataRead)
        {
            if (!GooglePlayManager.IsAuthenticated)
            {
                onDataRead(SavedGameRequestStatus.AuthenticationError, null);
                return;
            }

            OpenData(filename, (status, metadata) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    gameClient.ReadBinaryData(metadata, onDataRead);
                    currentMetadata = metadata;
                }
            });
        }
        public static void WriteData(byte[] data)
        {
            if (!GooglePlayManager.IsAuthenticated || data == null || data.Length == 0) return;

            TimeSpan currentSpan = DateTime.Now - startDateTime;

            Action onDataWrite = () =>
            {
                TimeSpan totalPlayTime = currentMetadata.TotalTimePlayed + currentSpan;

                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder()
                    .WithUpdatedDescription("Saved game at " + DateTime.Now)
                    .WithUpdatedPlayedTime(totalPlayTime);

                SavedGameMetadataUpdate updatedMetadata = builder.Build();



                gameClient.CommitUpdate(currentMetadata,
                    updatedMetadata,
                    data,
                    (status, metadata) => currentMetadata = metadata);

                startDateTime = DateTime.Now;
            };

            // If first entry
            if (currentMetadata == null)
            {
                OpenData(DEFAULT_SAVE_NAME, (status, metadata) =>
                {
                    Debug.Log("Cloud data write status is " + status);
                    if (status == SavedGameRequestStatus.Success)
                    {
                        currentMetadata = metadata;
                        onDataWrite();
                    }
                });
                return;
            }

            onDataWrite();
        }


        public static void ShowSavesUI(Action<SavedGameRequestStatus, byte[]> onDataRead, Action onDataCreate)
        {
            gameClient.ShowSelectSavedGameUI("Select save",
                5,
                false,
                true,
                (status, metadata) =>
                {
                    if (status == SelectUIStatus.SavedGameSelected && metadata != null)
                    {
                        if (string.IsNullOrWhiteSpace(metadata.Filename)) onDataCreate();
                        else ReadData(metadata.Filename, onDataRead);
                    }
                });
        }






        private static void OpenData(string filename, Action<SavedGameRequestStatus, ISavedGameMetadata> onDataOpen)
        {
            if (!GooglePlayManager.IsAuthenticated)
            {
                onDataOpen(SavedGameRequestStatus.AuthenticationError, null);
                return;
            }

            gameClient.OpenWithAutomaticConflictResolution(
                filename,
                DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime,
                onDataOpen);
        }


        private static void GetSavesList(Action<SavedGameRequestStatus, List<ISavedGameMetadata>> onRecieveList)
        {
            if (!GooglePlayManager.IsAuthenticated)
            {
                onRecieveList(SavedGameRequestStatus.AuthenticationError, null);
                return;
            }

            gameClient.FetchAllSavedGames(DataSource.ReadNetworkOnly, onRecieveList);
        }

    }
}