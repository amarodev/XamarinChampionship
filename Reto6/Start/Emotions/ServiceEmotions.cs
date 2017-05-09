using Microsoft.ProjectOxford.Emotion;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Emotions
{
    public class ServiceEmotions
    {
        private const string Key = "";
        public static async Task<Dictionary<string, float>> GetEmotions(Stream stream)
        {
            var cliente = new EmotionServiceClient(Key);
            var emotions = await cliente.RecognizeAsync(stream);
            if (emotions == null || !emotions.Any())
                return null;
            return emotions[0].Scores.ToRankedList().ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
