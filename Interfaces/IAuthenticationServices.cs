using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using tl2_tp8_2025_michdeaver.Models;

namespace tl2_tp8_2025_michdeaver.Interfaces
{
    public interface IAuthenticationServices
    {
        bool Login(string username, string password);
        void Logout();
        bool IsAuthenticated();
        bool HasAccessLevel(string requiredAccessLevel);
    }
}