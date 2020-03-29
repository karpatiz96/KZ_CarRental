using CarRental.Bll.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.IServices
{
    public interface ICloudStorageService
    {
        Task SendMessage(QueueEmailMessage message);
    }
}
