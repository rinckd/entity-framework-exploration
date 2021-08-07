using System;
using SamuraiApp.Data;

namespace SamuraiApp.Data
{
    public class ReplacementUserIdService : IUserIdService
    {
        public Guid GetUserId()
        {
            return Guid.Empty;
        }
    }
}