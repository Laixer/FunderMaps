using Laixer.EventBus.Handler;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Laixer.EventBus.Tests
{
    public class EventBus
    {
        private interface IMyEvent : IEvent { }

        private class MyEvent : IMyEvent { }

        private class MyEventHandler : IEventHandler<IMyEvent>
        {
            public Task HandleEventAsync(EventHandlerContext<IMyEvent> context, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }

        [Fact]
        public void Test1()
        {
            //new Laixer.EventBus.Internal.DefaultEventBusService(null, null, null);

            var serviceCollection = new ServiceCollection();

            var builder = serviceCollection.AddEventBus();
            //builder.AddHandler<MyEvent, MyEventHandler>(nameof(MyEventHandler));
        }
    }
}
