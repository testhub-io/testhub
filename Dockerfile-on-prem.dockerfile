FROM nginx:1.17-alpine
RUN apk add --update nodejs npm git


RUN mkdir -p /usr/share/nginx/html && rm -rf /usr/share/nginx/html/*
WORKDIR /usr/share/nginx/html
COPY . .
CMD ["./start-frontend.sh"]
