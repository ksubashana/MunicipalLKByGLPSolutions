using System;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.Interfaces
{
    public interface IEmailService
    {
        Task SendInspectionAssignmentEmailAsync(string toEmail, string inspectorName, string applicationNumber, DateTime assignmentDate, string remarks = null);
    }
}