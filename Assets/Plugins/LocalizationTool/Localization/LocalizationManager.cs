using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Localization manager.
	/// </summary>
    public static class LocalizationManager
    {
		/// <summary>
		/// Fired when localization changed.
		/// </summary>
        public static event Action LocalizationChanged = () => { }; 

        public static readonly Dictionary<string, Dictionary<string, string>> Dictionary = new Dictionary<string, Dictionary<string, string>>();
        public static string folderPath = "GameLocalization/Master";

        private static string _language = "en";

        /// <summary>
        /// Get or set language.
        /// </summary>
        public static string Language => _language;

        public static void SwitchLanguage(string lang)
        {
            Debug.Log("LocalizationManager change to: " + lang);
            _language = lang;
            LocalizationChanged?.Invoke();
        }
		/// <summary>
		/// Read localization spreadsheets.
		/// </summary>
		public static void Read(bool force = false)
        {
            if (Dictionary.Count > 0 && !force) return;

            if (_language == "Unknown")
            {
                Debug.LogError("Unknown localization language. English will be used instead");
                _language = "English";
            }



            var textAssets = Resources.LoadAll<TextAsset>(folderPath);

            foreach (var textAsset in textAssets)
            {
                var text = ReplaceMarkers(textAsset.text);
                var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");
                


                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
				var languages = lines[0].Split('|').Select(i => i.Trim()).ToList();


                for (var i = 1; i < languages.Count; i++)
                {
                    if (!Dictionary.ContainsKey(languages[i]))
                    {
                        Dictionary.Add(languages[i], new Dictionary<string, string>());
                    }
                }

                for (var i = 1; i < lines.Length; i++)
                {
					var columns = lines[i].Split('|').Select(j => j.Trim()).Select(j => j.Replace("[N]","\n")).ToList();
					var key = columns[0];

                    for (var j = 1; j < languages.Count; j++)
                    {
                        if(key.Contains("//") || key == "" || key == "\n" || key == @"
" || key == " ") continue;
                        try
                        {
                            Dictionary[languages[j]].Add(key, columns[j]);
                        }
                        catch (Exception err)
                        {
                            Debug.LogWarning("Line " + key + " => " + err.Message);
                            Dictionary[languages[j]][key] = columns[j];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get localized value by localization key.
        /// </summary>
        public static string Localize(string localizationKey)
        {
            ReadIfNeeded();

            if (!Dictionary.ContainsKey(Language)) { Debug.LogError("Language not found: " + Language); return localizationKey; }
            if (!Dictionary[Language].ContainsKey(localizationKey)) { Debug.LogError("Translation not found: " + localizationKey); return localizationKey; }

            return Dictionary[Language][localizationKey].Replace(@"\n", @"
");
        }

	    /// <summary>
	    /// Get localized value by localization key.
	    /// </summary>
		public static string Localize(string localizationKey, params object[] args)
        {
            var pattern = Localize(localizationKey);

            return string.Format(pattern, args);
        }


        public static bool HasLocalization(string localizationKey)
        {
            if (localizationKey == null) return false;

            ReadIfNeeded();
            if (Dictionary.Values.All(c => !c.ContainsKey(localizationKey))) return false;

            return true;
        }


        private static string ReplaceMarkers(string text)
        {
            return text.Replace("[Newline]", "\n");
        }

        private static void ReadIfNeeded()
        {
            if (Dictionary.Count == 0)
            {
                Read();
            }
        }
    }
}