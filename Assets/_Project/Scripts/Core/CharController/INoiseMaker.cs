namespace DaftAppleGames.Darskerry.Core.CharController
{
    public interface INoiseMaker
    {
        public void MakeNoise(float noiseHeard);
        public float GetNoiseLevel();
    }
}