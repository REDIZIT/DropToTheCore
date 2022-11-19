using InGame.Tutorial;

namespace InGame.Secrets
{
    [System.Serializable]
    public class SecretsModel
    {
        public RecordsModel Records = new RecordsModel();


        public int Coins = 0;

        public int GravityPower = 0;
        public int JumpPower = 0;
        public int ShieldLevel = 0;
        public bool IsTutorialPassed = false;



        public TutorialPager.Page PassedTutorials = TutorialPager.Page.None;
    }
}