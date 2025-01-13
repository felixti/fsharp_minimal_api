module WebApplication4.Repositories

open Microsoft.Extensions.Configuration
open MongoDB.Driver
open WebApplication4.Models

let private getDatabase (settings: IConfiguration) =
    let client = new MongoClient(settings.GetConnectionString "MongoDb")
    client.GetDatabase(settings.["Db:DatabaseName"])

type MongoDbRepository(settings: IConfiguration) =
    let db = getDatabase settings
    let collection = db.GetCollection<Person>(settings.["Db:CollectionName"])

    member _.GetAll() =
        collection.Find(FilterDefinition<Person>.Empty).ToListAsync()

    member _.Add(person: Person) = collection.InsertOneAsync(person)

    member _.Update(id: string, person: Person) =
        let filter = Builders<Person>.Filter.Eq(_.Id, id)
        collection.ReplaceOneAsync(filter, person)

    member _.Delete(id: string) =
        collection.DeleteOneAsync(fun p -> p.Id = id)
