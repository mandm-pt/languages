using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace languages.HttpModules
{
    internal class MoviesModule : BaseModule
    {
        const string Path = "/Movies";
        const string paramName = "id";

        protected override string ModuleName => nameof(MoviesModule);

        public override bool CanProcess(HttpListenerRequest request)
        {
            return request.Url.PathAndQuery.StartsWith(Path, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override async Task ProcessRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (string.IsNullOrWhiteSpace(request.QueryString.Get(paramName)))
            {
                await PrintUsageAsync(response);
                return;
            }

            int id;
            string title;
            bool success = int.TryParse(request.QueryString.GetValues(paramName)[0], out id);

            if (!success)
                throw new ApplicationException("You want explode the server! NOT TODAY!");

            using (var connection = new SqliteConnection("Data Source=./../movies_sqlite.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT title
                                        FROM movies
                                        WHERE id = $id";

                command.Parameters.AddWithValue("$id", id);

                title = (string)await command.ExecuteScalarAsync();
            }

            if (string.IsNullOrWhiteSpace(title))
                await response.WriteResponseTextAsync(
                    $"<h1>There's no movie with id {id}</h1>"
                );
            else
                await response.WriteResponseTextAsync(
                    $"<h1>The movie with id {id} is {title}</h1>"
                );
        }

        private async Task PrintUsageAsync(HttpListenerResponse response)
        {
            await response.WriteResponseTextAsync(
                $"<h1>Parameter missing</h1><h2>Usage:</h2>{Path}?{paramName}=1"
            );
        }
    }
}