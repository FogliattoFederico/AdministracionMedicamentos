# Sistema de Gestión de Medicación para Residencias Geriátricas 🏥

Este sistema nace de mi experiencia directa como **Kinesiólogo** en el ámbito de la salud de adultos mayores. Al identificar los riesgos, la falta de trazabilidad y el desorden que implica el registro de medicación en papel, decidí desarrollar una solución digital que profesionalice y asegure este proceso crítico.

La aplicación permite llevar un control exhaustivo de pacientes y medicamentos, registrando cada toma diaria con precisión (quién administró, a qué hora y el resultado de la toma), garantizando que la información sea legible, segura y auditable.

## 🌟 Características Principales
*   **Gestión de Pacientes:** Perfil detallado de cada residente.
*   **Control de Medicación:** Carga y seguimiento de fármacos por paciente.
*   **Registro de Tomas:** Interfaz diseñada para el personal de enfermería/asistencia, permitiendo registrar la administración en tiempo real.
*   **Trazabilidad:** Historial digital de tomas para auditorías y seguimiento médico.

## 🛠️ Stack Tecnológico
*   **Backend:** ASP.NET Core MVC.
*   **Base de Datos:** SQL Server (Script de creación incluido).
*   **Frontend:** Bootstrap (interfaz responsive y fácil de usar).
*   **Arquitectura:** Diseño en capas para separar la lógica de negocio, acceso a datos y presentación.

## 🚀 Instalación y Configuración
1.  **Clonar el repositorio:**
    ```bash
    git clone https://github.com
    ```
2.  **Base de Datos:**
    *   Localizá el archivo del script SQL en la carpeta del proyecto.
    *   Ejecutalo en tu instancia de SQL Server para crear las tablas y la estructura necesaria.
3.  **Configuración:**
    *   Revisá el archivo `appsettings.json` y ajustá la cadena de conexión (`ConnectionString`) con tus credenciales locales.
4.  **Ejecutar:**
    *   Abrí la solución en **Visual Studio 2022**.
    *   Presioná `F5` para compilar y ejecutar la aplicación.

---
**Proyecto desarrollado por un profesional de la salud con pasión por la tecnología.** 🩺💻
