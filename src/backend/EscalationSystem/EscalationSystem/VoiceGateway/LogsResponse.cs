using EscalationSystem.Domain;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace EscalationSystem.VoiceGateway
{
    public partial class LogsResponse
    {
        [JsonProperty("results")]
        public Result[] Results { get; set; }

        public CallStatus CallStatus => 
            Results.FirstOrDefault()?.Error?.Name switch
            {
                "VOICE_ANSWERED" => CallStatus.Answered,
                "VOICE_ANSWERED_MACHINE" => CallStatus.Lost,
                _ => CallStatus.Calling
            };
    }

    public partial class Result
    {
        [JsonProperty("bulkId")]
        public Guid BulkId { get; set; }

        [JsonProperty("messageId")]
        public Guid MessageId { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("sentAt")]
        public string SentAt { get; set; }

        [JsonProperty("doneAt")]
        public string DoneAt { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("mccMnc")]
        public long MccMnc { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("status")]
        public Error Status { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    public partial class Error
    {
        [JsonProperty("groupId")]
        public long GroupId { get; set; }

        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("permanent", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Permanent { get; set; }
    }

    public partial class Price
    {
        [JsonProperty("pricePerSecond")]
        public long PricePerSecond { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
