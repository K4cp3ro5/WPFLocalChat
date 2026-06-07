using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WPF_LocalChatSqlite.Services
{
    public static class ChatbotService
    {
        private const string ApiUrl =
            "https://api.groq.com/openai/v1/chat/completions";

        // DOPISAÆ
        private const string ApiKey =
            "PLACEHOLDER";

        private static readonly HttpClient client = new HttpClient();

        static ChatbotService()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ApiKey);
        }

        public static async Task<string> GetChatbotReplyAsync(string userMessage)
        {
            try
            {
                var payload = new
                {
                    model = "meta-llama/llama-4-scout-17b-16e-instruct",

                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = userMessage
                        }
                    },

                    temperature = 0.7,
                    max_completion_tokens = 1024,
                    top_p = 1,
                    stream = false
                };

                var json = JsonSerializer.Serialize(payload);

                using var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(ApiUrl, content);

                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"[ERROR {(int)response.StatusCode}] {responseText}";
                }

                using var doc = JsonDocument.Parse(responseText);

                var chatbotReply = doc
                    .RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return chatbotReply ?? "";
            }
            catch (Exception ex)
            {
                return "[EXCEPTION] " + ex.Message;
            }
        }
    }
}