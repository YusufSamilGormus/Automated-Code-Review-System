# 1. A�ama: Uygulamay� build et
FROM node:22-alpine as build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# 2. A�ama: Static dosyalar� serve et
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html

# Nginx yap�land�rmas� (opsiyonel ama tavsiye edilir)
EXPOSE 80
