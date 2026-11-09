# Sistema de Gestión de Productos

**Tarea programada – Programación II**

Universidad Internacional San Isidro Labrador (UISIL) – Facultad de Ingeniería de Sistemas

**Profesor:** Ing. Joshua Granados Loría

**Estudiante:** Félix Lorenzo Araya Marín

## Descripción del sistema

Aplicación de escritorio para administrar los productos de una pequeña tienda. Permite gestionar categorías y productos (registrar, consultar, modificar y eliminar), buscar productos por nombre y ver el valor del inventario de cada producto (precio × stock). Cada producto pertenece a una categoría y los datos se guardan en una base de datos SQL Server.

## Tecnologías utilizadas

- C#
- Windows Forms
- Visual Studio
- Microsoft SQL Server (LocalDB)
- SQL Server Management Studio (SSMS)
- Git y GitHub

## Estructura del proyecto

- `Categoria.cs`, `Producto.cs`: clases que representan los datos (propiedades, get/set y encapsulamiento).
- `Datos/`: capa de acceso a datos (conexión y consultas SQL).
- `Logica/`: capa de lógica (validaciones y reglas del sistema).
- `FrmPrincipal.cs`, `FrmCategorias.cs`, `FrmProductos.cs`: capa de interfaz.
- `Database/script.sql`: script de la base de datos.

## Requisitos para ejecutar el programa

- Windows con Visual Studio instalado (carga de trabajo "Desarrollo de escritorio con .NET").
- SQL Server LocalDB (se instala junto con Visual Studio) o SQL Server.
- SQL Server Management Studio (SSMS) para ejecutar el script.

## Instrucciones para crear la base de datos

1. Abrir SSMS y conectarse al servidor `(localdb)\MSSQLLocalDB`.
2. Abrir el archivo `Database/script.sql` (Archivo → Abrir → Archivo).
3. Ejecutarlo con F5. El script crea la base de datos `GestionProductosDB`, las tablas `Categorias` y `Productos` (con llaves primarias y llave foránea) y datos de prueba.

Nota: si las tablas ya existen, el script las elimina y las vuelve a crear.

## Información para la conexión a SQL Server

La cadena de conexión está en `Datos/Conexion.cs`:

```
Server=(localdb)\MSSQLLocalDB;Database=GestionProductosDB;Integrated Security=True;
```

Usa autenticación de Windows. Si se usa otro servidor, se debe cambiar el valor de `Server` en ese archivo.

## Instrucciones para ejecutar el sistema

1. Clonar el repositorio o descargarlo.
2. Crear la base de datos (sección anterior).
3. Abrir la solución (`.slnx`) en Visual Studio.
4. Presionar F5 para ejecutar.
5. En el menú principal, el botón "Probar conexión" verifica la conexión con SQL Server.

## Funcionalidades principales

- Administración de categorías: registrar, consultar, modificar y eliminar (con confirmación).
- Administración de productos: registrar, consultar, modificar y eliminar (con confirmación).
- Asignación obligatoria de una categoría a cada producto.
- Búsqueda de productos por nombre.
- Visualización de los productos en un DataGridView, con fecha de registro, categoría y valor del inventario calculado.
- Validaciones con ErrorProvider y mensajes con MessageBox.

## Uso de inteligencia artificial

| Herramienta | Propósito | Parte del proyecto |
|---|---|---|
| Claude (Anthropic) | Se reciben explicación y ejemplos de lo que puedo utilizar | Con el temas de las Clases y Lógica, se hacen las correcciones. |

Usé la inteligencia artificial como apoyo porque la creación de la base de datos y la programación por capas es compleja. Yo diseñé los formularios en Visual Studio, configuré los controles y sus nombres, probé cada funcionalidad, corregí errores y fui subiendo el avance con commits. Revisé el código generado para entenderlo y poder explicarlo en la defensa.