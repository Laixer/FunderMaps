using FunderMaps.Core.Types.BackgroundTasks;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Grpc.DummyTasks
{
    // TODO Remove
    public class AsyncDummyTask : BackgroundTaskBase
    {
        public AsyncDummyTask()
        {
        }

        public override bool CanHandle(object value) =>
            value is string s && s == "asyncdummytask";

        public override async Task ProcessAsync(BackgroundTaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Simulate work.
            await Task.Delay(2000);
        }
    }
}
