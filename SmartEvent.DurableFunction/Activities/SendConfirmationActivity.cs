using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SmartEvent.DurableFunction.Common;
using SmartEvent.DurableFunction.Models.Requests;
using SmartEvent.DurableFunction.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Activities
{
    public static class SendConfirmationActivity
    {
        [Function(nameof(SendConfirmationActivity))]
        public static RegistrationActivityResult Run(
            [ActivityTrigger] RegistrationOrchestrationInput input,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(SendConfirmationActivity));

            logger.LogInformation(
                "Sending confirmation for registration - RegistrationId: {RegistrationId}",
                input.RegistrationId);

            try
            {
                // Send confirmation email/notification
                // Replace with actual email service call

                logger.LogInformation(
                    "Confirmation sent successfully for RegistrationId: {RegistrationId}",
                    input.RegistrationId);

                return new RegistrationActivityResult
                {
                    IsSuccess = true,
                    Message = ValidationConstant.ConfirmationSent
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error sending confirmation for RegistrationId: {RegistrationId}",
                    input.RegistrationId);

                return new RegistrationActivityResult
                {
                    IsSuccess = false,
                    Message =ValidationConstant.ConfirmationFailed,
                    ErrorDetails = ex.Message
                };
            }
        }
    }
}
