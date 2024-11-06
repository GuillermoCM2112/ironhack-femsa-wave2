#### CODIGO ANTERIOR
```
FUNCTION authenticateUser(username, password):
  QUERY database WITH username AND password
  IF found
    RETURN True
  ELSE
    RETURN False
```

#### EXPLICACIÓN DE VULNERABILIDADES
- La contraseña no esta encriptada, se manda en texto plano.
- Se puede hacer inyección de SQL, ya que los parametros no estan sanitizados.
- Es mejor retornar un mensaje de error, para diferenciar entre un error de credenciales y un error interno de la base de datos.
- No se limita el número de intentos para hacer login

#### CODIGO REFACTORIZADO
```
FUNCTION authenticateUser(username, password):
  // Intentar obtener el registro del usuario
  SET userRecord = QUERY database WITH
    "SELECT username, passwordHash, failedAttempts, accountLockedUntil FROM users WHERE username = @username"
    USING PARAMETERS (@username = username)

  // Verificar si el usuario existe
  IF userRecord not found:
    RETURN { status: "Failed", message: "Credenciales inválidas" }

  // Verificar si la cuenta está bloqueada temporalmente
  IF userRecord.accountLockedUntil > currentTime:
    RETURN { status: "Locked", message: "Cuenta bloqueada. Intente más tarde." }

  // Hashear la contraseña ingresada
  SET passwordHashed = Hash(password)

  // Comparar la contraseña
  IF passwordHashed == userRecord.passwordHash:
    // Reiniciar el contador de intentos fallidos en caso de éxito
    UPDATE database SET failedAttempts = 0, accountLockedUntil = NULL WHERE username = @username
    SET token = generateJWT(username) // Generar un token de sesión
    RETURN { status: "Success", token: token }

  ELSE:
    // Incrementar el contador de intentos fallidos
    SET failedAttempts = userRecord.failedAttempts + 1

    IF failedAttempts >= maxAttempts:
      // Bloquear la cuenta temporalmente
      SET accountLockedUntil = currentTime + lockoutDuration
      UPDATE database SET failedAttempts = failedAttempts, accountLockedUntil = accountLockedUntil WHERE username = @username
      RETURN { status: "Locked", message: "Cuenta bloqueada. Intente más tarde." }

    ELSE:
      // Actualizar el número de intentos fallidos en la base de datos
      UPDATE database SET failedAttempts = failedAttempts WHERE username = @username
      RETURN { status: "Failed", message: "Credenciales inválidas" }

```

#### EXPLICACIÓN DEL CODIGO REFACTORIZADO
- La contraseña se esta encriptando con Hash.
- Se parametrizo la consulta para evitar inyecciones SQL.
- Se retorna un mensaje de error para poder evitar ataques de fuerza bruta.
- Se limita el número de logeos para poder evitar ataques de fuerza bruta.