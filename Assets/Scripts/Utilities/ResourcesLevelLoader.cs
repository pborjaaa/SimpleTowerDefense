using System;
using Models;
using UnityEngine;

namespace Utilities
{
    public class ResourcesLevelLoader : ILevelLoader
    {
        private const string LevelsRelativePath = "Levels/";

        public Level Load(string levelName)
        {
            var path = LevelsRelativePath + levelName;
            try
            {
                var loadedLevel = Resources.Load<Level>(path);
                if (loadedLevel != null)
                {
                    return loadedLevel;
                }

                Debug.LogWarning("Level was not found at path: " + path);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading level: " + e.Message);
            }

            return null;
        }
    }
}