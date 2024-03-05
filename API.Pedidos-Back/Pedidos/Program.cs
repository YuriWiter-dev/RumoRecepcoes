using DTI.Pedidos.DbContexto;
using DTI.Pedidos.Entidades;
using DTI.Pedidos.Filters;
using DTI.Pedidos.Middlewares;
using DTI.Pedidos.Migrations;
using DTI.Pedidos.Repositorios;
using DTI.Pedidos.Servicos;
using DTI.Pedidos.Utils;
using DTI.Pedidos.Validator;
using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DTI.Pedidos;

public class Program
{
    public static IServiceProvider _serviceProvider;

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
                formatter: new CompactJsonFormatter(),
                path: "logs/log.json",
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 10485760, // 10 MB
                retainedFileCountLimit: 20
             )
            .CreateLogger();

        try
        {
            Log.Information("Starting up");
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
            });
            SetarAppSettings();
            InjetarDependencias(builder.Services);
            Configure(builder.Build());
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, ex.Message);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void Configure(WebApplication app)
    {
        SetarMiddlewareDeTratativaDeErros(app);

        SetarPastaLogs(app);

        app.UseCors("AllOrigins");

        SetarLogsPtBrCulture(app);

        #region Swagger

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Pedidos V1");
        });

        #endregion Swagger

        MapearRotasMinimalApi(app);

        ExecutarMigrations();

        app.Run();
    }

    private static void MapearRotasMinimalApi(WebApplication app)
    {
        app.UseHttpsRedirection();

        #region Pedido

        app.MapGet("/pedido/{id}", async (IPedidoServico servico, int id) =>
        {
            return Results.Ok(await servico.ObterPedido(id));
        })
        .WithName("ObterPedido")
        .WithTags("Pedido");

        app.MapGet("/pedidos", async (IPedidoServico servico) =>
        {
            return Results.Ok(await servico.ObterPedidos());
        })
        .WithName("ObterPedidos")
        .WithTags("Pedido");

        app.MapPost("/pedido", async (
            IPedidoServico servico,
            IValidator<Pedido> validator,
            Pedido pedido) =>
        {
            Constantes.ValidarRequest(await validator.ValidateAsync(pedido));
            await servico.InserirPedido(pedido);
            return Results.Ok();
        })
        .WithName("InserirPedido")
        .WithTags("Pedido");

        app.MapPut("/pedido/{id}", async (
            IPedidoServico servico,
            IValidator<Pedido> validator,
            int id,
            Pedido pedido) =>
        {
            Constantes.ValidarRequest(await validator.ValidateAsync(pedido));
            pedido.Id = id;
            await servico.AlterarPedido(pedido);
            return Results.Ok();
        })
        .WithName("AlterarPedido")
        .WithTags("Pedido");

        app.MapDelete("/pedido/{id}", async (IPedidoServico servico, int id) =>
        {
            await servico.DeletarPedido(id);
            return Results.Ok();
        })
        .WithName("DeletarPedido")
        .WithTags("Pedido");

        #endregion Pedido

        #region Cozinha/Copa

        app.MapGet("/cozinha", async (IPedidoServico servico) =>
        {
            return Results.Ok(await servico.ObterPedidosCozinha());
        })
        .WithName("ObterCozinhaPedidos")
        .WithTags("Cozinha");

        app.MapPatch("/cozinha/{id}", async (
            IPedidoServico servico,
            int id) =>
        {
            await servico.AtualizarPratosProntos(id);
            return Results.Ok();
        })
        .WithName("AtualizarPratosProntos")
        .WithTags("Cozinha");

        app.MapGet("/copa", async (IPedidoServico servico) =>
        {
            return Results.Ok(await servico.ObterPedidosCopa());
        })
        .WithName("ObterCopaPedidos")
        .WithTags("Copa");

        app.MapPatch("/copa/{id}", async (
            IPedidoServico servico,
            int id) =>
        {
            await servico.AtualizarBebidaPronta(id);
            return Results.Ok();
        })
        .WithName("AtualizarBebidaPronta")
        .WithTags("Copa");


        #endregion Cozinha/Copa
    }

    private static void InjetarDependencias(IServiceCollection services)
    {
        services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
        services.AddScoped<IPedidoServico, PedidoServico>();
        services.AddSingleton<IDbContext, SQLiteDbContext>();

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        // FluentValidation
        services.AddScoped<IValidator<Pedido>, PedidoValidator>();

        services.AddCors(option =>
        {
            option.AddPolicy("AllOrigins",
                builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod();
                });
        });

        InjetarMigrations(services);

        #region Swagger

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "API Pedido",
                Description = "",
                Contact = new OpenApiContact
                {
                    Name = "",
                    Email = string.Empty
                }
            });
            c.OperationFilter<SwaggerJsonIgnoreFilter>();
        });

        #endregion Swagger
    }

    private static void InjetarMigrations(IServiceCollection services)
    {
        _serviceProvider = services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString($@"Data Source={Constantes.CaminhoBancoDeDados}; Version=3;")
                .ScanIn(typeof(PedidoCreateTable).Assembly)
                .ScanIn(typeof(PedidoBebidaCreateTable).Assembly)
                .ScanIn(typeof(PedidoPratoCreateTable).Assembly)
                .For.Migrations())
            .BuildServiceProvider(false);
    }

    public static void CriarBancoSQLiteCasoNaoExista()
    {
        try
        {
            FileInfo file = new FileInfo(Constantes.CaminhoBancoDeDados);
            if (!file.Exists)
                SQLiteConnection.CreateFile(Constantes.CaminhoBancoDeDados);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
    }

    private static void ExecutarMigrations()
    {
        CriarBancoSQLiteCasoNaoExista();

        IMigrationRunner migrationRunner =
            _serviceProvider.GetRequiredService<IMigrationRunner>();

        migrationRunner.MigrateUp();
    }

    private static IConfiguration SetarAppSettings()
    {
        string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        if (!string.IsNullOrWhiteSpace(envName))
            configurationBuilder.AddJsonFile($"appsettings.{envName}.json", optional: true);
        configurationBuilder.AddEnvironmentVariables();
        return configurationBuilder.Build();
    }

    private static void SetarLogsPtBrCulture(WebApplication app)
    {
        CultureInfo[] supportedCultures = new[] { new CultureInfo("pt-Br") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(culture: "pt-Br", uiCulture: "pt-Br"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });
    }

    private static void SetarMiddlewareDeTratativaDeErros(WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }

    private static void SetarPastaLogs(WebApplication app)
    {
        app.UseSerilogRequestLogging();

        try
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                 Path.Combine(app.Environment.ContentRootPath, "logs")),
                RequestPath = "/logs"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(app.Environment.ContentRootPath, "logs")),
                RequestPath = "/logs"
            });
        }
        catch (Exception ex)
        {
            Log.Error($"Terminal - Startup.cs - Falha ao configurar logs via navegador. Erro: {ex.Message}");
        }
    }
}
