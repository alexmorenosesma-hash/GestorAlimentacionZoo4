using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Firebase.Realtime
{
    public interface IRealtimeService
    {
        public  Task<string> AddAsync<T>(string path, T data);

        public  Task<List<(string Id, T Data)>> GetAllAsync<T>(string path);
        public  Task UpdateAsync<T>(string path, string id, T data);

        public  Task DeleteAsync(string path, string id);
    }
}
