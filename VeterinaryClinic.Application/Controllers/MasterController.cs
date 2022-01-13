using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using VeterinaryClinic.Application.Models;

namespace VeterinaryClinic.Application.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class MasterController: ControllerBase
{
    private string ConnectionString => $"User ID={Request.Cookies["username"]};Password={Request.Cookies["userpassword"]};Host=localhost;Port=5432;Database=veterinaryclinic_db;";
    
    
    [HttpPost]
    [Route("create-item")]
    public IActionResult Create([FromBody]Master master)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                var returnId = connection.ExecuteScalar<int>(
                    "insert into master (id, phone, address, fullname) values (nextval('master_sequence'), @phone, @address, @fullname) RETURNING Id",
                    new
                    {
                        phone = master.Phone,
                        address = master.Address,
                        fullname = master.FullName
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
                var master = connection.Query<Master>("select * from Master where id = @id",
                    new
                    {
                        id
                    });
                return new JsonResult(new
                {
                    master
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
    public IActionResult Update(Master master)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                connection.Execute("update master set phone = @phone, address = @address, fullname = @fullname where id = @id", new
                {
                    id = master.Id,
                    phone = master.Phone,
                    address = master.Address,
                    fullname = master.FullName
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
                connection.Execute("delete from Master where id = @id", new
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
                var masters = connection.Query<Master>("select * from master").ToList();
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