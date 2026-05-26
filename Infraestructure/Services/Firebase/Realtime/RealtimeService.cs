using Aplication.Interfaces.Firebase.Realtime;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Services.Firebase.Realtime
{
    public class RealtimeService:IRealtimeService
    {
        readonly FirebaseClient _client;
        

        public RealtimeService()
        {
            _client = new FirebaseClient("https://gestoralimentacionzoo-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        public FirebaseClient Client => _client;

        public async Task<string> AddAsync<T>(string path, T data)
        {
            var result = await _client.Child(path).PostAsync(data);
            return result.Key;
        }

        public async Task<List<(string Id, T Data)>> GetAllAsync<T>(string path)
        {
            var result = await _client.Child(path).OnceAsync<T>();
            return result.Select(x => (x.Key, x.Object)).ToList();
        }
        public async Task UpdateAsync<T>(string path, string id, T data)
        {
            await _client.Child(path).Child(id).PutAsync(data);
        }

        public async Task DeleteAsync(string path, string id)
        {
            await _client.Child(path).Child(id).DeleteAsync();
        }



    }
}
