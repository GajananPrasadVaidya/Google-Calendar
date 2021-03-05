using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCalendarAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientId = "Your Client Id";
            string clientSecret = "Your Client secret";

            string[] scopes = new string[] {
                     CalendarService.Scope.Calendar, // Manage your calendars
 	                 CalendarService.Scope.CalendarReadonly, // View your Calendars
                 };

            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            }, scopes, "user", CancellationToken.None).Result;    

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Test",
            });

            Console.WriteLine("Enter email");
            string email = Console.ReadLine();
            EventsResource.ListRequest request = service.Events.List(email);
            request.TimeMin = DateTime.Now;
            request.TimeMax = DateTime.Now.AddDays(1);
            request.TimeZone = "India Standard Time";
            Events events = new Events();
            try
            {
                events = request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured", ex);
            }
            
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }

                    Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }

            Console.ReadKey();
        }
    }
}
