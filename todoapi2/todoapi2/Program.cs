using todoapi2;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString
    ("DefaultConnetion")));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


async Task<List<Todo>> GetAllTodo(DataContext context) => await context.Todo.ToListAsync();
app.MapGet("/Todo",async (DataContext context) => await context.Todo.ToListAsync());
app.MapGet("/Todo/{id}",async (DataContext context,int id) => await context.Todo.FindAsync(id) is Todo Item ? Results.Ok(Item) : Results.NotFound("Item not found"));
app.MapPost("App/Todo", async (DataContext context, Todo Item) =>
{
    context.Todo.Add(Item);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllTodo(context));

});
app.MapPut("/Todo/{id}", async (DataContext context, Todo Item, int id) =>
{
    var todoitem = await context.Todo.FindAsync(id);
    if (todoitem == null) return Results.NotFound("Items not Found");
    todoitem.FirstName = Item.FirstName;
    todoitem.LastName = Item.LastName;
    todoitem.Class = Item.Class;
    todoitem.Subject = Item.Subject;

    await context.SaveChangesAsync();
    return Results.Ok(await GetAllTodo(context));
  });
app.MapDelete("/Delete/{id}", async (DataContext context, int id) =>
{
    var todoitem = context.Todo.FindAsync(id);
    if (await todoitem == null) return Results.NotFound("Items not Found");
    context.Remove(todoitem);
    await context.SaveChangesAsync();
    return Results.Ok(await GetAllTodo(context));

});

app.Run();
