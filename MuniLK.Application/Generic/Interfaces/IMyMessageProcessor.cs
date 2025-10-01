using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.Interfaces
{
    public interface IMyMessageProcessor
    {
        Task ProcessMessageAsync(string message);
    }

    // In Application/Services
    //public class MyMessageProcessor : IMyMessageProcessor
    //{
    //    public Task ProcessMessageAsync(string message)
    //    {
    //        // Deserialize and process your message here
    //        // e.g. save to DB, call domain services, etc.
    //        return Task.CompletedTask;
    //    }
    //}
}
