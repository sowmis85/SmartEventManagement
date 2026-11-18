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
    public static class ValidateRegistrationActivity
    {
        [Function(nameof(ValidateRegistrationActivity))]
        public static RegistrationActivityResult Run(
            [ActivityTrigger] RegistrationOrchestrationInput input,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(ValidateRegistrationActivity));

            logger.LogInformation(
                "Validating registration - EventId: {EventId}, UserId: {UserId}, RegistrationId: {RegistrationId}",
                input.EventId,
                input.UserId,
                input.RegistrationId);

            try
            {
                // Validate input parameters
                if (string.IsNullOrWhiteSpace(input.EventId))
                {
                    return new RegistrationActivityResult
                    {
                        IsSuccess = false,
                        Message = ValidationConstant.ValidationFailed,
                        ErrorDetails = ValidationConstant.EventIDRequired,
                    };
                }

                if (string.IsNullOrWhiteSpace(input.UserId))
                {
                    return new RegistrationActivityResult
                    {
                        IsSuccess = false,
                        Message = ValidationConstant.ValidationFailed,
                        ErrorDetails = ValidationConstant.UserIDRequired
                    };
                }

                if (string.IsNullOrWhiteSpace(input.RegistrationId))
                {
                    return new RegistrationActivityResult
                    {
                        IsSuccess = false,
                        Message = ValidationConstant.ValidationFailed,
                        ErrorDetails =ValidationConstant.RegisIDRequired
                    };
                }

                logger.LogInformation(
                    "Registration validation completed successfully for RegistrationId: {RegistrationId}",
                    input.RegistrationId);

                return new RegistrationActivityResult
                {
                    IsSuccess = true,
                    Message = ValidationConstant.RegistrationValidation
                };
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error validating registration for RegistrationId: {RegistrationId}",
                    input.RegistrationId);

                return new RegistrationActivityResult
                {
                    IsSuccess = false,
                    Message = ValidationConstant.ValidationFailed,
                    ErrorDetails = ex.Message
                };
            }
        }
    }
}
