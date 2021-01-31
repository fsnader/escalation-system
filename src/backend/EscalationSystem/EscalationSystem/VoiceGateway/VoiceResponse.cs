using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscalationSystem.VoiceGateway
{
    public class VoiceResponse
    {
        [JsonProperty("bulkId")]
        public string BulkId { get; set; }

        [JsonProperty("messages")]
        public ResponseMessage[] Messages { get; set; }
    }

    public class ResponseMessage
    {
        [JsonProperty("to")]
        public string To { get; set; }
        [JsonProperty("status")]
        public Status Status { get; set; }
        [JsonProperty("messageId")]
        public string MessageId { get; set; }
    }

    public class Status
    {
        [JsonProperty("groupId")]
        public string GroupId { get; set; }

        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}