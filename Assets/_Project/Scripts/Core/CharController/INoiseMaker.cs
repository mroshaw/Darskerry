namespace DaftAppleGames.Darskerry.Core.CharController.AiController
{
    public interface INoiseMaker
    {
        public void MakeNoise(float noiseHeard);
        public float GetNoiseLevel();
    }
}