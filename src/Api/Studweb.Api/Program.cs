using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Studweb.Api.Common;
using Studweb.Api.Endpoints;
using Studweb.Api.OptionsSetup;
using Studweb.Application;
using Studweb.Domain;
using Studweb.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    
    // Add services to the container.
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("frontend", policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
        });
    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer();


    builder.Services.ConfigureOptions<JwtOptionsSetup>();
    builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
    
    builder.Services
        .AddApplication(builder.Configuration)
        .AddInfrastructure(builder.Configuration)
        .AddSingleton<ProblemDetailsFactory, StudwebProblemDetailsFactory>();

    //builder.Services.AddAuthorization(builder.Configuration);

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Wprowadź token JWT w polu 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.AddAuthenticationEndpoints();
    app.AddTestEnpoints();
    app.AddNoteEndpoints();

    app.UseHttpsRedirection();

    app.UseCors("frontend");
    
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

public partial class Program
{
}