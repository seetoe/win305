using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Amazon.Comprehend;
using Amazon.Comprehend.Model;

namespace ComprehendLab
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please provide text file.");

                return;
            }

            var filename = args[0];

            ComprehendInputFile(filename);
        }

        static void ComprehendInputFile(string fileName)
        {
            var inputText = File.ReadAllText(fileName);

            var comprehendClient = new AmazonComprehendClient(Amazon.RegionEndpoint.EUWest1);

            Console.WriteLine("Calling DetectDominantLanguage\n");
            var text1 = DetectDominantLanguage(inputText, comprehendClient);

            Console.WriteLine("Calling DetectEntities\n");
            var text2 = DetectEntities(inputText, comprehendClient);

            Console.WriteLine("Calling DetectKeyPhrases\n");
            var text3 = DetectKeyPhrases(inputText, comprehendClient);

            Console.WriteLine("Calling DetectSentiment\n");
            var text4 = DetectSentiment(inputText, comprehendClient);

            var outputBuilder = new StringBuilder();
            outputBuilder.AppendLine(text1);
            outputBuilder.AppendLine(text2);
            outputBuilder.AppendLine(text3);
            outputBuilder.AppendLine(text4);

            var outputFileName = fileName.Replace(Path.GetExtension(fileName), "_comprehend.txt");
            File.WriteAllText(outputFileName, outputBuilder.ToString());
        }

        private static string DetectDominantLanguage(string text, AmazonComprehendClient comprehendClient)
        {
            var stringBuilder = new StringBuilder();

            var detectDominantLanguageRequest = new DetectDominantLanguageRequest()
            {
                Text = text
            };

            var detectDominantLanguageResponse = comprehendClient.DetectDominantLanguage(detectDominantLanguageRequest);

            stringBuilder.AppendLine("Detect Dominant Language:");
            stringBuilder.AppendLine("==========================");

            foreach (var dominantLanguage in detectDominantLanguageResponse.Languages)
            {
                stringBuilder.AppendLine(string.Format("Language Code: {0}, Score: {1}",
                    dominantLanguage.LanguageCode, dominantLanguage.Score));
            }

            Console.WriteLine("DetectDominantLanguage => Done\n");

            return stringBuilder.ToString();
        }

        private static string DetectEntities(string text, AmazonComprehendClient comprehendClient)
        {
            var stringBuilder = new StringBuilder();

            var detectEntitiesRequest = new DetectEntitiesRequest()
            {
                Text = text,
                LanguageCode = "en"
            };

            var detectEntitiesResponse = comprehendClient.DetectEntities(detectEntitiesRequest);

            stringBuilder.AppendLine("Detect Entities:");
            stringBuilder.AppendLine("==========================");

            foreach (var entity in detectEntitiesResponse.Entities)
            {
                stringBuilder.AppendLine(string.Format("Text: {0}, Type: {1}, Score: {2}, BeginOffset: {3}, EndOffset: {4}",
                    entity.Text, entity.Type, entity.Score, entity.BeginOffset, entity.EndOffset));
            }

            Console.WriteLine("DetectEntities => Done\n");

            return stringBuilder.ToString();
        }

        private static string DetectKeyPhrases(string text, AmazonComprehendClient comprehendClient)
        {
            var stringBuilder = new StringBuilder();

            var detectKeyPhrasesRequest = new DetectKeyPhrasesRequest()
            {
                Text = text,
                LanguageCode = "en"
            };

            var detectKeyPhrasesResponse = comprehendClient.DetectKeyPhrases(detectKeyPhrasesRequest);

            stringBuilder.AppendLine("Detect Key Phrases:");
            stringBuilder.AppendLine("==========================");

            foreach (var keyPhrase in detectKeyPhrasesResponse.KeyPhrases)
            {
                stringBuilder.AppendLine(string.Format("Text: {0}, Score: {1}, BeginOffset: {2}, EndOffset: {3}",
                    keyPhrase.Text, keyPhrase.Score, keyPhrase.BeginOffset, keyPhrase.EndOffset));
            }

            Console.WriteLine("DetectKeyPhrases => Done\n");

            return stringBuilder.ToString();
        }

        private static string DetectSentiment(string text, AmazonComprehendClient comprehendClient)
        {
            var stringBuilder = new StringBuilder();

            var detectSentimentRequest = new DetectSentimentRequest()
            {
                Text = text,
                LanguageCode = "en"
            };

            var detectSentimentResponse = comprehendClient.DetectSentiment(detectSentimentRequest);

            stringBuilder.AppendLine("Detect Sentiment:");
            stringBuilder.AppendLine("==========================");

            stringBuilder.AppendLine(detectSentimentResponse.Sentiment);

            Console.WriteLine("DetectSentiment => Done\n");

            return stringBuilder.ToString();
        }
    }
}
