FROM alpine:3.13.5

RUN apk add --no-cache \
        libc6-compat

COPY bin/testhub-cli /usr/bin/testhub-cli
COPY docker-entrypoint.sh /usr/bin/run-cli.sh


CMD /usr/bin/run-cli.sh