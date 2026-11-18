    using global::SmartEvent.DurableFunction.Services;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
using SmartEvent.DurableFunction.Common;
using SmartEvent.DurableFunction.Models.Requests;
using SmartEvent.DurableFunction.Models.Responses;
using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    namespace SmartEvent.DurableFunction.Activities
    {
        public class ProcessRegistrationActivity
        {
            private readonly ISeatRegisService _seatService;

            public ProcessRegistrationActivity(ISeatRegisService seatService)
            {
                _seatService = seatService;
            }

            [Function(nameof(ProcessRegistrationActivity))]
            public async Task<RegistrationActivityResult> Run(
                [ActivityTrigger] RegistrationOrchestrationInput input,
                FunctionContext executionContext)
            {
                var logger = executionContext.GetLogger(nameof(ProcessRegistrationActivity));

                logger.LogInformation(
                    "Processing registration - RegistrationId: {RegistrationId}, EventId: {EventId}, Quantity: {Quantity}",
                    input.RegistrationId,
                    input.EventId,
                    input.Quantity);

                try
                {
                    // Step 1: Reserve seats via Hackdemo API
                    string idempotencyKey = $"{input.EventId}-{input.RegistrationId}-RESERVE";

                    logger.LogInformation(
                        "Reserving seats for RegistrationId: {RegistrationId}, Quantity: {Quantity}",
                        input.RegistrationId,
                        input.Quantity);

                    await _seatService.ReserveAsync(
                        input.EventId,
                        idempotencyKey,
                        new SeatRequest
                        {
                            RegistrationId = input.RegistrationId,
                            Quantity = input.Quantity
                        },
                        executionContext.CancellationToken);

                    logger.LogInformation(
                        "Seats reserved successfully - RegistrationId: {RegistrationId}",
                        input.RegistrationId);

                    // Step 2: Additional business logic (can be extended)
                    // - Send confirmation email
                    // - Update registration status

                    logger.LogInformation(
                        "Registration processed successfully - RegistrationId: {RegistrationId}",
                        input.RegistrationId);

                    return new RegistrationActivityResult
                    {
                        IsSuccess = true,
                        Message = ValidationConstant.RegistrationSuccess,
                        RegistrationId = input.RegistrationId,
                        EventId = input.EventId
                    };
                }
                catch (HttpRequestException httpEx)
                {
                    logger.LogError(
                        httpEx,
                        "Hackdemo API error during seat reservation - RegistrationId: {RegistrationId}, EventId: {EventId}",
                        input.RegistrationId,
                        input.EventId);

                    return new RegistrationActivityResult
                    {
                        IsSuccess = false,
                        Message = ValidationConstant.ReservationFailed,
                        ErrorDetails = $"API Error: {httpEx.Message}",
                        RegistrationId = input.RegistrationId,
                        EventId = input.EventId
                    };
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Error processing registration for RegistrationId: {RegistrationId}",
                        input.RegistrationId);

                    return new RegistrationActivityResult
                    {
                        IsSuccess = false,
                        Message = ValidationConstant.ProcessingFailed,
                        ErrorDetails = ex.Message,
                        RegistrationId = input.RegistrationId,
                        EventId = input.EventId
                    };
                }
            }
        }
    }


