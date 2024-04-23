# Kubernetes gRPC and REST Echo Service

A simple test service which replies with the pods information, to test ingress is working correctly for both gRPC and REST.  
I've created one app that will echo some kubernetes information and the request for both REST and gRPC.  
The C# application and Kubernetes configuration files allow to differentiate which protocol to use in the path
ie.  
- localhost:443/grpc : is a gPRC call
- localhost:443/rest/echo : the rest portion of the path causes the webapi (REST) to be called

### Certificates

gRPC requires certificates to work.

#### Certificate authority (CA)

This is for testing on a local host.  
Plaguerized from: https://gist.github.com/cecilemuller/9492b848eb8fe46d462abeb26656c4f8

```
openssl req -x509 -nodes -new -sha256 -days 1024 -newkey rsa:2048 -keyout RootCA.key -out RootCA.pem -subj "/C=US/CN=Example-Root-CA" 
openssl x509 -outform pem -in RootCA.pem -out RootCA.crt
```

#### Domain name certificate

First, create a file ```domains.ext``` that lists all your local domains:

```
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names
[alt_names]
DNS.1 = localhost
```
\
Generate ```localhost.key```, ```localhost.csr```, and ```localhost.crt```:

```
openssl req -new -nodes -newkey rsa:2048 -keyout localhost.key -out localhost.csr -subj "/C=US/ST=YourState/L=YourCity/O=Example-Certificates/CN=localhost.local"
openssl x509 -req -sha256 -days 1024 -in localhost.csr -CA RootCA.pem -CAkey RootCA.key -CAcreateserial -extfile domains.ext -out localhost.crt
```

#### Deploy the Certificates to Kubernetes as a Secret
Secret Name: grpc-tls-secret   
Name Space: echo-demo

#### Create the echo-demo namespace
```
kubectl apply -f namespace.yaml
```

#### Put the certs into the secret

```
kubectl create secret tls grpc-tls-secret --key certs/localhost.key --cert certs/localhost.crt --namespace echo-demo
```
Once completed the Ingress can use the certs. Clients need to ignore certificate errors.

### Deploy the Echo Service via Helm

#### Add the helm repository

```
helm repo add brooksyott https://raw.githubusercontent.com/brooksyott/brooksyott.github.io/main/charts
helm repo update
```

#### Deploy the NGINX Ingress

```
helm install ingress-nginx brooksyott/nginx --namespace ingress-nginx --create-namespace
```

#### Deploy the Echo Service

```
helm install echo-demo brooksyott/k8s-grpc-rest-echo --namespace echo-demo
```

### Deploy the Echo Service via Kubernetes Files

#### Create an Ingress

If an Ingress isn't defined, to deploy NGINX run
```
kubectl apply -f ingress-deploy.yaml
```

#### Deploy a REST echo service
```
kubectl apply -f deploy-rest.yaml
```

#### Deploy a gRPC echo service
```
kubectl apply -f deploy-grpc.yaml
```





