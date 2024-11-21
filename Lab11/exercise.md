### **Parte 1: Diseño de la Infraestructura en la Nube**
#### **Arquitectura Propuesta**
- **Cómputo:**
  - Utilizar un **Grupo de Auto Scaling (ASG)** para las instancias EC2, permitiendo que escalen dinámicamente según el tráfico.
  - Desplegar instancias en al menos **dos zonas de disponibilidad (AZ)** para alta disponibilidad.
  - Configurar un **Elastic Load Balancer (ELB)** para distribuir el tráfico entrante.

- **Almacenamiento:**
  - Almacenar activos estáticos, logs o respaldos en un **bucket S3**.
  - Activar **versionado** y políticas de ciclo de vida para gestionar respaldos y datos obsoletos.

- **Redes:**
  - Crear una **VPC** con subredes públicas y privadas:
    - **Subred Pública:** Aloja el ELB y un NAT Gateway.
    - **Subred Privada:** Aloja las instancias EC2, aislándolas del acceso directo a internet.
  - Configurar una **tabla de rutas** para controlar el tráfico entre las subredes.

---

### **Parte 2: Configuración de IAM**
#### **Roles y Políticas de IAM**
- **Rol para Desarrolladores:**
  - Acceso a repositorios de código (ejemplo: **AWS CodeCommit**).
  - Acceso limitado de solo lectura a los **logs de CloudWatch** para depuración.
  - Sin permisos para modificar infraestructura en producción.

- **Rol para Administradores:**
  - Acceso completo para gestionar recursos de AWS, incluyendo EC2, VPC e IAM.
  - Requerir **MFA (Autenticación de Múltiples Factores)** para mayor seguridad.

- **Rol para la Aplicación (EC2):**
  - Permitir a las instancias EC2 leer y escribir en **buckets S3** específicos.
  - Sin acceso para usuarios humanos; solo la aplicación utiliza este rol.


### **Parte 3: Estrategia de Gestión de Recursos**
#### **Estrategia Propuesta**
- **Auto Scaling:**
  - Definir políticas de escalado basadas en umbrales de **utilización de CPU**.
  - Mantener un mínimo de 2 instancias y un máximo de 10 según la demanda.

- **Elastic Load Balancer (ELB):**
  - Usar un **Application Load Balancer (ALB)** para manejar tráfico HTTP/HTTPS.
  - Terminar SSL en el ALB para manejar tráfico seguro.

- **Optimización de Costos:**
  - Configurar **AWS Budgets** para monitorear los gastos mensuales y recibir alertas.
  - Implementar **reglas de ciclo de vida en S3** para mover datos poco usados a **S3 Glacier**.
  - Evaluar el uso de **Instancias Spot** para cargas de trabajo no críticas.

---

### **Parte 4: Implementación Teórica**
#### **Flujo de Datos y Control**
1. **Interacción del Usuario:**
   - Los usuarios acceden a la aplicación a través del **ALB**, que enruta el tráfico a las instancias EC2 en múltiples AZs.

2. **Servidores de Aplicación (EC2):**
   - Procesan solicitudes de los usuarios e interactúan con S3 para almacenar activos o respaldos.
   - Utilizan **roles de IAM** para acceder a recursos de forma segura.

3. **Almacenamiento (S3):**
   - Aloja contenido estático (imágenes, archivos CSS/JS).
   - Almacena logs y respaldos de la base de datos.

4. **Redes (VPC):**
   - Subredes privadas alojan las instancias EC2, asegurando que no estén expuestas directamente a internet.
   - Un NAT Gateway en la subred pública permite que las instancias privadas accedan a internet para actualizaciones o parches.

#### **Roles y Responsabilidades de los Servicios**
- **EC2:** Aloja el backend de la aplicación.
- **S3:** Almacena archivos estáticos, respaldos y logs.
- **VPC:** Proporciona un entorno seguro y aislado para los recursos.
- **IAM:** Gestiona permisos para asegurar interacciones seguras entre los componentes.

---

### **Parte 5: Discusión y Evaluación**
#### **Selección de Servicios**
- **¿Por qué EC2 y ALB?**
  - EC2 ofrece instancias personalizables, mientras que ALB asegura alta disponibilidad y distribución adecuada del tráfico.

- **¿Por qué S3?**
  - S3 proporciona almacenamiento altamente disponible y rentable para activos y respaldos.

- **¿Por qué VPC?**
  - La VPC garantiza una red segura dividiendo los recursos entre subredes privadas y públicas.

#### **Políticas de IAM y Seguridad**
- **Cumplimiento del Principio de Privilegio Mínimo:**
  - Se restringen accesos según los roles, garantizando que no haya permisos innecesarios.
  - MFA asegura un acceso seguro para roles administrativos.

#### **Escalabilidad y Eficiencia de Costos**
- **Auto Scaling** asegura escalado dinámico con la demanda.
- **Instancias Spot** y reglas de ciclo de vida en S3 reducen costos innecesarios.

---
