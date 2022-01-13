using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using VeterinaryClinic.Application.Models;

namespace VeterinaryClinic.Application.Controllers;

[ApiController]
[Route("api/[controller]/")]
public class DiagnosisController: ControllerBase
{
    private string ConnectionString => $"User ID={Request.Cookies["username"]};Password={Request.Cookies["userpassword"]};Host=localhost;Port=5432;Database=veterinaryclinic_db;";

    [HttpPost]
    [Route("create-item")]
    public IActionResult Create([FromBody] Diagnosis diagnosis)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                var returnId = connection.ExecuteScalar<int>(
                    "insert into diagnosis (id, name, setupdate, doctorId, animalId) values (nextval('diagnosis_sequence'),
                    @name, @setupdate, @doctorId, @animalId) RETURNING Id",
                    new
                    {
                        name = diagnosis.Name,
                        setupdate = diagnosis.SetupDate,
                        doctorId = diagnosis.DoctorId,
                        animalId = diagnosis.AnimalId
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
                var diagnosis = connection.Query<Diagnosis>("select * from Diagnosis where id = @id",
                    new
                    {
                        id
                    });
                return new JsonResult(new
                {
                    diagnosis
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
    public IActionResult Update(Diagnosis diagnosis)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            try
            {
                connection.Execute("update Diagnosis set name = @name, setupdate = @setupdate, doctorId = @doctorId, animalId = @animalId where id = @id", new
                {
                    id = diagnosis.Id,
                    name = diagnosis.Name,
                    setupdate = diagnosis.SetupDate,
                    doctorId = diagnosis.DoctorId,
                    animalId = diagnosis.AnimalId
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
                connection.Execute("delete from diagnosis where id = @id", new
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
                var masters = connection.Query<Diagnosis>("select * from diagnosis").ToList();
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