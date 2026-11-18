using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using SmartEvent.DurableFunction.Activities;
using SmartEvent.DurableFunction.Models.Responses;
using SmartEvent.DurableFunction.Models.Requests;
using System;
using System.Threading.Tasks;

namespace SmartEventDurableFunction.Orchestrators
{
    public static class RegistrationOrchestrator
    {
        [Function(nameof(RegistrationOrchestrator))]
        public static async Task RunAsync(
            [OrchestrationTrigger] TaskOrchestrationContext context,
            SmartEvent.DurableFunction.Models.Requests.RegistrationOrchestrationInput input)
        {
            var logger = context.CreateReplaySafeLogger(nameof(RegistrationOrchestrator));

            if (!context.IsReplaying)
            {
                logger.LogInformation(
                    "Starting registration orchestration - EventId: {EventId}, UserId: {UserId}, RegistrationId: {RegistrationId}",
                    input.EventId,
                    input.UserId,
                    input.RegistrationId);
            }

            try
            {
                // Step 1: Validate registration
                var validationResult = await context.CallActivityAsync<RegistrationActivityResult>(
                    nameof(ValidateRegistrationActivity),
                    input);

                if (!validationResult.IsSuccess)
                {
                    logger.LogError(
                        "Registration validation failed: {Error}",
                        validationResult.ErrorDetails);
                    throw new InvalidOperationException(validationResult.ErrorDetails);
                }

                // Step 2: Process registration
                var processingResult = await context.CallActivityAsync<RegistrationActivityResult>(
                    nameof(ProcessRegistrationActivity),
                    input);

                if (!processingResult.IsSuccess)
                {
                    logger.LogError(
                        "Registration processing failed: {Error}",
                        processingResult.ErrorDetails);
                    throw new InvalidOperationException(processingResult.ErrorDetails);
                }

                // Step 3: Send confirmation
                var confirmationResult = await context.CallActivityAsync<RegistrationActivityResult>(
                    nameof(SendConfirmationActivity),
                    input);

                if (!confirmationResult.IsSuccess)
                {
                    logger.LogError(
                        "Confirmation sending failed: {Error}",
                        confirmationResult.ErrorDetails);
                    // Don't throw here - confirmation failure shouldn't fail the registration
                }

                logger.LogInformation(
                    "Registration orchestration completed successfully - RegistrationId: {RegistrationId}",
                    input.RegistrationId);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Registration orchestration failed - RegistrationId: {RegistrationId}",
                    input.RegistrationId);
                throw;
            }
        }
    }
}