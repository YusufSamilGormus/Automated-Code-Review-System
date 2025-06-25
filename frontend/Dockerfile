# 1. Aþama: Uygulamayý build et
FROM node:22-alpine as build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# 2. Aþama: Static dosyalarý serve et
FROM nginx:stable-alpine
COPY --from=build /app/build /usr/share/nginx/html

# Nginx yapýlandýrmasý (opsiyonel ama tavsiye edilir)
EXPOSE 80
