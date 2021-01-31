using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.VoiceGateway
{
    public interface IInfoBipApi
    {
        [Post("/tts/3/advanced")]
        Task<ApiResponse<VoiceResponse>> SendVoiceMessage([Header("Authorization")] string authorization, [Body]VoiceRequest body);

        [Get("/tts/3/logs")]
        Task<ApiResponse<LogsResponse>> GetLogs([Query("messageId")] string messageId);
    }
}
