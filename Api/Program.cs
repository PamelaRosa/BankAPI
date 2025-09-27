using BankAPI.Core.Dtos;
using BankAPI.Core.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAccountService, AccountService>();

var app = builder.Build();

app.MapPost("/reset", (IAccountService service) =>
{
    service.Reset();
    return Results.Text("OK");
});

app.MapGet("/balance", (IAccountService service, [FromQuery] string? account_id) =>
{
    if (string.IsNullOrEmpty(account_id))
    {
        return Results.BadRequest("O parâmetro 'account_id' é obrigatório.");
    }
    var account = service.GetAccount(account_id);
    return account is null ? Results.NotFound(0) : Results.Ok(account.Balance);
});

app.MapPost("/event", (EventDto eventDto, IAccountService service) =>
{
    try
    {
        switch (eventDto.Type)
        {
            case "deposit":
                var depositAccount = service.Deposit(eventDto.Destination!, eventDto.Amount);
                return Results.Created("", new { destination = depositAccount });
            case "withdraw":
                var withdrawAccount = service.Withdraw(eventDto.Origin!, eventDto.Amount);
                return withdrawAccount is null ? Results.NotFound(0) : Results.Created("", new { origin = withdrawAccount });
            case "transfer":
                var result = service.Transfer(eventDto.Origin!, eventDto.Destination!, eventDto.Amount);
                return result is null ? Results.NotFound(0) : Results.Created("", new { origin = result?.origin, destination = result?.destination });

            default:
                return Results.BadRequest(new { error = "Tipo inválido." });
        }
    }
    catch(InvalidOperationException ex)
    {
        return Results.UnprocessableEntity(new { error = ex.Message });
    }
    catch(ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.Run();
