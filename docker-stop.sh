#!/bin/bash -e

docker kill $(docker ps -q)
docker rm $(docker ps -a -q)