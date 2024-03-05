using DTI.Pedidos.Utils;
using System.Data;
using System.Data.SQLite;

namespace DTI.Pedidos.DbContexto;

public interface IDbContext
{
    void Open();
    void Close();
}

public class SQLiteDbContext : IDbContext
{
    protected readonly SQLiteConnection _connection;
    private readonly string _connectionString = $"Data Source={Constantes.CaminhoBancoDeDados}; Version=3;";

    public SQLiteDbContext()
    {
        _connection = new SQLiteConnection(_connectionString);
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
    }

    public void Open()
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
    }

    public void Close()
    {
        _connection.Close();
    }
}
