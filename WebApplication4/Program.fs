namespace WebApplication4

#nowarn "20"

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Http.Json
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open WebApplication4.Api
open WebApplication4.Models
open WebApplication4.Repositories

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.Configure<JsonOptions>(fun (options: JsonOptions) ->
            options.SerializerOptions.PropertyNamingPolicy <- Text.Json.JsonNamingPolicy.CamelCase
            options.SerializerOptions.PropertyNameCaseInsensitive <- true)

        builder.Services.AddSingleton(MongoDbRepository(builder.Configuration))

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.MapGet("/people", Func<HttpContext, MongoDbRepository, IResult>(Handlers.getPeopleHandler))
        app.MapPost("/people", Func<HttpContext, MongoDbRepository, Person, IResult>(Handlers.addPersonHandler))

        app.Run()

        exitCode
