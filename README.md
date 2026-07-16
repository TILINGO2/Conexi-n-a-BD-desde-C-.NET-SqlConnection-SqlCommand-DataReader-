# Ejercicio Demo — Catálogo de Productos con ADO.NET

Este ejercicio sirve como **demostración explicativa** del tema _Conexión a Base de Datos desde C#/.NET usando SqlConnection, SqlCommand y SqlDataReader_.

Muestra en un solo programa los **tres métodos de ejecución más importantes** de ADO.NET:

| Método              | Se usa para...                                  | Aparece en el método... |
| ------------------- | ----------------------------------------------- | ----------------------- |
| `ExecuteReader()`   | Consultas `SELECT` (varias filas)               | `ListarProductos()`     |
| `ExecuteNonQuery()` | `INSERT`, `UPDATE`, `DELETE`                    | `InsertarProducto()`    |
| `ExecuteScalar()`   | Consultas de un único valor (`COUNT`, `MAX`...) | `ContarProductos()`     |

---

## Objetivo del ejercicio

Al finalizar la explicación, el estudiante debe ser capaz de:

1. Reconocer el rol de cada una de las tres clases principales de ADO.NET.
2. Entender el ciclo de vida de una conexión (`Open` → `Ejecutar` → `Close`).
3. Identificar cuándo usar cada método de ejecución del `SqlCommand`.
4. Aplicar `using` para liberar recursos y `SqlParameter` para evitar inyección SQL.

---

## Requisitos previos

- **.NET SDK 8.0** (o 6.0 / 7.0) instalado.
- **SQL Server** local o remoto (puede ser SQL Server Express o LocalDB).
- **SQL Server Management Studio (SSMS)** o **Azure Data Studio** para ejecutar el script.
- Editor: **Visual Studio 2022** o **VS Code** con la extensión de C#.

---

## Estructura del proyecto

```
DemoProductos/
├── DemoProductos.csproj    # Configuración del proyecto y paquetes NuGet
├── Program.cs              # Código principal con las 3 demostraciones
├── script.sql              # Script para crear la base de datos y datos de prueba
└── README.md               # Este archivo
```

---

## Paso a paso para ejecutarlo

### 1) Preparar la base de datos

Abrir **SSMS** o **Azure Data Studio**, conectarse al servidor y ejecutar el archivo `script.sql`.
Esto crea:

- La base de datos `CatalogoDemo`.
- La tabla `Productos`.
- 5 registros de ejemplo.

### 2) Crear el proyecto (si se hace desde cero)

```bash
dotnet new console -n DemoProductos
cd DemoProductos
dotnet add package Microsoft.Data.SqlClient
```

> Se usa `Microsoft.Data.SqlClient` (nuevo) en lugar de `System.Data.SqlClient` (antiguo), porque es la versión recomendada actualmente por Microsoft.

Reemplazar el contenido de `Program.cs` por el archivo incluido.

### 3) Configurar la cadena de conexión

Dentro de `Program.cs`, revisar la constante `cadenaConexion` y ajustar según el entorno:

**Autenticación de Windows (más común en desarrollo local):**

```csharp
"Server=localhost;Database=CatalogoDemo;Integrated Security=True;TrustServerCertificate=True;"
```

**Autenticación de SQL Server (usuario y contraseña):**

```csharp
"Server=localhost;Database=CatalogoDemo;User Id=sa;Password=TuPassword;TrustServerCertificate=True;"
```

### 4) Ejecutar el programa

```bash
dotnet run
```

Salida esperada (resumida):

```
=== DEMO ADO.NET: Catálogo de Productos ===

--- LISTA DE PRODUCTOS ---
[1] Teclado mecánico          | Perifericos  | $  45.90 | Stock: 20
[2] Mouse inalámbrico         | Perifericos  | $  22.50 | Stock: 35
...

--- INSERTANDO PRODUCTO NUEVO ---
Se insertaron 1 fila(s).

--- CANTIDAD TOTAL DE PRODUCTOS ---
Total de productos en catálogo: 6

--- LISTA DE PRODUCTOS ---
... (ahora aparece el nuevo producto)
```

---

## Puntos clave para explicar en clase

### A. Ciclo de vida de una conexión

```
new SqlConnection() → Open() → [operaciones] → Close() (automático con 'using')
```

### B. La instrucción `using`

Se aplica a los **tres objetos** (`SqlConnection`, `SqlCommand`, `SqlDataReader`) para garantizar la liberación de recursos, incluso ante excepciones.

```csharp
using (SqlConnection conexion = new SqlConnection(...))
using (SqlCommand comando = new SqlCommand(...))
using (SqlDataReader reader = comando.ExecuteReader())
{
    // usar el reader
} // aquí se cierran los 3, en orden inverso
```

### C. ¿Por qué usar parámetros?

**Malo (vulnerable a inyección SQL):**

```csharp
string sql = "INSERT INTO Productos VALUES ('" + nombreUsuario + "', ...)";
```

**Bueno (seguro):**

```csharp
string sql = "INSERT INTO Productos VALUES (@nombre, ...)";
comando.Parameters.AddWithValue("@nombre", nombreUsuario);
```

### D. Manejo de excepciones

- `SqlException` → errores específicos del motor de SQL Server (conexión, sintaxis, restricciones).
- `Exception` → cualquier otro error inesperado.

---

## Preguntas guía para el aula

1. ¿Qué pasaría si eliminamos el `using` de la conexión?
2. ¿Por qué `ExecuteScalar` devuelve `object` en lugar de un tipo específico?
3. ¿Podríamos usar `ExecuteReader` para hacer un `INSERT`? ¿Sería correcto?
4. ¿Qué ventaja tiene `SqlDataReader` frente a cargar todo en una lista de memoria?

---

## Recursos adicionales

- [Documentación oficial Microsoft.Data.SqlClient](https://learn.microsoft.com/es-es/sql/connect/ado-net/microsoft-ado-net-sql-server)
- [Referencia de SqlConnection](https://learn.microsoft.com/es-es/dotnet/api/microsoft.data.sqlclient.sqlconnection)
- [Referencia de SqlCommand](https://learn.microsoft.com/es-es/dotnet/api/microsoft.data.sqlclient.sqlcommand)
- [Referencia de SqlDataReader](https://learn.microsoft.com/es-es/dotnet/api/microsoft.data.sqlclient.sqldatareader)
