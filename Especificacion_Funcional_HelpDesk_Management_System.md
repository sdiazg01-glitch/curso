# Especificación Funcional
## HelpDesk Management System

**Versión:** 1.0  
**Estado:** Borrador para validación con las áreas usuarias  
**Idioma de interfaz:** Español  
**Propósito del documento:** Definir el comportamiento funcional esperado de una plataforma de gestión de solicitudes de soporte.

## 1. Propósito y problema

La organización necesita un punto único para recibir, clasificar, asignar y resolver solicitudes de soporte. Cuando las solicitudes llegan por canales dispersos o se administran sin trazabilidad, se dificulta conocer su responsable, prioridad, tiempo de atención, comunicaciones y solución.

El HelpDesk Management System centralizará estas solicitudes como tickets, permitirá seguir su ciclo de vida y ofrecerá información operativa para solicitantes, agentes, supervisores y administradores.

## 2. Objetivos

- Registrar y consultar solicitudes de soporte desde un canal centralizado.
- Asignar cada ticket a un equipo y, cuando corresponda, a un agente responsable.
- Priorizar el trabajo según impacto y urgencia.
- Controlar tiempos de primera respuesta y resolución mediante SLA configurables.
- Mantener un historial auditable de cambios, comunicaciones y acciones.
- Dar visibilidad del estado de cada solicitud a quien la reportó.
- Medir volúmenes, cumplimiento de SLA, carga de trabajo y causas recurrentes.

## 3. Alcance

### Incluido en la primera versión

- Acceso autenticado y control de permisos por rol.
- Portal para crear, consultar y complementar tickets propios.
- Consola operativa para agentes y supervisores.
- Clasificación, prioridad, asignación, estados, comentarios y adjuntos.
- Alertas y notificaciones por correo configurables.
- Reglas de SLA, escalamiento, búsqueda, tableros e informes básicos.
- Administración de usuarios, equipos, categorías, prioridades, horarios y reglas.
- Base de conocimiento básica vinculable a tickets.

### Fuera del alcance inicial

- Inventario y ciclo de vida de activos, salvo que se apruebe como requerimiento adicional.
- Facturación, cobros o gestión comercial.
- Automatización avanzada con inteligencia artificial.
- Integraciones con herramientas externas no identificadas durante el levantamiento.
- Ingreso automático de correos como tickets. El correo se contempla inicialmente para notificaciones salientes.

## 4. Actores y permisos

| Actor | Responsabilidades y permisos principales |
|---|---|
| Solicitante | Crear tickets, consultar los propios, responder preguntas, adjuntar evidencia, confirmar solución o solicitar reapertura. |
| Agente de soporte | Consultar tickets de sus equipos, tomar o recibir asignaciones, actualizar clasificación y estado, comunicarse con el solicitante y registrar resolución. |
| Supervisor | Administrar colas de trabajo, reasignar tickets, monitorear SLA y carga, escalar casos y consultar informes del equipo. |
| Administrador funcional | Administrar usuarios, roles, equipos, categorías, prioridades, horarios, SLA, plantillas y parámetros del sistema. |
| Auditor o consulta | Consultar información autorizada e historial sin modificar tickets. |

El acceso a datos se limitará por rol, equipo y relación con el ticket. La consulta de tickets ajenos por parte de solicitantes estará denegada salvo autorización expresa.

## 5. Requisitos funcionales

### RF-01. Autenticación y sesión

El sistema deberá permitir iniciar y cerrar sesión, validar credenciales y aplicar permisos por rol. Las sesiones expirarán según una política configurable. Los intentos fallidos y cambios de permisos deberán quedar registrados.

### RF-02. Alta de ticket

El solicitante podrá crear un ticket con, como mínimo:

- Asunto y descripción del problema.
- Servicio o categoría, cuando aplique.
- Impacto y urgencia o prioridad sugerida.
- Medio de contacto y disponibilidad, si difieren del perfil.
- Archivos adjuntos opcionales.

El sistema asignará un identificador único, fecha y hora de creación, solicitante, estado inicial y canal de ingreso. Antes de guardar validará los campos obligatorios y límites de adjuntos.

### RF-03. Clasificación y prioridad

El ticket podrá clasificarse por servicio, categoría y subcategoría configurables. El sistema calculará una prioridad usando una matriz de impacto y urgencia administrable. Los roles autorizados podrán modificarla; toda modificación conservará valor anterior, nuevo valor, usuario, fecha y motivo.

### RF-04. Consulta del solicitante

