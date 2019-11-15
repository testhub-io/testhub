FROM nginx:1.17-alpine

COPY nginx/default.conf /etc/nginx/conf.d/

RUN rm -rf /usr/share/nginx/html/*

COPY dist/testshub-frontend /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]
