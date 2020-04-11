FROM nginx:1.17-alpine
RUN apk add --update nodejs nodejs-npm
COPY nginx/default.conf /etc/nginx/conf.d/
RUN rm -rf /usr/share/nginx/html/*
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app
COPY . /usr/src/app
RUN npm install && npm run build
COPY /usr/src/app/public/* /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]
