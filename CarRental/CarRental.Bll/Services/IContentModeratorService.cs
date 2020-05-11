using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Bll.Services
{
    public interface IContentModeratorService
    {
        Task<string> ModerateText(string input);
    }
}