El solicitante podrá ver el estado, prioridad visible, equipo o agente responsable cuando la política lo permita, comunicaciones públicas, fechas relevantes y solución de sus tickets. No podrá ver notas internas ni datos restringidos de otros usuarios.

### RF-05. Asignación y colas

El sistema permitirá asignar tickets a un equipo, a un agente o a una cola sin agente individual. Supervisores y agentes autorizados podrán tomar, reasignar y escalar tickets. Cada cambio registrará responsable anterior y nuevo responsable.

### RF-06. Ciclo de vida del ticket

Estados funcionales propuestos:

1. **Nuevo:** creado y pendiente de clasificación o asignación.
2. **Abierto:** aceptado para atención.
3. **En progreso:** un agente está trabajando en el caso.
4. **Pendiente del solicitante:** se requiere información o acción de quien reportó.
5. **Pendiente de tercero:** depende de proveedor u otro equipo.
6. **Resuelto:** se registró una solución y se espera confirmación o cierre automático.
7. **Cerrado:** atención finalizada; solo consulta, salvo reapertura autorizada.
8. **Cancelado:** solicitud anulada con motivo.

Las transiciones permitidas dependerán del estado actual y el rol. Las transiciones no permitidas se rechazarán con un mensaje claro.

### RF-07. Comunicación y notas

Agentes y solicitantes podrán intercambiar comentarios públicos en el ticket. El personal de soporte podrá registrar notas internas que no se enviarán ni mostrarán al solicitante. El sistema identificará claramente ambos tipos y conservará autor y fecha.

### RF-08. Adjuntos

Los usuarios autorizados podrán adjuntar archivos a un ticket o comentario. El sistema validará tamaño, extensión y tipo permitido, almacenará metadatos y verificará permisos de descarga. La lista de tipos y el tamaño máximo serán configurables.

### RF-09. SLA y calendario

El sistema calculará plazos de primera respuesta y resolución según prioridad, servicio, horario laboral, festivos y calendario configurados. Mostrará tiempo transcurrido y tiempo restante o vencido. La política de pausa durante estados pendientes será configurable y auditable.

### RF-10. Alertas y escalamiento

El sistema notificará eventos configurables, como creación, asignación, comentario, cambio de estado, vencimiento próximo y resolución. Al aproximarse o incumplirse un SLA, podrá notificar al agente y supervisor, y aplicar reglas de escalamiento. El medio y destinatarios serán administrables.

### RF-11. Resolución y reapertura

Para resolver un ticket, el agente deberá indicar una categoría de resolución y una descripción de la solución. El solicitante podrá confirmar o pedir reapertura durante el plazo configurado. El sistema conservará el historial previo y registrará el motivo de reapertura.

### RF-12. Búsqueda y filtros

Agentes y supervisores podrán buscar por identificador, asunto, solicitante, categoría, agente, equipo, prioridad y estado, dentro de sus permisos. Los filtros podrán combinarse y ordenarse por fecha, prioridad o vencimiento de SLA.

### RF-13. Tableros e informes

El sistema ofrecerá vistas por rol. Los informes iniciales incluirán:

- Tickets abiertos, nuevos, pendientes, resueltos y vencidos.
- Tickets por servicio, categoría, prioridad, equipo y agente.
- Tiempo medio de primera respuesta y resolución.
- Cumplimiento de SLA y volumen de reaperturas.
- Tendencias por periodo y solicitudes recurrentes.

Los resultados respetarán los permisos del usuario y permitirán filtrar por rango de fechas.

### RF-14. Base de conocimiento

Los agentes autorizados podrán crear, editar, publicar y retirar artículos con título, contenido, categorías y estado. Los agentes podrán asociar artículos a tickets. El portal podrá sugerir artículos al solicitante antes de crear una solicitud, sin impedir que la registre.

### RF-15. Administración funcional

El administrador funcional podrá gestionar usuarios, equipos, membresías, roles, categorías, matriz de prioridad, calendarios, políticas SLA, reglas de escalamiento, plantillas y parámetros de notificación. Los cambios que afecten tickets activos deberán tener efecto definido y quedar auditados.

### RF-16. Auditoría

El sistema mantendrá un historial no editable desde la interfaz de usuario para creación, cambios de campos, asignaciones, transiciones, notas, resolución, reapertura, adjuntos y cambios administrativos relevantes. Cada evento contendrá usuario, fecha y hora, acción y valores aplicables.

## 6. Reglas de negocio

