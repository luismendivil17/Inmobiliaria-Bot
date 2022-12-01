// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace Microsoft.BotBuilderSamples.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly FlightBookingRecognizer _cluRecognizer;
        protected readonly ILogger Logger;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(FlightBookingRecognizer cluRecognizer, BookingDialog bookingDialog, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _cluRecognizer = cluRecognizer;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(bookingDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_cluRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: CLU is not configured. To enable all capabilities, add 'CluProjectName', 'CluDeploymentName', 'CluAPIKey' and 'CluAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var weekLaterDate = DateTime.Now.AddDays(7).ToString("MMMM d, yyyy");
            var messageText = stepContext.Options?.ToString() ?? $"Bienvenido a Inmobiliaria Cibertec, en que podemos ayudarte?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_cluRecognizer.IsConfigured)
            {
                // CLU is not configured, we just run the BookingDialog path with an empty BookingDetailsInstance.
                return await stepContext.BeginDialogAsync(nameof(BookingDialog), new BookingDetails(), cancellationToken);
            }

            // Call CLU and gather any potential booking details. (Note the TurnContext has the response to the prompt.)
            var cluResult = await _cluRecognizer.RecognizeAsync<FlightBooking>(stepContext.Context, cancellationToken);
            switch (cluResult.GetTopIntent().intent)
            {
                /* case FlightBooking.Intent.BookFlight:
                     // Initialize BookingDetails with any entities we may have found in the response.
                     var bookingDetails = new BookingDetails()
                     {
                         Destination = cluResult.Entities.GetToCity(),
                         Origin = cluResult.Entities.GetFromCity(),
                         TravelDate = cluResult.Entities.GetFlightDate(),
                     };

                     // Run the BookingDialog giving it whatever details we have from the CLU call, it will fill out the remainder.
                     return await stepContext.BeginDialogAsync(nameof(BookingDialog), bookingDetails, cancellationToken);

                 case FlightBooking.Intent.GetWeather:
                     // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                     var getWeatherMessageText = "TODO: get weather flow here";
                     var getWeatherMessage = MessageFactory.Text(getWeatherMessageText, getWeatherMessageText, InputHints.IgnoringInput);
                     await stepContext.Context.SendActivityAsync(getWeatherMessage, cancellationToken);
                     break;
                */
                case FlightBooking.Intent.Saludar:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText6 = "Hola";
                    var getWeatherMessage6 = MessageFactory.Text(getWeatherMessageText6, getWeatherMessageText6, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage6, cancellationToken);
                    break;
                case FlightBooking.Intent.ConsultaVisitante:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText = "Sí, no hay limite de personas y para registrarlos tienes que ir al Visitantes->Registrar un Visitante -- http://localhost:4200/spring/registraVisitante";
                    var getWeatherMessage = MessageFactory.Text(getWeatherMessageText, getWeatherMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage, cancellationToken);
                    break;
                case FlightBooking.Intent.ConsultaPropietario:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText8 = "Sí, para registrarte tienes que ir al Propietario->Registrar un Propietario -- http://localhost:4200/spring/registraPropietario";
                    var getWeatherMessage8 = MessageFactory.Text(getWeatherMessageText8, getWeatherMessageText8, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage8, cancellationToken);
                    break;
                case FlightBooking.Intent.ConsultaMascota:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText2 = "Si, para registrarlo debes ir a Mascota - Registrar Mascota -- http://localhost:4200/spring/registraMascota";
                    var getWeatherMessage2 = MessageFactory.Text(getWeatherMessageText2, getWeatherMessageText2, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage2, cancellationToken);
                    break;
                case FlightBooking.Intent.ConsultaBoleta:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText3 = "Si, son virtuales y se ven en Consulta Boleta -- http://localhost:4200/spring/actualizaBoleta";
                    var getWeatherMessage3 = MessageFactory.Text(getWeatherMessageText3, getWeatherMessageText3, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage3, cancellationToken);
                    break;
                case FlightBooking.Intent.ConsultaDepartamento:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText4 = "Si, para registrarlo debes ir a Departamento - Registrar Departamento -- http://localhost:4200/spring/registraDepartamento";
                    var getWeatherMessage4 = MessageFactory.Text(getWeatherMessageText4, getWeatherMessageText4, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage4, cancellationToken);
                    break;
                case FlightBooking.Intent.ConsultaVisita:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText9 = "Para registrarlo debes ir a Visita - Registrar Visita -- http://localhost:4200/spring/registraVisita";
                    var getWeatherMessage9 = MessageFactory.Text(getWeatherMessageText9, getWeatherMessageText9, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage9, cancellationToken);
                    break;
                case FlightBooking.Intent.ConsultaIncidente:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText5 = "Para registrarlo debes ir a Incidente - Registrar Incidente -- http://localhost:4200/spring/registraIncidente";
                    var getWeatherMessage5 = MessageFactory.Text(getWeatherMessageText5, getWeatherMessageText5, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage5, cancellationToken);
                    break;
                case FlightBooking.Intent.Despedir:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText7 = "Adios";
                    var getWeatherMessage7 = MessageFactory.Text(getWeatherMessageText7, getWeatherMessageText7, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage7, cancellationToken);
                    break;
                default:
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {cluResult.GetTopIntent().intent})";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }
        
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is BookingDetails result)
            {
                // Now we have all the booking details call the booking service.

                // If the call to the booking service was successful tell the user.

                var timeProperty = new TimexProperty(result.TravelDate);
                var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
                var messageText = $"I have you booked to {result.Destination} from {result.Origin} on {travelDateMsg}";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }

           // Restart the main dialog with a different message the second time around
            var promptMessage = "";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
