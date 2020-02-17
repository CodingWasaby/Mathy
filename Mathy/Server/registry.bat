@echo off
SET day=%date:~0,4%%date:~5,2%%date:~8,2%
SET "image=ues:%day%"
docker build -t  %image%  .
docker tag  %image% wasabycode/%image%
docker push  wasabycode/%image%
docker rmi %image%
docker rmi wasabycode/%image%
pause