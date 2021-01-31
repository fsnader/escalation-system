using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EscalationSystem.VoiceGateway
{
    public class VoiceRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("voice")]
        public Voice Voice { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        public static VoiceRequest Create(string text, string to)
        {
            return new VoiceRequest
            {
                Text = text,
                From = "",
                To = to,
                Voice = new Voice
                {
                    Name = "Vitoria",
                    Gender = "female"
                },
                Language = "pt-br"
            };
        }
    }

    public class Voice
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }
    }
}