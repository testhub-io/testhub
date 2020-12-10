docker build -t testhub-frontend:v1 .
docker run -itd -p 8080:80 $(docker images testhub-frontend --format "{{.ID}}")
