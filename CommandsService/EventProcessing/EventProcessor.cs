using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProccessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                addPlatform(message);
                break;
                default:
                break;
            }
        }

        private void addPlatform(string PlatformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                //kur smunesh me bo dependency injection se scopes of services i ki tndryshem
                 var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var PlatformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(PlatformPublishedMessage);

                try
                {
                     var plat = _mapper.Map<Platform>(PlatformPublishedDto);
                     if(!repo.ExternalPlatformExists(plat.ExternalID))
                     {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine("-->Platform added...");
                     }
                     else
                     {
                         Console.WriteLine("-->Platform already exists...");
                     }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"--> Could not add platform to DB{ex.Message}");
                }
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published": 
                Console.WriteLine("--> Platform published event detected"); 
                return EventType.PlatformPublished;
                default: Console.WriteLine("--> Could not determine event type");
                return EventType.Undetermined;
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}