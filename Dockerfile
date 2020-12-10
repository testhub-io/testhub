FROM nginx:1.17-alpine
RUN apk add --update nodejs npm git

COPY nginx/default.conf /etc/nginx/conf.d/

RUN mkdir -p /usr/share/nginx/html && mkdir -p /usr/share/nginx/landing &&  rm -rf /usr/share/nginx/html/* && rm -rf /usr/share/nginx/landing/*

COPY public /usr/share/nginx/html
WORKDIR /tmp
RUN git clone https://shah-newaz:512a63946c1472196c7737bf37c5e97650909287@github.com/testhub-io/testhub-landing
RUN cd testhub-landing && npm install && npm run build && cp -Rv public/* /usr/share/nginx/landing
CMD ["nginx", "-g", "daemon off;"]
