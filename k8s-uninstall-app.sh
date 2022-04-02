#!/bin/bash -e

helm ls --short | grep service | xargs -L1 helm delete
helm ls --short | grep frontend | xargs -L1 helm delete
helm ls --short | grep backend | xargs -L1 helm delete