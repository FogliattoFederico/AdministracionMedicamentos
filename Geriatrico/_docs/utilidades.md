# Utilidades del Sistema Geriátrico

## Resetear contraseña de un usuario

Si un usuario olvidó su contraseña o se recreó la base de datos, seguir estos pasos:

### 1 — Agregar en `UsuarioNegocio.cs`

```csharp
public void ActualizarPassword(string email, string nuevaPassword)
{
    using var conn = new SqlConnection(_connectionString);
    conn.Open();

    var hash = HashPassword(nuevaPassword);

    var cmd = new SqlCommand("UPDATE usuarios SET password = @Hash WHERE email = @Email", conn);
    cmd.Parameters.AddWithValue("@Hash", hash);
    cmd.Parameters.AddWithValue("@Email", email);

    cmd.ExecuteNonQuery();
}
```

### 2 — Agregar en `AccountController.cs`

```csharp
public IActionResult InicializarPassword()
{
    _negocio.ActualizarPassword("admin@geriatrico.com", "admin123");
    return Content("Contraseña actualizada correctamente.");
}
```

### 3 — Acceder a la URL
https://localhost:7126/Account/InicializarPassword

### 4 — Borrar los métodos al terminar

Eliminar ambos métodos del código una vez reseteada la contraseña.


## Recuperar contraseña del admin

Si el admin olvidó su contraseña, acceder desde la misma máquina a:
https://localhost:7126/Account/Recuperar

Esta página solo funciona desde localhost. Ingresar el email y la nueva contraseña.
---

## Credenciales por defecto

| Campo | Valor |
|-------|-------|
| Email | admin@geriatrico.com |
| Contraseña | admin123 |
| Rol | admin |

---

## Recrear la base de datos desde cero

1. Ejecutar el script `database.sql` en SQL Server Management Studio
2. Resetear la contraseña del admin con el procedimiento de arriba
3. Verificar la cadena de conexión en `appsettings.json`

---

## Cadena de conexión

Ubicación: `appsettings.json`

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Geriatrico;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## Roles del sistema

| Rol | Permisos |
|-----|----------|
| admin | Acceso total, gestión de usuarios |
| empleado | Acceso a pacientes, medicamentos y administración diaria |

---

## Estructura del proyecto
Geriatrico/
_docs/
database.sql              ← Script completo de la base de datos
utilidades.md             ← Este archivo
Controllers/
AccountController.cs      ← Login y logout
AdministracionesController.cs
HomeController.cs         ← Dashboard
MedicamentosController.cs
MedicamentosPacientesController.cs
PacientesController.cs
UsuariosController.cs
Models/
Dominio/
Administracion.cs
Medicamento.cs
MedicamentoPaciente.cs
Paciente.cs
Usuario.cs
Negocio/
AdministracionNegocio.cs
DashboardNegocio.cs
MedicamentoNegocio.cs
MedicamentoPacienteNegocio.cs
PacienteNegocio.cs
UsuarioNegocio.cs
ViewModels/
DashboardViewModel.cs
HojaDiariaViewModel.cs
LoginViewModel.cs
PacienteDetalleViewModel.cs
PacienteListaViewModel.cs
PerfilViewModel.cs
Views/
Account/
Login.cshtml
Administraciones/
HojaDiaria.cshtml
Home/
Index.cshtml
Medicamentos/
Agregar.cshtml
Index.cshtml
Modificar.cshtml
MedicamentosPacientes/
Agregar.cshtml
Modificar.cshtml
Pacientes/
Agregar.cshtml
Detalle.cshtml
Index.cshtml
Imprimir.cshtml
Modificar.cshtml
Shared/
_Layout.cshtml
Usuarios/
Agregar.cshtml
Index.cshtml
Perfil.cshtml

---

## Tecnologías utilizadas

| Tecnología | Versión | Uso |
|------------|---------|-----|
| .NET | 10 | Framework principal |
| ASP.NET Core MVC | 10 | Arquitectura web |
| SQL Server | 16 | Base de datos |
| Microsoft.Data.SqlClient | 7.0.1 | Conexión a SQL Server |
| Bootstrap | 5 | Estilos y componentes UI |
| ADO.NET | - | Acceso a datos |