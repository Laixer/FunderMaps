using FunderMaps.Core.Entities;
using System;

namespace FunderMaps.Testing.Repositories
{
    public class UserRecord
    {
        public User User { get; set; }
        public uint AccessFailedCount { get; set; }
        public uint AccessCount { get; set; }
        public DateTime LastLogin { get; set; }
        public string Password { get; set; }
        public bool IsLockedOut { get; set; }
    }
}
