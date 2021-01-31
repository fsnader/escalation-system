using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EscalationSystem.VoiceGateway
{
    public class VoiceRequest
    {
        [JsonProperty("messages")]
        public Message[] Messages { get; set; }

        public static VoiceRequest Create(string text, string to)
        {
            return new VoiceRequest
            {
                Messages = new Message[]
                {
                    new Message
                    {
                        From = "",
                        Destinations = new Destination[]
                        {
                            new Destination
                            {
                                To = to,
                            }
                        },
                        Voice = new Voice
                        {
                            Name = "Vitoria",
                            Gender = "female"
                        },
                        Text = text,
                        Language = "pt-br",
                        SpeechRate = 1,
                        CallTimeout = 130,
                        Pause = 1,
                        MachineDetection = "hangup",
                        DeliveryTimeWindow = new DeliveryTimeWindow
                        {
                            From = new HourAndMinute
                            {
                                Hour = 0,
                                Minute = 0
                            },
                            To = new HourAndMinute
                            {
                                Hour = 23,
                                Minute = 59
                            },
                            Days = new string[]
                            {
                                "MONDAY",
                                "TUESDAY",
                                "WEDNESDAY",
                                "THURSDAY",
                                "FRIDAY",
                                "SATURDAY",
                                "SUNDAY"
                            }
                        }
                    }
                }
            };
        }
    }

    public class Message
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("destinations")]
        public Destination[] Destinations { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("voice")]
        public Voice Voice { get; set; }

        [JsonProperty("speechRate")]
        public long SpeechRate { get; set; }

        [JsonProperty("callTimeout")]
        public long CallTimeout { get; set; }

        [JsonProperty("pause")]
        public long Pause { get; set; }

        [JsonProperty("machineDetection")]
        public string MachineDetection { get; set; }

        [JsonProperty("deliveryTimeWindow")]
        public DeliveryTimeWindow DeliveryTimeWindow { get; set; }
    }

    public class DeliveryTimeWindow
    {
        [JsonProperty("from")]
        public HourAndMinute From { get; set; }

        [JsonProperty("to")]
        public HourAndMinute To { get; set; }

        [JsonProperty("days")]
        public string[] Days { get; set; }
    }

    public class HourAndMinute
    {
        [JsonProperty("hour")]
        public long Hour { get; set; }

        [JsonProperty("minute")]
        public long Minute { get; set; }
    }

    public class Destination
    {
        [JsonProperty("to")]
        public string To { get; set; }
    }

    public class Voice
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }
    }
}