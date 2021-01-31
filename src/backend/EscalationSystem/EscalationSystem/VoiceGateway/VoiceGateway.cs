using EscalationSystem.Domain;
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
        public async Task<CallStatus> CallNumberAsync(string phoneNumber, string name, CancellationToken cancellationToken)
        {
            var infoBipApi = RestService.For<IInfoBipApi>("https://4mk5e1.api.infobip.com");

            var payload = VoiceRequest.Create(GetVoiceMessage(name), phoneNumber);

            var result = await infoBipApi.SendVoiceMessage(
                GetAuthorization(phoneNumber),
                payload);

            if (!result.IsSuccessStatusCode)
                return CallStatus.Lost;

            var messageId = result.Content?.Messages?.FirstOrDefault()?.MessageId;
            return await GetCallStatusByPolling(infoBipApi, phoneNumber, messageId, cancellationToken);
        }

        private async Task<CallStatus> GetCallStatusByPolling(
            IInfoBipApi infobipApi,
            string messageId,
            string phoneNumber,
            CancellationToken cancellationToken)
        {
            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, i => TimeSpan.FromMinutes(1));

            return await policy.ExecuteAsync(async () => 
                await GetCallStatus(infobipApi, phoneNumber, messageId, cancellationToken));
        }

        private async Task<CallStatus> GetCallStatus(
            IInfoBipApi infobipApi,
            string phoneNumber,
            string messageId,
            CancellationToken cancellationToken)
        {
            var response = await infobipApi.GetLogs(GetAuthorization(phoneNumber), messageId);

            if (!response.IsSuccessStatusCode || response.Content.CallStatus == CallStatus.Calling)
            {
                throw new Exception();
            }

            return response.Content.CallStatus;
        }

        private string GetAuthorization(string phoneNumber)
        {
            switch (phoneNumber)
            {
                case "5531980219052":
                    return "App e7dd12c97915c1e6887cbd2379acdc80-cc6751f9-1040-4c96-b012-d4fe66817dda";
                case "xxxx":
                    return "";
                default:
                    return "App e7dd12c97915c1e6887cbd2379acdc80-cc6751f9-1040-4c96-b012-d4fe66817dda";
            }
        }

        private string GetVoiceMessage(string name)
        {
            return $"Central de monitoramento X P. ., Olá {name}, ocorreu um incidente em produção e precisamos que você atue. Um chamado no Teilor foi atribuído para o seu time. Favor acessar o sistema para maiores informações";
        }
    }
}