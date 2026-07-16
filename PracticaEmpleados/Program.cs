// ============================================================
// PRÁCTICA: Sistema de Gestión de Empleados
// Tema: SqlConnection, SqlCommand y SqlDataReader
// ------------------------------------------------------------
// INSTRUCCIONES:
// Este archivo contiene un menú funcional pero VARIOS métodos
// están incompletos. Debes completar los bloques marcados con
//    // TODO (N): ...
// para que la aplicación funcione correctamente.
//
// Al terminar, la aplicación debe permitir:
//   1) Listar todos los empleados
//   2) Buscar empleados por departamento
//   3) Insertar un nuevo empleado
//   4) Actualizar el salario de un empleado
//   5) Eliminar un empleado por Id
//   6) Contar cuántos empleados hay
// ============================================================

using Microsoft.Data.SqlClient;
using System;
using System.Globalization;

namespace PracticaEmpleados
{
    class Program
    {
        // ------------------------------------------------------------
        // TODO (1): Completar la cadena de conexión con los datos
        // de tu servidor SQL Server local.
        // Ejemplo: Server=localhost;Database=EmpresaPractica;Integrated Security=True;TrustServerCertificate=True;
        // ------------------------------------------------------------
        private static readonly string cadenaConexion =
            "Server=___;Database=___;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": ListarEmpleados(); break;
                    case "2": BuscarPorDepartamento(); break;
                    case "3": InsertarEmpleado(); break;
                    case "4": ActualizarSalario(); break;
                    case "5": EliminarEmpleado(); break;
                    case "6": ContarEmpleados(); break;
                    case "0": salir = true; break;
                    default:  Console.WriteLine("Opción no válida."); break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void MostrarMenu()
        {
            Console.WriteLine("======== GESTIÓN DE EMPLEADOS ========");
            Console.WriteLine("1. Listar todos los empleados");
            Console.WriteLine("2. Buscar por departamento");
            Console.WriteLine("3. Insertar nuevo empleado");
            Console.WriteLine("4. Actualizar salario");
            Console.WriteLine("5. Eliminar empleado");
            Console.WriteLine("6. Contar empleados");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");
        }

        // ============================================================
        // Opción 1: Listar todos los empleados (SELECT + SqlDataReader)
        // ============================================================
        static void ListarEmpleados()
        {
            Console.WriteLine("\n--- LISTA DE EMPLEADOS ---");

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT Id, Nombre, Apellido, Departamento, Salario, FechaIngreso FROM Empleados";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        // TODO (2): Recorrer el reader con un bucle while(reader.Read())
                        // e imprimir cada empleado en una línea. Formato sugerido:
                        // [Id] Nombre Apellido | Departamento | $Salario | FechaIngreso
                        //
                        // Pistas:
                        //   - reader.GetInt32(reader.GetOrdinal("Id"))
                        //   - reader["Nombre"].ToString()
                        //   - reader.GetDecimal(reader.GetOrdinal("Salario"))
                        //   - reader.GetDateTime(reader.GetOrdinal("FechaIngreso"))

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error de SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        // ============================================================
        // Opción 2: Buscar por departamento (SELECT con parámetro)
        // ============================================================
        static void BuscarPorDepartamento()
        {
            Console.Write("\nDepartamento a buscar: ");
            string? depto = Console.ReadLine();

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    // TODO (3): Definir la consulta SQL usando un parámetro @dep
                    // para filtrar por la columna Departamento.
                    string sql = "";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        // TODO (4): Agregar el parámetro @dep con el valor de la variable 'depto'.


                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            // TODO (5): Recorrer los resultados e imprimirlos.

                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error de SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        // ============================================================
        // Opción 3: Insertar nuevo empleado (ExecuteNonQuery)
        // ============================================================
        static void InsertarEmpleado()
        {
            Console.WriteLine("\n--- NUEVO EMPLEADO ---");

            Console.Write("Nombre: ");
            string? nombre = Console.ReadLine();

            Console.Write("Apellido: ");
            string? apellido = Console.ReadLine();

            Console.Write("Departamento: ");
            string? departamento = Console.ReadLine();

            Console.Write("Salario: ");
            decimal salario = decimal.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);

            Console.Write("Fecha de ingreso (AAAA-MM-DD): ");
            DateTime fecha = DateTime.Parse(Console.ReadLine() ?? DateTime.Today.ToString("yyyy-MM-dd"));

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    // TODO (6): Escribir la instrucción INSERT usando parámetros
                    // @nombre, @apellido, @dep, @salario, @fecha
                    string sql = "";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        // TODO (7): Agregar los 5 parámetros al comando con AddWithValue.


                        // TODO (8): Ejecutar el comando con ExecuteNonQuery
                        // y mostrar cuántas filas se insertaron.

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error de SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        // ============================================================
        // Opción 4: Actualizar salario (UPDATE)
        // ============================================================
        static void ActualizarSalario()
        {
            Console.Write("\nId del empleado a actualizar: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Nuevo salario: ");
            decimal nuevoSalario = decimal.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    // TODO (9): Escribir la consulta UPDATE para modificar el
                    // salario del empleado cuyo Id coincida. Usar parámetros.
                    string sql = "";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        // TODO (10): Agregar los parámetros @salario y @id.


                        int filas = comando.ExecuteNonQuery();

                        if (filas > 0)
                            Console.WriteLine($"Empleado {id} actualizado correctamente.");
                        else
                            Console.WriteLine($"No se encontró ningún empleado con Id {id}.");
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error de SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        // ============================================================
        // Opción 5: Eliminar empleado (DELETE)
        // ============================================================
        static void EliminarEmpleado()
        {
            Console.Write("\nId del empleado a eliminar: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    // TODO (11): Escribir el DELETE con parámetro @id.
                    string sql = "";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        // TODO (12): Agregar el parámetro y ejecutar con ExecuteNonQuery.
                        // Mostrar un mensaje indicando si se eliminó o no.

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error de SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }

        // ============================================================
        // Opción 6: Contar empleados (ExecuteScalar)
        // ============================================================
        static void ContarEmpleados()
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();

                    string sql = "SELECT COUNT(*) FROM Empleados";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        // TODO (13): Usar ExecuteScalar para obtener el total
                        // y mostrarlo en pantalla.

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error de SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }
            }
        }
    }
}
