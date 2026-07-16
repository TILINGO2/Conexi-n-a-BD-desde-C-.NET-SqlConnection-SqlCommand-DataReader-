# Práctica en Clase — Gestión de Empleados con ADO.NET

Práctica guiada para aplicar los conceptos de **SqlConnection**, **SqlCommand** y **SqlDataReader** en un proyecto real de consola en C#/.NET 8.

Deberás completar 13 `// TODO` distribuidos en el archivo `Program.cs` para que la aplicación funcione al 100%.

---

## Objetivos de aprendizaje

Al terminar esta práctica, el estudiante habrá:

- Configurado un proyecto .NET con el paquete `Microsoft.Data.SqlClient`.
- Escrito y ejecutado consultas `SELECT`, `INSERT`, `UPDATE`, `DELETE` desde C#.
- Utilizado los tres métodos de ejecución de `SqlCommand`: `ExecuteReader`, `ExecuteNonQuery` y `ExecuteScalar`.
- Aplicado `SqlParameter` para prevenir inyección SQL.
- Implementado manejo de excepciones con `SqlException` y `Exception`.
- Usado `using` para liberar recursos automáticamente.

---

## Requisitos previos

- .NET SDK **8.0** (o 6.0/7.0).
- SQL Server (Express, LocalDB o Developer).
- SSMS o Azure Data Studio.
- Editor: Visual Studio 2022 o VS Code con la extensión de C#.

---

## Estructura del proyecto

```
PracticaEmpleados/
├── PracticaEmpleados.csproj    # Configuración del proyecto y NuGet
├── Program.cs                  # Código con los TODO a completar
├── script.sql                  # Script para crear la BD y datos de prueba
└── README.md                   # Este archivo (guía paso a paso)
```

---

## Puesta en marcha

### Paso 1 — Crear la base de datos

En SSMS o Azure Data Studio, abre `script.sql` y ejecútalo. Se creará:

- Base de datos: `EmpresaPractica`
- Tabla: `Empleados` (con 6 registros de ejemplo)

Verifica ejecutando:

```sql
SELECT * FROM Empleados;
```

### Paso 2 — Restaurar dependencias

Desde la carpeta del proyecto:

```bash
dotnet restore
```

Esto instala el paquete `Microsoft.Data.SqlClient`.

### Paso 3 — Completar los TODO

