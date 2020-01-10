echo -e " Please \e[31mMAKE SURE\e[0m that local docker image testshub-frontend image isnt used and press enter"
echo -e "\e[45m                                DOCKERIZING STARTED                                    "
echo -e "\e[0m\n"
echo -e "\e[35m -- (1/4) BUILDING THE APP \e[0m"
ng build --prod
echo -e "\e[35m -- (2/4) DELETING LAST IMAGE \e[0m"
docker rmi testshub-frontend
echo -e "\e[35m -- (3/4) MAKING DOCKER IMAGE \e[0m"
docker build -t testshub-frontend .
# echo -e "\e[35m -- (4/4) SAVING IMAGE FILE \e[0m"
# docker save -o ./testshub-frontend-docker-image testshub-frontend
# echo -e "\n\n"
# echo -e "Docker image is saved in project dir with name \e[7m testshub-frontend-docker-image \e[0m"
# echo -e "\n\n"
# #sleep 2
# #read -p "(Press enter to close)"
echo -e "\e[35m -- (3/4) Pushing DOCKER IMAGE \e[0m"
docker tag  testshub-frontend testhubio.azurecr.io/testhub-frontend
docker push  testhubio.azurecr.io/testhub-frontend