using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Text.Core;
using Microsoft.ProjectOxford.Text.Language;
using System.Configuration;

namespace LanguageDetect.Dialogs
{
    [Serializable]
    public class CognitiveDialog : IDialog<object>
    {

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var apiKey = "b765dceb46c444ddbd106e101d194e46";

            var document = new Document()
            {
                Id = "777",
                Text = activity.Text
            };

            var client = new LanguageClient(apiKey);

            var request = new LanguageRequest();
            request.Documents.Add(document);

            try
            {
                var response = await client.GetLanguagesAsync(request);

                foreach (var doc in response.Documents)
                {
                    foreach (var lang in doc.DetectedLanguages)
                    {
                        await context.PostAsync($"Your sentence is in {lang.Name}, ( {lang.Iso639Name} ) with confidence score of: {lang.Score * 100}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // return our reply to the user

            context.Wait(MessageReceivedAsync);
        }
    }
}