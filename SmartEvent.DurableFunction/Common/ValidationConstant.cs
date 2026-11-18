using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Common
{
    public class ValidationConstant
    {
        public const string RegistrationSuccess = "Registration processed successfully.";
        public const string RegistrationValidation = "Registration validated successfully.";
        public const string ReservationFailed = "Seat reservation failed";
        public const string ProcessingFailed = "Processing failed";
        public const string ConfirmationSent = "Confirmation sent successfully.";
        public const string ConfirmationFailed = "Confirmation failed.";
        public const string ValidationFailed = "Validation Failed";
        public const string EventCreated = "Event Created";
        public const string EventIDRequired = "EventId is required.";
        public const string UserIDRequired = "UserId is required.";
        public const string RegisIDRequired = "RegistrationId is required.";
        public const string HTTPStarterMessage="HTTP starter function called for registration orchestrator.";
        public const string OrchestratorStarted = "Orchestrator started successfully.";
        public const string RequestBodyParseError = "Request body could not be parsed.";
        public const string MissingIDInputParameters = "EventId, UserId, and RegistrationId are required.";
        public const string OrchestratorStartFailed = "An error occurred while starting the orchestration";
        public const string URLConfigurationError = "ApiSettings:HackDemoApiUrl is not configured.";
        public const string OrchestrationInstanceId = "Orchestration started with InstanceId: {InstanceId}";
    }
}
