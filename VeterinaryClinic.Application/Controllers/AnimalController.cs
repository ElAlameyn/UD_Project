using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using VeterinaryClinic.Application.Models;

namespace VeterinaryClinic.Application.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class AnimalController: ControllerBase
{
    private string ConnectionString => $"User ID={Request.Cookies["username"]};Password={Request.Cookies["userpassword"]};Host=localhost;Port=5432;Database=veterinaryclinic_db;";

    [HttpPost]
    [Route("create-item")]
    public IActionResult Create([FromBody] Animal animal)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                var returnId = connection.ExecuteScalar<int>(
                    "insert into animal (id, masterid, name, breed) values (nextval('animal_sequence'), @masterid, @name, @breed) RETURNING Id",
                    new
                    {
                        masterid = animal.MasterId,
                        name = animal.Name,
                        breed = animal.Breed
                    });
                return new JsonResult(new
                {
                    returnId
                });
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status502BadGateway);
            }
        }
    }
    
    [HttpGet]
    [Route("get-item")]
    public IActionResult Get(int id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                var animal = connection.Query<Animal>("select * from Animal where id = @id",
                    new
                    {
                        id
                    });
                return new JsonResult(new
                {
                    animal
                });
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status502BadGateway);
            }
        }
    }
    
    [HttpPost]
    [Route("update-item")]
    public IActionResult Update(Animal animal)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                connection.Execute("update animal set masterid = @masterid, name = @name, breed = @breed where id = @id", new
                {
                    id = animal.Id,
                    masterid = animal.MasterId,
                    name = animal.Name,
                    breed = animal.Breed
                });
                return new JsonResult(new
                {
                    updated = "updated"
                });
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status502BadGateway);
            }
        }
    }
    
    [HttpGet]
    [Route("remove-item")]
    public IActionResult Delete(int id)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                connection.Execute("delete from Animal where id = @id", new
                {
                    id
                });
                return new JsonResult(new
                {
                    status = StatusCodes.Status200OK
                });
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status502BadGateway);
            }
        }
    }
    
    [HttpGet]
    [Route("get-all-items")]
    public IActionResult GetAll()
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                var masters = connection.Query<Animal>("select * from Animal").ToList();
                return new JsonResult(new
                {
                    masters
                });
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status502BadGateway);
            }
        }
    }
}