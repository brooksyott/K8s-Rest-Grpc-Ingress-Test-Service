#!/bin/sh

# This script is used to create a secret in the Kubernetes cluster with the TLS certificates
# which is REQUIRED for the gRPC server to work.
kubectl create secret tls grpc-tls-secret --key certs/localhost.key --cert certs/localhost.crt --namespace echo-demo