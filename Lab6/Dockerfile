# Dockerfile.build
# Etapa de construcción
FROM node:18-alpine as builder

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar archivos de dependencias e instalarlas
COPY package*.json ./
RUN npm install

# Copiar el código fuente y construir la aplicación
COPY . .
RUN npm run build


# Dockerfile.run
# Usar una imagen base minimalista para ejecutar la aplicación
FROM node:18-alpine

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar archivos desde la etapa de construcción
COPY --from=mi-app-build /app/dist /app/dist
COPY --from=mi-app-build /app/package*.json ./

# Instalar solo las dependencias necesarias para producción
RUN npm install --only=production

# Exponer el puerto de la aplicación
EXPOSE 1234

# Comando para ejecutar la aplicación
CMD ["node", "dist/app.js"]