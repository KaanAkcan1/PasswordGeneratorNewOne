using Newtonsoft.Json;
using PasswordGeneratorOld.Models;
using PasswordGeneratorOld.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PasswordGenerateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapGet("/v2/generate", (string? length, string? count, string? numbers, string? upperChars, string? lowerChars, string? specialChars,
    string? mustHave, string? startsWith, string? endsWith, string? include, string? exclude, string? type, PasswordGenerateService passwordGenerate) =>
{
    var request = new RequestModel()
    {
        Length = (passwordGenerate.RequestCheck(length) == null) ? 16 : (int)passwordGenerate.RequestCheck(length),
        Count = (passwordGenerate.RequestCheck(count) == null) ? 1 : (int)passwordGenerate.RequestCheck(count),
        Numbers = (passwordGenerate.RequestCheck(numbers) == null) ? 0 : (int)passwordGenerate.RequestCheck(numbers),
        UpperChars = (passwordGenerate.RequestCheck(upperChars) == null) ? 0 : (int)passwordGenerate.RequestCheck(upperChars),
        LowerChars = (passwordGenerate.RequestCheck(lowerChars) == null) ? 0 : (int)passwordGenerate.RequestCheck(lowerChars),
        SpecialChars = (passwordGenerate.RequestCheck(specialChars) == null) ? 1 : (int)passwordGenerate.RequestCheck(specialChars),
        MustHave = mustHave,
        StartsWith = startsWith,
        EndsWith = endsWith,
        Include = include,
        Exclude = exclude,
        Type = (type == null) ? "" : type
    };

    var data = passwordGenerate.GeneratePasswordList(request);

    if (data.Success)
    {
        if (request.Type == "json")
        {
            var json = JsonConvert.SerializeObject(data.Data);
            //var response = passwordGenerate.ToJson(data.Data);
            var filePath = Path.Combine("PasswordList.json");
            System.IO.File.WriteAllText(filePath, json);
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return Results.File(memory, "application/json", Path.GetFileName(filePath));

            //return Results.Json(json);
        }
        else if (request.Type == "text")
        {
            File.WriteAllLines("PasswordList.txt", data.Data);
            var filePath = Path.Combine("PasswordList.txt");
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return Results.File(bytes, "text/plain", Path.GetFileName(filePath));
        }
        else
        {
            return Results.Ok(data.Data);
        }
    }

    else
        return Results.NotFound();

})
.WithName("GetNew");

app.MapPost("/v2/generate", (FirstRequestModel firstRequest, PasswordGenerateService passwordGenerate) =>
{
    var request = new RequestModel();
    request = passwordGenerate.PostRequestCheck(firstRequest);

    var data = passwordGenerate.GeneratePasswordList(request);

    if (data.Success)
    {
        if (request.Type == "json")
        {
            var json = JsonConvert.SerializeObject(data.Data);
            //var response = passwordGenerate.ToJson(data.Data);
            var filePath = Path.Combine("PasswordList.json");
            System.IO.File.WriteAllText(filePath, json);
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return Results.File(memory, "application/json", Path.GetFileName(filePath));

            //return Results.Json(json);
        }
        else if (request.Type == "text")
        {
            File.WriteAllLines("PasswordList.txt", data.Data);
            var filePath = Path.Combine("PasswordList.txt");
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return Results.File(bytes, "text/plain", Path.GetFileName(filePath));
        }
        else
        {
            return Results.Ok(data.Data);
        }
    }

    else
        return Results.NotFound();

})
.WithName("PostNew");

app.Run();

//How it work
//1 GeneratePassworList main service works

//2 ControlOfMustConditions service checks the must Conditions
//Must conditionlarındaki stringler CanNotContain deki charları içermesine göre null'a döndürüldü.

//3 PasswordLengthOrganizer change request.PasswordLenth
//Eğer Must conditionları null değilse bunları PasswordLength'den çıkarıp random atayacağımız char sayısını bulduk
//Minumum char sayılarının toplamını kalan sayıdan çıkarıp eğer sıfırdan küçükse onu yoksa önceki değeri dönderdik

//4 PasswordLength < 0 => MinimalizeRequest service decreases Minimumumvalues at request.

//5 GeneratePassword service works

//6 OrganizeCharLists service works for correcting char lists according to CanContain and CanNotContain in request

//7 OrganizeMinumumNumber service corrects Minimumchar numbers