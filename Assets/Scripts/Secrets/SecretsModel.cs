using InGame.Tutorial;

namespace InGame.Secrets
{
    public class SecretsModel
    {
        public RecordsModel Records { get; } = new RecordsModel();


        public int Coins { get; set; } = 0;

        public int GravityPower { get; set; } = 0;
        public int JumpPower { get; set; } = 0;
        public int ShieldLevel { get; set; } = 0;



        public TutorialPager.Page PassedTutorials { get; set; } = TutorialPager.Page.None;
    }
}