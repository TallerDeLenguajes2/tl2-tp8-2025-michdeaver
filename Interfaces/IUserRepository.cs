using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using tl2_tp8_2025_michdeaver.Models;

namespace tl2_tp8_2025_michdeaver.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(string username, string password);
    }
}