- Cada ticket tendrá un identificador único, visible y no reutilizable.
- Todo ticket deberá pertenecer a un solicitante y tener asunto, descripción, prioridad y estado.
- La prioridad se derivará de la matriz configurada; las excepciones exigirán permiso y motivo.
- Solo una comunicación marcada como pública podrá ser visible para el solicitante.
- El cierre requerirá que exista una solución registrada o un motivo de cancelación.
- El solicitante solo podrá modificar o complementar tickets propios y no cerrados, dentro de las reglas definidas.
- La pausa y reanudación de SLA dependerán de estados y calendarios configurables.
- El historial de auditoría no se eliminará al cerrar o reabrir un ticket.
- Las reglas de retención y eliminación de datos deberán ajustarse a la política institucional y legislación aplicable.

## 7. Entidades principales

- **Usuario:** identidad, datos de contacto, rol, equipo y estado.
- **Ticket:** identificador, asunto, descripción, solicitante, servicio, categoría, impacto, urgencia, prioridad, estado, canal, fechas, equipo y agente.
- **Comentario:** ticket relacionado, autor, contenido, visibilidad pública o interna y fecha.
- **Adjunto:** ticket o comentario relacionado, nombre, tipo, tamaño, ubicación segura, autor y fecha.
- **Asignación:** ticket, equipo o agente, asignador, fecha y motivo.
- **Registro SLA:** política aplicada, vencimientos, pausas, reanudaciones y resultado.
- **Evento de auditoría:** actor, acción, valores y marca de tiempo.
- **Artículo de conocimiento:** contenido, categoría, estado, versión y autor.
- **Configuración:** equipos, categorías, prioridades, calendarios, SLA y reglas de notificación.

## 8. Requisitos no funcionales

- **Seguridad:** control de acceso por mínimo privilegio, protección de credenciales, sesiones seguras, validación de entradas y acceso autorizado a adjuntos.
- **Privacidad:** limitar la exposición de información personal y evitar mostrar notas internas a solicitantes.
- **Disponibilidad:** definir con el área responsable el objetivo de disponibilidad y el horario de soporte.
- **Rendimiento:** las búsquedas y vistas operativas deberán responder dentro del objetivo acordado para el volumen previsto.
- **Usabilidad:** interfaz en español, adaptable a escritorio y móvil, con mensajes de validación claros y navegación por teclado.
- **Trazabilidad:** registrar marcas de tiempo consistentes y cambios relevantes.
- **Respaldo y recuperación:** establecer frecuencia de copias, retención y objetivo de recuperación con el equipo de infraestructura.
- **Configurabilidad:** SLA, horarios, prioridades y categorías no deben depender de cambios de código rutinarios.

## 9. Criterios de aceptación

1. Un solicitante autenticado crea un ticket válido, recibe un identificador único y puede consultarlo desde su portal.
2. Un ticket incompleto no se guarda y muestra los campos que deben corregirse.
3. Un agente solo puede consultar y modificar tickets permitidos por su rol y equipo.
4. Una nota interna no aparece en la vista del solicitante ni en notificaciones públicas.
5. Al cambiar una asignación, prioridad o estado, el sistema registra el actor y el valor anterior y nuevo.
6. El SLA refleja el calendario y las pausas configuradas; al aproximarse el vencimiento se ejecuta la notificación definida.
7. El solicitante puede responder a un ticket pendiente y el agente ve esa respuesta en el historial.
8. Un ticket resuelto puede cerrarse o reabrirse de acuerdo con la regla configurada, conservando su historial.
9. Los informes aplican filtros y permisos, y sus totales coinciden con los tickets consultables por el usuario.
10. Los artículos publicados son consultables por los perfiles autorizados y los borradores no son visibles al público.

## 10. Supuestos y decisiones pendientes

Antes de aprobar la construcción deberán definirse:

- Nombre, horario laboral, zona horaria y calendario de festivos.
- Matriz de prioridad y objetivos de SLA por servicio y prioridad.
- Reglas para pausar SLA y plazos para confirmar o reabrir resoluciones.
- Roles definitivos, equipos, visibilidad entre áreas y política de acceso de solicitantes.
- Límites de adjuntos, extensiones permitidas, retención y ubicación de archivos.
- Proveedor de correo, remitente y plantillas institucionales.
- Volumen esperado, disponibilidad, respaldo, retención y requisitos legales.
- Si se incluirán posteriormente inventario de activos, ingreso por correo, integración con directorio institucional u otras herramientas.
