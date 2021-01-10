FROM nginx:1.17-alpine
RUN apk add --update nodejs npm git

COPY nginx/default.conf /etc/nginx/conf.d/

RUN mkdir -p /usr/share/nginx/html && mkdir -p /usr/share/nginx/landing &&  rm -rf /usr/share/nginx/html/* && rm -rf /usr/share/nginx/landing/*

COPY public /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]
