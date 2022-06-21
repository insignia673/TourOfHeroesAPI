# TourOfHeroesAPI

Angular project with C# ASP.NET core back-end api as well as SQL.

To start project you must run the c# api and also run the angular project with "ng serve" command

# ORMF23

Create an instance of OrmF23ConnectionFactory and pass the database connection string in the constructor.
for dependency injection: create an singleton of type IDbConnectionFactory.

IDbConnectionFactory will provide an SqlConnection instance with the OpenDbConnection() method.

### Example:
```cs
// dependency injection section
serviceCollection.AddSingleton<IDbConnectionFactory>(new OrmF23ConnectionFactory(Configuration.GetConnectionString("Default")));
```

```cs
private readonly IDbConnection _db;
public ClassConstructor(IDbConnectionFactory connectionFactory)
{
    _db = connectionFactory.OpenDbConnection();
}

public async Task<User> GetUsers()
{
    return await _db.SelectAsync<User>(u => u.Id > 5);
}
```

### Database Methods
```cs
  //Method name                         Parameters
  
  //Selects all that match from database table of type T
  SelectAsync<T>                      Func<T, bool> predicate             
  
  //Selects a single match from database table of type T
  SingleAsync<T>                      Func<T, bool> predicate             
  
  //Inserts T entity into database of type T, returns entity id if selectIdentity is true
  InsertAsync<T>                      T entity, bool SelectIdentity       
  
  //Updates T entity into database of type T, returns entity id if selectIdentity is true
  UpdateAsync<T>                      T entity, bool selectIdentity       
  
  //Deletes entities from database matching predicate
  DeleteAsync<T>                      Func<T, bool> predicate        
  
  //Checks if any entity matches predicate
  ExistsAsync<T>                      Func<T, bool> predicate             
  
  //If database table of type T exist it will delete it, then it will create table of type T
  DropAndCreateTableAsync<T>          N/A                                  
  
  //Drops existing table of type T
  DropTableAsync<T>                   N/A                                 
```
