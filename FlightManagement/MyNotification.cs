using System;
using System.Collections.Generic;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL
{
    public class MyNotification : INotification
    {
        // some properties
    }

    public class MyNotificationHandler1 : INotificationHandler<MyNotification>
    {
        public Task Handle(MyNotification notification, CancellationToken cancellationToken)
        {
            // do stuff

            return Task.CompletedTask;
        }
    }

    public class MyNotificationHandler2 : INotificationHandler<MyNotification>
    {
        public Task Handle(MyNotification notification, CancellationToken cancellationToken)
        {
            // do stuff

            return Task.CompletedTask;
        }
    }
}
