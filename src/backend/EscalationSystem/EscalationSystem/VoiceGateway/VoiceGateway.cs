using EscalationSystem.Domain;
using Microsoft.Extensions.Logging;
using Polly;
using Refit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EscalationSystem.VoiceGateway
{
    public class VoiceGateway : IVoiceGateway
    {
        private readonly ILogger<VoiceGateway> _logger;

        public VoiceGateway(ILogger<VoiceGateway> logger)
        {
            _logger = logger;
        }

        public async Task<string> CallNumberAsync(string phoneNumber, string name, CancellationToken cancellationToken)
        {
            var infoBipApi = RestService.For<IInfoBipApi>(GetUrl(phoneNumber));
            var payload = VoiceRequest.Create(GetVoiceMessage(name), phoneNumber);

            var result = await infoBipApi.SendVoiceMessage(
                GetAuthorization(phoneNumber),
                payload);

            if (!result.IsSuccessStatusCode)
                return null;

            return result.Content?.Messages?.FirstOrDefault()?.MessageId;
        }

        public async Task<CallStatus> GetCallStatusAsync(string messageId, string phoneNumber, CancellationToken cancellationToken)
        {
            var infoBipApi = RestService.For<IInfoBipApi>(GetUrl(phoneNumber));
            var response = await infoBipApi.GetLogs(GetAuthorization(phoneNumber), messageId);
            return response.IsSuccessStatusCode ? response.Content.CallStatus : CallStatus.Lost;
        }

        private string GetUrl(string phoneNumber)
        {
            switch (phoneNumber)
            {
                case "5531980219052":
                    return "https://4mk5e1.api.infobip.com";
                case "5531998232269":
                    return "https://wp4yd1.api.infobip.com";
                default:
                    return "https://4mk5e1.api.infobip.com";
            }
        }

        private string GetAuthorization(string phoneNumber)
        {
            switch (phoneNumber)
            {
                case "5531980219052":
                    return "App e7dd12c97915c1e6887cbd2379acdc80-cc6751f9-1040-4c96-b012-d4fe66817dda";
                case "5531998232269":
                    return "App 529c6315e546fa65b16f93425daf3dc9-f23e882e-5d9b-4e76-9cef-aa42119da700";
                default:
                    return "App e7dd12c97915c1e6887cbd2379acdc80-cc6751f9-1040-4c96-b012-d4fe66817dda";
            }
        }

        private string GetVoiceMessage(string name)
        {
            return $"Central de monitoramento X P.... ,Olá {name}, ocorreu um incidente em produção e precisamos que você atue. Um chamado no Teilor foi atribuído para o seu time. Favor acessar o sistema para maiores informações";
        }
    }
}