Abre `Program.cs` y resuelve los 13 puntos marcados con `// TODO`. Ver la sección [Guía de resolución](#-guía-de-resolución) más abajo.

### Paso 4 — Ejecutar

```bash
dotnet run
```

Verás un menú de 6 opciones. Prueba cada una.

---

## Menú final esperado

```
======== GESTIÓN DE EMPLEADOS ========
1. Listar todos los empleados
2. Buscar por departamento
3. Insertar nuevo empleado
4. Actualizar salario
5. Eliminar empleado
6. Contar empleados
0. Salir
```

---

## Guía de resolución

Estas son **pistas**, no la solución completa. Debes escribir el código.

### 🔹 TODO (1) — Cadena de conexión

Sustituye `___` por el nombre de tu servidor y de la base de datos.

Ejemplo típico local:

```csharp
"Server=localhost;Database=EmpresaPractica;Integrated Security=True;TrustServerCertificate=True;"
```

Si usas LocalDB:

```csharp
"Server=(localdb)\\MSSQLLocalDB;Database=EmpresaPractica;Integrated Security=True;"
```

### 🔹 TODO (2) — Leer resultados con `SqlDataReader`

Debes recorrer las filas devueltas por el reader y mostrarlas por consola.

Estructura:

```csharp
while (reader.Read())
{
    int id = reader.GetInt32(reader.GetOrdinal("Id"));
    string nombre = reader["Nombre"].ToString()!;
    // ... completa con las demás columnas
    Console.WriteLine($"[{id}] {nombre} ...");
}
```

### 🔹 TODO (3) — SELECT con parámetro

Escribe un `SELECT` que use un parámetro para filtrar por departamento:

```csharp
string sql = "SELECT Id, Nombre, Apellido, Salario FROM Empleados WHERE Departamento = @dep";
```

### 🔹 TODO (4) — Agregar el parámetro

```csharp
comando.Parameters.AddWithValue("@dep", depto);
```

### 🔹 TODO (5) — Recorrer resultados

Igual que el TODO (2), pero con las columnas que devolviste en el SELECT.

### 🔹 TODO (6) — INSERT

```csharp
string sql = @"INSERT INTO Empleados (Nombre, Apellido, Departamento, Salario, FechaIngreso)
               VALUES (@nombre, @apellido, @dep, @salario, @fecha)";
```

### 🔹 TODO (7) — Parámetros del INSERT

Un `AddWithValue` por cada `@parametro`.

```csharp
comando.Parameters.AddWithValue("@nombre", nombre);
comando.Parameters.AddWithValue("@apellido", apellido);
// ... etc
```

### 🔹 TODO (8) — Ejecutar

```csharp
int filas = comando.ExecuteNonQuery();
Console.WriteLine($"Se insertaron {filas} fila(s).");
```

### 🔹 TODO (9) — UPDATE

```csharp
string sql = "UPDATE Empleados SET Salario = @salario WHERE Id = @id";
```

### 🔹 TODO (10) — Parámetros del UPDATE

```csharp
comando.Parameters.AddWithValue("@salario", nuevoSalario);
comando.Parameters.AddWithValue("@id", id);
```

### 🔹 TODO (11) — DELETE

```csharp
string sql = "DELETE FROM Empleados WHERE Id = @id";
```

### 🔹 TODO (12) — Ejecutar el DELETE

```csharp
comando.Parameters.AddWithValue("@id", id);
int filas = comando.ExecuteNonQuery();
if (filas > 0)
    Console.WriteLine("Empleado eliminado.");
else
    Console.WriteLine("No existe un empleado con ese Id.");
```

### 🔹 TODO (13) — Contar con `ExecuteScalar`

```csharp
object? resultado = comando.ExecuteScalar();
int total = Convert.ToInt32(resultado);
Console.WriteLine($"Total de empleados: {total}");
```

---

## Checklist de entrega

Antes de entregar, verifica que:

- [ ] La cadena de conexión funciona (no lanza `SqlException` al conectar).
- [ ] Todos los métodos usan `using` para `SqlConnection`, `SqlCommand` y `SqlDataReader`.
- [ ] Todas las consultas con valores del usuario usan **parámetros** (nada de concatenación).
- [ ] Cada método tiene bloque `try/catch` para `SqlException` y `Exception`.
- [ ] La aplicación no se cierra por errores inesperados (por ejemplo, ingresar texto donde se espera un número puede requerir un `TryParse`).
- [ ] Las 6 opciones del menú funcionan correctamente.

---

## Retos extra (opcional)

Si terminas antes de tiempo, intenta:

1. **Validación de entrada:** usar `int.TryParse` y `decimal.TryParse` en lugar de `Parse`, para no romper el programa si el usuario escribe mal.
2. **Ordenar la lista** de empleados por salario descendente.
3. **Buscar por rango de salario** (entre X e Y).
4. **Mostrar el promedio de salario por departamento** usando `ExecuteScalar` y `AVG`.
5. **Refactorizar** el código extrayendo la creación de la conexión a un método auxiliar `SqlConnection ObtenerConexion()`.

---

## Rúbrica sugerida (para el docente)

| Criterio                                        | Puntos |
| ----------------------------------------------- | ------ |
| El proyecto compila y ejecuta sin errores       | 2      |
| Las 6 opciones del menú funcionan correctamente | 4      |
| Uso correcto de `using` en las tres clases      | 1      |
| Uso de parámetros (no hay concatenación de SQL) | 1      |
| Manejo de excepciones en todos los métodos      | 1      |
| Comentarios propios agregados al código         | 1      |
| **Total**                                       | **10** |
