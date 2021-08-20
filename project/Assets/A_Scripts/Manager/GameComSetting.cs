using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace EazyGF
{

    [System.Serializable]
    public class GameComSettingSer
    {
        public int sLanguageIndex;
    }

    public class GameComSetting : SingleSD<GameComSetting>
    {
        public int languageIndex;

        protected override string GetSavaDataName()
        {
            return "GameComSetting.Data";
        }

        protected override void MInitData()
        {
            languageIndex = (int)Application.systemLanguage;
        }

        protected override void SaveData()
        {
            GameComSettingSer serData = new GameComSettingSer();
            serData.sLanguageIndex = languageIndex;
            SerializHelp.SerializeFile(SavePath, serData);
        }

        protected override bool ReadData()
        {
            GameComSettingSer serData = SerializHelp.DeserializeFileToObj<GameComSettingSer>(SavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                languageIndex = serData.sLanguageIndex;
            }

            return loadSuccess;
        }

        public void SetLanguageMgr(SystemLanguage systemLanguage)
        {
            languageIndex = (int)systemLanguage;
            SaveData();

            //SceneManager.LoadScene("Main");
        }
    }
}