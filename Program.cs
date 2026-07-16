// ============================================================
// DEMO: Conexión a Base de Datos desde C#/.NET
// Clases utilizadas: SqlConnection, SqlCommand y SqlDataReader
// ------------------------------------------------------------
// Este programa demuestra los tres métodos de ejecución más
// comunes de ADO.NET:
//   1) ExecuteReader   -> Para consultas SELECT (varias filas)
//   2) ExecuteNonQuery -> Para INSERT / UPDATE / DELETE
//   3) ExecuteScalar   -> Para consultas de un único valor
// ============================================================

using Microsoft.Data.SqlClient;      // Proveedor moderno de SQL Server
using System;
using System.Data;                   // Para CommandType

namespace DemoProductos
{
    class Program
    {
        // ------------------------------------------------------------
        // Cadena de conexión
        // ------------------------------------------------------------
        // Contiene los datos necesarios para conectarse al servidor:
        //   - Server:   nombre o IP de la instancia de SQL Server
        //   - Database: base de datos a la que se conecta
        //   - Integrated Security=True: usa las credenciales de Windows
        //     (si no, usar User Id=xxx;Password=xxx;)
        //   - TrustServerCertificate=True: acepta el certificado del
        //     servidor (útil en desarrollo local)
        // ------------------------------------------------------------
        private static readonly string cadenaConexion =
            "Server=localhost;Database=CatalogoDemo;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== DEMO ADO.NET: Catálogo de Productos ===\n");

            // Se ejecutan las tres demostraciones en orden
            ListarProductos();      // ExecuteReader   -> SqlDataReader
            InsertarProducto();     // ExecuteNonQuery
            ContarProductos();      // ExecuteScalar
            ListarProductos();      // Se vuelve a listar para ver el nuevo registro

            Console.WriteLine("\nPresione una tecla para salir...");
            Console.ReadKey();
        }

        // ============================================================
        // 1) SELECT con SqlDataReader (ExecuteReader)
        // ============================================================
        // Muestra cómo recorrer un conjunto de filas devuelto por SQL.
        // El SqlDataReader mantiene la conexión abierta y avanza fila
        // por fila, lo que es eficiente en memoria.
        // ============================================================
        static void ListarProductos()
        {
            Console.WriteLine("\n--- LISTA DE PRODUCTOS ---");

            // 'using' garantiza que la conexión se cierre automáticamente,
            // incluso si ocurre una excepción
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    // Se abre la conexión con el servidor
                    conexion.Open();

                    // Instrucción SQL a ejecutar
                    string sql = "SELECT Id, Nombre, Categoria, Precio, Stock FROM Productos";

                    // Se crea el comando asociado a la conexión
                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        // ExecuteReader devuelve un SqlDataReader
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            // Read() avanza una fila cada vez.
                            // Devuelve false cuando no hay más registros.
                            while (reader.Read())
                            {
                                // Acceso a las columnas por nombre
                                int id            = reader.GetInt32(reader.GetOrdinal("Id"));
                                string nombre     = reader["Nombre"].ToString()!;
                                string categoria  = reader["Categoria"].ToString()!;
                                decimal precio    = reader.GetDecimal(reader.GetOrdinal("Precio"));
                                int stock         = reader.GetInt32(reader.GetOrdinal("Stock"));

                                Console.WriteLine(
                                    $"[{id}] {nombre,-25} | {categoria,-12} | ${precio,7:F2} | Stock: {stock}");
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Excepción específica de SQL Server
                    Console.WriteLine($"Error de SQL Server: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Excepción general
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            } // Aquí se cierra y libera la conexión automáticamente
        }

        // ============================================================
        // 2) INSERT con ExecuteNonQuery
        // ============================================================
        // ExecuteNonQuery se usa para instrucciones que NO devuelven
        // filas (INSERT, UPDATE, DELETE). Devuelve el número de filas
        // afectadas por la operación.
        // Se utilizan PARÁMETROS para evitar inyección SQL.
        // ============================================================
        static void InsertarProducto()
        {
            Console.WriteLine("\n--- INSERTANDO PRODUCTO NUEVO ---");

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    // La consulta usa parámetros (@nombre, @categoria, etc.)
                    // en lugar de concatenar valores directamente.
                    string sql = @"INSERT INTO Productos (Nombre, Categoria, Precio, Stock)
                                   VALUES (@nombre, @categoria, @precio, @stock)";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        // Se agregan los parámetros con sus valores
                        comando.Parameters.AddWithValue("@nombre",    "Alfombrilla gamer");
                        comando.Parameters.AddWithValue("@categoria", "Accesorios");
                        comando.Parameters.AddWithValue("@precio",    12.99m);
                        comando.Parameters.AddWithValue("@stock",     50);

                        // ExecuteNonQuery devuelve el número de filas afectadas
                        int filasAfectadas = comando.ExecuteNonQuery();

                        Console.WriteLine($"Se insertaron {filasAfectadas} fila(s).");
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error de SQL Server: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        // ============================================================
        // 3) COUNT con ExecuteScalar
        // ============================================================
        // ExecuteScalar se usa cuando la consulta devuelve un ÚNICO
        // valor (por ejemplo, COUNT, MAX, MIN, SUM). Devuelve la primera
        // columna de la primera fila como un 'object'.
        // ============================================================
        static void ContarProductos()
        {
            Console.WriteLine("\n--- CANTIDAD TOTAL DE PRODUCTOS ---");

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT COUNT(*) FROM Productos";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        // ExecuteScalar devuelve un 'object', hay que convertirlo
                        object? resultado = comando.ExecuteScalar();
                        int total = Convert.ToInt32(resultado);

                        Console.WriteLine($"Total de productos en catálogo: {total}");
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error de SQL Server: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }
    }
}
