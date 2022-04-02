#!/bin/bash -e

helm ls --short | xargs -L1 helm delete