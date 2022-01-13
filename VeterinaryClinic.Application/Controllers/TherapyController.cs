using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using VeterinaryClinic.Application.Models;

namespace VeterinaryClinic.Application.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class TherapyController: ControllerBase
{
    private string ConnectionString => $"User ID={Request.Cookies["username"]};Password={Request.Cookies["userpassword"]};Host=localhost;Port=5432;Database=veterinaryclinic_db;";

    [HttpPost]
    [Route("create-item")]
    public IActionResult Create([FromBody] Therapy therapy)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                var returnId = connection.ExecuteScalar<int>(
                    "insert into Therapy (id, diagnosisId, name, price) values (nextval('therapy_sequence'), @diagnosisId, @name, @price) RETURNING Id",
                    new
                    {
                        diagnosisId = therapy.DiagnosisId,
                        name = therapy.Name,
                        price = therapy.Price
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
                var therapy = connection.Query<Therapy>("select * from therapy where id = @id",
                    new
                    {
                        id
                    });
                return new JsonResult(new
                {
                    therapy
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
    public IActionResult Update(Therapy therapy)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                connection.Execute("update therapy set diagnosisId = @diagnosisId, name = @name, price = @price where id = @id", new
                {
                    id = therapy.Id,
                    diagnosisId = therapy.DiagnosisId,
                    name = therapy.Name,
                    price = therapy.Price
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
                connection.Execute("delete from therapy where id = @id", new
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
                var masters = connection.Query<Therapy>("select * from therapy").ToList();
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