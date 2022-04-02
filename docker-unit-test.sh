#!/bin/bash -e

#  ./docker_unit_test.sh service_stores
export COMPONENT=$1

if [ -z "$1" ] 
then
    echo "You must supply a component such as frontend_admin"
    exit -1
fi

docker-compose -f docker-compose-unit-tests.yml up --abort-on-container-exit --exit-code-from $COMPONENT $COMPONENT