namespace WebApplication4.Api

open Microsoft.AspNetCore.Http
open WebApplication4.Models
open WebApplication4.Repositories

module Handlers =

    let getPeopleHandler (ctx: HttpContext) (db: MongoDbRepository) =
        let people = db.GetAll() |> Async.AwaitTask |> Async.RunSynchronously
        Results.Ok(people)

    let addPersonHandler (ctx: HttpContext) (db: MongoDbRepository) (input: Person) =
        db.Add input |> Async.AwaitTask |> Async.RunSynchronously
        Results.Created($"/persons/{input.Id}", input)

// let updatePersonHandler (ctx: HttpContext) (id: string) (input: Person) = Results.Ok(input)
//
// let deletePersonHandler (ctx: HttpContext) (id: string) = Results.NoContent()
